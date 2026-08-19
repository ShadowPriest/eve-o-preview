using System;
using System.Drawing;
using System.Windows.Forms;

namespace EveOPreview.View
{
	/// <summary>Minimal single-line text prompt used to name and rename cycle groups</summary>
	sealed class TextPromptDialog : Form
	{
		private readonly TextBox _valueBox;

		public TextPromptDialog(string title, string prompt, string initialValue)
		{
			this.Text = title;
			this.FormBorderStyle = FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.ShowInTaskbar = false;
			this.StartPosition = FormStartPosition.CenterParent;
			this.AutoScaleMode = AutoScaleMode.Font;
			this.AutoScaleDimensions = new SizeF(7F, 15F);
			this.ClientSize = new Size(320, 108);

			Label promptLabel = new Label { Text = prompt, Location = new Point(12, 12), AutoSize = true };

			this._valueBox = new TextBox
			{
				Location = new Point(12, 33),
				Size = new Size(296, 23),
				Text = initialValue ?? string.Empty
			};
			this._valueBox.SelectAll();

			Button okButton = new Button
			{
				Text = "OK",
				Location = new Point(152, 69),
				Size = new Size(75, 27),
				DialogResult = DialogResult.OK
			};

			Button cancelButton = new Button
			{
				Text = "Cancel",
				Location = new Point(233, 69),
				Size = new Size(75, 27),
				DialogResult = DialogResult.Cancel
			};

			this.AcceptButton = okButton;
			this.CancelButton = cancelButton;

			this.Controls.Add(promptLabel);
			this.Controls.Add(this._valueBox);
			this.Controls.Add(okButton);
			this.Controls.Add(cancelButton);
		}

		public string Value => this._valueBox.Text.Trim();
	}
}
