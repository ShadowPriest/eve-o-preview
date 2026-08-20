using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using EveOPreview.Configuration;

namespace EveOPreview.Services
{
	/// <summary>
	/// Tails the EVE combat log files on a dedicated background thread, so the file
	/// I/O never competes with the UI thread or the thumbnail refresh cycle.
	/// The state is exposed as a lock-free snapshot (GetAggro) plus a change event
	/// </summary>
	sealed class GameLogMonitor : IGameLogMonitor
	{
		#region Private constants
		// A frame is dropped once no attack event arrived for this long
		private const int AGGRO_TIMEOUT_MS = 5000;

		// The tail-read cadence; also defines how quickly a fresh event lights the frame up
		private const int POLL_PERIOD_MS = 250;

		// Full folder rescans (new sessions / new characters) are much rarer than tail reads
		private const int FOLDER_SCAN_PERIOD_MS = 5000;

		// Only logs of live sessions are of interest; EVE keeps years of old files around
		private static readonly TimeSpan LOG_FILE_MAX_AGE = TimeSpan.FromHours(24);

		// Test mode: yellow for one phase, then red for one phase
		private const int TEST_PHASE_MS = 4000;

		// A log whose header could not be parsed this long after creation never will be
		private static readonly TimeSpan HEADER_PARSE_GRACE = TimeSpan.FromMinutes(5);
		#endregion

		#region Private classes
		private sealed class CharacterAggro
		{
			// Written on the worker thread, read on the UI thread. Tick values are
			// monotonic (Environment.TickCount64) so stale reads only delay a frame
			// by one poll period at worst
			public long YellowTick;
			public long RedTick;
			public AggroLevel NotifiedLevel;
		}

		private sealed class TrackedLog
		{
			public string Path;
			public string Listener;
			public FileStream Stream;
			public byte[] Carry = Array.Empty<byte>();
		}
		#endregion

		#region Private fields
		private readonly IThumbnailConfiguration _configuration;
		private readonly ConcurrentDictionary<string, CharacterAggro> _aggro;
		private readonly Dictionary<string, TrackedLog> _trackedLogs;
		private readonly HashSet<string> _ignoredFiles;

		private Thread _thread;
		private ManualResetEventSlim _stopSignal;

		private string _scannedFolder;
		private long _lastFolderScanTick;

		private long _testStartTick;
		private AggroLevel _lastTestLevel;
		#endregion

		public GameLogMonitor(IThumbnailConfiguration configuration)
		{
			this._configuration = configuration;
			this._aggro = new ConcurrentDictionary<string, CharacterAggro>(StringComparer.OrdinalIgnoreCase);
			this._trackedLogs = new Dictionary<string, TrackedLog>(StringComparer.OrdinalIgnoreCase);
			this._ignoredFiles = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
		}

		public event Action<string> AggroChanged;

		public void Start()
		{
			if (this._thread != null)
			{
				return;
			}

			this._stopSignal = new ManualResetEventSlim(false);
			this._thread = new Thread(this.MonitorLoop)
			{
				IsBackground = true,
				Name = "GameLogMonitor"
			};
			this._thread.Start();
		}

		public void Stop()
		{
			if (this._thread == null)
			{
				return;
			}

			this._stopSignal.Set();
			this._thread.Join(2000);
			this._thread = null;

			this.CloseAllLogs();
		}

		public AggroLevel GetAggro(string characterName)
		{
			long now = Environment.TickCount64;

			AggroLevel testLevel = this.GetTestLevel(now);
			if (testLevel != AggroLevel.None)
			{
				return testLevel;
			}

			if (string.IsNullOrEmpty(characterName) || !this._aggro.TryGetValue(characterName, out CharacterAggro entry))
			{
				return AggroLevel.None;
			}

			return GameLogMonitor.ComputeLevel(entry, now);
		}

		public void InjectTestSequence()
		{
			Interlocked.Exchange(ref this._testStartTick, Environment.TickCount64);
			this.AggroChanged?.Invoke(null);
		}

		private static AggroLevel ComputeLevel(CharacterAggro entry, long now)
		{
			if (now - Interlocked.Read(ref entry.RedTick) <= GameLogMonitor.AGGRO_TIMEOUT_MS)
			{
				return AggroLevel.Red;
			}

			if (now - Interlocked.Read(ref entry.YellowTick) <= GameLogMonitor.AGGRO_TIMEOUT_MS)
			{
				return AggroLevel.Yellow;
			}

			return AggroLevel.None;
		}

		private AggroLevel GetTestLevel(long now)
		{
			long testStart = Interlocked.Read(ref this._testStartTick);

			if (testStart == 0)
			{
				return AggroLevel.None;
			}

			long elapsed = now - testStart;

			if (elapsed < GameLogMonitor.TEST_PHASE_MS)
			{
				return AggroLevel.Yellow;
			}

			if (elapsed < 2 * GameLogMonitor.TEST_PHASE_MS)
			{
				return AggroLevel.Red;
			}

			return AggroLevel.None;
		}

