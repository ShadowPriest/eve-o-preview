using System.Drawing;

namespace EveOPreview.Configuration
{
	/// <summary>
	/// A group of characters that share one game account. Membership is stored on the
	/// characters themselves (CharacterInfo.GroupId) so that there is a single source of
	/// truth for it. Groups are built automatically: every character seen in one client
	/// process belongs to one account, as the client no longer allows switching the
	/// account without a restart
	/// </summary>
	public sealed class CharacterGroup
	{
		public CharacterGroup()
		{
			this.Id = "";
			this.Name = "";
			this.ManageAsWhole = true;
			this.Color = Color.Empty;
			this.Hotkeys = new System.Collections.Generic.List<string>();
		}

		public string Id { get; set; }
		public string Name { get; set; }

		/// <summary>
		/// When set, every preview setting written for one member of the group
		/// (position, size, overlay appearance) is written for all of its members
		/// </summary>
		public bool ManageAsWhole { get; set; }

		/// <summary>
		/// Hotkeys that switch to this account. Only one of its characters can be online
		/// at a time, so the account is a single switch target no matter which character
		/// is logged in right now
		/// </summary>
		public System.Collections.Generic.List<string> Hotkeys { get; set; }

		/// <summary>
		/// Color the group is marked with in the lists, so that the characters of one
		/// account are recognizable as a group and not as three names that happen to
		/// stand next to each other
		/// </summary>
		public Color Color { get; set; }
	}
}
