using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using EveOPreview.Configuration;
using EveOPreview.Localization;

namespace EveOPreview.View
{
	/// <summary>
	/// Editor of the preview settings of a single client (or of a whole character group,
	/// when that group is managed as a whole). Every value shown here starts as the one
	/// the client uses right now: its own where it has one, the global one everywhere else
	/// </summary>
	sealed class PreviewSettingsDialog : Form
	{
		#region Private constants
		private const int LABEL_WIDTH = 150;
		private const int EDITOR_LEFT = 168;
		private const int ROW_HEIGHT = 27;
		private const int ANCHOR_ROW_HEIGHT = 56;
		#endregion

		private readonly PreviewSettings _globals;

		private readonly CheckBox _useCustomSettings;
		private readonly TabControl _tabs;

		private NumericUpDown _width;
		private NumericUpDown _height;
		private NumericUpDown _opacity;
		private CheckBox _showFrames;
		private CheckBox _preventPreviews;
		private Panel _preventPreviewColor;
		private CheckBox _zoomEnabled;
		private NumericUpDown _zoomFactor;
		private AnchorPicker _zoomAnchor;

		private CheckBox _showOverlay;
		private CheckBox _overlayAlwaysOnTop;
		private CheckBox _highlightActiveClient;
		private NumericUpDown _highlightThickness;
		private Panel _highlightColor;

		private CheckBox _showClientName;
		private AnchorPicker _labelAnchor;
		private Panel _labelColor;
		private Button _labelFont;
		private CheckBox _labelOutline;
		private NumericUpDown _labelOutlineThickness;
		private Panel _labelOutlineColor;

		private CheckBox _showGroupName;
		private AnchorPicker _groupNameAnchor;
		private Panel _groupNameColor;
		private Button _groupNameFont;
		private CheckBox _groupNameOutline;
		private NumericUpDown _groupNameOutlineThickness;
		private Panel _groupNameOutlineColor;

		public PreviewSettingsDialog(string caption, string groupHint, PreviewSettings values, PreviewSettings globals)
		{
			this._globals = globals;

			this.Text = string.Format(Strings.PreviewSettings_Title, caption);
			this.FormBorderStyle = FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.ShowInTaskbar = false;
			this.StartPosition = FormStartPosition.CenterParent;
			this.AutoScaleMode = AutoScaleMode.Font;
			this.AutoScaleDimensions = new SizeF(7F, 15F);
			this.ClientSize = new Size(420, 452);

			this._useCustomSettings = new CheckBox
			{
				Text = Strings.PreviewSettings_UseCustom,
				Location = new Point(12, 12),
				AutoSize = true,
				Checked = values.UseCustomSettings
			};
			this._useCustomSettings.CheckedChanged += (sender, args) => this._tabs.Enabled = this._useCustomSettings.Checked;

			Label hintLabel = new Label
			{
				Text = groupHint ?? string.Empty,
				Location = new Point(30, 34),
				Size = new Size(378, 30),
				ForeColor = SystemColors.GrayText
			};

			this._tabs = new TabControl
			{
				Location = new Point(12, 68),
				Size = new Size(396, 340),
				Enabled = values.UseCustomSettings
			};

			this._tabs.TabPages.Add(this.CreatePreviewPage());
			this._tabs.TabPages.Add(this.CreateOverlayPage());
			this._tabs.TabPages.Add(this.CreateWindowNamePage());
			this._tabs.TabPages.Add(this.CreateGroupNamePage());

			Button copyGlobalsButton = new Button
			{
				Text = Strings.PreviewSettings_CopyGlobal,
				Location = new Point(12, 415),
				Size = new Size(160, 27)
			};
			copyGlobalsButton.Click += (sender, args) => this.ApplyValues(this._globals);

			Button okButton = new Button
			{
				Text = Strings.Common_Ok,
				Location = new Point(252, 415),
				Size = new Size(75, 27),
				DialogResult = DialogResult.OK
			};

			Button cancelButton = new Button
			{
				Text = Strings.Common_Cancel,
				Location = new Point(333, 415),
				Size = new Size(75, 27),
				DialogResult = DialogResult.Cancel
			};

			this.AcceptButton = okButton;
			this.CancelButton = cancelButton;

			this.Controls.Add(this._useCustomSettings);
			this.Controls.Add(hintLabel);
			this.Controls.Add(this._tabs);
			this.Controls.Add(copyGlobalsButton);
			this.Controls.Add(okButton);
			this.Controls.Add(cancelButton);

			this.ApplyValues(values);
		}

