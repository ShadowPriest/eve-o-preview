using EveOPreview.Configuration;
using EveOPreview.Services.Interop;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace EveOPreview.Services.Implementation
{
	sealed class ProcessMonitor : IProcessMonitor
	{
		#region Private constants
		private const string DEFAULT_PROCESS_NAME = "ExeFile";
		private const string CURRENT_PROCESS_NAME = "EVE-O-Preview";
		#endregion

		#region Private fields
		private readonly IDictionary<IntPtr, ClientWindow> _processCache;
		private IProcessInfo _currentProcessInfo;
		private readonly IThumbnailConfiguration _configuration;
		#endregion

		public ProcessMonitor(IThumbnailConfiguration configuration)
		{
			this._processCache = new Dictionary<IntPtr, ClientWindow>(512);
			this._configuration = configuration;

			// This field cannot be initialized properly in constructor
			// At the moment this code is executed the main application window is not yet initialized
			this._currentProcessInfo = new ProcessInfo(IntPtr.Zero, "");
		}

		private bool IsMonitoredProcess(string processName)
		{
			// This is a possible extension point
			return _configuration.IsExecutableToPreview(processName);
		}

		private IProcessInfo GetCurrentProcessInfo()
		{
			var currentProcess = Process.GetCurrentProcess();
			return new ProcessInfo(currentProcess.MainWindowHandle, currentProcess.MainWindowTitle);
		}

		public IProcessInfo GetMainProcess()
		{
			if (this._currentProcessInfo.Handle == IntPtr.Zero)
			{
				var processInfo = this.GetCurrentProcessInfo();

				// Are we initialized yet?
				if (processInfo.Title != "")
				{
					this._currentProcessInfo = processInfo;
				}
			}

			return this._currentProcessInfo;
		}

		public ICollection<IProcessInfo> GetAllProcesses()
		{
			ICollection<IProcessInfo> result = new List<IProcessInfo>(this._processCache.Count);

			// TODO Lock list here just in case
			foreach (KeyValuePair<IntPtr, ClientWindow> entry in this._processCache)
			{
				result.Add(ProcessMonitor.ToProcessInfo(entry.Key, entry.Value));
			}

			return result;
		}

		public void GetUpdatedProcesses(out ICollection<IProcessInfo> addedProcesses, out ICollection<IProcessInfo> updatedProcesses, out ICollection<IProcessInfo> removedProcesses)
		{
			addedProcesses = new List<IProcessInfo>(16);
			updatedProcesses = new List<IProcessInfo>(16);
			removedProcesses = new List<IProcessInfo>(16);

			IList<IntPtr> knownProcesses = new List<IntPtr>(this._processCache.Keys);

			foreach (ClientWindow window in this.GetClientWindows())
			{
				if (!this._processCache.TryGetValue(window.Handle, out ClientWindow cachedWindow))
				{
					// This is a new client window
					this._processCache.Add(window.Handle, window);
					addedProcesses.Add(ProcessMonitor.ToProcessInfo(window.Handle, window));
				}
				else
				{
					// This is an already known client window
					if (cachedWindow.Title != window.Title)
					{
						this._processCache[window.Handle] = window;
						updatedProcesses.Add(ProcessMonitor.ToProcessInfo(window.Handle, window));
					}

					knownProcesses.Remove(window.Handle);
				}
			}

			foreach (IntPtr index in knownProcesses)
			{
				removedProcesses.Add(ProcessMonitor.ToProcessInfo(index, this._processCache[index]));
				this._processCache.Remove(index);
			}
		}

		/// <summary>
		/// Enumerates every visible unowned top-level window of the monitored processes.
		/// Process.MainWindowHandle is deliberately NOT used here: it reports a single
		/// 'main' window per process, and after an in-game disconnect the EVE client can
		/// end up presenting its live UI in a different window than the reported one,
		/// leaving the preview forever bound to a dead window (a stuck black thumbnail)
		/// </summary>
		private List<ClientWindow> GetClientWindows()
		{
			// The start time is read together with the id: Windows reuses process ids, and
			// the character grouping relies on the identity of a single client run
			Dictionary<uint, long> monitoredProcessIds = new Dictionary<uint, long>();

			foreach (Process process in Process.GetProcesses())
			{
				// Process instances hold system handles; this method runs on a timer
				// so they have to be released deterministically
				using (process)
				{
					if (!this.IsMonitoredProcess(process.ProcessName))
					{
						continue;
					}

					long startTime;

					try
					{
						startTime = process.StartTime.ToFileTimeUtc();
					}
					catch (Exception)
					{
						// An elevated (or already exited) process does not report it
						startTime = 0;
					}

					monitoredProcessIds[(uint)process.Id] = startTime;
				}
			}

			List<ClientWindow> clientWindows = new List<ClientWindow>(16);

			if (monitoredProcessIds.Count == 0)
			{
				return clientWindows;
			}

			StringBuilder titleBuffer = new StringBuilder(512);

			User32NativeMethods.EnumWindows((hwnd, lparam) =>
			{
				User32NativeMethods.GetWindowThreadProcessId(hwnd, out uint processId);

				if (!monitoredProcessIds.TryGetValue(processId, out long processStartTime)
					|| !User32NativeMethods.IsWindowVisible(hwnd)
					|| (User32NativeMethods.GetWindow(hwnd, User32NativeMethods.GW_OWNER) != IntPtr.Zero))
				{
					return true;
				}

				titleBuffer.Clear();
				User32NativeMethods.GetWindowText(hwnd, titleBuffer, titleBuffer.Capacity);

				// Auxiliary windows without a title are of no interest
				if (titleBuffer.Length > 0)
				{
					clientWindows.Add(new ClientWindow(hwnd, titleBuffer.ToString().Replace("—", "-"), processId, processStartTime));
				}

				return true;
			}, IntPtr.Zero);

			return clientWindows;
		}

		private static IProcessInfo ToProcessInfo(IntPtr handle, ClientWindow window)
		{
			return new ProcessInfo(handle, window.Title, window.ProcessId, window.ProcessStartTime);
		}

		private readonly struct ClientWindow
		{
			public ClientWindow(IntPtr handle, string title, uint processId, long processStartTime)
			{
				this.Handle = handle;
				this.Title = title;
				this.ProcessId = processId;
				this.ProcessStartTime = processStartTime;
			}

			public IntPtr Handle { get; }
			public string Title { get; }
			public uint ProcessId { get; }
			public long ProcessStartTime { get; }
		}
	}
}
