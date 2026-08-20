using EveOPreview.Configuration;
using EveOPreview.Mediator.Messages;
using EveOPreview.Services.Interop;
using EveOPreview.UI.Hotkeys;
using EveOPreview.View;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;

namespace EveOPreview.Services
{
	sealed class ThumbnailManager : IThumbnailManager
	{
		#region Private constants
		private const int WINDOW_POSITION_THRESHOLD_LOW = -10_000;
		private const int WINDOW_POSITION_THRESHOLD_HIGH = 31_000;
		private const int WINDOW_SIZE_THRESHOLD = 10;
		private const int FORCED_REFRESH_CYCLE_THRESHOLD = 2;
		private const int DEFAULT_LOCATION_CHANGE_NOTIFICATION_DELAY = 2;

		private const string DEFAULT_CLIENT_TITLE = "EVE";
		#endregion

		#region Private fields
		private readonly IMediator _mediator;
		private readonly IProcessMonitor _processMonitor;
		private readonly IWindowManager _windowManager;
		private readonly IThumbnailConfiguration _configuration;
		private readonly DispatcherTimer _thumbnailUpdateTimer;
		private readonly IThumbnailViewFactory _thumbnailViewFactory;
		private readonly Dictionary<IntPtr, IThumbnailView> _thumbnailViews;

		private (IntPtr Handle, string Title) _activeClient;
		private IntPtr _externalApplication;

		private readonly object _locationChangeNotificationSyncRoot;
		private (IntPtr Handle, string Title, string ActiveClient, Point Location, int Delay) _enqueuedLocationChangeNotification;

		private bool _ignoreViewEvents;
		private bool _isHoverEffectActive;

		private int _refreshCycleCount;
		private int _hideThumbnailsDelay;

		private List<HotkeyHandler> _cycleClientHotkeyHandlers = new List<HotkeyHandler>();
		private readonly IMouseHookService _mouseHook;
		private bool _areHotkeysSuspended;
		private bool _areAllPreviewsHidden;

		// The delegate is stored in a field so that GC does not collect it while the hook is set
		private User32NativeMethods.WinEventDelegate _foregroundHookCallback;
		private IntPtr _foregroundHook;

		// Activating / minimizing a client window (AttachThreadInput, SetForegroundWindow,
		// WM_SYSCOMMAND) is synchronous with the input thread of the target process and can
		// block for a long time on a busy client. All such calls are executed on a worker
		// task with 'latest activation wins' coalescing, so rapid cycling stays responsive
		private readonly object _activationSyncRoot = new object();
		private readonly HashSet<IntPtr> _pendingMinimizeHandles = new HashSet<IntPtr>();
		private (IntPtr Handle, string Title) _pendingActivation;
		private bool _isActivationWorkerRunning;

		// DWM does not compose minimized windows and minimized clients stop presenting
		// frames, so their thumbnails freeze. Minimized clients are periodically 'woken up'
		// (restored without activation for a moment, then minimized back) to refresh them
		private readonly HashSet<IntPtr> _pendingWakeHandles = new HashSet<IntPtr>();
		private readonly Dictionary<IntPtr, long> _minimizedClientWakeTimestamps = new Dictionary<IntPtr, long>();
		#endregion

		public ThumbnailManager(IMediator mediator, IThumbnailConfiguration configuration, IProcessMonitor processMonitor, IWindowManager windowManager, IThumbnailViewFactory factory, IMouseHookService mouseHook)
		{
			this._mediator = mediator;
			this._mouseHook = mouseHook;
			this._processMonitor = processMonitor;
			this._windowManager = windowManager;
			this._configuration = configuration;
			this._thumbnailViewFactory = factory;

			this._activeClient = (IntPtr.Zero, ThumbnailManager.DEFAULT_CLIENT_TITLE);

			this.EnableViewEvents();
			this._isHoverEffectActive = false;

			this._refreshCycleCount = 0;
			this._locationChangeNotificationSyncRoot = new object();
			this._enqueuedLocationChangeNotification = (IntPtr.Zero, null, null, Point.Empty, -1);

			this._thumbnailViews = new Dictionary<IntPtr, IThumbnailView>();

			//  DispatcherTimer setup
			this._thumbnailUpdateTimer = new DispatcherTimer();
			this._thumbnailUpdateTimer.Tick += ThumbnailUpdateTimerTick;
			this._thumbnailUpdateTimer.Interval = new TimeSpan(0, 0, 0, 0, configuration.ThumbnailRefreshPeriod);

			this._hideThumbnailsDelay = this._configuration.HideThumbnailsDelay;

			this.RegisterConfiguredHotkeys();
		}

		private void RegisterConfiguredHotkeys()
		{
			foreach (CycleGroup group in this._configuration.CycleGroups)
			{
				RegisterCycleClientHotkey(group.ForwardHotkeys.Select(x => this._configuration.StringToKey(x)), true, group.ClientsOrder);
				RegisterCycleClientHotkey(group.BackwardHotkeys.Select(x => this._configuration.StringToKey(x)), false, group.ClientsOrder);
			}

			RegisterMinimizeAllClientsHotkey(this._configuration.MinimizeAllClientsHotkeys?.Select(x => this._configuration.StringToKey(x)));
			RegisterToggleAllPreviewsHotkey(this._configuration.ToggleAllPreviewsHotkeys?.Select(x => this._configuration.StringToKey(x)));
			RegisterClickThroughHotkey(this._configuration.ClickThroughHotkeys?.Select(x => this._configuration.StringToKey(x)));

			this.RefreshMouseBindings();
		}

		public void RegisterToggleAllPreviewsHotkey(IEnumerable<Keys> keys)
		{
			foreach (var hotkey in keys)
			{
				if (hotkey == Keys.None)
				{
					continue;
				}

				var newHandler = new HotkeyHandler(default(IntPtr), hotkey);
				newHandler.Pressed += (object s, HandledEventArgs e) =>
				{
					this.ToggleAllPreviews();
					e.Handled = true;
				};

				newHandler.Register();
				this._cycleClientHotkeyHandlers.Add(newHandler);
			}
		}

		public void RegisterClickThroughHotkey(IEnumerable<Keys> keys)
		{
			foreach (var hotkey in keys)
			{
				if (hotkey == Keys.None)
				{
					continue;
				}

				var newHandler = new HotkeyHandler(default(IntPtr), hotkey);
				newHandler.Pressed += (object s, HandledEventArgs e) =>
				{
					this.ToggleClickThrough();
					e.Handled = true;
				};

				newHandler.Register();
				this._cycleClientHotkeyHandlers.Add(newHandler);
			}
		}

		// While the click-through mode is on, the previews (and their overlays) are
		// transparent for the mouse, so anything behind them can be interacted with.
		// The previews are also dimmed so the mode is visually obvious
		private bool _isClickThroughActive;

		private void ToggleClickThrough()
		{
			this._isClickThroughActive = !this._isClickThroughActive;

			foreach (KeyValuePair<IntPtr, IThumbnailView> entry in this._thumbnailViews)
			{
				entry.Value.SetClickThrough(this._isClickThroughActive);
			}

			this.RefreshThumbnails();
		}

