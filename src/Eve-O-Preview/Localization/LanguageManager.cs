using System;
using System.Collections.Generic;
using System.Globalization;

namespace EveOPreview.Localization
{
	/// <summary>
	/// Owns the list of languages the UI is translated into and applies the configured
	/// one to the process. Has to run before the first form is created - WinForms reads
	/// the UI culture while the controls are being built
	/// </summary>
	public static class LanguageManager
	{
		/// <summary>Configuration value that means 'follow the Windows UI language'</summary>
		public const string SYSTEM_LANGUAGE = "auto";

		private static readonly string[] SUPPORTED_LANGUAGES = { "en", "ru" };

		public static IList<string> SupportedLanguages => Array.AsReadOnly(LanguageManager.SUPPORTED_LANGUAGES);

		public static bool IsSupported(string language)
		{
			return Array.IndexOf(LanguageManager.SUPPORTED_LANGUAGES, language) >= 0;
		}

		/// <summary>
		/// Reduces any stored value to either a supported language code or the 'system' marker
		/// </summary>
		public static string Normalize(string language)
		{
			return LanguageManager.IsSupported(language) ? language : LanguageManager.SYSTEM_LANGUAGE;
		}

		public static void Apply(string language)
		{
			if (!LanguageManager.IsSupported(language))
			{
				// The system language is what the process already runs with
				return;
			}

			CultureInfo culture = new CultureInfo(language);

			CultureInfo.DefaultThreadCurrentUICulture = culture;
			CultureInfo.CurrentUICulture = culture;
		}

		/// <summary>
		/// Name of a language in that language itself - a user looking for their own
		/// language should not have to read the current one first
		/// </summary>
		public static string GetDisplayName(string language)
		{
			if (language == LanguageManager.SYSTEM_LANGUAGE)
			{
				return Strings.Language_System;
			}

			CultureInfo culture = new CultureInfo(language);
			string name = culture.NativeName;

			return name.Length > 0 ? char.ToUpper(name[0], culture) + name.Substring(1) : language;
		}
	}
}
