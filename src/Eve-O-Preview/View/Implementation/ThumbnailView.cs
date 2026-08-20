using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using EveOPreview.Configuration;
using EveOPreview.Services;
using EveOPreview.Services.Interop;
using EveOPreview.UI.Hotkeys;

namespace EveOPreview.View
{
	public abstract partial class ThumbnailView : Form, IThumbnailView
	{
		#region Private constants
		private const double OPACITY_THRESHOLD = 0.9;
		private const double OPACITY_EPSILON = 0.1;
		#endregion

		#region Private fields
		private ThumbnailOverlay _overlay;

		// Part of the logic (namely current size / position management)
		// was moved to the view due to the performance reasons
		private bool _isOverlayVisible;
		private bool _isTopMost;
		private bool _isHighlightEnabled;
		private bool _isHighlightRequested;
		private int _highlightWidth;

		private bool _isLocationChanged;
		private bool _isSizeChanged;

		private bool _isCustomMouseModeActive;

		private double _opacity;
		 
		private DateTime _suppressResizeEventsTimestamp;
		private Size _baseZoomSize;
		private Point _baseZoomLocation;
		private Point _baseMousePosition;
		private Size _baseZoomMaximumSize;

		private HotkeyHandler _hotkeyHandler;

		private IThumbnailConfiguration _config;
		private Lazy<Color> _myBorderColor;
		private Lazy<Color> _preventPreviewColor;
		private Lazy<bool> _preventPreviews;
		private IThumbnailManager _thumbnailManager;
		#endregion

		protected ThumbnailView(IWindowManager windowManager, IThumbnailConfiguration config, IThumbnailManager thumbnailManager)
		{
			this._config = config;
			this.SuppressResizeEvent();

			this.WindowManager = windowManager;

			this.IsActive = false;

			this.IsOverlayEnabled = false;
			this._isOverlayVisible = false;
			this.IsExcludedFromCycleGroup = false;

			this._isTopMost = false;
			this._isHighlightEnabled = false;
			this._isHighlightRequested = false;

			this._isLocationChanged = true;
			this._isSizeChanged = true;

			this._isCustomMouseModeActive = false;

			this._opacity = 0.1;

			InitializeComponent();

			this.RefreshPreviewSettings();
			SetDefaultBorderColor();
			SetPreventPreviews();
			this._thumbnailManager = thumbnailManager;
		}

		// The overlay window is only created when it has something to display and
		// is released as soon as it is not needed, so a disabled overlay costs no memory
		private ThumbnailOverlay Overlay
		{
			get
			{
				if (this._overlay == null)
				{
					this._overlay = new ThumbnailOverlay(this,
						this.MouseEnter_Handler,
						this.MouseLeave_Handler,
						this.MouseDown_Handler,
						this.MouseUp_Handler,
						this.MouseMove_Handler
						);

					this.ApplyOverlayState();
				}

				return this._overlay;
			}
		}

		// Deliberately not tied to the current window visibility: the overlay window is
		// hidden and shown along with the thumbnail, it is only unloaded when the whole
		// overlay feature is switched off
		private bool IsOverlayRequired => this.IsOverlayEnabled || this.IsPreventPreviews();

		private void ApplyOverlayState()
		{
			if (this._overlay == null)
			{
				return;
			}

			string title = this.Text ?? string.Empty;

			// The overlay is created lazily, so it has to catch up with the current
			// topmost state. Otherwise placing the thumbnail below a non-topmost overlay
			// would strip the topmost flag off the thumbnail itself
			this._overlay.TopMost = this._isTopMost;

			this._overlay.SetOverlayLabel(title.Replace("EVE - ", "").Replace("EVE Frontier - ", "*"));
			this._overlay.SetPropertiesOverlayLabel(this._settings.OverlayLabelFont, this._settings.OverlayLabelColor.Value, this._settings.OverlayLabelAnchor.Value);
			this._overlay.SetOverlayLabelOutline(this._settings.OverlayLabelOutlineEnabled.Value, this._settings.OverlayLabelOutlineThickness.Value, this._settings.OverlayLabelOutlineColor.Value);
			this._overlay.SetCycleGroupNameOutline(this._settings.CycleGroupNameOutlineEnabled.Value, this._settings.CycleGroupNameOutlineThickness.Value, this._settings.CycleGroupNameOutlineColor.Value);

			// A lazily created overlay has to catch up with the click-through mode
			if (this._isClickThrough)
			{
				ThumbnailView.ApplyClickThroughStyle(this._overlay.Handle, true);
			}
			this._overlay.EnableFakePreview(this._preventPreviews.Value, false, 0, SystemColors.Control);
			this._overlay.SetCycleGroupIndicator(this.IsExcludedFromCycleGroup, this._settings.CycleGroupIndicatorAnchor.Value);

			// The group name might have been cached before the overlay existed
			// (SetCycleGroupName is called earlier in the refresh cycle than the lazy
			// overlay creation), so the freshly created overlay has to catch up with it
			if (!string.IsNullOrEmpty(this._cycleGroupName))
			{
				this._overlay.SetCycleGroupName(this._cycleGroupName, this._cycleGroupNameAnchor, this._cycleGroupNameFont, this._cycleGroupNameColor);
				this._cycleGroupNameLayoutSize = this._overlay.Size;
			}
		}