		/// <summary>Hides / shows every thumbnail at once without touching the saved settings</summary>
		private void ToggleAllPreviews()
		{
			this._areAllPreviewsHidden = !this._areAllPreviewsHidden;

			if (!this._areAllPreviewsHidden)
			{
				// Client switches made while the previews were hidden are only coarsely
				// tracked, so on unhide the active client is re-synced with the actual
				// foreground window and the highlight is drawn on it right away
				IntPtr foreground = this._windowManager.GetForegroundWindowHandle();
				if ((foreground != IntPtr.Zero) && this._thumbnailViews.TryGetValue(foreground, out IThumbnailView foregroundView))
				{
					this.SwitchActiveClient(foreground, foregroundView.Title);
				}
			}

			this.RefreshThumbnails();
		}

		private void RefreshMouseBindings()
		{
			this._mouseHook.UnregisterAll();

			foreach (CycleGroup group in this._configuration.CycleGroups)
			{
				Dictionary<string, int> cycleOrder = group.ClientsOrder;

				foreach (string binding in group.ForwardHotkeys.Where(MouseBinding.IsMouseBinding))
				{
					this._mouseHook.Register(binding, () => this.CycleNextClient(true, cycleOrder));
				}

				foreach (string binding in group.BackwardHotkeys.Where(MouseBinding.IsMouseBinding))
				{
					this._mouseHook.Register(binding, () => this.CycleNextClient(false, cycleOrder));
				}
			}

			foreach (string binding in (this._configuration.MinimizeAllClientsHotkeys ?? new List<string>()).Where(MouseBinding.IsMouseBinding))
			{
				this._mouseHook.Register(binding, () => this.MinimizeAllClients());
			}

			foreach (string binding in (this._configuration.ToggleAllPreviewsHotkeys ?? new List<string>()).Where(MouseBinding.IsMouseBinding))
			{
				this._mouseHook.Register(binding, () => this.ToggleAllPreviews());
			}

			foreach (string binding in (this._configuration.ClickThroughHotkeys ?? new List<string>()).Where(MouseBinding.IsMouseBinding))
			{
				this._mouseHook.Register(binding, () => this.ToggleClickThrough());
			}

			foreach (KeyValuePair<IntPtr, IThumbnailView> entry in this._thumbnailViews)
			{
				string title = entry.Value.Title;
				string binding = this._configuration.GetClientHotkeyString(title);

				if ((binding != null) && MouseBinding.IsMouseBinding(binding))
				{
					this._mouseHook.Register(binding, () => this.ActivateClientByTitle(title));
				}
			}
		}

		// Names of the cycle groups the client belongs to, rendered on the thumbnail overlay
		private string GetCycleGroupNames(string title)
		{
			if (!this._configuration.ShowCycleGroupName)
			{
				return null;
			}

			return string.Join(", ", this._configuration.CycleGroups.Where(x => x.ClientsOrder.ContainsKey(title)).Select(x => x.Name));
		}

		private void ActivateClientByTitle(string title)
		{
			foreach (KeyValuePair<IntPtr, IThumbnailView> entry in this._thumbnailViews)
			{
				if (entry.Value.Title != title)
				{
					continue;
				}

				this.SetActive(entry);
				return;
			}
		}

		public void UpdateHotkeys()
		{
			if (this._areHotkeysSuspended)
			{
				return;
			}

			// Re-register cycle group / minimize-all hotkeys from the current configuration
			this.UnregisterAllHotkeys();

			this.RegisterConfiguredHotkeys();

			// Re-register per-client hotkeys on the active thumbnail views
			foreach (KeyValuePair<IntPtr, IThumbnailView> entry in this._thumbnailViews)
			{
				entry.Value.RegisterHotkey(this._configuration.GetClientHotkey(entry.Value.Title));
			}
		}

		private void UnregisterAllHotkeys()
		{
			foreach (HotkeyHandler handler in this._cycleClientHotkeyHandlers)
			{
				handler.Dispose();
			}
			this._cycleClientHotkeyHandlers.Clear();

			this._mouseHook.UnregisterAll();

			foreach (KeyValuePair<IntPtr, IThumbnailView> entry in this._thumbnailViews)
			{
				entry.Value.UnregisterHotkey();
			}
		}

		/// <summary>
		/// Releases every registered hotkey so that the hotkey editor can capture
		/// a combination that is already bound to an action
		/// </summary>
		public void SuspendHotkeys()
		{
			if (this._areHotkeysSuspended)
			{
				return;
			}

			this._areHotkeysSuspended = true;
			this.UnregisterAllHotkeys();
		}

		public void ResumeHotkeys()
		{
			if (!this._areHotkeysSuspended)
			{
				return;
			}

			this._areHotkeysSuspended = false;
			this.UpdateHotkeys();
		}

		public IThumbnailView GetClientByTitle(string title)
		{
			return _thumbnailViews.FirstOrDefault(x => x.Value.Title == title).Value;
		}

		public IThumbnailView GetClientByPointer(IntPtr ptr)
		{
			return this._thumbnailViews.TryGetValue(ptr, out IThumbnailView view) ? view : null;
		}

		public IThumbnailView GetActiveClient()
		{
			return GetClientByPointer(this._activeClient.Handle);
		}

		public void SetActive(KeyValuePair<IntPtr, IThumbnailView> newClient)
		{
			// The border is moved BEFORE the window activation: the visual feedback
			// has to be instant, the actual focus switch catches up on the worker task.
			// With the previews hidden there is nothing to draw at all
			if (!this._areAllPreviewsHidden)
			{
				this.GetActiveClient()?.ClearBorder();
				newClient.Value.SetHighlight();
				newClient.Value.Refresh(true);
			}

			this.SwitchActiveClient(newClient.Key, newClient.Value.Title);
			this.QueueClientWindowActivation(newClient.Key, newClient.Value.Title, IntPtr.Zero);
		}

		/// <summary>
		/// Enqueues a client window activation (and optionally a minimization of another window)
		/// for the background worker. Only the latest requested activation is executed - during
		/// rapid cycling the focus jumps straight to the final client instead of walking
		/// through every intermediate one
		/// </summary>
		private void QueueClientWindowActivation(IntPtr activateHandle, string activateTitle, IntPtr minimizeHandle)
		{
			bool startWorker;

			lock (this._activationSyncRoot)
			{
				if (minimizeHandle != IntPtr.Zero)
				{
					this._pendingMinimizeHandles.Add(minimizeHandle);
				}

				if (activateHandle != IntPtr.Zero)
				{
					// The window that is about to be activated must not be minimized
					// by an earlier queued request
					this._pendingMinimizeHandles.Remove(activateHandle);
					this._pendingActivation = (activateHandle, activateTitle);
				}

				startWorker = !this._isActivationWorkerRunning;
				this._isActivationWorkerRunning = true;
			}

			if (startWorker)
			{
				Task.Run(this.ProcessPendingActivations);
			}
		}

