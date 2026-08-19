using System;

namespace EveOPreview.UI.Hotkeys
{
	/// <summary>
	/// Global mouse bindings (wheel scroll, middle / extra buttons with optional modifiers)
	/// backed by a low level mouse hook
	/// </summary>
	public interface IMouseHookService
	{
		void Register(string binding, Action action);
		void UnregisterAll();
	}
}