		private void ReleaseOverlay()
		{
			if (this._overlay == null)
			{
				return;
			}

			ThumbnailOverlay overlay = this._overlay;
			this._overlay = null;
			this._isOverlayVisible = false;

			// A re-created overlay starts empty, so the cached label state is stale
			this._cycleGroupName = null;
			this._cycleGroupNameFont = null;
			this._cycleGroupNameLayoutSize = Size.Empty;

			overlay.Hide();
			overlay.Close();
			overlay.Dispose();
		}

		public IWindowManager WindowManager { get; }

		public IntPtr Id { get; set; }

		public string Title
		{
			get => this.Text;
			set
			{
				this.Text = value;
				this.RefreshPreviewSettings();
				SetDefaultBorderColor();
				SetPreventPreviews();
				this.ApplyOverlayState();
			}
		}

		public bool IsActive { get; set; }

		public bool IsOverlayEnabled { get; set; }
		public bool IsExcludedFromCycleGroup { get; set; }
		public ZoomAnchor ClientZoomAnchor { get; set; }

		public Point ThumbnailLocation
		{
			get => this.Location;
			set
			{
				this.StartPosition = FormStartPosition.Manual;
				this.Location = value;
			}
		}

		public Size ThumbnailSize
		{
			get => this.ClientSize;
			set => this.ClientSize = value;
		}

		public Action<IntPtr> ThumbnailResized { get; set; }

		public Action<IntPtr> ThumbnailMoved { get; set; }

		public Action<IntPtr> ThumbnailFocused { get; set; }

		public Action<IntPtr> ThumbnailLostFocus { get; set; }

		public Action<IntPtr> ThumbnailActivated { get; set; }

		public Action<IntPtr, bool> ThumbnailDeactivated { get; set; }
		public Action<IntPtr> ThumbnailToggleCycleGroup { get; set; }

		private bool WindowMoved = false;

		public void SetDefaultBorderColor()
		{
			this._myBorderColor = new Lazy<Color>(() => this._settings.ActiveClientHighlightColor.Value);
		}

		public bool IsPreventPreviews()
		{
			return this._preventPreviews.Value;
		}
		public void SetPreventPreviews()
		{
			this._preventPreviews = new Lazy<bool>(() => this._settings.PreventPreviews.Value);

			this._preventPreviewColor = new Lazy<Color>(() => this._settings.PreventPreviewColor.Value);
		}

		public new void Show()
		{
			this.SuppressResizeEvent();

			base.Show();

			this._isLocationChanged = true;
			this._isSizeChanged = true;
			this._isOverlayVisible = false;

			this.Refresh(true);

			this.IsActive = true;
		}

		public new void Hide()
		{
			this.SuppressResizeEvent();

			this.IsActive = false;

			this._isOverlayVisible = false;
			this._overlay?.Hide();
			this._aggroFrame?.Clear();
			base.Hide();
		}

		public new virtual void Close()
		{
			this.SuppressResizeEvent();

			this.IsActive = false;
			this.ReleaseOverlay();
			this.ReleaseAggroFrame();
			base.Close();
		}

		// This method is used to determine if the provided Handle is related to client or its thumbnail
		public bool IsKnownHandle(IntPtr handle)
		{
			return (this.Id == handle) || (this.Handle == handle) || ((this._overlay != null) && (this._overlay.Handle == handle));
		}

		public void SetSizeLimitations(Size minimumSize, Size maximumSize)
		{
			this.MinimumSize = minimumSize;
			this.MaximumSize = maximumSize;
		}

