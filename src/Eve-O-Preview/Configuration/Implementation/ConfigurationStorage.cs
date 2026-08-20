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

			JsonConvert.PopulateObject(rawData, this._thumbnailConfiguration, jsonSerializerSettings);

			// Validate data after loading it
			this._thumbnailConfiguration.ApplyRestrictions();
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

			string rawData = JsonConvert.SerializeObject(this._thumbnailConfiguration, Formatting.Indented);
			string filename = this.GetConfigFileName();

			try
			{
				File.WriteAllText(filename, rawData);
			}
			catch (IOException)
			{
				// Ignore error if for some reason the updated config cannot be written down
			}
		}

		private string GetConfigFileName()
		{
			return string.IsNullOrEmpty(this._appConfig.ConfigFileName) ? ConfigurationStorage.CONFIGURATION_FILE_NAME : this._appConfig.ConfigFileName;
		}
	}
}