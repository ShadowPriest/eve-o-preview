using System.Drawing;

namespace EveOPreview.View
{
	/// <summary>A character group (account) as it is shown in the settings window</summary>
	public sealed class CharacterGroupViewItem
	{
		public CharacterGroupViewItem(string id, string name, bool manageAsWhole, Color color)
		{
			this.Id = id;
			this.Name = name;
			this.ManageAsWhole = manageAsWhole;
			this.Color = color;
		}

		public string Id { get; }
		public string Name { get; }

		/// <summary>Preview settings written for one member are written for all of them</summary>
		public bool ManageAsWhole { get; }

		/// <summary>Color the group is marked with in the lists</summary>
		public Color Color { get; }
	}
}
