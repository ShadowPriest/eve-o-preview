using System;
using System.Text;
using System.Windows.Forms;

namespace EveOPreview.UI.Hotkeys
{
	/// <summary>
	/// Helper for the actions bound to a click on a preview window.
	/// Unlike <see cref="MouseBinding"/> these bindings never reach the system wide mouse
	/// hook: they are only compared against the clicks a preview window receives itself,
	/// which is why the left and the right button are allowed here.
	/// Bindings are stored as strings like "Shift+LButton" or "Control+Shift+LButton";
	/// an empty value means that the action has no click assigned to it
	/// </summary>
	static class PreviewClickBinding
	{
		public const string LEFT_BUTTON = "LButton";
		public const string MIDDLE_BUTTON = "MButton";
		public const string RIGHT_BUTTON = "RButton";
		public const string X_BUTTON_1 = "XButton1";
		public const string X_BUTTON_2 = "XButton2";

		/// <summary>Modifiers a click is compared by; the rest of the key state is ignored</summary>
		public const Keys SUPPORTED_MODIFIERS = Keys.Control | Keys.Shift | Keys.Alt;

		private static readonly (string Name, MouseButtons Button)[] Buttons =
		{
			(PreviewClickBinding.LEFT_BUTTON, MouseButtons.Left),
			(PreviewClickBinding.MIDDLE_BUTTON, MouseButtons.Middle),
			(PreviewClickBinding.RIGHT_BUTTON, MouseButtons.Right),
			(PreviewClickBinding.X_BUTTON_1, MouseButtons.XButton1),
			(PreviewClickBinding.X_BUTTON_2, MouseButtons.XButton2)
		};

		public static string[] GetButtonNames()
		{
			string[] names = new string[PreviewClickBinding.Buttons.Length];

			for (int index = 0; index < PreviewClickBinding.Buttons.Length; index++)
			{
				names[index] = PreviewClickBinding.Buttons[index].Name;
			}

			return names;
		}

		public static bool TryParse(string binding, out Keys modifiers, out MouseButtons button)
		{
			modifiers = Keys.None;
			button = MouseButtons.None;

			if (string.IsNullOrWhiteSpace(binding))
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

				foreach ((string name, MouseButtons knownButton) in PreviewClickBinding.Buttons)
				{
					if (string.Equals(part, name, StringComparison.OrdinalIgnoreCase))
					{
						button = knownButton;
						return true;
					}
				}
			}

			return false;
		}

		public static string Compose(Keys modifiers, MouseButtons button)
		{
			string name = PreviewClickBinding.GetButtonName(button);

			if (name == null)
			{
				return "";
			}

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

			value.Append(name);

			return value.ToString();
		}

		public static string GetButtonName(MouseButtons button)
		{
			foreach ((string name, MouseButtons knownButton) in PreviewClickBinding.Buttons)
			{
				if (knownButton == button)
				{
					return name;
				}
			}

			return null;
		}

		/// <summary>Canonical form used to compare and store bindings; an unparsable one is dropped</summary>
		public static string Normalize(string binding)
		{
			return PreviewClickBinding.TryParse(binding, out Keys modifiers, out MouseButtons button)
					? PreviewClickBinding.Compose(modifiers, button)
					: "";
		}

		/// <summary>True when the click matches the binding exactly, modifiers included</summary>
		public static bool Matches(string binding, MouseButtons button, Keys modifiers)
		{
			if (!PreviewClickBinding.TryParse(binding, out Keys boundModifiers, out MouseButtons boundButton))
			{
				return false;
			}

			return (boundButton == button) && (boundModifiers == (modifiers & PreviewClickBinding.SUPPORTED_MODIFIERS));
		}

		/// <summary>
		/// The plain left click activates the client and cannot be taken away from it,
		/// so it is not a binding the user may assign
		/// </summary>
		public static bool IsReservedForActivation(string binding)
		{
			return PreviewClickBinding.TryParse(binding, out Keys modifiers, out MouseButtons button)
					&& (button == MouseButtons.Left)
					&& (modifiers == Keys.None);
		}
	}
}