		public void SetOpacity(double opacity)
		{
			if (opacity >= OPACITY_THRESHOLD)
			{
				opacity = 1.0;
			}

			if (Math.Abs(opacity - this._opacity) < OPACITY_EPSILON)
			{
				return;
			}

			try
			{
				this.Opacity = opacity;

				// Overlay opacity settings
				// Of the thumbnail's opacity is almost full then set the overlay's one to
				// full. Otherwise set it to half of the thumbnail opacity
				// Opacity value is stored even if the overlay is not displayed atm
				if (this._overlay != null) { this._overlay.Opacity = opacity > 0.8 ? 1.0 : 1.0 - (1.0 - opacity) / 2; }

				this._opacity = opacity;
			}
			catch (Win32Exception)
			{
				// Something went wrong in WinForms internals
				// Opacity will be updated in the next cycle
			}
		}

		/// <summary>
		/// Preview settings of this particular client with every value filled in. Cached
		/// so that the paint paths do not resolve them over and over; refreshed on a title
		/// change and once per refresh cycle
		/// </summary>
		private PreviewSettings _settings;

		public void RefreshPreviewSettings()
		{
			this._settings = this._config.ResolvePreviewSettings(this.Title);
		}

		public void SetFrames(bool enable)
		{
			FormBorderStyle style = enable ? FormBorderStyle.SizableToolWindow : FormBorderStyle.None;

			// No need to change the borders style if it is ALREADY correct
			if (this.FormBorderStyle == style)
			{
				return;
			}

			this.SuppressResizeEvent();

			this.FormBorderStyle = style;
		}
		public void SetOverlayLabel()
		{
		}
		public void SetCycleGroupIndicator(bool displayCycleGroup, ZoomAnchor anchor)
		{
			this._overlay?.SetCycleGroupIndicator(displayCycleGroup, anchor);
		}

		public void SetCycleGroupName(string groupName)
		{
			// Called on every refresh cycle: the overlay relayouts the label (incl. text
			// measurement) on each call, so unchanged values are filtered out here
			if ((this._overlay != null)
				&& (this._cycleGroupName == groupName)
				&& (this._cycleGroupNameFont == this._settings.CycleGroupNameFont)
				&& (this._cycleGroupNameColor == this._settings.CycleGroupNameColor.Value)
				&& (this._cycleGroupNameAnchor == this._settings.CycleGroupIndicatorAnchor.Value))
			{
				return;
			}

			this._cycleGroupName = groupName;
			this._cycleGroupNameFont = this._settings.CycleGroupNameFont;
			this._cycleGroupNameColor = this._settings.CycleGroupNameColor.Value;
			this._cycleGroupNameAnchor = this._settings.CycleGroupIndicatorAnchor.Value;

			this._overlay?.SetCycleGroupName(groupName, this._cycleGroupNameAnchor, this._cycleGroupNameFont, this._cycleGroupNameColor);
			this._cycleGroupNameLayoutSize = this._overlay?.Size ?? Size.Empty;
		}
		private string _cycleGroupName;
		private Font _cycleGroupNameFont;
		private Color _cycleGroupNameColor;
		private ZoomAnchor _cycleGroupNameAnchor;

		// Overlay size the group name label was last laid out for: the layout is cached,
		// but a resized overlay (f.e. the hover zoom) invalidates the label position
		private Size _cycleGroupNameLayoutSize;

		// Click-through mode: the preview and its overlay stop receiving mouse input,
		// so the user can interact with whatever is behind them
		private bool _isClickThrough;

		public void SetClickThrough(bool enable)
		{
			this._isClickThrough = enable;

			ThumbnailView.ApplyClickThroughStyle(this.Handle, enable);

			if (this._overlay != null)
			{
				ThumbnailView.ApplyClickThroughStyle(this._overlay.Handle, enable);
			}
		}

		private static void ApplyClickThroughStyle(IntPtr handle, bool enable)
		{
			uint exStyle = User32NativeMethods.GetWindowLong(handle, InteropConstants.GWL_EXSTYLE);

			// WS_EX_TRANSPARENT makes hit testing skip the window entirely; it is only
			// honored reliably for layered windows, so WS_EX_LAYERED is set along with it
			uint newExStyle = enable
				? exStyle | InteropConstants.WS_EX_LAYERED | InteropConstants.WS_EX_TRANSPARENT
				: exStyle & ~InteropConstants.WS_EX_TRANSPARENT;

			if (newExStyle != exStyle)
			{
				User32NativeMethods.SetWindowLong(handle, InteropConstants.GWL_EXSTYLE, newExStyle);
			}
		}

		public void SetTopMost(bool enableTopmost)
		{
			if (this._isTopMost == enableTopmost)
			{
				return;
			}

			if (this._overlay != null) { this._overlay.TopMost = enableTopmost; }
			this.TopMost = enableTopmost;

			this._isTopMost = enableTopmost;

			this.RaiseOverlayAboveThumbnail();
		}

