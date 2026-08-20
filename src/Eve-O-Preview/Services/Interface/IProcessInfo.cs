using System;

namespace EveOPreview.Services
{
	public interface IProcessInfo
	{
		IntPtr Handle { get; }
		string Title { get; }

		/// <summary>Id of the process this window belongs to</summary>
		uint ProcessId { get; }

		/// <summary>
		/// Start time of the process (file time, 0 when it could not be read).
		/// Windows reuses process ids, so the pair of the two is what actually identifies
		/// a single run of a client
		/// </summary>
		long ProcessStartTime { get; }
	}
}