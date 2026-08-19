using MediatR;

namespace EveOPreview.Mediator.Messages
{
	/// <summary>
	/// Raised while the user captures a key combination in the hotkey editor.
	/// Registered hotkeys are suspended for that time, otherwise an already
	/// bound combination would be swallowed by its own handler and never reach the editor
	/// </summary>
	sealed class HotkeyCaptureModeChanged : INotification
	{
		public HotkeyCaptureModeChanged(bool isCapturing)
		{
			this.IsCapturing = isCapturing;
		}

		public bool IsCapturing { get; }
	}
}
