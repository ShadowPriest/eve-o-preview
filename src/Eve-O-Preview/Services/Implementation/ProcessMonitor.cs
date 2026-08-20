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
		private readonly IDictionary<IntPtr, string> _processCache;
		private IProcessInfo _currentProcessInfo;
		private readonly IThumbnailConfiguration _configuration;
		#endregion

		public ProcessMonitor(IThumbnailConfiguration configuration)
		{
			this._processCache = new Dictionary<IntPtr, string>(512);
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
			foreach (KeyValuePair<IntPtr, string> entry in this._processCache)
			{
				result.Add(new ProcessInfo(entry.Key, entry.Value));
			}

			return result;
		}

		public void GetUpdatedProcesses(out ICollection<IProcessInfo> addedProcesses, out ICollection<IProcessInfo> updatedProcesses, out ICollection<IProcessInfo> removedProcesses)
		{
			addedProcesses = new List<IProcessInfo>(16);
			updatedProcesses = new List<IProcessInfo>(16);
			removedProcesses = new List<IProcessInfo>(16);

			IList<IntPtr> knownProcesses = new List<IntPtr>(this._processCache.Keys);

			foreach ((IntPtr handle, string title) in this.GetClientWindows())
			{
				this._processCache.TryGetValue(handle, out string cachedTitle);

				if (cachedTitle == null)
				{
					// This is a new client window
					this._processCache.Add(handle, title);
					addedProcesses.Add(new ProcessInfo(handle, title));
				}
				else
				{
					// This is an already known client window
					if (cachedTitle != title)
					{
						this._processCache[handle] = title;
						updatedProcesses.Add(new ProcessInfo(handle, title));
					}

					knownProcesses.Remove(handle);
				}
			}

			foreach (IntPtr index in knownProcesses)
			{
				string title = this._processCache[index];
				removedProcesses.Add(new ProcessInfo(index, title));
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
		private List<(IntPtr Handle, string Title)> GetClientWindows()
		{
			HashSet<uint> monitoredProcessIds = new HashSet<uint>();

			foreach (Process process in Process.GetProcesses())
			{
				// Process instances hold system handles; this method runs on a timer
				// so they have to be released deterministically
				using (process)
				{
					if (this.IsMonitoredProcess(process.ProcessName))
					{
						monitoredProcessIds.Add((uint)process.Id);
					}
				}
			}

			List<(IntPtr Handle, string Title)> clientWindows = new List<(IntPtr, string)>(16);

			if (monitoredProcessIds.Count == 0)
			{
				return clientWindows;
			}

			StringBuilder titleBuffer = new StringBuilder(512);

			User32NativeMethods.EnumWindows((hwnd, lparam) =>
			{
				User32NativeMethods.GetWindowThreadProcessId(hwnd, out uint processId);

				if (!monitoredProcessIds.Contains(processId)
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
					clientWindows.Add((hwnd, titleBuffer.ToString().Replace("—", "-")));
				}

				return true;
			}, IntPtr.Zero);

			return clientWindows;
		}
	}
}