		/// <summary>
		/// Raises this thumbnail (and its overlay) above every other thumbnail window.
		/// Used when the thumbnail zooms on hover so the expanded window is never
		/// covered by the neighboring previews. The overlay is raised first and the
		/// thumbnail is placed directly below it, preserving the overlay-on-top invariant
		/// </summary>
		public void BringAboveOtherThumbnails()
		{
			IntPtr insertAfter = this._isTopMost ? User32NativeMethods.HWND_TOPMOST : User32NativeMethods.HWND_TOP;

			if ((this._overlay != null) && this._isOverlayVisible)
			{
				User32NativeMethods.SetWindowPos(this._overlay.Handle, insertAfter, 0, 0, 0, 0,
					User32NativeMethods.SWP_NOMOVE | User32NativeMethods.SWP_NOSIZE | User32NativeMethods.SWP_NOACTIVATE);
				User32NativeMethods.SetWindowPos(this.Handle, this._overlay.Handle, 0, 0, 0, 0,
					User32NativeMethods.SWP_NOMOVE | User32NativeMethods.SWP_NOSIZE | User32NativeMethods.SWP_NOACTIVATE);
			}
			else
			{
				User32NativeMethods.SetWindowPos(this.Handle, insertAfter, 0, 0, 0, 0,
					User32NativeMethods.SWP_NOMOVE | User32NativeMethods.SWP_NOSIZE | User32NativeMethods.SWP_NOACTIVATE);
			}
		}

		/// <summary>
		/// Keeps the overlay window above its thumbnail. Hovering or activating the
		/// thumbnail raises it in the z-order, which would otherwise cover the overlay
		/// </summary>
		private void RaiseOverlayAboveThumbnail()
		{
			if ((this._overlay == null) || !this._settings.OverlayAlwaysOnTop.Value || !this._isOverlayVisible)
			{
				return;
			}

			// Skip the SetWindowPos call when the thumbnail already sits right below its overlay
			if (User32NativeMethods.GetWindow(this._overlay.Handle, User32NativeMethods.GW_HWNDNEXT) == this.Handle)
			{
				return;
			}

			// Both windows must share the topmost state: inserting a window after one
			// from the other z-order band makes Windows change the window's topmost flag
			if (this._overlay.TopMost != this._isTopMost)
			{
				this._overlay.TopMost = this._isTopMost;
			}

			// hWndInsertAfter names the window that ends up ABOVE the positioned one,
			// so the thumbnail is placed directly below its overlay
			User32NativeMethods.SetWindowPos(this.Handle, this._overlay.Handle, 0, 0, 0, 0,
				User32NativeMethods.SWP_NOMOVE | User32NativeMethods.SWP_NOSIZE | User32NativeMethods.SWP_NOACTIVATE);
		}

		public void SetHighlight()
		{
			SetHighlight(this._settings.EnableActiveClientHighlight.Value, this._settings.ActiveClientHighlightThickness.Value);
		}

		public void SetHighlight(bool enabled, int width)
		{
			// Color and thickness are re-read on every call so that settings changes
			// are reflected on already highlighted thumbnails right away
			Color borderColor = enabled ? this._myBorderColor.Value : Color.Black;

			if ((this._isHighlightRequested == enabled) && (this._highlightWidth == width) && (this.BackColor == borderColor))
			{
				return;
			}

			this._isHighlightRequested = enabled;
			this._highlightWidth = enabled ? width : this._highlightWidth;
			this.BackColor = borderColor;

			this._isSizeChanged = true;
		}

		public void ClearBorder()
		{
			this.SetHighlight(false, 0);
			this.Refresh(true);
		}

		// The flashing yellow/red 'aggro' frame driven by the game log monitor.
		// The window is created lazily on the first alert and reused afterwards
		private AggroFrameView _aggroFrame;

		public void SetAggroFrame(AggroLevel level)
		{
			if ((level == AggroLevel.None) || !this.Visible)
			{
				this._aggroFrame?.Clear();
				return;
			}

			if (this._aggroFrame == null)
			{
				this._aggroFrame = new AggroFrameView();
			}

			Color color = level == AggroLevel.Red ? this._config.AggroRedColor : this._config.AggroYellowColor;
			this._aggroFrame.SetState(level, color, this._config.AggroFillPercent, new Rectangle(this.GetOverlayLocation(), this.ClientSize));
		}

		private void ReleaseAggroFrame()
		{
			if (this._aggroFrame == null)
			{
				return;
			}

			AggroFrameView frame = this._aggroFrame;
			this._aggroFrame = null;

			frame.Clear();
			frame.Close();
			frame.Dispose();
		}

