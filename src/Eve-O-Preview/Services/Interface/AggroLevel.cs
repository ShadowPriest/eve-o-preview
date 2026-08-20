namespace EveOPreview.Services
{
	/// <summary>Aggression state of a client, derived from its combat log</summary>
	public enum AggroLevel
	{
		None,

		/// <summary>NPC are shooting at the client but nothing landed yet (misses, warp scramble attempts)</summary>
		Yellow,

		/// <summary>The client is actually taking damage</summary>
		Red
	}
}
