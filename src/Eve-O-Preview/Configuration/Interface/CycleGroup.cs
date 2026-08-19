using System.Collections.Generic;

namespace EveOPreview.Configuration
{
	/// <summary>A dynamic cycle group: named set of clients cycled with the assigned hotkeys</summary>
	public sealed class CycleGroup
	{
		public CycleGroup()
		{
			this.Name = "";
			this.ForwardHotkeys = new List<string>();
			this.BackwardHotkeys = new List<string>();
			this.ClientsOrder = new Dictionary<string, int>();
		}

		public string Name { get; set; }
		public List<string> ForwardHotkeys { get; set; }
		public List<string> BackwardHotkeys { get; set; }
		public Dictionary<string, int> ClientsOrder { get; set; }
	}
}