		private void ProcessPendingActivations()
		{
			while (true)
			{
				IntPtr[] minimizeHandles;
				(IntPtr Handle, string Title) activation;
				IntPtr wakeHandle = IntPtr.Zero;

				lock (this._activationSyncRoot)
				{
					if ((this._pendingActivation.Handle == IntPtr.Zero)
						&& (this._pendingMinimizeHandles.Count == 0)
						&& (this._pendingWakeHandles.Count == 0))
					{
						this._isActivationWorkerRunning = false;
						return;
					}

					minimizeHandles = this._pendingMinimizeHandles.ToArray();
					this._pendingMinimizeHandles.Clear();

					activation = this._pendingActivation;
					this._pendingActivation = (IntPtr.Zero, null);

					// Wake-ups are background housekeeping: one per iteration and only when
					// no user-driven activation is waiting, so switching stays responsive
					if ((activation.Handle == IntPtr.Zero) && (minimizeHandles.Length == 0) && (this._pendingWakeHandles.Count > 0))
					{
						foreach (IntPtr handle in this._pendingWakeHandles)
						{
							wakeHandle = handle;
							break;
						}

						this._pendingWakeHandles.Remove(wakeHandle);
					}

					this._isUserActivationExecuting = (activation.Handle != IntPtr.Zero) || (minimizeHandles.Length > 0);
				}

				try
				{
					// The activation goes first: minimizing the foreground window vacates the
					// foreground and Windows falls back to whatever is behind it, which shows up
					// as a desktop flash. Raising the new client beforehand leaves no such gap
					if (activation.Handle != IntPtr.Zero)
					{
#if LINUX
						this._windowManager.ActivateWindow(activation.Handle, activation.Title);
#else
						this._windowManager.ActivateWindow(activation.Handle, this._configuration.WindowsAnimationStyle);
#endif

						// Restoring a minimized window is asynchronous, so the new client can
						// still be on its way up. Minimizing the old one right now would
						// uncover the desktop for exactly that moment
						if (minimizeHandles.Length > 0)
						{
							this.WaitForClientWindowActivation(activation.Handle);
						}
					}

					foreach (IntPtr handle in minimizeHandles)
					{
						// The no-activation minimize: a regular one would make Windows
						// activate the next window in the Z order, stealing the focus
						// from the client that was just raised
						this._windowManager.MinimizeWindowWithoutActivation(handle);
					}

					if (wakeHandle != IntPtr.Zero)
					{
						this.WakeMinimizedClient(wakeHandle);
					}
				}
				catch (Exception)
				{
					// A failed window operation (f.e. the window was closed in the meantime)
					// must not kill the worker loop
				}
				finally
				{
					lock (this._activationSyncRoot)
					{
						this._isUserActivationExecuting = false;
					}
				}
			}
		}

		/// <summary>
		/// Lets a minimized client render a couple of fresh frames for its thumbnail:
		/// the window is restored without activation, given a moment to present and then
		/// minimized back. Runs on the worker task
		/// </summary>
		private void WakeMinimizedClient(IntPtr handle)
		{
			const int WAKE_RENDER_TIME = 300;

			// The user could have restored the window while this request sat in the queue
			if (!this._windowManager.IsWindowMinimized(handle))
			{
				return;
			}

			int wakeStartTick = Environment.TickCount;

			this._windowManager.RestoreWindowWithoutActivation(handle);

			System.Threading.Thread.Sleep(WAKE_RENDER_TIME);

			// The window owning the foreground now is honored only when actual user input
			// happened during the wake (the user clicked / switched to it). Some windows
			// activate themselves on restore - those are minimized back regardless
			if ((this._windowManager.GetForegroundWindowHandle() == handle) && ThumbnailManager.WasUserInputAfter(wakeStartTick))
			{
				return;
			}

			// Minimized strictly without activation: SW_MINIMIZE would activate the next
			// window in the Z order, silently stealing the focus from whatever the user
			// is interacting with (f.e. the settings window) on every wake cycle
			this._windowManager.MinimizeWindowWithoutActivation(handle);
		}

		private static bool WasUserInputAfter(int startTick)
		{
			User32NativeMethods.LASTINPUTINFO lastInput = new User32NativeMethods.LASTINPUTINFO
			{
				cbSize = (uint)System.Runtime.InteropServices.Marshal.SizeOf<User32NativeMethods.LASTINPUTINFO>()
			};

			if (!User32NativeMethods.GetLastInputInfo(ref lastInput))
			{
				return false;
			}

			// Tick counts wrap around, so the difference is compared in signed arithmetic
			return unchecked((int)lastInput.dwTime - startTick) >= 0;
		}

		/// <summary>
		/// Waits (on the worker task) until the requested window actually owns the foreground.
		/// Gives up after a short timeout - an activation Windows refuses would block the
		/// worker otherwise
		/// </summary>
		private void WaitForClientWindowActivation(IntPtr handle)
		{
			const int ACTIVATION_TIMEOUT = 200;
			const int ACTIVATION_POLL_INTERVAL = 10;

			for (int elapsed = 0; elapsed < ACTIVATION_TIMEOUT; elapsed += ACTIVATION_POLL_INTERVAL)
			{
				if (this._windowManager.GetForegroundWindowHandle() == handle)
				{
					return;
				}

				System.Threading.Thread.Sleep(ACTIVATION_POLL_INTERVAL);
			}
		}

		// While a queued activation has not been executed yet the real foreground window
		// lags behind this._activeClient, so the poll / WinEvent hook must not treat that
		// stale foreground as a user-made switch. Background wake-ups of minimized clients
		// deliberately do NOT count: they never change the foreground, and blocking the
		// hook for their duration would delay the highlight on real alt+tab switches
		private bool IsActivationInFlight()
		{
			lock (this._activationSyncRoot)
			{
				return this._isUserActivationExecuting
					|| (this._pendingActivation.Handle != IntPtr.Zero)
					|| (this._pendingMinimizeHandles.Count > 0);
			}
		}

		private bool _isUserActivationExecuting;

		public void MinimizeAllClients()
		{
			// Queued to the worker task: minimizing is synchronous with the target window's
			// input thread, doing it inline for every client would freeze the UI thread
			foreach (var x in _thumbnailViews.Reverse())
			{
				this.QueueClientWindowActivation(IntPtr.Zero, null, x.Value.Id);
			}
		}
		public void CycleNextClient(bool isForwards, Dictionary<string, int> cycleOrder)
		{
			IOrderedEnumerable<KeyValuePair<string, int>> clientOrder;
			Dictionary<string, int> _cycleOrder = new Dictionary<string, int>(cycleOrder);

			if ( _cycleOrder.Count == 0 )
			{
				int order = 0;
				foreach( var x in _thumbnailViews )
				{
					// Several clients can share one title: every window still sitting on the
					// login screen is called "EVE". A plain Add would throw on the duplicate
					// (and crash the app); one entry per title is enough - the cycling logic
					// below walks same-titled clients by their window handles
					if (!_cycleOrder.ContainsKey(x.Value.Title))
					{
						_cycleOrder.Add(x.Value.Title, order++);
					}
				}
			}

			if (isForwards)
			{
				clientOrder = _cycleOrder.OrderBy(x => x.Value);
			}
			else
			{
				clientOrder = _cycleOrder.OrderByDescending(x => x.Value);
			}

			bool setNextClient = false;
			IThumbnailView lastClient = null;

			foreach (var t in clientOrder)
			{
				if (t.Key == _activeClient.Title && t.Key != DEFAULT_CLIENT_TITLE)
				{
					setNextClient = true;
					lastClient = _thumbnailViews.FirstOrDefault(x => x.Value.Title == t.Key).Value;
					continue;
				}

				// cycle through login screens ?
				if (t.Key == _activeClient.Title && t.Key == DEFAULT_CLIENT_TITLE)
				{
					lastClient = _thumbnailViews.FirstOrDefault(x => x.Value.Title == t.Key && x.Value.Id == _activeClient.Handle).Value;
					if (lastClient == null)
					{
						setNextClient = true;
						continue;
					}
					var possibleClients = (isForwards ? _thumbnailViews.OrderBy(x => x.Value.Id.ToInt64()) : _thumbnailViews.OrderByDescending(x => x.Value.Id.ToInt64())).Where(x => x.Value.Title == t.Key && ! x.Value.IsExcludedFromCycleGroup);
					foreach (var pc in possibleClients)
					{
						if ( pc.Value.Id.Equals(lastClient.Id) )
						{
							setNextClient = true;
							continue;
						}

						if (!setNextClient)
						{
							continue;
						}

						// this is the next client (at login screen)
						SetActive(pc);
						return;
					}

					// rolled off top of list - back to first (if any there!)
					// set next client ?
					continue;
				}

				if (!setNextClient)
				{
					continue;
				}

				if (_thumbnailViews.Any(x => x.Value.Title == t.Key && !x.Value.IsExcludedFromCycleGroup))
				{
					var ptr = t.Key.Equals(DEFAULT_CLIENT_TITLE) ? 
						(isForwards ? _thumbnailViews.OrderBy(x => x.Value.Id.ToInt64()) : _thumbnailViews.OrderByDescending(x => x.Value.Id.ToInt64())).FirstOrDefault(x => x.Value.Title == t.Key && ! x.Value.IsExcludedFromCycleGroup)
						: _thumbnailViews.First(x => x.Value.Title == t.Key && !x.Value.IsExcludedFromCycleGroup);
					SetActive(ptr);
					return;
				}
			}

			// we didn't get a next one. just get the first one from the start.
			foreach (var t in clientOrder)
			{
				if (_thumbnailViews.Any(x => x.Value.Title == t.Key && !x.Value.IsExcludedFromCycleGroup))
				{
					var ptr = t.Key.Equals(DEFAULT_CLIENT_TITLE) ?
						(isForwards ? _thumbnailViews.OrderBy(x => x.Value.Id.ToInt64()) : _thumbnailViews.OrderByDescending(x => x.Value.Id.ToInt64())).FirstOrDefault(x => x.Value.Title == t.Key && !x.Value.IsExcludedFromCycleGroup)
						: _thumbnailViews.First(x => x.Value.Title == t.Key && !x.Value.IsExcludedFromCycleGroup);
					SetActive(ptr);
					_activeClient = (ptr.Key, t.Key);
					return;
				}
			}

			// unable to select anything !
			return;
		}

