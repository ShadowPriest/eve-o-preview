using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using EveOPreview.Localization;
using EveOPreview.UI.Hotkeys;

namespace EveOPreview.View
{
	/// <summary>
	/// Modal dialog used to create and edit hotkey bindings.
	/// In 'add' mode an action is picked first: cycle group actions expose a direction
	/// selector and the 'activate client' action exposes a client selector.
	/// In 'edit' mode only the key combination is editable.
	/// </summary>
	sealed class HotkeyEditDialog : Form
	{
		#region Private constants
		private const string ACTION_CLIENT_PREFIX = "client:";
		#endregion

		#region Private classes
		private sealed class ActionItem
		{
			public ActionItem(string id, string cycleGroupName, bool isClientSelector, string displayName)
			{
				this.Id = id;
				this.CycleGroupName = cycleGroupName;
				this.IsClientSelector = isClientSelector;
				this.DisplayName = displayName;
			}

			// Direct action id; null when the id depends on the sub-selector value
			public string Id { get; }
			public string CycleGroupName { get; }
			public bool IsClientSelector { get; }
			public string DisplayName { get; }

			public override string ToString() => this.DisplayName;
		}
		#endregion

		#region Private fields
		private readonly IList<(string ActionId, string ActionName, string Hotkey)> _existingBindings;
		private readonly string _editedActionId;
		private readonly string _editedHotkey;
		private readonly bool _isEditMode;

		private readonly Label _actionLabel;
		private readonly ComboBox _actionCombo;
		private readonly Label _subSelectorLabel;
		private readonly ComboBox _directionCombo;
		private readonly ComboBox _clientCombo;
		private readonly Label _captureLabel;
		private readonly TextBox _captureBox;
		private readonly Label _statusLabel;
		private readonly Button _okButton;
		private readonly Button _cancelButton;

		private string _capturedBinding;
		private bool _hasConflict;

		// Control.Visible reports false until the form is shown, so the intended
		// visibility is tracked explicitly for the layout pass
		private bool _isActionVisible;
		private bool _isDirectionVisible;
		private bool _isClientVisible;
		#endregion

		public HotkeyEditDialog(IList<(string ActionId, string DisplayName)> actions,
								IList<string> activeClients,
								IList<(string ActionId, string ActionName, string Hotkey)> existingBindings)
			: this(actions, activeClients, existingBindings, null, null)
		{
		}

		public HotkeyEditDialog(IList<(string ActionId, string DisplayName)> actions,
								IList<string> activeClients,
								IList<(string ActionId, string ActionName, string Hotkey)> existingBindings,
								string editedActionId,
								string editedHotkey)
		{
			this._existingBindings = existingBindings ?? new List<(string, string, string)>();
			this._editedActionId = editedActionId;
			this._editedHotkey = editedHotkey;
			this._isEditMode = editedActionId != null;

			this.Text = this._isEditMode ? Strings.HotkeyDialog_EditTitle : Strings.HotkeyDialog_AddTitle;
			this.FormBorderStyle = FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.ShowInTaskbar = false;
			this.StartPosition = FormStartPosition.CenterParent;
			this.AutoScaleMode = AutoScaleMode.Font;
			this.AutoScaleDimensions = new SizeF(7F, 15F);

			this._actionLabel = new Label { Text = Strings.HotkeyDialog_Action, AutoSize = true };

			this._actionCombo = new ComboBox
			{
				DropDownStyle = ComboBoxStyle.DropDownList,
				Size = new Size(336, 23)
			};
			this._actionCombo.SelectedIndexChanged += this.ActionCombo_SelectedIndexChanged_Handler;

			this._subSelectorLabel = new Label { AutoSize = true };

			this._directionCombo = new ComboBox
			{
				DropDownStyle = ComboBoxStyle.DropDownList,
				Size = new Size(336, 23)
			};
			this._directionCombo.Items.Add(Strings.HotkeyDialog_Forward);
			this._directionCombo.Items.Add(Strings.HotkeyDialog_Backward);
			this._directionCombo.SelectedIndex = 0;

			this._clientCombo = new ComboBox
			{
				DropDownStyle = ComboBoxStyle.DropDownList,
				Size = new Size(336, 23)
			};

			foreach (string client in activeClients ?? new List<string>())
			{
				this._clientCombo.Items.Add(client);
			}

			if (this._clientCombo.Items.Count > 0)
			{
				this._clientCombo.SelectedIndex = 0;
			}

			this._captureLabel = new Label { Text = Strings.HotkeyDialog_CaptureLabel, AutoSize = true };

			this._captureBox = new TextBox
			{
				Size = new Size(336, 23),
				ReadOnly = true,
				PlaceholderText = Strings.HotkeyDialog_CapturePlaceholder
			};
			this._captureBox.KeyDown += this.CaptureBox_KeyDown_Handler;
			this._captureBox.MouseDown += this.CaptureBox_MouseDown_Handler;
			this._captureBox.MouseWheel += this.CaptureBox_MouseWheel_Handler;

			this._statusLabel = new Label { Size = new Size(336, 30), ForeColor = SystemColors.GrayText };

			this._okButton = new Button { Text = this._isEditMode ? Strings.HotkeyDialog_Save : Strings.HotkeyDialog_Add, Size = new Size(75, 27) };
			this._okButton.Click += this.OkButton_Click_Handler;

			this._cancelButton = new Button { Text = Strings.Common_Cancel, Size = new Size(75, 27), DialogResult = DialogResult.Cancel };
			this.CancelButton = this._cancelButton;

			this.Controls.Add(this._actionLabel);
			this.Controls.Add(this._actionCombo);
			this.Controls.Add(this._subSelectorLabel);
			this.Controls.Add(this._directionCombo);
			this.Controls.Add(this._clientCombo);
			this.Controls.Add(this._captureLabel);
			this.Controls.Add(this._captureBox);
			this.Controls.Add(this._statusLabel);
			this.Controls.Add(this._okButton);
			this.Controls.Add(this._cancelButton);

			if (this._isEditMode)
			{
				// Only the key combination is editable, the action stays as it is
				this._isActionVisible = false;
				this.SetCapturedBinding(editedHotkey);
			}
			else
			{
				this._isActionVisible = true;
				this.PopulateActions(actions);
			}

			this.LayoutControls();
		}

