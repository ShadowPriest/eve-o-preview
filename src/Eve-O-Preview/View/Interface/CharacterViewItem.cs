namespace EveOPreview.View
{
	/// <summary>A character of the registry as it is shown in the settings window</summary>
	public sealed class CharacterViewItem
	{
		public CharacterViewItem(string title, string name, string groupId, bool isOnline, bool isIgnored, string lastSeen)
		{
			this.Title = title;
			this.Name = name;
			this.GroupId = groupId;
			this.IsOnline = isOnline;
			this.IsIgnored = isIgnored;
			this.LastSeen = lastSeen;
		}

		/// <summary>Client window title - the key every setting of this character is stored under</summary>
		public string Title { get; }

		/// <summary>Character name as it is displayed in the list</summary>
		public string Name { get; }

		/// <summary>Group (account) of the character, null for an ungrouped one</summary>
		public string GroupId { get; }

		public bool IsOnline { get; }

		/// <summary>Blacklisted: kept in the registry but out of every other list</summary>
		public bool IsIgnored { get; }

		/// <summary>Preformatted 'last seen' text</summary>
		public string LastSeen { get; }
	}
}