		// Screen position of the thumbnail client area - the region the text overlay
		// and the aggro frame both cover
		private Point GetOverlayLocation()
		{
			Point location = this.Location;

			int borderWidth = (this.Size.Width - this.ClientSize.Width) / 2;
			location.X += borderWidth;
			location.Y += (this.Size.Height - this.ClientSize.Height) - borderWidth;

			return location;
		}

		// Set while the thumbnail is expanded by the hover zoom. Used to reset the zoom
		// as soon as the cursor leaves the ORIGINAL (small) thumbnail bounds instead of
		// keeping it up while the cursor wanders around the expanded window
		private bool _isZoomedByHover;

		public void ZoomIn(ViewZoomAnchor anchor, int zoomFactor)
		{
			this._isZoomedByHover = true;
			int oldWidth = this._baseZoomSize.Width;
			int oldHeight = this._baseZoomSize.Height;

			int locationX = this.Location.X;
			int locationY = this.Location.Y;

			int clientSizeWidth = this.ClientSize.Width;
			int clientSizeHeight = this.ClientSize.Height;
			int newWidth = (zoomFactor * clientSizeWidth) + (this.Size.Width - clientSizeWidth);
			int newHeight = (zoomFactor * clientSizeHeight) + (this.Size.Height - clientSizeHeight);

			// The zoomed window must stay on the monitor the thumbnail is on,
			// so its size is capped by the size of that monitor
			Rectangle screenBounds = Screen.FromPoint(new Point(locationX + oldWidth / 2, locationY + oldHeight / 2)).Bounds;
			newWidth = Math.Min(newWidth, screenBounds.Width);
			newHeight = Math.Min(newHeight, screenBounds.Height);

			// First change size, THEN move the window
			// Otherwise there is a chance to fail in a loop
			// Zoom required -> Moved the windows 1st -> Focus is lost -> Window is moved back -> Focus is back on -> Zoom required -> ...
			this.MaximumSize = new Size(0, 0);
			this.Size = new Size(newWidth, newHeight);

			switch (anchor)
			{
				case ViewZoomAnchor.NW:
					break;
				case ViewZoomAnchor.N:
					this.Location = new Point(locationX - newWidth / 2 + oldWidth / 2, locationY);
					break;
				case ViewZoomAnchor.NE:
					this.Location = new Point(locationX - newWidth + oldWidth, locationY);
					break;

				case ViewZoomAnchor.W:
					this.Location = new Point(locationX, locationY - newHeight / 2 + oldHeight / 2);
					break;
				case ViewZoomAnchor.C:
					this.Location = new Point(locationX - newWidth / 2 + oldWidth / 2, locationY - newHeight / 2 + oldHeight / 2);
					break;
				case ViewZoomAnchor.E:
					this.Location = new Point(locationX - newWidth + oldWidth, locationY - newHeight / 2 + oldHeight / 2);
					break;

				case ViewZoomAnchor.SW:
					this.Location = new Point(locationX, locationY - newHeight + this._baseZoomSize.Height);
					break;
				case ViewZoomAnchor.S:
					this.Location = new Point(locationX - newWidth / 2 + oldWidth / 2, locationY - newHeight + oldHeight);
					break;
				case ViewZoomAnchor.SE:
					this.Location = new Point(locationX - newWidth + oldWidth, locationY - newHeight + oldHeight);
					break;
			}

			// Whatever position the anchor produced, the zoomed window is pushed back
			// inside the monitor bounds instead of expanding off-screen
			int clampedX = Math.Max(screenBounds.Left, Math.Min(this.Location.X, screenBounds.Right - newWidth));
			int clampedY = Math.Max(screenBounds.Top, Math.Min(this.Location.Y, screenBounds.Bottom - newHeight));

			if ((clampedX != this.Location.X) || (clampedY != this.Location.Y))
			{
				this.Location = new Point(clampedX, clampedY);
			}
		}

		public void ZoomOut()
		{
			this.RestoreWindowSizeAndLocation();
		}

		public void RegisterHotkey(Keys hotkey)
		{
			if (this._hotkeyHandler != null)
			{
				this.UnregisterHotkey();
			}

			if (hotkey == Keys.None)
			{
				return;
			}

			this._hotkeyHandler = new HotkeyHandler(this.Handle, hotkey);
			this._hotkeyHandler.Pressed += HotkeyPressed_Handler;
			this._hotkeyHandler.Register();
		}

