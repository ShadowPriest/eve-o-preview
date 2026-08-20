using System;

namespace EveOPreview.Services
{
	/// <summary>
	/// Watches the EVE game log folder (Documents\EVE\logs\Gamelogs) and derives
	/// the per-character aggression state from the combat entries. The files are
	/// only read - nothing is ever sent to the game
	/// </summary>
	public interface IGameLogMonitor
	{
		void Start();
		void Stop();

		/// <summary>Current aggression state of the given character (log 'Listener')</summary>
		AggroLevel GetAggro(string characterName);

		/// <summary>
		/// Raised (on a worker thread) when the aggression state of a character changes.
		/// A null character name means 'every character' (used by the test mode)
		/// </summary>
		event Action<string> AggroChanged;

		/// <summary>Preview of the aggro frames: yellow for a few seconds, then red, on every thumbnail</summary>
		void InjectTestSequence();
	}
}