		public void RegisterCycleClientHotkey(IEnumerable<Keys> keys, bool isForwards, Dictionary<string, int> cycleOrder)
		{
			foreach (var hotkey in keys)
			{
				if (hotkey == Keys.None)
				{
					continue;
				}

				var newHandler = new HotkeyHandler(default(IntPtr), hotkey);
				newHandler.Pressed += (object s, HandledEventArgs e) =>
				{
					this.CycleNextClient(isForwards, cycleOrder);
					e.Handled = true;
				};

				newHandler.Register();
				this._cycleClientHotkeyHandlers.Add(newHandler);
			}
		}
		public void RegisterMinimizeAllClientsHotkey(IEnumerable<Keys> keys)
		{
			foreach (var hotkey in keys)
			{
				if (hotkey == Keys.None)
				{
					continue;
				}

				var newHandler = new HotkeyHandler(default(IntPtr), hotkey);
				newHandler.Pressed += (object s, HandledEventArgs e) =>
				{
					this.MinimizeAllClients();
					e.Handled = true;
				};

				newHandler.Register();
				this._cycleClientHotkeyHandlers.Add(newHandler);
			}
		}

		public void Start()
		{
			this._thumbnailUpdateTimer.Start();

			// The refresh timer only polls the foreground window every ThumbnailRefreshPeriod ms,
			// which makes the active client highlight lag on Alt+Tab / direct window clicks.
			// A WinEvent hook delivers the foreground change instantly instead
			// (WINEVENT_OUTOFCONTEXT: the callback is posted to this thread's message loop)
			if (this._foregroundHook == IntPtr.Zero)
			{
				this._foregroundHookCallback = this.ForegroundWindowChangedHook;
				this._foregroundHook = User32NativeMethods.SetWinEventHook(
					User32NativeMethods.EVENT_SYSTEM_FOREGROUND, User32NativeMethods.EVENT_SYSTEM_FOREGROUND,
					IntPtr.Zero, this._foregroundHookCallback, 0, 0, User32NativeMethods.WINEVENT_OUTOFCONTEXT);
			}

			this.RefreshThumbnails();
		}

		public void Stop()
		{
			this._thumbnailUpdateTimer.Stop();

			if (this._foregroundHook != IntPtr.Zero)
			{
				User32NativeMethods.UnhookWinEvent(this._foregroundHook);
				this._foregroundHook = IntPtr.Zero;
				this._foregroundHookCallback = null;
			}
		}

		private void ForegroundWindowChangedHook(IntPtr hook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint eventThread, uint eventTime)
		{
			if (idObject != User32NativeMethods.OBJID_WINDOW)
			{
				return;
			}

			this.RefreshActiveClientHighlight(hwnd);
		}

		// A lightweight subset of RefreshThumbnails: only tracks the active client change
		// and moves the highlight border, everything else stays on the timer cadence
		private void RefreshActiveClientHighlight(IntPtr foregroundWindowHandle)
		{
			// Hidden previews are a fully blocking state: nothing to draw, and the active
			// client is re-synced with the foreground window when the previews are unhidden
			if (this._areAllPreviewsHidden)
			{
				return;
			}

			if ((foregroundWindowHandle == IntPtr.Zero) || (foregroundWindowHandle == this._activeClient.Handle))
			{
				return;
			}

			// Foreground changes caused by a queued activation still in flight are not
			// user-made switches and must not move the highlight around
			if (this.IsActivationInFlight())
			{
				return;
			}

			// Only direct client window activations are of interest here: thumbnail clicks and
			// hotkeys go through SetActive, and transient windows (f.e. the Alt+Tab task switcher
			// itself) must not be recorded as the active client or the external application
			if (!this._thumbnailViews.TryGetValue(foregroundWindowHandle, out IThumbnailView newActiveView))
			{
				return;
			}

			IntPtr previousActiveHandle = this._activeClient.Handle;

			this.SwitchActiveClient(foregroundWindowHandle, newActiveView.Title);

			if (this._thumbnailViews.TryGetValue(previousActiveHandle, out IThumbnailView previousActiveView))
			{
				previousActiveView.SetHighlight(false, this._configuration.ActiveClientHighlightThickness);
				previousActiveView.Refresh(false);
			}

			newActiveView.SetHighlight(this._configuration.EnableActiveClientHighlight, this._configuration.ActiveClientHighlightThickness);
			newActiveView.Refresh(false);
		}

		private void ThumbnailUpdateTimerTick(object sender, EventArgs e)
		{
			this.UpdateThumbnailsList();
			this.RefreshThumbnails();
		}

		// Single-flight guard for the background process scan: touched only on the UI thread
		private bool _isProcessScanRunning;