		public void UnregisterHotkey()
		{
			if (this._hotkeyHandler == null)
			{
				return;
			}

			this._hotkeyHandler.Unregister();
			this._hotkeyHandler.Pressed -= HotkeyPressed_Handler;
			this._hotkeyHandler.Dispose();
			this._hotkeyHandler = null;
		}

		public void Refresh(bool forceRefresh)
		{
			this.RefreshThumbnail(forceRefresh);
			this.HighlightThumbnail(forceRefresh || this._isSizeChanged);
			this.RefreshOverlay(forceRefresh || this._isSizeChanged || this._isLocationChanged);

			this._isSizeChanged = false;

			// Hovering or activating a thumbnail raises it above the overlay,
			// so the overlay is put back on top on every refresh
			this.RaiseOverlayAboveThumbnail();
		}

		protected abstract void RefreshThumbnail(bool forceRefresh);

		protected abstract void ResizeThumbnail(int baseWidth, int baseHeight, int highlightWidthTop, int highlightWidthRight, int highlightWidthBottom, int highlightWidthLeft);

		private void HighlightThumbnail(bool forceRefresh)
		{
			if (!forceRefresh && (this._isHighlightRequested == this._isHighlightEnabled))
			{
				// Nothing to do here
				return;
			}

			bool stateChanged = this._isHighlightEnabled != this._isHighlightRequested;
			this._isHighlightEnabled = this._isHighlightRequested;

			int baseWidth = this.ClientSize.Width;
			int baseHeight = this.ClientSize.Height;

			if (!this._isHighlightRequested)
			{
				//No highlighting enabled, so no math required
				this.ResizeThumbnail(baseWidth, baseHeight, 0, 0, 0, 0);
				this._overlay?.EnableFakePreview(this._preventPreviews.Value, false, 0, this._preventPreviewColor.Value);
				this.ForceRepaintOnHighlightChange(stateChanged);
				return;
			}

			double baseAspectRatio = ((double)baseWidth) / baseHeight;

			int actualHeight = baseHeight - 2 * this._highlightWidth;
			double desiredWidth = actualHeight * baseAspectRatio;
			int actualWidth = (int)Math.Round(desiredWidth, MidpointRounding.AwayFromZero);
			int highlightWidthLeft = (baseWidth - actualWidth) / 2;
			int highlightWidthRight = baseWidth - actualWidth - highlightWidthLeft;

			this._overlay?.EnableFakePreview(this._preventPreviews.Value, true, this._highlightWidth, this._preventPreviewColor.Value);
			this.ResizeThumbnail(this.ClientSize.Width, this.ClientSize.Height, this._highlightWidth, highlightWidthRight, this._highlightWidth, highlightWidthLeft);
			this.ForceRepaintOnHighlightChange(stateChanged);
		}

		// The border is just the form background revealed by the shrunken DWM thumbnail,
		// and background painting relies on WM_PAINT - the lowest-priority message.
		// During rapid switching (wheel cycling through the low-level mouse hook, held-down
		// hotkeys) the message queue never gets empty, so the queued repaints would all
		// collapse into a single one drawn only after the input stops. Painting is forced
		// synchronously instead, so every intermediate client shows its border immediately
		private void ForceRepaintOnHighlightChange(bool stateChanged)
		{
			if (stateChanged)
			{
				this.Update();
			}
		}

		private void RefreshOverlay(bool forceRefresh)
		{
			if (this._isOverlayVisible && !forceRefresh)
			{
				// No need to update anything. Everything is already set up
				return;
			}

			// A disabled overlay is unloaded rather than just hidden
			if (!this.IsOverlayRequired)
			{
				this.ReleaseOverlay();
				return;
			}

			ThumbnailOverlay overlay = this.Overlay;

			// Only show overlay if enabled AND thumbnail is active/visible.
			overlay.EnableOverlayLabel(this.IsOverlayEnabled && this.Visible && this._settings.ShowClientName.Value);

			// The overlay strictly follows its thumbnail: a forced refresh of a hidden
			// thumbnail (f.e. a highlight update while all previews are toggled off)
			// must never resurrect the overlay window on an empty spot
			if (!this._isOverlayVisible && this.Visible && !_config.IsThumbnailDisabled(this.Title))
			{
				// One-time action to show the Overlay before it is set up
				// Otherwise its position won't be set
				overlay.Show();
				this._isOverlayVisible = true;
			}

			Size overlaySize = this.ClientSize;
			Point overlayLocation = this.Location;

			int borderWidth = (this.Size.Width - this.ClientSize.Width) / 2;
			overlayLocation.X += borderWidth;
			overlayLocation.Y += (this.Size.Height - this.ClientSize.Height) - borderWidth;

			this._isLocationChanged = false;
			overlay.Size = overlaySize;

			overlay.SetPropertiesOverlayLabel(this._settings.OverlayLabelFont, this._settings.OverlayLabelColor.Value, this._settings.OverlayLabelAnchor.Value);
			overlay.SetOverlayLabelOutline(this._settings.OverlayLabelOutlineEnabled.Value, this._settings.OverlayLabelOutlineThickness.Value, this._settings.OverlayLabelOutlineColor.Value);
			overlay.SetCycleGroupNameOutline(this._settings.CycleGroupNameOutlineEnabled.Value, this._settings.CycleGroupNameOutlineThickness.Value, this._settings.CycleGroupNameOutlineColor.Value);

			// The group name label layout is cached for a specific overlay size (text
			// measurement is not free), so a resized overlay (f.e. the hover zoom)
			// has to lay it out again - same as the window name label right above
			if (!string.IsNullOrEmpty(this._cycleGroupName) && (this._cycleGroupNameLayoutSize != overlaySize))
			{
				overlay.SetCycleGroupName(this._cycleGroupName, this._cycleGroupNameAnchor, this._cycleGroupNameFont, this._cycleGroupNameColor);
				this._cycleGroupNameLayoutSize = overlaySize;
			}

			overlay.Location = overlayLocation;
			overlay.Refresh();

			this.RaiseOverlayAboveThumbnail();
		}

