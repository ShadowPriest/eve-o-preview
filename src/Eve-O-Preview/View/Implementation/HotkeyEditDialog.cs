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
	/// Modal dialog used to create and edit the hotkeys of one action.
	/// In 'add' mode an action is picked first: cycle group actions expose a direction
	/// selector and the 'activate client' action exposes a client selector.
	/// An action can be reached by several combinations, so the dialog edits the whole
	/// set of them: every combination has its own capture field, a new field is added
	/// with the plus button and every field but the first one can be dropped again.
	/// </summary>
	sealed class HotkeyEditDialog : Form
	{
		#region Private constants
		private const string ACTION_CLIENT_PREFIX = "client:";

		private const int DIALOG_WIDTH = 360;
		private const int MARGIN = 12;
		private const int FIELD_WIDTH = 336;
		private const int SMALL_BUTTON_SIZE = 24;
		private const int FRAME_THICKNESS = 2;
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

		/// <summary>One capture field with its frame and the button that drops it</summary>
		private sealed class CaptureRow
		{
			public CaptureRow(Panel frame, TextBox box, Button removeButton)
			{
				this.Frame = frame;
				this.Box = box;
				this.RemoveButton = removeButton;
			}

			/// <summary>Sits behind the field and shows the red frame of an empty one</summary>
			public Panel Frame { get; }

			public TextBox Box { get; }

			/// <summary>Null for the very first row: that one is never dropped</summary>
			public Button RemoveButton { get; }

			public string Binding { get; set; }

			/// <summary>The field has been left empty and is marked as required</summary>
			public bool IsMissing { get; set; }
		}
		#endregion

		#region Private fields
		private readonly IList<(string ActionId, string ActionName, string Hotkey)> _existingBindings;
		private readonly string _editedActionId;
		private readonly bool _isEditMode;

		private readonly Label _actionLabel;
		private readonly ComboBox _actionCombo;
		private readonly Label _subSelectorLabel;
		private readonly ComboBox _directionCombo;
		private readonly ComboBox _clientCombo;
		private readonly Label _captureLabel;
		private readonly Button _addRowButton;
		private readonly Label _statusLabel;
		private readonly Button _okButton;
		private readonly Button _cancelButton;

		private readonly List<CaptureRow> _rows = new List<CaptureRow>();

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
								IList<string> editedHotkeys)
		{
			this._existingBindings = existingBindings ?? new List<(string, string, string)>();
			this._editedActionId = editedActionId;
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
				Size = new Size(HotkeyEditDialog.FIELD_WIDTH, 23)
			};
			this._actionCombo.SelectedIndexChanged += this.ActionCombo_SelectedIndexChanged_Handler;

			this._subSelectorLabel = new Label { AutoSize = true };

			this._directionCombo = new ComboBox
			{
				DropDownStyle = ComboBoxStyle.DropDownList,
				Size = new Size(HotkeyEditDialog.FIELD_WIDTH, 23)
			};
			this._directionCombo.Items.Add(Strings.HotkeyDialog_Forward);
			this._directionCombo.Items.Add(Strings.HotkeyDialog_Backward);
			this._directionCombo.SelectedIndex = 0;
			this._directionCombo.SelectedIndexChanged += this.SubSelector_SelectedIndexChanged_Handler;

			this._clientCombo = new ComboBox
			{
				DropDownStyle = ComboBoxStyle.DropDownList,
				Size = new Size(HotkeyEditDialog.FIELD_WIDTH, 23)
			};

			foreach (string client in activeClients ?? new List<string>())
			{
				this._clientCombo.Items.Add(client);
			}

			if (this._clientCombo.Items.Count > 0)
			{
				this._clientCombo.SelectedIndex = 0;
			}

			this._clientCombo.SelectedIndexChanged += this.SubSelector_SelectedIndexChanged_Handler;

			this._captureLabel = new Label { Text = Strings.HotkeyDialog_CaptureLabel, AutoSize = true };

			this._addRowButton = new Button
			{
				Text = "+",
				Size = new Size(HotkeyEditDialog.SMALL_BUTTON_SIZE, HotkeyEditDialog.SMALL_BUTTON_SIZE),
				Font = new Font(this.Font.FontFamily, 10F, FontStyle.Bold),
				UseVisualStyleBackColor = true
			};
			this._addRowButton.Click += this.AddRowButton_Click_Handler;

			this._statusLabel = new Label { Size = new Size(HotkeyEditDialog.FIELD_WIDTH, 30), ForeColor = SystemColors.GrayText };

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
			this.Controls.Add(this._addRowButton);
			this.Controls.Add(this._statusLabel);
			this.Controls.Add(this._okButton);
			this.Controls.Add(this._cancelButton);

			if (this._isEditMode)
			{
				// Only the combinations are editable, the action stays as it is
				this._isActionVisible = false;
				this.LoadBindings(editedHotkeys);
			}
			else
			{
				this._isActionVisible = true;
				this.PopulateActions(actions);
				this.LoadBindings(this.GetBindingsOfSelectedAction());
			}

			this.LayoutControls();
		}

		public string SelectedActionId { get; private set; }

		/// <summary>Every combination of the action, in the order the fields hold them</summary>
		public IList<string> HotkeyStrings { get; private set; } = new List<string>();

		#region Capture rows
		private void LoadBindings(IList<string> bindings)
		{
			foreach (CaptureRow row in this._rows.ToList())
			{
				this.DropRow(row, false);
			}

			foreach (string binding in bindings ?? new List<string>())
			{
				if (!string.IsNullOrEmpty(binding))
				{
					this.AddRow(binding);
				}
			}

			// There is always at least one field to type into
			if (this._rows.Count == 0)
			{
				this.AddRow(null);
			}

			this.RefreshConflictState();
		}

		private CaptureRow AddRow(string binding)
		{
			bool isFirst = this._rows.Count == 0;
			int boxWidth = isFirst ? HotkeyEditDialog.FIELD_WIDTH : (HotkeyEditDialog.FIELD_WIDTH - HotkeyEditDialog.SMALL_BUTTON_SIZE - 4);

			TextBox box = new TextBox
			{
				Location = new Point(HotkeyEditDialog.FRAME_THICKNESS, HotkeyEditDialog.FRAME_THICKNESS),
				Size = new Size(boxWidth, 23),
				ReadOnly = true,
				PlaceholderText = Strings.HotkeyDialog_CapturePlaceholder,
				Text = binding ?? string.Empty
			};

			box.KeyDown += this.CaptureBox_KeyDown_Handler;
			box.MouseDown += this.CaptureBox_MouseDown_Handler;
			box.MouseWheel += this.CaptureBox_MouseWheel_Handler;
			box.Enter += this.CaptureBox_Enter_Handler;

			// The frame is a plain panel behind the field: a WinForms text box cannot
			// paint a border of its own color
			Panel frame = new Panel
			{
				Size = new Size(boxWidth + (2 * HotkeyEditDialog.FRAME_THICKNESS), 23 + (2 * HotkeyEditDialog.FRAME_THICKNESS)),
				BackColor = SystemColors.Control
			};

			frame.Controls.Add(box);

			Button removeButton = null;

			if (!isFirst)
			{
				removeButton = new Button
				{
					Text = "✕",
					Size = new Size(HotkeyEditDialog.SMALL_BUTTON_SIZE, 23),
					ForeColor = Color.Firebrick,
					FlatStyle = FlatStyle.Flat,
					UseVisualStyleBackColor = true
				};

				removeButton.FlatAppearance.BorderSize = 0;
				removeButton.Click += this.RemoveRowButton_Click_Handler;

				this.Controls.Add(removeButton);
			}

			CaptureRow row = new CaptureRow(frame, box, removeButton) { Binding = binding };

			this._rows.Add(row);
			this.Controls.Add(frame);

			return row;
		}

		private void DropRow(CaptureRow row, bool relayout)
		{
			this._rows.Remove(row);

			this.Controls.Remove(row.Frame);
			row.Frame.Dispose();

			if (row.RemoveButton != null)
			{
				this.Controls.Remove(row.RemoveButton);
				row.RemoveButton.Dispose();
			}

			if (relayout)
			{
				this.LayoutControls();
				this.RefreshConflictState();
			}
		}

		private void AddRowButton_Click_Handler(object sender, EventArgs e)
		{
			CaptureRow row = this.AddRow(null);

			this.LayoutControls();
			this.RefreshConflictState();

			row.Box.Focus();
		}

		private void RemoveRowButton_Click_Handler(object sender, EventArgs e)
		{
			CaptureRow row = this._rows.FirstOrDefault(candidate => object.ReferenceEquals(candidate.RemoveButton, sender));

			if (row != null)
			{
				this.DropRow(row, true);
			}
		}

		private CaptureRow FindRow(object box)
		{
			return this._rows.FirstOrDefault(row => object.ReferenceEquals(row.Box, box));
		}

		// Clicking into a field that was marked as required starts it clean again
		private void CaptureBox_Enter_Handler(object sender, EventArgs e)
		{
			CaptureRow row = this.FindRow(sender);

			if (row != null)
			{
				this.ClearMissingMark(row);
			}
		}

		private void ClearMissingMark(CaptureRow row)
		{
			if (!row.IsMissing)
			{
				return;
			}

			row.IsMissing = false;
			row.Frame.BackColor = SystemColors.Control;
			row.Box.ForeColor = SystemColors.WindowText;
			row.Box.Text = row.Binding ?? string.Empty;
		}

		/// <summary>An empty field cannot be saved: it is framed red and says so</summary>
		private void MarkMissingRows()
		{
			foreach (CaptureRow row in this._rows)
			{
				if (!string.IsNullOrEmpty(row.Binding))
				{
					this.ClearMissingMark(row);
					continue;
				}

				row.IsMissing = true;
				row.Frame.BackColor = Color.Firebrick;
				row.Box.ForeColor = Color.Firebrick;
				row.Box.Text = Strings.HotkeyDialog_Required;
			}
		}
		#endregion

		// Controls are stacked top to bottom, hidden ones do not leave gaps
		private void LayoutControls()
		{
			int top = HotkeyEditDialog.MARGIN;

			void Place(Control control, bool isVisible, int gap)
			{
				control.Visible = isVisible;

				if (!isVisible)
				{
					return;
				}

				control.Location = new Point(HotkeyEditDialog.MARGIN, top);
				top = control.Bottom + gap;
			}

			bool isSubSelectorVisible = this._isDirectionVisible || this._isClientVisible;

			Place(this._actionLabel, this._isActionVisible, 3);
			Place(this._actionCombo, this._isActionVisible, 10);
			Place(this._subSelectorLabel, isSubSelectorVisible, 3);
			Place(this._directionCombo, this._isDirectionVisible, 10);
			Place(this._clientCombo, this._isClientVisible, 10);
			Place(this._captureLabel, true, 3);

			foreach (CaptureRow row in this._rows)
			{
				row.Frame.Location = new Point(HotkeyEditDialog.MARGIN - HotkeyEditDialog.FRAME_THICKNESS, top - HotkeyEditDialog.FRAME_THICKNESS);

				if (row.RemoveButton != null)
				{
					row.RemoveButton.Location = new Point(row.Frame.Right + 2, top);
				}

				top = row.Frame.Bottom - HotkeyEditDialog.FRAME_THICKNESS + 5;
			}

			this._addRowButton.Location = new Point(HotkeyEditDialog.MARGIN, top + 1);
			top = this._addRowButton.Bottom + 8;

			this._statusLabel.Location = new Point(HotkeyEditDialog.MARGIN, top);
			top = this._statusLabel.Bottom + 10;

			this._okButton.Location = new Point(HotkeyEditDialog.DIALOG_WIDTH - 75 - 75 - HotkeyEditDialog.MARGIN - 6, top);
			this._cancelButton.Location = new Point(HotkeyEditDialog.DIALOG_WIDTH - 75 - HotkeyEditDialog.MARGIN, top);

			this.ClientSize = new Size(HotkeyEditDialog.DIALOG_WIDTH, this._okButton.Bottom + HotkeyEditDialog.MARGIN);
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

			// The dialog always shows the whole set of the action, so switching to
			// another one brings in the combinations that action already has
			this.LoadBindings(this.GetBindingsOfSelectedAction());

			this.LayoutControls();
			this.RefreshConflictState();
		}

		private void SubSelector_SelectedIndexChanged_Handler(object sender, EventArgs e)
		{
			this.LoadBindings(this.GetBindingsOfSelectedAction());

			this.LayoutControls();
			this.RefreshConflictState();
		}

		private IList<string> GetBindingsOfSelectedAction()
		{
			string actionId = this.ResolveSelectedActionId();

			if (actionId == null)
			{
				return new List<string>();
			}

			return this._existingBindings.Where(binding => binding.ActionId == actionId).Select(binding => binding.Hotkey).ToList();
		}

		/// <summary>Action id the dialog is working on, null when it cannot be resolved yet</summary>
		private string ResolveSelectedActionId()
		{
			if (this._isEditMode)
			{
				return this._editedActionId;
			}

			if (!(this._actionCombo.SelectedItem is ActionItem action))
			{
				return null;
			}

			if (action.IsClientSelector)
			{
				return (this._clientCombo.SelectedItem is string client) ? (HotkeyEditDialog.ACTION_CLIENT_PREFIX + client) : null;
			}

			if (action.CycleGroupName != null)
			{
				return "cycle:" + (this._directionCombo.SelectedIndex == 0 ? "F" : "B") + ":" + action.CycleGroupName;
			}

			return action.Id;
		}

		#region Capture handlers
		private void CaptureBox_KeyDown_Handler(object sender, KeyEventArgs e)
		{
			e.SuppressKeyPress = true;
			e.Handled = true;

			if (e.KeyCode == Keys.Escape)
			{
				this.SetCapturedBinding(sender, null);
				return;
			}

			// A modifier alone is not a valid hotkey
			if ((e.KeyCode == Keys.ControlKey) || (e.KeyCode == Keys.ShiftKey) || (e.KeyCode == Keys.Menu))
			{
				return;
			}

			this.SetCapturedBinding(sender, (new KeysConverter()).ConvertToInvariantString(e.KeyData));
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

			this.SetCapturedBinding(sender, MouseBinding.Compose(Control.ModifierKeys, button));
		}

		private void CaptureBox_MouseWheel_Handler(object sender, MouseEventArgs e)
		{
			if (e.Delta == 0)
			{
				return;
			}

			this.SetCapturedBinding(sender, MouseBinding.Compose(Control.ModifierKeys, e.Delta > 0 ? MouseBinding.SCROLL_UP : MouseBinding.SCROLL_DOWN));
		}

		private void SetCapturedBinding(object box, string binding)
		{
			CaptureRow row = this.FindRow(box);

			if (row == null)
			{
				return;
			}

			row.Binding = binding;
			row.Box.Text = binding ?? string.Empty;

			this.ClearMissingMark(row);

			this.RefreshConflictState();
		}
		#endregion

		/// <summary>
		/// Every captured combination is always displayed; a conflicting one is marked red
		/// and explained instead of being silently dropped
		/// </summary>
		private void RefreshConflictState()
		{
			string actionId = this.ResolveSelectedActionId();
			string message = null;

			this._hasConflict = false;

			foreach (CaptureRow row in this._rows)
			{
				string conflict = null;

				if (row.IsMissing)
				{
					continue;
				}

				if (!string.IsNullOrEmpty(row.Binding))
				{
					// A combination typed into two fields of this very dialog
					if (this._rows.Any(other => !object.ReferenceEquals(other, row)
												&& string.Equals(other.Binding, row.Binding, StringComparison.OrdinalIgnoreCase)))
					{
						conflict = Strings.HotkeyDialog_DuplicateBinding;
					}
					else
					{
						// The combinations of the edited action itself are not a conflict:
						// this dialog owns all of them
						(string ActionId, string ActionName, string Hotkey) taken = this._existingBindings.FirstOrDefault(
							binding => (binding.ActionId != actionId)
										&& string.Equals(binding.Hotkey, row.Binding, StringComparison.OrdinalIgnoreCase));

						if (taken.ActionId != null)
						{
							conflict = string.Format(Strings.HotkeyDialog_ConflictWith, taken.ActionName);
						}
					}
				}

				row.Box.ForeColor = (conflict == null) ? SystemColors.WindowText : Color.Firebrick;

				if ((conflict != null) && (message == null))
				{
					message = conflict;
					this._hasConflict = true;
				}
			}

			this._statusLabel.ForeColor = this._hasConflict ? Color.Firebrick : SystemColors.GrayText;
			this._statusLabel.Text = message ?? string.Empty;
		}

		private void OkButton_Click_Handler(object sender, EventArgs e)
		{
			// Every field has to carry a combination, an empty one blocks the save
			if (this._rows.Any(row => string.IsNullOrEmpty(row.Binding)))
			{
				this.MarkMissingRows();

				this._statusLabel.ForeColor = Color.Firebrick;
				this._statusLabel.Text = Strings.HotkeyDialog_NoBinding;
				return;
			}

			List<string> bindings = this._rows.Select(row => row.Binding).ToList();

			if (this._hasConflict)
			{
				// The message is already displayed by RefreshConflictState
				return;
			}

			string actionId = this.ResolveSelectedActionId();

			if (actionId == null)
			{
				this._statusLabel.ForeColor = Color.Firebrick;
				this._statusLabel.Text = (this._actionCombo.SelectedItem is ActionItem action) && action.IsClientSelector
											? Strings.HotkeyDialog_NoClient
											: Strings.HotkeyDialog_NoAction;
				return;
			}

			this.SelectedActionId = actionId;
			this.HotkeyStrings = bindings;

			this.DialogResult = DialogResult.OK;
			this.Close();
		}
	}
}
