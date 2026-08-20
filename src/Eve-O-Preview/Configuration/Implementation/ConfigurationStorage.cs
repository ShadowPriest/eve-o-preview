using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EveOPreview.Configuration.Implementation
{
	class ConfigurationStorage : IConfigurationStorage
	{
		private const string CONFIGURATION_FILE_NAME = "EVE-O-Preview.json";

		private const string BROKEN_CONFIGURATION_EXTENSION = ".broken.json";

		private const string LEGACY_CONFIGURATION_BACKUP_EXTENSION = ".v1.backup.json";

		/// <summary>
		/// Layout version of the configuration file this build writes. Has to be kept in
		/// sync with the version ThumbnailConfiguration migrates the stored settings to
		/// </summary>
		private const int CURRENT_CONFIGURATION_VERSION = 2;

		private readonly IAppConfig _appConfig;
		private readonly IThumbnailConfiguration _thumbnailConfiguration;
		private bool _isSaveBlocked;

		public ConfigurationStorage(IAppConfig appConfig, IThumbnailConfiguration thumbnailConfiguration)
		{
			this._appConfig = appConfig;
			this._thumbnailConfiguration = thumbnailConfiguration;
			this._isSaveBlocked = false;
		}

		/// <summary>
		/// Set by the last Load() when the stored configuration could not be read.
		/// Holds the name the unreadable file was moved to, or the name of the file
		/// itself when it could not be moved at all
		/// </summary>
		public string BrokenConfigurationFileName { get; private set; }

		/// <summary>
		/// True when an unreadable configuration file is still in place. Settings are not
		/// written in that state, so the file the user has to fix is not overwritten
		/// </summary>
		public bool IsSaveBlocked => this._isSaveBlocked;

		public void Load()
		{
			string filename = this.GetConfigFileName();

			this.BrokenConfigurationFileName = null;
			this._isSaveBlocked = false;

			if (!File.Exists(filename))
			{
				// Still let the configuration initialize defaults (f.e. cycle groups)
				this._thumbnailConfiguration.ApplyRestrictions();
				return;
			}

			string rawData;

			try
			{
				rawData = File.ReadAllText(filename);
			}
			catch (Exception)
			{
				// A file that cannot even be read (locked by another process, no access) is
				// left completely alone - moving it aside would fail for the same reason
				this.BrokenConfigurationFileName = filename;
				this._isSaveBlocked = true;
				this._thumbnailConfiguration.ApplyRestrictions();
				return;
			}

			JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings()
			{
				ObjectCreationHandling = ObjectCreationHandling.Replace
			};

			// StageHotkeyArraysToAvoidDuplicates(rawData);

			// The data is read into a throwaway instance first. A file that fails halfway
			// through would otherwise leave the live configuration partially overwritten,
			// and every consumer of it would then work with a mix of stored and default values
			try
			{
				JsonConvert.DeserializeObject<ThumbnailConfiguration>(rawData, jsonSerializerSettings);
			}
			catch (Exception)
			{
				this.HandleBrokenConfiguration(filename);
				this._thumbnailConfiguration.ApplyRestrictions();
				return;
			}

			this.BackupLegacyConfiguration(filename, rawData);

			JsonConvert.PopulateObject(rawData, this._thumbnailConfiguration, jsonSerializerSettings);

			// Validate data after loading it
			this._thumbnailConfiguration.ApplyRestrictions();
		}

		/// <summary>
		/// Loading a configuration written before the character registry existed rewrites
		/// the layout of the file: the legacy per-client entries are migrated into the
		/// registry and are not written back. The file as the previous builds wrote it is
		/// kept aside once, so that nothing is lost if the migration is not what the user
		/// wanted
		/// </summary>
		private void BackupLegacyConfiguration(string filename, string rawData)
		{
			try
			{
				int version = (int?)JObject.Parse(rawData)["ConfigVersion"] ?? 1;

				if (version >= ConfigurationStorage.CURRENT_CONFIGURATION_VERSION)
				{
					return;
				}

				string backupFilename = Path.ChangeExtension(filename, ConfigurationStorage.LEGACY_CONFIGURATION_BACKUP_EXTENSION);

				if (!File.Exists(backupFilename))
				{
					File.Copy(filename, backupFilename);
				}
			}
			catch (Exception)
			{
				// A backup copy that could not be made is not a reason to fail the start
			}
		}

		/// <summary>
		/// An unreadable configuration file is never overwritten - it is the only copy of
		/// the user's settings. It is moved aside so the app can start with a fresh file;
		/// if even that fails, saving is blocked and the file is left untouched
		/// </summary>
		private void HandleBrokenConfiguration(string filename)
		{
			string brokenFilename = Path.ChangeExtension(filename, ConfigurationStorage.BROKEN_CONFIGURATION_EXTENSION);

			try
			{
				File.Move(filename, brokenFilename, true);
				this.BrokenConfigurationFileName = brokenFilename;
			}
			catch (Exception)
			{
				this.BrokenConfigurationFileName = filename;
				this._isSaveBlocked = true;
			}
		}

		/// <summary>
		/// Reads the stored UI language without touching the configuration instance.
		/// The language is needed before the views are created, while a full Load() at that
		/// point would publish half-initialized settings to the rest of the application
		/// </summary>
		public string ReadLanguage()
		{
			string filename = this.GetConfigFileName();

			if (!File.Exists(filename))
			{
				return null;
			}

			try
			{
				return (string)JObject.Parse(File.ReadAllText(filename))["Language"];
			}
			catch (Exception)
			{
				// An unreadable config leaves the app in the system language
				return null;
			}
		}

		public void Save()
		{
			if (this._isSaveBlocked)
			{
				return;
			}

			// Serialization runs on the caller's thread so it captures a consistent state.
			// The disk write itself goes to a worker task with 'latest data wins' coalescing:
			// a slow disk or an antivirus scan must never stall the UI thread, and rapid
			// setting changes (f.e. holding a numeric spinner arrow) collapse into one write
			string rawData = JsonConvert.SerializeObject(this._thumbnailConfiguration, Formatting.Indented);

			bool startWorker;

			lock (this._writeSyncRoot)
			{
				this._pendingWriteData = rawData;

				startWorker = !this._isWriteWorkerRunning;
				this._isWriteWorkerRunning = true;
			}

			if (startWorker)
			{
				System.Threading.Tasks.Task.Run(this.WritePendingData);
			}
		}

		/// <summary>
		/// Blocks until the queued settings write has hit the disk. Called on shutdown -
		/// the process must not exit while the latest settings are still in the queue
		/// </summary>
		public void Flush()
		{
			for (int i = 0; i < 150; i++)
			{
				lock (this._writeSyncRoot)
				{
					if (!this._isWriteWorkerRunning)
					{
						return;
					}
				}

				System.Threading.Thread.Sleep(20);
			}
		}

		private void WritePendingData()
		{
			while (true)
			{
				string rawData;

				lock (this._writeSyncRoot)
				{
					rawData = this._pendingWriteData;
					this._pendingWriteData = null;

					if (rawData == null)
					{
						this._isWriteWorkerRunning = false;
						return;
					}
				}

				try
				{
					File.WriteAllText(this.GetConfigFileName(), rawData);
				}
				catch (Exception)
				{
					// Ignore error if for some reason the updated config cannot be written down
				}
			}
		}

		private readonly object _writeSyncRoot = new object();
		private string _pendingWriteData;
		private bool _isWriteWorkerRunning;

		private string GetConfigFileName()
		{
			return string.IsNullOrEmpty(this._appConfig.ConfigFileName) ? ConfigurationStorage.CONFIGURATION_FILE_NAME : this._appConfig.ConfigFileName;
		}
	}
}