		private void SuppressResizeEvent()
		{
			// Workaround for WinForms issue with the Resize event being fired with inconsistent ClientSize value
			// Any Resize events fired before this timestamp will be ignored
			this._suppressResizeEventsTimestamp = DateTime.UtcNow.AddMilliseconds(_config.ThumbnailResizeTimeoutPeriod);
		}

		#region GUI events
		protected override CreateParams CreateParams
		{
			get
			{
				var Params = base.CreateParams;
				Params.ExStyle |= (int)InteropConstants.WS_EX_TOOLWINDOW;
				return Params;
			}
		}

		private void Move_Handler(object sender, EventArgs e)
		{
			this._isLocationChanged = true;
			this.ThumbnailMoved?.Invoke(this.Id);
		}

		private void Resize_Handler(object sender, EventArgs e)
		{
			if (DateTime.UtcNow < this._suppressResizeEventsTimestamp)
			{
				return;
			}

			this._isSizeChanged = true;

			this.ThumbnailResized?.Invoke(this.Id);
		}

		private void MouseEnter_Handler(object sender, EventArgs e)
		{
			this.ExitCustomMouseMode();
			this.SaveWindowSizeAndLocation();

			this.ThumbnailFocused?.Invoke(this.Id);
		}

		private void MouseLeave_Handler(object sender, EventArgs e)
		{
			this.ThumbnailLostFocus?.Invoke(this.Id);
		}

		private void MouseDown_Handler(object sender, MouseEventArgs e)
		{
			this.MouseDownEventHandler(e.Button, Control.ModifierKeys);
		}

		private void MouseMove_Handler(object sender, MouseEventArgs e)
		{
			if (this._isCustomMouseModeActive)
			{
				this.ProcessCustomMouseMode(e.Button.HasFlag(MouseButtons.Left), e.Button.HasFlag(MouseButtons.Right));
				return;
			}

			// The zoom is reset as soon as the cursor leaves the area the SMALL thumbnail
			// occupied, not the whole expanded window
			if (this._isZoomedByHover && !new Rectangle(this._baseZoomLocation, this._baseZoomSize).Contains(Control.MousePosition))
			{
				this.ThumbnailLostFocus?.Invoke(this.Id);
			}
		}

		private void MouseUp_Handler(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				this.ExitCustomMouseMode();

				// Snap to Grid on release of mouse (if moved)
				if (_config.ThumbnailSnapToGrid && this.WindowMoved)
				{
					// The grid can be shifted relative to the (0, 0) screen origin
					int offsetX = _config.ThumbnailSnapToGridOffsetX;
					int offsetY = _config.ThumbnailSnapToGridOffsetY;

					// The preview is placed INSIDE the cell, padded away from the grid
					// lines instead of covering them with its top/left corner
					int padding = _config.ThumbnailSnapToGridCellPadding;

					var x = (int)Math.Round((double)(this.Location.X - offsetX) / (double)_config.ThumbnailSnapToGridSizeX) * _config.ThumbnailSnapToGridSizeX + offsetX + padding;
					var y = (int)Math.Round((double)(this.Location.Y - offsetY) / (double)_config.ThumbnailSnapToGridSizeY) * _config.ThumbnailSnapToGridSizeY + offsetY + padding;
					this.Location = new Point(x, y);
					this._baseZoomLocation = this.Location;

					// The snapped preview can be stretched over the whole grid cell
					// (minus the padding on both sides, so the lines stay visible)
					if (_config.ThumbnailSnapToGridFillCell)
					{
						Size cellSize = new Size(
							Math.Max(10, _config.ThumbnailSnapToGridSizeX - 2 * padding),
							Math.Max(10, _config.ThumbnailSnapToGridSizeY - 2 * padding));

						if (this.Size != cellSize)
						{
							this.MaximumSize = new Size(0, 0);
							this.Size = cellSize;
						}

						this._baseZoomSize = this.Size;
					}

					this.WindowMoved = false;

                }
			}
		}