		/// <summary>The edited settings, null when this client is to follow the global ones</summary>
		public PreviewSettings Value
		{
			get
			{
				if (!this._useCustomSettings.Checked)
				{
					return null;
				}

				return new PreviewSettings
				{
					UseCustomSettings = true,

					ThumbnailSize = new Size((int)this._width.Value, (int)this._height.Value),
					ThumbnailOpacity = (double)this._opacity.Value / 100.0,
					ShowThumbnailFrames = this._showFrames.Checked,

					PreventPreviews = this._preventPreviews.Checked,
					PreventPreviewColor = this._preventPreviewColor.BackColor,

					ThumbnailZoomEnabled = this._zoomEnabled.Checked,
					ThumbnailZoomFactor = (int)this._zoomFactor.Value,
					ThumbnailZoomAnchor = this._zoomAnchor.Value,

					ShowThumbnailOverlays = this._showOverlay.Checked,
					OverlayAlwaysOnTop = this._overlayAlwaysOnTop.Checked,
					EnableActiveClientHighlight = this._highlightActiveClient.Checked,
					ActiveClientHighlightThickness = (int)this._highlightThickness.Value,
					ActiveClientHighlightColor = this._highlightColor.BackColor,

					ShowClientName = this._showClientName.Checked,
					OverlayLabelAnchor = this._labelAnchor.Value,
					OverlayLabelColor = this._labelColor.BackColor,
					OverlayLabelFont = this._labelFont.Font,
					OverlayLabelOutlineEnabled = this._labelOutline.Checked,
					OverlayLabelOutlineThickness = (int)this._labelOutlineThickness.Value,
					OverlayLabelOutlineColor = this._labelOutlineColor.BackColor,

					ShowCycleGroupName = this._showGroupName.Checked,
					CycleGroupIndicatorAnchor = this._groupNameAnchor.Value,
					CycleGroupNameColor = this._groupNameColor.BackColor,
					CycleGroupNameFont = this._groupNameFont.Font,
					CycleGroupNameOutlineEnabled = this._groupNameOutline.Checked,
					CycleGroupNameOutlineThickness = (int)this._groupNameOutlineThickness.Value,
					CycleGroupNameOutlineColor = this._groupNameOutlineColor.BackColor
				};
			}
		}

		private void ApplyValues(PreviewSettings values)
		{
			this._width.Value = PreviewSettingsDialog.Clamp(this._width, values.ThumbnailSize.Value.Width);
			this._height.Value = PreviewSettingsDialog.Clamp(this._height, values.ThumbnailSize.Value.Height);
			this._opacity.Value = PreviewSettingsDialog.Clamp(this._opacity, (int)Math.Round(values.ThumbnailOpacity.Value * 100.0));
			this._showFrames.Checked = values.ShowThumbnailFrames.Value;

			this._preventPreviews.Checked = values.PreventPreviews.Value;
			this._preventPreviewColor.BackColor = values.PreventPreviewColor.Value;

			this._zoomEnabled.Checked = values.ThumbnailZoomEnabled.Value;
			this._zoomFactor.Value = PreviewSettingsDialog.Clamp(this._zoomFactor, values.ThumbnailZoomFactor.Value);
			this._zoomAnchor.Value = values.ThumbnailZoomAnchor.Value;

			this._showOverlay.Checked = values.ShowThumbnailOverlays.Value;
			this._overlayAlwaysOnTop.Checked = values.OverlayAlwaysOnTop.Value;
			this._highlightActiveClient.Checked = values.EnableActiveClientHighlight.Value;
			this._highlightThickness.Value = PreviewSettingsDialog.Clamp(this._highlightThickness, values.ActiveClientHighlightThickness.Value);
			this._highlightColor.BackColor = values.ActiveClientHighlightColor.Value;

			this._showClientName.Checked = values.ShowClientName.Value;
			this._labelAnchor.Value = values.OverlayLabelAnchor.Value;
			this._labelColor.BackColor = values.OverlayLabelColor.Value;
			PreviewSettingsDialog.SetFontButton(this._labelFont, values.OverlayLabelFont);
			this._labelOutline.Checked = values.OverlayLabelOutlineEnabled.Value;
			this._labelOutlineThickness.Value = PreviewSettingsDialog.Clamp(this._labelOutlineThickness, values.OverlayLabelOutlineThickness.Value);
			this._labelOutlineColor.BackColor = values.OverlayLabelOutlineColor.Value;

			this._showGroupName.Checked = values.ShowCycleGroupName.Value;
			this._groupNameAnchor.Value = values.CycleGroupIndicatorAnchor.Value;
			this._groupNameColor.BackColor = values.CycleGroupNameColor.Value;
			PreviewSettingsDialog.SetFontButton(this._groupNameFont, values.CycleGroupNameFont);
			this._groupNameOutline.Checked = values.CycleGroupNameOutlineEnabled.Value;
			this._groupNameOutlineThickness.Value = PreviewSettingsDialog.Clamp(this._groupNameOutlineThickness, values.CycleGroupNameOutlineThickness.Value);
			this._groupNameOutlineColor.BackColor = values.CycleGroupNameOutlineColor.Value;
		}

