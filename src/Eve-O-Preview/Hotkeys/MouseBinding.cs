using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace EveOPreview.UI.Hotkeys
{
	/// <summary>
	/// Helper for hotkey bindings based on mouse buttons / wheel.
	/// Bindings are stored as strings like "Control+ScrollUp" or "Shift+XButton1"
	/// side by side with the keyboard hotkey strings.
	/// </summary>
	static class MouseBinding
	{
		public const string SCROLL_UP = "ScrollUp";
		public const string SCROLL_DOWN = "ScrollDown";
		public const string MIDDLE_BUTTON = "MButton";
		public const string X_BUTTON_1 = "XButton1";
		public const string X_BUTTON_2 = "XButton2";

		private static readonly string[] Buttons = { MouseBinding.SCROLL_UP, MouseBinding.SCROLL_DOWN, MouseBinding.MIDDLE_BUTTON, MouseBinding.X_BUTTON_1, MouseBinding.X_BUTTON_2 };

		public static bool IsMouseBinding(string binding)
		{
			return MouseBinding.TryParse(binding, out _, out _);
		}

		public static bool TryParse(string binding, out Keys modifiers, out string button)
		{
			modifiers = Keys.None;
			button = null;

			if (string.IsNullOrEmpty(binding))
			{
				return false;
			}

			string[] parts = binding.Split('+');

			for (int index = 0; index < parts.Length; index++)
			{
				string part = parts[index].Trim();
				bool isLast = index == parts.Length - 1;

				if (!isLast)
				{
					switch (part.ToLowerInvariant())
					{
						case "control":
						case "ctrl":
							modifiers |= Keys.Control;
							break;
						case "shift":
							modifiers |= Keys.Shift;
							break;
						case "alt":
							modifiers |= Keys.Alt;
							break;
						default:
							return false;
					}

					continue;
				}

				foreach (string knownButton in MouseBinding.Buttons)
				{
					if (string.Equals(part, knownButton, StringComparison.OrdinalIgnoreCase))
					{
						button = knownButton;
						return true;
					}
				}
			}

			return false;
		}

		public static string Compose(Keys modifiers, string button)
		{
			StringBuilder value = new StringBuilder();

			if ((modifiers & Keys.Control) == Keys.Control)
			{
				value.Append("Control+");
			}

			if ((modifiers & Keys.Shift) == Keys.Shift)
			{
				value.Append("Shift+");
			}

			if ((modifiers & Keys.Alt) == Keys.Alt)
			{
				value.Append("Alt+");
			}

			value.Append(button);

			return value.ToString();
		}

		/// <summary>Canonical form used to compare and store bindings</summary>
		public static string Normalize(string binding)
		{
			return MouseBinding.TryParse(binding, out Keys modifiers, out string button) ? MouseBinding.Compose(modifiers, button) : binding;
		}
	}
}