		public string SelectedActionId { get; private set; }

		public string HotkeyString { get; private set; }

		// Controls are stacked top to bottom, hidden ones do not leave gaps
		private void LayoutControls()
		{
			const int MARGIN = 12;
			int top = MARGIN;

			void Place(Control control, bool isVisible, int gap)
			{
				control.Visible = isVisible;

				if (!isVisible)
				{
					return;
				}

				control.Location = new Point(MARGIN, top);
				top = control.Bottom + gap;
			}

			bool isSubSelectorVisible = this._isDirectionVisible || this._isClientVisible;

			Place(this._actionLabel, this._isActionVisible, 3);
			Place(this._actionCombo, this._isActionVisible, 10);
			Place(this._subSelectorLabel, isSubSelectorVisible, 3);
			Place(this._directionCombo, this._isDirectionVisible, 10);
			Place(this._clientCombo, this._isClientVisible, 10);
			Place(this._captureLabel, true, 3);
			Place(this._captureBox, true, 8);
			Place(this._statusLabel, true, 10);

			this._okButton.Location = new Point(360 - 75 - 75 - MARGIN - 6, top);
			this._cancelButton.Location = new Point(360 - 75 - MARGIN, top);

			this.ClientSize = new Size(360, this._okButton.Bottom + MARGIN);
		}

		private void PopulateActions(IList<(string ActionId, string DisplayName)> actions)
		{
			HashSet<string> cycleGroups = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			bool hasClientActions = false;

			foreach ((string actionId, string displayName) in actions)
			{
				// Cycle group forward/backward pairs collapse into a single item
				// with a direction sub-selector
				if (HotkeyEditDialog.TryParseCycleActionId(actionId, out string groupName))
				{
					if (cycleGroups.Add(groupName))
					{
						this._actionCombo.Items.Add(new ActionItem(null, groupName, false, string.Format(Strings.HotkeyDialog_CycleGroupItem, groupName)));
					}

					continue;
				}

				// All per-client actions collapse into a single item with a client sub-selector
				if (actionId.StartsWith(HotkeyEditDialog.ACTION_CLIENT_PREFIX, StringComparison.Ordinal))
				{
					hasClientActions = true;
					continue;
				}

				this._actionCombo.Items.Add(new ActionItem(actionId, null, false, displayName));
			}

			if (hasClientActions || (this._clientCombo.Items.Count > 0))
			{
				this._actionCombo.Items.Insert(0, new ActionItem(null, null, true, Strings.HotkeyDialog_ActivateClientItem));
			}

			if (this._actionCombo.Items.Count > 0)
			{
				this._actionCombo.SelectedIndex = 0;
			}
		}

		private static bool TryParseCycleActionId(string actionId, out string groupName)
		{
			groupName = null;

			string[] parts = actionId.Split(new[] { ':' }, 3);

			if ((parts.Length != 3) || (parts[0] != "cycle"))
			{
				return false;
			}

			groupName = parts[2];
			return true;
		}

		private void ActionCombo_SelectedIndexChanged_Handler(object sender, EventArgs e)
		{
			ActionItem action = this._actionCombo.SelectedItem as ActionItem;

			this._isDirectionVisible = action?.CycleGroupName != null;
			this._isClientVisible = (action != null) && action.IsClientSelector;

			if (this._isDirectionVisible)
			{
				this._subSelectorLabel.Text = Strings.HotkeyDialog_Direction;
			}
			else if (this._isClientVisible)
			{
				this._subSelectorLabel.Text = Strings.HotkeyDialog_ClientWindow;
			}

			this.LayoutControls();
			this.RefreshConflictState();
		}