		#region Pages
		private TabPage CreatePreviewPage()
		{
			TabPage page = new TabPage(Strings.Tab_Previews) { BackColor = SystemColors.Control };
			int top = 12;

			this._width = this.AddNumeric(page, Strings.Preview_Width, 64, 4000, ref top);
			this._height = this.AddNumeric(page, Strings.Preview_Height, 36, 4000, ref top);
			this._opacity = this.AddNumeric(page, Strings.Preview_Opacity, 20, 100, ref top);
			this._showFrames = this.AddCheckBox(page, Strings.Overlay_ShowFrames, ref top);
			this._preventPreviews = this.AddCheckBox(page, Strings.Preview_PreventPreviews, ref top);
			this._preventPreviewColor = this.AddColor(page, Strings.Preview_PlaceholderColor, ref top);
			this._zoomEnabled = this.AddCheckBox(page, Strings.Preview_ZoomOnHover, ref top);
			this._zoomFactor = this.AddNumeric(page, Strings.Preview_ZoomFactor, 2, 10, ref top);
			this._zoomAnchor = this.AddAnchor(page, Strings.Preview_ZoomAnchor, ref top);

			return page;
		}

		private TabPage CreateOverlayPage()
		{
			TabPage page = new TabPage(Strings.OverlayTab_General) { BackColor = SystemColors.Control };
			int top = 12;

			this._showOverlay = this.AddCheckBox(page, Strings.Overlay_ShowOverlay, ref top);
			this._overlayAlwaysOnTop = this.AddCheckBox(page, Strings.Overlay_AlwaysOnTop, ref top);
			this._highlightActiveClient = this.AddCheckBox(page, Strings.Overlay_HighlightActiveClient, ref top);
			this._highlightThickness = this.AddNumeric(page, Strings.Overlay_BorderThickness, 1, 6, ref top);
			this._highlightColor = this.AddColor(page, Strings.Overlay_Color, ref top);

			return page;
		}

		private TabPage CreateWindowNamePage()
		{
			TabPage page = new TabPage(Strings.OverlayTab_WindowName) { BackColor = SystemColors.Control };
			int top = 12;

			this._showClientName = this.AddCheckBox(page, Strings.Overlay_ShowWindowName, ref top);
			this._labelAnchor = this.AddAnchor(page, Strings.Overlay_Position, ref top);
			this._labelColor = this.AddColor(page, Strings.Overlay_Color, ref top);
			this._labelFont = this.AddFont(page, Strings.Overlay_Font, ref top);
			this._labelOutline = this.AddCheckBox(page, Strings.Overlay_Outline, ref top);
			this._labelOutlineThickness = this.AddNumeric(page, Strings.Overlay_BorderThickness, 1, 5, ref top);
			this._labelOutlineColor = this.AddColor(page, Strings.Overlay_Color, ref top);

			return page;
		}

		private TabPage CreateGroupNamePage()
		{
			TabPage page = new TabPage(Strings.OverlayTab_GroupName) { BackColor = SystemColors.Control };
			int top = 12;

			this._showGroupName = this.AddCheckBox(page, Strings.Overlay_ShowGroupName, ref top);
			this._groupNameAnchor = this.AddAnchor(page, Strings.Overlay_Position, ref top);
			this._groupNameColor = this.AddColor(page, Strings.Overlay_Color, ref top);
			this._groupNameFont = this.AddFont(page, Strings.Overlay_Font, ref top);
			this._groupNameOutline = this.AddCheckBox(page, Strings.Overlay_Outline, ref top);
			this._groupNameOutlineThickness = this.AddNumeric(page, Strings.Overlay_BorderThickness, 1, 5, ref top);
			this._groupNameOutlineColor = this.AddColor(page, Strings.Overlay_Color, ref top);

			return page;
		}
		#endregion

		#region Row builders
		private CheckBox AddCheckBox(Control parent, string caption, ref int top)
		{
			CheckBox checkBox = new CheckBox
			{
				Text = caption,
				Location = new Point(12, top + 2),
				AutoSize = true
			};

			parent.Controls.Add(checkBox);
			top += PreviewSettingsDialog.ROW_HEIGHT;

			return checkBox;
		}

		private NumericUpDown AddNumeric(Control parent, string caption, int minimum, int maximum, ref int top)
		{
			parent.Controls.Add(PreviewSettingsDialog.CreateCaption(caption, top));

			NumericUpDown editor = new NumericUpDown
			{
				Location = new Point(PreviewSettingsDialog.EDITOR_LEFT, top),
				Size = new Size(80, 23),
				Minimum = minimum,
				Maximum = maximum,
				BorderStyle = BorderStyle.FixedSingle
			};

			parent.Controls.Add(editor);
			top += PreviewSettingsDialog.ROW_HEIGHT;

			return editor;
		}