		private async void UpdateThumbnailsList()
		{
			// The process/window scan (Process.GetProcesses + EnumWindows) is the heaviest
			// part of the refresh cycle. It runs on a worker task so the UI thread only
			// applies the results; a scan still in progress just skips this tick
			if (this._isProcessScanRunning)
			{
				return;
			}

			this._isProcessScanRunning = true;

			ICollection<IProcessInfo> addedProcesses = null;
			ICollection<IProcessInfo> updatedProcesses = null;
			ICollection<IProcessInfo> removedProcesses = null;

			try
			{
				await Task.Run(() =>
				{
					this._processMonitor.GetUpdatedProcesses(out ICollection<IProcessInfo> added, out ICollection<IProcessInfo> updated, out ICollection<IProcessInfo> removed);

					addedProcesses = added;
					updatedProcesses = updated;
					removedProcesses = removed;
				});
			}
			catch (Exception)
			{
				// A failed scan is not fatal - the next tick retries
				return;
			}
			finally
			{
				this._isProcessScanRunning = false;
			}

			List<string> viewsAdded = new List<string>();
			List<string> viewsRemoved = new List<string>();

			foreach (IProcessInfo process in addedProcesses)
			{
				Size initialSize = this._configuration.ThumbnailSize;
				if (this._configuration.PerClientThumbnailSize.Any(x => x.Key == process.Title))
				{
					initialSize = this._configuration.PerClientThumbnailSize[process.Title];
				}

				IThumbnailView view = this._thumbnailViewFactory.Create(process.Handle, process.Title, this._configuration.ThumbnailSize);
				view.IsOverlayEnabled = this._configuration.ShowThumbnailOverlays;
				view.IsExcludedFromCycleGroup = false;
				view.SetFrames(this._configuration.ShowThumbnailFrames);
				// Max/Min size limitations should be set AFTER the frames are disabled
				// Otherwise thumbnail window will be unnecessary resized
				view.SetSizeLimitations(this._configuration.ThumbnailMinimumSize, this._configuration.ThumbnailMaximumSize);
				view.SetTopMost(this._configuration.ShowThumbnailsAlwaysOnTop);

				// Clients without a character name (login / loading screen) share a single
				// remembered position: LoginThumbnailLocation is the fallback for the very first run
				view.ThumbnailLocation = this.IsManageableThumbnail(view)
											? this._configuration.GetThumbnailLocation(view.Title, this._activeClient.Title, view.ThumbnailLocation)
											: this._configuration.GetThumbnailLocation(view.Title, this._activeClient.Title, this._configuration.LoginThumbnailLocation);

				this._thumbnailViews.Add(view.Id, view);

				view.ThumbnailResized = this.ThumbnailViewResized;
				view.ThumbnailMoved = this.ThumbnailViewMoved;
				view.ThumbnailFocused = this.ThumbnailViewFocused;
				view.ThumbnailLostFocus = this.ThumbnailViewLostFocus;
				view.ThumbnailActivated = this.ThumbnailActivated;
				view.ThumbnailDeactivated = this.ThumbnailDeactivated;

				view.ThumbnailToggleCycleGroup = this.ThumbnailToggleCycleGroup;

				view.RegisterHotkey(this._configuration.GetClientHotkey(view.Title));

				if (this._isClickThroughActive)
				{
					view.SetClickThrough(true);
				}

				this.ApplyClientLayout(view);
				this.ApplyCaptionBar(view);

				// TODO Add extension filter here later
				if (view.Title != ThumbnailManager.DEFAULT_CLIENT_TITLE)
				{
					viewsAdded.Add(view.Title);
				}
			}

			foreach (IProcessInfo process in updatedProcesses)
			{
				this._thumbnailViews.TryGetValue(process.Handle, out IThumbnailView view);

				if (view == null)
				{
					// Something went terribly wrong
					continue;
				}

				if (process.Title != view.Title) // update thumbnail title
				{
					viewsRemoved.Add(view.Title);
					view.Title = process.Title;
					viewsAdded.Add(view.Title);

					view.RegisterHotkey(this._configuration.GetClientHotkey(process.Title));

					this.ApplyClientLayout(view);
					this.ApplyCaptionBar(view);
				}
			}

			foreach (IProcessInfo process in removedProcesses)
			{
				this._minimizedClientWakeTimestamps.Remove(process.Handle);

				if (!this._thumbnailViews.TryGetValue(process.Handle, out IThumbnailView view))
				{
					continue;
				}

				this._thumbnailViews.Remove(view.Id);
				if (view.Title != ThumbnailManager.DEFAULT_CLIENT_TITLE)
				{
					viewsRemoved.Add(view.Title);
				}

				view.UnregisterHotkey();

				view.ThumbnailResized = null;
				view.ThumbnailMoved = null;
				view.ThumbnailFocused = null;
				view.ThumbnailLostFocus = null;
				view.ThumbnailActivated = null;
				view.ThumbnailToggleCycleGroup = null;

				view.Close();
			}

			if ((viewsAdded.Count > 0) || (viewsRemoved.Count > 0))
			{
				this.RefreshMouseBindings();
				await this._mediator.Publish(new ThumbnailListUpdated(viewsAdded, viewsRemoved));
			}
		}

