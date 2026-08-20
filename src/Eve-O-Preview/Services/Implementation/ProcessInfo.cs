using System;

namespace EveOPreview.Services.Implementation
{
	sealed class ProcessInfo : IProcessInfo
	{
		public ProcessInfo(IntPtr handle, string title)
			: this(handle, title, 0, 0)
		{
		}

		public ProcessInfo(IntPtr handle, string title, uint processId, long processStartTime)
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