		private void HotkeyPressed_Handler(object sender, HandledEventArgs e)
		{
			// Same immediate border redraw as the thumbnail click path (MouseDownEventHandler):
			// without the explicit Refresh the border would wait for the async activation
			// to complete and the next full refresh pass.
			// A hidden thumbnail (f.e. all previews toggled off) draws nothing at all
			var oldWindow = this._thumbnailManager.GetActiveClient();
			this.ThumbnailActivated?.Invoke(this.Id);

			if (this.IsActive)
			{
				this.SetHighlight();
				this.Refresh(true);

				if (!object.ReferenceEquals(oldWindow, this))
				{
					oldWindow?.ClearBorder();
				}
			}

			e.Handled = true;
		}
		#endregion

		#region Custom Mouse mode
		// This pair of methods saves/restores certain window properties
		// Methods are used to remove the 'Zoom' effect (if any) when the
		// custom resize/move mode is activated
		// Methods are kept on this level because moving to the presenter
		// the code that responds to the mouse events like movement
		// seems like a huge overkill
		private void SaveWindowSizeAndLocation()
		{
			this._baseZoomSize = this.Size;
			this._baseZoomLocation = this.Location;
			this._baseZoomMaximumSize = this.MaximumSize;
		}

		private void RestoreWindowSizeAndLocation()
		{
			this._isZoomedByHover = false;

			this.Size = this._baseZoomSize;
			this.MaximumSize = this._baseZoomMaximumSize;
			this.Location = this._baseZoomLocation;
		}

		private void EnterCustomMouseMode()
		{
			this.RestoreWindowSizeAndLocation();

			this._isCustomMouseModeActive = true;
			this._baseMousePosition = Control.MousePosition;
		}

		private void ProcessCustomMouseMode(bool leftButton, bool rightButton)
		{
			Point mousePosition = Control.MousePosition;
			int offsetX = mousePosition.X - this._baseMousePosition.X;
			int offsetY = mousePosition.Y - this._baseMousePosition.Y;
			this._baseMousePosition = mousePosition;

			if (!_config.LockThumbnailLocation)
			{
                // Left + Right buttons trigger thumbnail resize
                // Right button only trigger thumbnail movement
                if (leftButton && rightButton)
                {
                    this.Size = new Size(this.Size.Width + offsetX, this.Size.Height + offsetY);
                    this._baseZoomSize = this.Size;
                }
                else
                {
                    this.Location = new Point(this.Location.X + offsetX, this.Location.Y + offsetY);
                    this._baseZoomLocation = this.Location;
					this.WindowMoved = true;
                }
            }
		}

		private void ExitCustomMouseMode()
		{
			this._isCustomMouseModeActive = false;
		}
		#endregion

		#region Custom GUI events
		protected virtual void MouseDownEventHandler(MouseButtons mouseButtons, Keys modifierKeys)
		{
			switch (mouseButtons)
			{
				case MouseButtons.Left when modifierKeys == Keys.Control:
					this.ThumbnailDeactivated?.Invoke(this.Id, false);
					break;
				case MouseButtons.Left when modifierKeys == Keys.Shift:
					this.ThumbnailToggleCycleGroup?.Invoke(this.Id);
					break;
				case MouseButtons.Left when modifierKeys == (Keys.Control | Keys.Shift):
					this.ThumbnailDeactivated?.Invoke(this.Id, true);
					break;
				case MouseButtons.Left:
					var oldWindow = this._thumbnailManager.GetActiveClient();
					this.ThumbnailActivated?.Invoke(this.Id);
					this.SetHighlight();
					this.Refresh(true);

					if (!object.ReferenceEquals(oldWindow, this))
					{
						oldWindow?.ClearBorder();
					}
					break;
				case MouseButtons.Right:
				case MouseButtons.Left | MouseButtons.Right:
					this.EnterCustomMouseMode();
					break;
			}
		}
		#endregion
	}
}