		private void RefreshThumbnails()
		{
			// Pick up refresh period changes made in the settings UI without a restart
			if (this._thumbnailUpdateTimer.Interval.TotalMilliseconds != this._configuration.ThumbnailRefreshPeriod)
			{
				this._thumbnailUpdateTimer.Interval = TimeSpan.FromMilliseconds(this._configuration.ThumbnailRefreshPeriod);
			}

			// TODO Split this method
			IntPtr foregroundWindowHandle = this._windowManager.GetForegroundWindowHandle();

			// The foreground window can be NULL in certain circumstances, such as when a window is losing activation.
			// It is safer to just skip this refresh round than to do something while the system state is undefined
			if (foregroundWindowHandle == IntPtr.Zero)
			{
				return;
			}

			string foregroundWindowTitle = null;

			// Check if the foreground window handle is one of the known handles for client windows or their thumbnails
			bool isClientWindow = this.IsClientWindowActive(foregroundWindowHandle);
			bool isMainWindowActive = this.IsMainWindowActive(foregroundWindowHandle);

			if (foregroundWindowHandle == this._activeClient.Handle)
			{
				foregroundWindowTitle = this._activeClient.Title;
			}
			else if (this._thumbnailViews.TryGetValue(foregroundWindowHandle, out IThumbnailView foregroundView))
			{
				// This code will work only on Alt+Tab switch between clients
				foregroundWindowTitle = foregroundView.Title;
			}
			else if (!isClientWindow)
			{
				this._externalApplication = foregroundWindowHandle;
			}

			// No need to minimize EVE clients when switching out to non-EVE window (like thumbnail)
			// While a queued activation is still in flight the foreground window is stale
			// and must not override the just-selected active client
			if (!string.IsNullOrEmpty(foregroundWindowTitle) && !this.IsActivationInFlight())
			{
				this.SwitchActiveClient(foregroundWindowHandle, foregroundWindowTitle);
			}

			bool hideAllThumbnails = this._areAllPreviewsHidden
									|| (this._configuration.HideThumbnailsOnLostFocus && !(isClientWindow || isMainWindowActive));

			// Wait for some time before hiding all previews
			// (the manual toggle takes effect immediately though)
			if (hideAllThumbnails && !this._areAllPreviewsHidden)
			{
				this._hideThumbnailsDelay--;
				if (this._hideThumbnailsDelay > 0)
				{
					hideAllThumbnails = false; // Postpone the 'hide all' operation
				}
				else
				{
					this._hideThumbnailsDelay = 0; // Stop the counter
				}
			}
			else
			{
				this._hideThumbnailsDelay = this._configuration.HideThumbnailsDelay; // Reset the counter
			}

			this._refreshCycleCount++;

			bool forceRefresh;
			if (this._refreshCycleCount >= ThumbnailManager.FORCED_REFRESH_CYCLE_THRESHOLD)
			{
				this._refreshCycleCount = 0;
				forceRefresh = true;
			}
			else
			{
				forceRefresh = false;
			}

			this.DisableViewEvents();

			// Snap thumbnail
			// No need to update Thumbnails while one of them is highlighted
			if ((!this._isHoverEffectActive) && this.TryDequeueLocationChange(out var locationChange))
			{
				if ((locationChange.ActiveClient == this._activeClient.Title) && this._thumbnailViews.TryGetValue(locationChange.Handle, out var view))
				{
					this.SnapThumbnailView(view);

					this.RaiseThumbnailLocationUpdatedNotification(view.Title);
				}
				else
				{
					this.RaiseThumbnailLocationUpdatedNotification(locationChange.Title);
				}
			}

			// Hide, show, resize and move - update ZoomAnchor setting
			foreach (KeyValuePair<IntPtr, IThumbnailView> entry in this._thumbnailViews)
			{
				IThumbnailView view = entry.Value;
				// update ZoomAnchor regardless
				view.ClientZoomAnchor = this._configuration.GetZoomAnchor(view.Title, this._configuration.ThumbnailZoomAnchor);


				if (hideAllThumbnails || this._configuration.IsThumbnailDisabled(view.Title))
				{
					if (view.IsActive)
					{
						view.Hide();
					}
					continue;
				}

				if (this._configuration.HideActiveClientThumbnail && (view.Id == this._activeClient.Handle))
				{
					if (view.IsActive)
					{
						view.Hide();
					}
					continue;
				}

				if (this._configuration.HideLoginClientThumbnail && (view.Title == DEFAULT_CLIENT_TITLE ))
				{
					if (view.IsActive)
					{
						view.Hide();
					}
					continue;
				}

				// No need to update Thumbnails while one of them is highlighted
				if (!this._isHoverEffectActive)
				{
					// Do not even move thumbnails with default caption
					if (this.IsManageableThumbnail(view))
					{
						view.ThumbnailLocation = this._configuration.GetThumbnailLocation(view.Title, this._activeClient.Title, view.ThumbnailLocation);

						// In the fill-cell mode the size is dictated by the grid cell
						// (minus the padding on both sides), not by the size settings;
						// the size limits are lifted so the cell size always wins
						if (this.IsGridCellFillActive())
						{
							view.SetSizeLimitations(new Size(10, 10), Size.Empty);
							view.ThumbnailSize = this.GetGridCellFillSize();
						}
						else
						{
							view.SetSizeLimitations(this._configuration.ThumbnailMinimumSize, this._configuration.ThumbnailMaximumSize);
							view.ThumbnailSize = this._configuration.GetThumbnailSize(view.Title, this._activeClient.Title, view.ThumbnailSize);
						}
					}

					// Click-through mode dims the previews so its being active is obvious
					view.SetOpacity(this._isClickThroughActive ? this._configuration.ThumbnailOpacity * 0.6 : this._configuration.ThumbnailOpacity);
					view.SetTopMost(this._configuration.ShowThumbnailsAlwaysOnTop);
				}

				view.IsOverlayEnabled = this._configuration.ShowThumbnailOverlays;
				view.SetCycleGroupName(this.GetCycleGroupNames(view.Title));

				view.SetHighlight(
					this._configuration.EnableActiveClientHighlight && (view.Id == this._activeClient.Handle), 
					this._configuration.ActiveClientHighlightThickness);

				if (!view.IsActive)
				{
					view.Show();
				}
				else
				{
					view.Refresh(forceRefresh);
				}
			}

			this.EnableViewEvents();

			this.EnqueueDueMinimizedClientWakes(hideAllThumbnails);
		}

		/// <summary>
		/// Schedules a wake-up for every minimized client whose thumbnail got stale.
		/// The countdown starts when a client is first seen minimized, so each of them
		/// is refreshed every MinimizedClientsRefreshPeriod seconds while it stays down
		/// </summary>
		private void EnqueueDueMinimizedClientWakes(bool areThumbnailsHidden)
		{
			int period = this._configuration.MinimizedClientsRefreshPeriod;

			// Nothing to refresh when the feature is off or no thumbnails are on the screen.
			// The feature follows the minimize-inactive setting - same dependency as in the GUI
			if (!this._configuration.EnableMinimizedClientsRefresh
				|| !this._configuration.MinimizeInactiveClients
				|| (period <= 0)
				|| areThumbnailsHidden)
			{
				return;
			}

			long now = Environment.TickCount64;

			foreach (KeyValuePair<IntPtr, IThumbnailView> entry in this._thumbnailViews)
			{
				if (!this._windowManager.IsWindowMinimized(entry.Key))
				{
					// The window renders on its own while it is not minimized
					this._minimizedClientWakeTimestamps.Remove(entry.Key);
					continue;
				}

				if (this._configuration.IsThumbnailDisabled(entry.Value.Title))
				{
					continue;
				}

				if (!this._minimizedClientWakeTimestamps.TryGetValue(entry.Key, out long lastWake))
				{
					// Just minimized: the thumbnail still shows the latest frame
					this._minimizedClientWakeTimestamps[entry.Key] = now;
					continue;
				}

				if (now - lastWake < period * 1000L)
				{
					continue;
				}

				this._minimizedClientWakeTimestamps[entry.Key] = now;
				this.QueueMinimizedClientWake(entry.Key);
			}
		}

		private void QueueMinimizedClientWake(IntPtr handle)
		{
			bool startWorker;

			lock (this._activationSyncRoot)
			{
				this._pendingWakeHandles.Add(handle);

				startWorker = !this._isActivationWorkerRunning;
				this._isActivationWorkerRunning = true;
			}

			if (startWorker)
			{
				Task.Run(this.ProcessPendingActivations);
			}
		}

		public void UpdateThumbnailsSize()
		{
			this.SetThumbnailsSize(this._configuration.ThumbnailSize);
		}
		public void UpdateCycleGroupIndicator()
		{
			this.SetCycleGroupIndicator(this._configuration.CycleGroupIndicatorAnchor);
		}

		private void SetCycleGroupIndicator(ZoomAnchor anchor)
		{
			this.DisableViewEvents();

			foreach (KeyValuePair<IntPtr, IThumbnailView> entry in this._thumbnailViews)
			{
				entry.Value.SetCycleGroupIndicator(entry.Value.IsExcludedFromCycleGroup, anchor);
				entry.Value.Refresh(false);
			}

			this.EnableViewEvents();
		}

		private void SetThumbnailsSize(Size size)
		{
			this.DisableViewEvents();

			foreach (KeyValuePair<IntPtr, IThumbnailView> entry in this._thumbnailViews)
			{
				entry.Value.ThumbnailSize = size;
				entry.Value.Refresh(false);
			}

			this.EnableViewEvents();
		}

		public void UpdateThumbnailFrames()
		{
			this.DisableViewEvents();

			foreach (KeyValuePair<IntPtr, IThumbnailView> entry in this._thumbnailViews)
			{
				entry.Value.SetFrames(this._configuration.ShowThumbnailFrames);
				ApplyCaptionBar(entry.Value);
				entry.Value.SetPreventPreviews();

				// The per-client border color is cached, refresh it in case the settings changed
				entry.Value.SetDefaultBorderColor();
			}

			this.EnableViewEvents();
		}

		private void EnableViewEvents()
		{
			this._ignoreViewEvents = false;
		}

		private void DisableViewEvents()
		{
			this._ignoreViewEvents = true;
		}