		#region Worker thread
		private void MonitorLoop()
		{
			while (!this._stopSignal.Wait(GameLogMonitor.POLL_PERIOD_MS))
			{
				try
				{
					this.MonitorTick();
				}
				catch (Exception)
				{
					// A transient I/O error (file deleted mid-read, folder on a flaky
					// network drive) must not kill the monitor thread
				}
			}
		}

		private void MonitorTick()
		{
			this.NotifyTestPhaseChanges();

			if (!this._configuration.EnableGameLogMonitor)
			{
				if (this._trackedLogs.Count > 0)
				{
					this.CloseAllLogs();
					this.ClearAggroStates();
				}

				return;
			}

			long now = Environment.TickCount64;

			if (now - this._lastFolderScanTick >= GameLogMonitor.FOLDER_SCAN_PERIOD_MS)
			{
				this._lastFolderScanTick = now;
				this.ScanFolder();
			}

			foreach (TrackedLog log in this._trackedLogs.Values)
			{
				this.ReadNewLines(log);
			}

			this.NotifyAggroChanges();
		}

		private void NotifyTestPhaseChanges()
		{
			AggroLevel testLevel = this.GetTestLevel(Environment.TickCount64);

			if (testLevel == this._lastTestLevel)
			{
				return;
			}

			this._lastTestLevel = testLevel;

			if (testLevel == AggroLevel.None)
			{
				Interlocked.Exchange(ref this._testStartTick, 0);
			}

			this.AggroChanged?.Invoke(null);
		}

		private void NotifyAggroChanges()
		{
			long now = Environment.TickCount64;

			foreach (KeyValuePair<string, CharacterAggro> entry in this._aggro)
			{
				AggroLevel level = GameLogMonitor.ComputeLevel(entry.Value, now);

				if (level == entry.Value.NotifiedLevel)
				{
					continue;
				}

				entry.Value.NotifiedLevel = level;
				this.AggroChanged?.Invoke(entry.Key);
			}
		}

		private void ClearAggroStates()
		{
			foreach (KeyValuePair<string, CharacterAggro> entry in this._aggro)
			{
				Interlocked.Exchange(ref entry.Value.YellowTick, 0);
				Interlocked.Exchange(ref entry.Value.RedTick, 0);
			}
		}

		private string ResolveLogFolder()
		{
			string folder = this._configuration.GameLogsFolder;

			if (!string.IsNullOrWhiteSpace(folder))
			{
				return folder;
			}

			return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "EVE", "logs", "Gamelogs");
		}

		private void ScanFolder()
		{
			string folder = this.ResolveLogFolder();

			// A folder change in the settings restarts the tracking from scratch
			if (!string.Equals(folder, this._scannedFolder, StringComparison.OrdinalIgnoreCase))
			{
				this.CloseAllLogs();
				this._ignoredFiles.Clear();
				this._scannedFolder = folder;
			}

			if (!Directory.Exists(folder))
			{
				return;
			}

			DateTime cutoffUtc = DateTime.UtcNow - GameLogMonitor.LOG_FILE_MAX_AGE;
			HashSet<string> liveFiles = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

			foreach (string path in Directory.EnumerateFiles(folder, "*.txt"))
			{
				DateTime lastWriteUtc;
				try
				{
					lastWriteUtc = File.GetLastWriteTimeUtc(path);
				}
				catch (Exception)
				{
					continue;
				}

				if (lastWriteUtc < cutoffUtc)
				{
					continue;
				}

				liveFiles.Add(path);

				if (this._trackedLogs.ContainsKey(path) || this._ignoredFiles.Contains(path))
				{
					continue;
				}

				this.TryTrackLog(path, lastWriteUtc);
			}

			// Logs that aged out or disappeared are released
			foreach (string path in this._trackedLogs.Keys.Where(x => !liveFiles.Contains(x)).ToList())
			{
				this.CloseLog(path);
			}
		}