		private AnchorPicker AddAnchor(Control parent, string caption, ref int top)
		{
			parent.Controls.Add(PreviewSettingsDialog.CreateCaption(caption, top));

			AnchorPicker editor = new AnchorPicker(parent, PreviewSettingsDialog.EDITOR_LEFT, top);
			top += PreviewSettingsDialog.ANCHOR_ROW_HEIGHT;

			return editor;
		}

		private Panel AddColor(Control parent, string caption, ref int top)
		{
			Label captionLabel = PreviewSettingsDialog.CreateCaption(caption, top);

			Panel swatch = new Panel
			{
				Location = new Point(PreviewSettingsDialog.EDITOR_LEFT, top + 2),
				Size = new Size(16, 16),
				BorderStyle = BorderStyle.FixedSingle,
				Cursor = Cursors.Hand
			};

			EventHandler pickColor = (sender, args) =>
			{
				using (ColorDialog dialog = new ColorDialog { Color = swatch.BackColor })
				{
					if (dialog.ShowDialog(this) == DialogResult.OK)
					{
						swatch.BackColor = dialog.Color;
					}
				}
			};

			swatch.Click += pickColor;
			captionLabel.Click += pickColor;

			parent.Controls.Add(captionLabel);
			parent.Controls.Add(swatch);
			top += PreviewSettingsDialog.ROW_HEIGHT;

			return swatch;
		}

		private Button AddFont(Control parent, string caption, ref int top)
		{
			parent.Controls.Add(PreviewSettingsDialog.CreateCaption(caption, top));

			Button editor = new Button
			{
				Location = new Point(PreviewSettingsDialog.EDITOR_LEFT, top - 1),
				Size = new Size(200, 25),
				TextAlign = ContentAlignment.MiddleLeft,

				// The button previews the font it carries, so a large one has to be clipped
				AutoEllipsis = true
			};

			editor.Click += (sender, args) =>
			{
				using (FontDialog dialog = new FontDialog { Font = editor.Font, ShowEffects = false })
				{
					if (dialog.ShowDialog(this) == DialogResult.OK)
					{
						PreviewSettingsDialog.SetFontButton(editor, dialog.Font);
					}
				}
			};

			parent.Controls.Add(editor);
			top += PreviewSettingsDialog.ROW_HEIGHT;

			return editor;
		}

		private static Label CreateCaption(string caption, int top)
		{
			return new Label
			{
				Text = caption,
				Location = new Point(12, top + 4),
				Size = new Size(PreviewSettingsDialog.LABEL_WIDTH, 19),
				AutoEllipsis = true,
				Cursor = Cursors.Default
			};
		}

		private static void SetFontButton(Button button, Font font)
		{
			if (font == null)
			{
				return;
			}

			button.Font = font;
			button.Text = font.Name + ", " + font.SizeInPoints.ToString("0.#") + "pt";
		}

		private static decimal Clamp(NumericUpDown editor, int value)
		{
			return Math.Min(Math.Max(value, editor.Minimum), editor.Maximum);
		}
		#endregion

		/// <summary>
		/// The same 3x3 grid of radio buttons the settings panels use: the buttons sit the
		/// way the anchor points sit on the preview itself
		/// </summary>
		private sealed class AnchorPicker
		{
			private readonly Dictionary<ZoomAnchor, RadioButton> _buttons = new Dictionary<ZoomAnchor, RadioButton>();

			public AnchorPicker(Control parent, int left, int top)
			{
				Panel host = new Panel
				{
					Location = new Point(left, top),
					Size = new Size(60, 50)
				};

				int index = 0;

				// The enum is laid out row by row: NW N NE / W C E / SW S SE
				foreach (ZoomAnchor anchor in Enum.GetValues(typeof(ZoomAnchor)))
				{
					RadioButton button = new RadioButton
					{
						Location = new Point((index % 3) * 20, (index / 3) * 17),
						Size = new Size(14, 13),
						TabStop = true,
						UseVisualStyleBackColor = true
					};

					host.Controls.Add(button);
					this._buttons.Add(anchor, button);

					index++;
				}

				parent.Controls.Add(host);
			}

			public ZoomAnchor Value
			{
				get
				{
					foreach (KeyValuePair<ZoomAnchor, RadioButton> entry in this._buttons)
					{
						if (entry.Value.Checked)
						{
							return entry.Key;
						}
					}

					return ZoomAnchor.NW;
				}

				set
				{
					if (this._buttons.TryGetValue(value, out RadioButton button))
					{
						button.Checked = true;
					}
				}
			}
		}
	}
}