		private void SwitchActiveClient(IntPtr foregroundClientHandle, string foregroundClientTitle)
		{
			// Check if any actions are needed
			if (this._activeClient.Handle == foregroundClientHandle)
			{
				return;
			}

			// Minimize the currently active client if needed. Both operations are handed to
			// the worker task, which raises the new client before minimizing this one
			if (this._configuration.MinimizeInactiveClients
				&& (this._activeClient.Handle != IntPtr.Zero)
				&& !this._configuration.IsPriorityClient(this._activeClient.Title))
			{
				this.QueueClientWindowActivation(foregroundClientHandle, foregroundClientTitle, this._activeClient.Handle);
			}

			this._activeClient = (foregroundClientHandle, foregroundClientTitle);
		}

		private void ThumbnailViewFocused(IntPtr id)
		{
			if (this._isHoverEffectActive)
			{
				return;
			}

			this._isHoverEffectActive = true;

			IThumbnailView view = this._thumbnailViews[id];

			view.SetTopMost(true);
			view.SetOpacity(1.0);

			if (this._configuration.ThumbnailZoomEnabled && ! view.IsPreventPreviews() )
			{
				// The expanded window must never be covered by the neighboring previews
				view.BringAboveOtherThumbnails();
				this.ThumbnailZoomIn(view);
			}
		}

		private void ThumbnailViewLostFocus(IntPtr id)
		{
			if (!this._isHoverEffectActive)
			{
				return;
			}

			IThumbnailView view = this._thumbnailViews[id];

			if (this._configuration.ThumbnailZoomEnabled)
			{
				this.ThumbnailZoomOut(view);
			}

			view.SetOpacity(this._configuration.ThumbnailOpacity);

			this._isHoverEffectActive = false;
		}

		private void ThumbnailActivated(IntPtr id)
		{
			IThumbnailView view = this._thumbnailViews[id];

			this.SwitchActiveClient(view.Id, view.Title);
			this.QueueClientWindowActivation(view.Id, view.Title, IntPtr.Zero);

			this.UpdateClientLayouts();
			this.RefreshThumbnails();
		}

		private void ThumbnailDeactivated(IntPtr id, bool switchOut)
		{
			if (switchOut)
			{
				this.QueueClientWindowActivation(this._externalApplication, null, IntPtr.Zero);
			}
			else
			{
				if (!this._thumbnailViews.TryGetValue(id, out IThumbnailView view))
				{
					return;
				}

				this._windowManager.MinimizeWindow(view.Id, this._configuration.WindowsAnimationStyle, true);
				this.RefreshThumbnails();
			}
		}

		private void ThumbnailToggleCycleGroup(IntPtr id)
		{
			var view = GetClientByPointer(id);
			if ( view != null )
			{
				view.IsExcludedFromCycleGroup = !view.IsExcludedFromCycleGroup;
				view.SetCycleGroupIndicator(view.IsExcludedFromCycleGroup, _configuration.CycleGroupIndicatorAnchor);

			}
			this.RefreshThumbnails();
		}


		// True when the snapped previews must occupy their whole grid cell:
		// the preview size is then locked to the cell size and cannot be edited
		private bool IsGridCellFillActive()
		{
			return this._configuration.ThumbnailSnapToGrid && this._configuration.ThumbnailSnapToGridFillCell;
		}

		private Size GetGridCellFillSize()
		{
			int padding = this._configuration.ThumbnailSnapToGridCellPadding;

			return new Size(
				Math.Max(10, this._configuration.ThumbnailSnapToGridSizeX - 2 * padding),
				Math.Max(10, this._configuration.ThumbnailSnapToGridSizeY - 2 * padding));
		}

		private async void ThumbnailViewResized(IntPtr id)
		{
			if (this._ignoreViewEvents)
			{
				return;
			}

			IThumbnailView view = this._thumbnailViews[id];

			// In the fill-cell mode the size is locked to the grid cell: a resize must
			// neither spread to the other previews nor overwrite the size settings
			if (this.IsGridCellFillActive())
			{
				view.Refresh(false);
				return;
			}

			this.SetThumbnailsSize(view.ThumbnailSize);

			view.Refresh(false);

			await this._mediator.Publish(new ThumbnailActiveSizeUpdated(view.ThumbnailSize));
		}

		private void ThumbnailViewMoved(IntPtr id)
		{
			if (this._ignoreViewEvents)
			{
				return;
			}

			IThumbnailView view = this._thumbnailViews[id];
			view.Refresh(false);
			this.EnqueueLocationChange(view);
		}

		// Checks whether currently active window belongs to an EVE client or its thumbnail
		private bool IsClientWindowActive(IntPtr windowHandle)
		{
			if (windowHandle == IntPtr.Zero)
			{
				return false;
			}

			foreach (KeyValuePair<IntPtr, IThumbnailView> entry in this._thumbnailViews)
			{
				IThumbnailView view = entry.Value;

				if (view.IsKnownHandle(windowHandle))
				{
					return true;
				}
			}

			return false;
		}

		// Check whether the currently active window belongs to EVE-O-Preview itself
		private bool IsMainWindowActive(IntPtr windowHandle)
		{
			return (this._processMonitor.GetMainProcess().Handle == windowHandle);
		}

		private void ThumbnailZoomIn(IThumbnailView view)
		{
			this.DisableViewEvents();

			view.ZoomIn(ViewZoomAnchorConverter.Convert(view.ClientZoomAnchor), this._configuration.ThumbnailZoomFactor);
			view.Refresh(false);

			this.EnableViewEvents();
		}

		private void ThumbnailZoomOut(IThumbnailView view)
		{
			this.DisableViewEvents();

			view.ZoomOut();
			view.Refresh(false);

			this.EnableViewEvents();
		}

		private void SnapThumbnailView(IThumbnailView view)
		{
			// Check if this feature is enabled
			if (!this._configuration.EnableThumbnailSnap)
			{
				return;
			}

			// Only borderless thumbnails can be docked
			if (this._configuration.ShowThumbnailFrames)
			{
				return;
			}

			int width = this._configuration.ThumbnailSize.Width;
			int height = this._configuration.ThumbnailSize.Height;

			// TODO Extract method
			int baseX = view.ThumbnailLocation.X;
			int baseY = view.ThumbnailLocation.Y;

			Point[] viewPoints = { new Point(baseX, baseY), new Point(baseX + width, baseY), new Point(baseX, baseY + height), new Point(baseX + width, baseY + height) };

			// TODO Extract constants
			int thresholdX = Math.Max(20, width / 10);
			int thresholdY = Math.Max(20, height / 10);

			foreach (var entry in this._thumbnailViews)
			{
				IThumbnailView testView = entry.Value;

				if (view.Id == testView.Id)
				{
					continue;
				}

				int testX = testView.ThumbnailLocation.X;
				int testY = testView.ThumbnailLocation.Y;

				Point[] testPoints = { new Point(testX, testY), new Point(testX + width, testY), new Point(testX, testY + height), new Point(testX + width, testY + height) };

				var delta = ThumbnailManager.TestViewPoints(viewPoints, testPoints, thresholdX, thresholdY);

				if ((delta.X == 0) && (delta.Y == 0))
				{
					continue;
				}

				view.ThumbnailLocation = new Point(view.ThumbnailLocation.X + delta.X, view.ThumbnailLocation.Y + delta.Y);
				this._configuration.SetThumbnailLocation(view.Title, this._activeClient.Title, view.ThumbnailLocation);
				break;
			}
		}