		private void TryTrackLog(string path, DateTime lastWriteUtc)
		{
			string listener = GameLogMonitor.TryReadListener(path);

			if (listener == null)
			{
				// The client might not have flushed the header yet - retried on the
				// next scans, given up on once the file is clearly not a game log
				if (DateTime.UtcNow - lastWriteUtc > GameLogMonitor.HEADER_PARSE_GRACE)
				{
					this._ignoredFiles.Add(path);
				}

				return;
			}

			try
			{
				FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete);

				// History is skipped on purpose: only events that happen from now on matter
				stream.Seek(0, SeekOrigin.End);

				this._trackedLogs[path] = new TrackedLog
				{
					Path = path,
					Listener = listener,
					Stream = stream
				};
			}
			catch (Exception)
			{
				// The file could be locked exclusively; retried on the next scan
			}
		}

		/// <summary>
		/// Extracts the character name from the log header. The header layout is the
		/// same in every client language, so the first 'key: value' line is taken
		/// without matching the localized 'Listener' keyword itself
		/// </summary>
		private static string TryReadListener(string path)
		{
			try
			{
				using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete))
				using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
				{
					for (int i = 0; i < 6; i++)
					{
						string line = reader.ReadLine();

						if (line == null)
						{
							return null;
						}

						line = line.Trim();

						// Log entries start with a '[ timestamp ]' - the header is over
						if (line.StartsWith("[", StringComparison.Ordinal))
						{
							return null;
						}

						int colonIndex = line.IndexOf(':');
						if (colonIndex <= 0)
						{
							continue;
						}

						string value = line.Substring(colonIndex + 1).Trim();
						return value.Length > 0 ? value : null;
					}
				}
			}
			catch (Exception)
			{
				// Unreadable file - not a log to track
			}

			return null;
		}

		private void ReadNewLines(TrackedLog log)
		{
			long length;
			try
			{
				length = log.Stream.Length;
			}
			catch (Exception)
			{
				return;
			}

			// A shrunken file was truncated / recreated - start over from its beginning
			if (length < log.Stream.Position)
			{
				log.Stream.Position = 0;
				log.Carry = Array.Empty<byte>();
			}

			if (length == log.Stream.Position)
			{
				return;
			}

			int available = (int)Math.Min(length - log.Stream.Position, 256 * 1024);
			byte[] buffer = new byte[log.Carry.Length + available];
			Array.Copy(log.Carry, buffer, log.Carry.Length);

			int read = log.Stream.Read(buffer, log.Carry.Length, available);
			int total = log.Carry.Length + Math.Max(read, 0);

			int lineStart = 0;
			for (int i = 0; i < total; i++)
			{
				if (buffer[i] != (byte)'\n')
				{
					continue;
				}

				int lineLength = i - lineStart;
				if ((lineLength > 0) && (buffer[i - 1] == (byte)'\r'))
				{
					lineLength--;
				}

				if (lineLength > 0)
				{
					this.ClassifyLine(Encoding.UTF8.GetString(buffer, lineStart, lineLength), log.Listener);
				}

				lineStart = i + 1;
			}

			// An unterminated tail is kept until the client writes the rest of the line
			log.Carry = new byte[total - lineStart];
			Array.Copy(buffer, lineStart, log.Carry, 0, log.Carry.Length);
		}

		/// <summary>
		/// Classifies a single log line as an incoming attack event.
		/// Only events aimed AT the listener are of interest:
		/// - incoming damage: '(combat)' line colored 0xffcc0000 with the 'из'/'from' direction marker
		/// - incoming fire that missed: the localized 'misses you' message
		/// - warp scramble attempt with the listener as the target
		/// </summary>
		private void ClassifyLine(string line, string listener)
		{
			if (line.IndexOf("(combat)", StringComparison.Ordinal) < 0)
			{
				return;
			}

			long now = Environment.TickCount64;

			// The damage color code is client markup, so it is language-independent;
			// the direction marker filters out the (differently colored) outgoing hits
			if ((line.IndexOf("0xffcc0000", StringComparison.OrdinalIgnoreCase) >= 0)
				&& ((line.IndexOf("из</font>", StringComparison.Ordinal) >= 0) || (line.IndexOf("from</font>", StringComparison.Ordinal) >= 0)))
			{
				Interlocked.Exchange(ref this.GetAggroEntry(listener).RedTick, now);
				return;
			}

			bool isMiss = (line.IndexOf("промах мимо вашего корабля", StringComparison.Ordinal) >= 0)
						|| (line.IndexOf("misses you completely", StringComparison.Ordinal) >= 0);

			bool isEwar = ((line.IndexOf("Попытка варп-глушения", StringComparison.Ordinal) >= 0)
						|| (line.IndexOf("Warp scramble attempt", StringComparison.Ordinal) >= 0)
						|| (line.IndexOf("Warp disruption attempt", StringComparison.Ordinal) >= 0))
						&& ((line.IndexOf(listener, StringComparison.Ordinal) >= 0) || (line.IndexOf("to you!", StringComparison.Ordinal) >= 0));

			if (isMiss || isEwar)
			{
				Interlocked.Exchange(ref this.GetAggroEntry(listener).YellowTick, now);
			}
		}

		private CharacterAggro GetAggroEntry(string listener)
		{
			return this._aggro.GetOrAdd(listener, _ => new CharacterAggro());
		}

		private void CloseLog(string path)
		{
			if (!this._trackedLogs.TryGetValue(path, out TrackedLog log))
			{
				return;
			}

			this._trackedLogs.Remove(path);

			try
			{
				log.Stream.Dispose();
			}
			catch (Exception)
			{
				// Nothing useful to do with a failed close
			}
		}

		private void CloseAllLogs()
		{
			foreach (string path in this._trackedLogs.Keys.ToList())
			{
				this.CloseLog(path);
			}
		}
		#endregion
	}
}
