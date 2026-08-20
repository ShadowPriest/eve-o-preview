using System;
using Newtonsoft.Json;

namespace EveOPreview.Configuration
{
	/// <summary>
	/// A character that has been seen logged in at least once.
	/// The client window title is the primary key here: it is the only character identity
	/// the application can observe, it is unique in EVE and it is already the key of every
	/// per-client setting stored in the configuration file
	/// </summary>
	[JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
	public sealed class CharacterInfo
	{
		/// <summary>Prefix the EVE client puts before the character name in the window title</summary>
		public const string TITLE_PREFIX = "EVE - ";

		/// <summary>Title of a client window that has no character logged in yet</summary>
		public const string LOGIN_TITLE = "EVE";

		public CharacterInfo()
		{
			this.Title = "";
		}

		public CharacterInfo(string title, DateTime? firstSeen, DateTime? lastSeen)
		{
			this.Title = title;
			this.FirstSeen = firstSeen;
			this.LastSeen = lastSeen;
		}

		public string Title { get; set; }

		/// <summary>Id of the character group (account) this character belongs to. Null when ungrouped</summary>
		public string GroupId { get; set; }

		/// <summary>UTC stamps, null for the characters migrated from an older configuration</summary>
		public DateTime? FirstSeen { get; set; }
		public DateTime? LastSeen { get; set; }

		/// <summary>
		/// Blacklisted character: one that is never going to be logged in again. It is kept
		/// in the registry (so that it does not come back through an old setting) but it is
		/// hidden from every list the application offers
		/// </summary>
		public bool Ignored { get; set; }

		/// <summary>Character name without the client title prefix. Used in the UI only</summary>
		[JsonIgnore]
		public string Name => CharacterInfo.GetDisplayName(this.Title);

		/// <summary>
		/// False for the windows that do not represent a logged in character: an empty
		/// title and the login screen, which every client shares
		/// </summary>
		public static bool IsCharacterTitle(string title)
		{
			return !string.IsNullOrWhiteSpace(title)
					&& !string.Equals(title, CharacterInfo.LOGIN_TITLE, StringComparison.Ordinal);
		}

		public static string GetDisplayName(string title)
		{
			if (string.IsNullOrEmpty(title))
			{
				return "";
			}

			return title.StartsWith(CharacterInfo.TITLE_PREFIX, StringComparison.Ordinal)
					? title.Substring(CharacterInfo.TITLE_PREFIX.Length)
					: title;
		}
	}
}