		private static (int X, int Y) TestViewPoints(Point[] viewPoints, Point[] testPoints, int thresholdX, int thresholdY)
		{
			// Point combinations that we need to check
			// No need to check all 4x4 combinations
			(int ViewOffset, int TestOffset)[] testOffsets =
								{   ( 0, 3 ), ( 0, 2 ), ( 1, 2 ),
									( 0, 1 ), ( 0, 0 ), ( 1, 0 ),
									( 2, 1 ), ( 2, 0 ), ( 3, 0 )};

			foreach (var testOffset in testOffsets)
			{
				Point viewPoint = viewPoints[testOffset.ViewOffset];
				Point testPoint = testPoints[testOffset.TestOffset];

				int deltaX = testPoint.X - viewPoint.X;
				int deltaY = testPoint.Y - viewPoint.Y;

				if ((Math.Abs(deltaX) <= thresholdX) && (Math.Abs(deltaY) <= thresholdY))
				{
					return (deltaX, deltaY);
				}
			}

			return (0, 0);
		}
		private bool SetWindowStyle(IThumbnailView view, UInt32 styleToChange, bool remove)
		{
			IntPtr handle = view.Id;
			uint style = User32NativeMethods.GetWindowLong(handle, InteropConstants.GWL_STYLE);
			if (((style & styleToChange) == styleToChange) && remove == true)
			{
				style = style & ~styleToChange;
				User32NativeMethods.SetWindowLong(handle, InteropConstants.GWL_STYLE, style);
				return true;
			}
			if (((style & styleToChange) != styleToChange) && remove == false)
			{
				style = style | styleToChange;
				User32NativeMethods.SetWindowLong(handle, InteropConstants.GWL_STYLE, style);
				return true;
			}
			return false;
		}
		private void ApplyCaptionBar(IThumbnailView view)

		{
			if (view.Title == ThumbnailManager.DEFAULT_CLIENT_TITLE) return;
			IntPtr handle = view.Id;

			bool enable = this._configuration.HideCaptionOnClients;
			bool changed = false;
			changed = changed | SetWindowStyle(view, InteropConstants.WS_CAPTION, enable);
			changed = changed | SetWindowStyle(view, InteropConstants.WS_THICKFRAME, enable);
		}
		private void ApplyClientLayout(IThumbnailView view)
		{
			IntPtr clientHandle = view.Id;
			string clientTitle = view.Title;

			if (!this._configuration.EnableClientLayoutTracking)
			{
				return;
			}

			// No need to apply layout for not yet logged-in clients
			if (clientTitle == ThumbnailManager.DEFAULT_CLIENT_TITLE)
			{
				return;
			}

			ClientLayout clientLayout = this._configuration.GetClientLayout(clientTitle);

			if (clientLayout == null)
			{
				return;
			}

			if (clientLayout.IsMaximized)
			{
				this._windowManager.MaximizeWindow(clientHandle);
			}
			else
			{
				this._windowManager.MoveWindow(clientHandle, clientLayout.X, clientLayout.Y, clientLayout.Width, clientLayout.Height);
			}
		}

		private void UpdateClientLayouts()
		{
			if (!this._configuration.EnableClientLayoutTracking)
			{
				return;
			}

			foreach (KeyValuePair<IntPtr, IThumbnailView> entry in this._thumbnailViews)
			{
				IThumbnailView view = entry.Value;

				// No need to save layout for not yet logged-in clients
				if (view.Title == ThumbnailManager.DEFAULT_CLIENT_TITLE)
				{
					continue;
				}

				(int Left, int Top, int Right, int Bottom) position = this._windowManager.GetWindowPosition(view.Id);
				int width = Math.Abs(position.Right - position.Left);
				int height = Math.Abs(position.Bottom - position.Top);

				var isMaximized = this._windowManager.IsWindowMaximized(view.Id);

				if (!(isMaximized || this.IsValidWindowPosition(position.Left, position.Top, width, height)))
				{
					continue;
				}

				this._configuration.SetClientLayout(view.Title, new ClientLayout(position.Left, position.Top, width, height, isMaximized));
			}
		}

		private void EnqueueLocationChange(IThumbnailView view)
		{
			string activeClientTitle = this._activeClient.Title;
			// TODO ??
			this._configuration.SetThumbnailLocation(view.Title, activeClientTitle, view.ThumbnailLocation);

			// Keep the login screen position setting in sync with the actual window
			if (!this.IsManageableThumbnail(view))
			{
				this._configuration.LoginThumbnailLocation = view.ThumbnailLocation;
			}

			lock (this._locationChangeNotificationSyncRoot)
			{
				if (this._enqueuedLocationChangeNotification.Handle == IntPtr.Zero)
				{
					this._enqueuedLocationChangeNotification = (view.Id, view.Title, activeClientTitle, view.ThumbnailLocation, ThumbnailManager.DEFAULT_LOCATION_CHANGE_NOTIFICATION_DELAY);
					return;
				}

				// Reset the delay and exit
				if ((this._enqueuedLocationChangeNotification.Handle == view.Id) &&
					(this._enqueuedLocationChangeNotification.ActiveClient == activeClientTitle))
				{
					this._enqueuedLocationChangeNotification.Delay = ThumbnailManager.DEFAULT_LOCATION_CHANGE_NOTIFICATION_DELAY;
					return;
				}

				this.RaiseThumbnailLocationUpdatedNotification(this._enqueuedLocationChangeNotification.Title);
				this._enqueuedLocationChangeNotification = (view.Id, view.Title, activeClientTitle, view.ThumbnailLocation, ThumbnailManager.DEFAULT_LOCATION_CHANGE_NOTIFICATION_DELAY);
			}
		}

		private bool TryDequeueLocationChange(out (IntPtr Handle, string Title, string ActiveClient, Point Location) change)
		{
			lock (this._locationChangeNotificationSyncRoot)
			{
				change = (IntPtr.Zero, null, null, Point.Empty);

				if (this._enqueuedLocationChangeNotification.Handle == IntPtr.Zero)
				{
					return false;
				}

				this._enqueuedLocationChangeNotification.Delay--;

				if (this._enqueuedLocationChangeNotification.Delay > 0)
				{
					return false;
				}

				change = (this._enqueuedLocationChangeNotification.Handle, this._enqueuedLocationChangeNotification.Title, this._enqueuedLocationChangeNotification.ActiveClient, this._enqueuedLocationChangeNotification.Location);
				this._enqueuedLocationChangeNotification = (IntPtr.Zero, null, null, Point.Empty, -1);

				return true;
			}
		}

		private async void RaiseThumbnailLocationUpdatedNotification(string title)
		{
			// The login screen thumbnail has no character name but its position is persisted too
			if (string.IsNullOrEmpty(title))
			{
				return;
			}

			await this._mediator.Send(new SaveConfiguration());
		}

		// We shouldn't manage some thumbnails (like thumbnail of the EVE client sitting on the login screen)
		// TODO Move to a service (?)
		private bool IsManageableThumbnail(IThumbnailView view)
		{
			return view.Title != ThumbnailManager.DEFAULT_CLIENT_TITLE;
		}

		// Quick sanity check that the window is not minimized
		private bool IsValidWindowPosition(int left, int top, int width, int height)
		{
			return (left > ThumbnailManager.WINDOW_POSITION_THRESHOLD_LOW) && (left < ThumbnailManager.WINDOW_POSITION_THRESHOLD_HIGH)
					&& (top > ThumbnailManager.WINDOW_POSITION_THRESHOLD_LOW) && (top < ThumbnailManager.WINDOW_POSITION_THRESHOLD_HIGH)
					&& (width > ThumbnailManager.WINDOW_SIZE_THRESHOLD) && (height > ThumbnailManager.WINDOW_SIZE_THRESHOLD);
		}
	}
}