		private void CaptureBox_KeyDown_Handler(object sender, KeyEventArgs e)
		{
			e.SuppressKeyPress = true;
			e.Handled = true;

			if (e.KeyCode == Keys.Escape)
			{
				this.SetCapturedBinding(null);
				return;
			}

			// A modifier alone is not a valid hotkey
			if ((e.KeyCode == Keys.ControlKey) || (e.KeyCode == Keys.ShiftKey) || (e.KeyCode == Keys.Menu))
			{
				return;
			}

			this.SetCapturedBinding((new KeysConverter()).ConvertToInvariantString(e.KeyData));
		}

		private void CaptureBox_MouseDown_Handler(object sender, MouseEventArgs e)
		{
			string button;

			switch (e.Button)
			{
				case MouseButtons.Middle:
					button = MouseBinding.MIDDLE_BUTTON;
					break;
				case MouseButtons.XButton1:
					button = MouseBinding.X_BUTTON_1;
					break;
				case MouseButtons.XButton2:
					button = MouseBinding.X_BUTTON_2;
					break;
				default:
					// Left / right clicks are needed to operate the dialog itself
					return;
			}

			this.SetCapturedBinding(MouseBinding.Compose(Control.ModifierKeys, button));
		}

		private void CaptureBox_MouseWheel_Handler(object sender, MouseEventArgs e)
		{
			if (e.Delta == 0)
			{
				return;
			}

			this.SetCapturedBinding(MouseBinding.Compose(Control.ModifierKeys, e.Delta > 0 ? MouseBinding.SCROLL_UP : MouseBinding.SCROLL_DOWN));
		}

		private void SetCapturedBinding(string binding)
		{
			this._capturedBinding = binding;
			this._captureBox.Text = binding ?? string.Empty;

			this.RefreshConflictState();
		}

		// The captured combination is always displayed; a conflicting one is
		// marked red and explained instead of being silently dropped
		private void RefreshConflictState()
		{
			this._hasConflict = false;

			if (string.IsNullOrEmpty(this._capturedBinding))
			{
				this._captureBox.ForeColor = SystemColors.WindowText;
				this._statusLabel.ForeColor = SystemColors.GrayText;
				this._statusLabel.Text = string.Empty;
				return;
			}

			string conflictingAction = null;

			foreach ((string actionId, string actionName, string hotkey) in this._existingBindings)
			{
				if (!string.Equals(hotkey, this._capturedBinding, StringComparison.OrdinalIgnoreCase))
				{
					continue;
				}

				// The binding being edited does not conflict with itself
				if ((actionId == this._editedActionId) && string.Equals(hotkey, this._editedHotkey, StringComparison.OrdinalIgnoreCase))
				{
					continue;
				}

				conflictingAction = actionName;
				break;
			}

			this._hasConflict = conflictingAction != null;

			this._captureBox.ForeColor = this._hasConflict ? Color.Firebrick : SystemColors.WindowText;
			this._statusLabel.ForeColor = this._hasConflict ? Color.Firebrick : SystemColors.GrayText;
			this._statusLabel.Text = this._hasConflict ? string.Format(Strings.HotkeyDialog_ConflictWith, conflictingAction) : string.Empty;
		}

		private void OkButton_Click_Handler(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(this._capturedBinding))
			{
				this._statusLabel.ForeColor = Color.Firebrick;
				this._statusLabel.Text = Strings.HotkeyDialog_NoBinding;
				return;
			}

			if (this._hasConflict)
			{
				// The message is already displayed by RefreshConflictState
				return;
			}

			if (this._isEditMode)
			{
				this.SelectedActionId = this._editedActionId;
			}
			else
			{
				if (!(this._actionCombo.SelectedItem is ActionItem action))
				{
					this._statusLabel.ForeColor = Color.Firebrick;
					this._statusLabel.Text = Strings.HotkeyDialog_NoAction;
					return;
				}

				if (action.IsClientSelector)
				{
					if (!(this._clientCombo.SelectedItem is string client))
					{
						this._statusLabel.ForeColor = Color.Firebrick;
						this._statusLabel.Text = Strings.HotkeyDialog_NoClient;
						return;
					}

					this.SelectedActionId = HotkeyEditDialog.ACTION_CLIENT_PREFIX + client;
				}
				else if (action.CycleGroupName != null)
				{
					this.SelectedActionId = "cycle:" + (this._directionCombo.SelectedIndex == 0 ? "F" : "B") + ":" + action.CycleGroupName;
				}
				else
				{
					this.SelectedActionId = action.Id;
				}
			}

			this.HotkeyString = this._capturedBinding;

			this.DialogResult = DialogResult.OK;
			this.Close();
		}
	}
}
