using System.Drawing;
using System.Windows.Forms;

namespace EveOPreview.View
{
	partial class MainForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>s
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
			ToolStripMenuItem RestoreWindowMenuItem;
			ToolStripMenuItem ExitMenuItem;
			ToolStripMenuItem TitleMenuItem;
			ToolStripSeparator SeparatorMenuItem;
			TabControl ContentTabControl;
			TabPage GeneralTabPage;
			TableLayoutPanel GeneralSettingsPanel;
			TabPage ClientWindowsTabPage;
			TableLayoutPanel ClientWindowsPanel;
			TabPage ThumbnailTabPage;
			Panel ThumbnailSettingsPanel;
			TabControl PreviewSubTabControl;
			TabPage PreviewGeneralSubPage;
			TabPage PreviewVisualSubPage;
			TabPage PreviewRenderingSubPage;
			TabPage PreviewLayoutSubPage;
			TabPage PreviewZoomSubPage;
			TableLayoutPanel PreviewGeneralTablePanel;
			TableLayoutPanel PreviewVisualTablePanel;
			TableLayoutPanel PreviewRenderingTablePanel;
			Label ThumbnailRefreshPeriodLabel;
			Label MinimizedRefreshPeriodLabel;
			Label MinimizedRefreshHintLabel;
			Label MinimizedRenderingNoteLabel;
			TableLayoutPanel PreviewLayoutTablePanel;
			TableLayoutPanel PreviewZoomTablePanel;
			Label HeightLabel;
			Label WidthLabel;
			Label OpacityLabel;
			Label ZoomFactorLabel;
			Label ZoomAnchorLabel;
			TabPage OverlayTabPage;
			Panel OverlaySettingsPanel;
			Label ActiveFrameThicknessLabel;
			TabPage ClientsTabPage;
			Panel ClientsPanel;
			Label ThumbnailsListLabel;
			Label ClientCycleGroupLabel;
			TabPage CycleGroupsTabPage;
			Panel CycleGroupsPanel;
			Label CycleGroupSelectLabel;
			Label CycleGroupClientsLabel;
			Label CycleGroupAddClientLabel;
			TabPage HotkeysTabPage;
			Panel HotkeysPanel;
			TabPage AboutTabPage;
			Panel AboutPanel;
			Label CreditMaintLabel;
			Label DocumentationLinkLabel;
			Label DescriptionLabel;
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			Label NameLabel;
			HideCaptionOnClientsCheckBox = new CheckBox();
			LanguageLabel = new Label();
			LanguageCombo = new ComboBox();
			LanguageRestartHintLabel = new Label();
			DisableAnimationCheckBox = new CheckBox();
			MinimizeInactiveClientsCheckBox = new CheckBox();
			EnableClientLayoutTrackingCheckBox = new CheckBox();
			HideActiveClientThumbnailCheckBox = new CheckBox();
			ShowThumbnailsAlwaysOnTopCheckBox = new CheckBox();
			HideThumbnailsOnLostFocusCheckBox = new CheckBox();
			EnablePerClientThumbnailsLayoutsCheckBox = new CheckBox();
			MinimizeToTrayCheckBox = new CheckBox();
			PreventPreviewColorLabel = new Label();
			PreventPreviewColorButton = new Panel();
			PreventPreviewsCheckBox = new CheckBox();
			ThumbnailSnapToGridCheckBox = new CheckBox();
			ThumbnailSnapToGridSizeYNumericEdit = new NumericUpDown();
			ActiveClientHighlightThicknessNumericEdit = new NumericUpDown();
			SnapYLabel = new Label();
			ThumbnailSnapToGridSizeXNumericEdit = new NumericUpDown();
			SnapXLabel = new Label();
			LockThumbnailLocationCheckbox = new CheckBox();
			ThumbnailsWidthNumericEdit = new NumericUpDown();
			ThumbnailsHeightNumericEdit = new NumericUpDown();
			ThumbnailOpacityTrackBar = new TrackBar();
			ZoomAnchorPanel = new Panel();
			ZoomAnchorNWRadioButton = new RadioButton();
			ZoomAnchorNRadioButton = new RadioButton();
			ZoomAnchorNERadioButton = new RadioButton();
			ZoomAnchorWRadioButton = new RadioButton();
			ZoomAnchorSERadioButton = new RadioButton();
			ZoomAnchorCRadioButton = new RadioButton();
			ZoomAnchorSRadioButton = new RadioButton();
			ZoomAnchorERadioButton = new RadioButton();
			ZoomAnchorSWRadioButton = new RadioButton();
			EnableThumbnailZoomCheckBox = new CheckBox();
			ThumbnailZoomFactorNumericEdit = new NumericUpDown();
			CycleGroupIndicatorPanel = new Panel();
			CycleGroupIndicatorNWRadioButton = new RadioButton();
			CycleGroupIndicatorNRadioButton = new RadioButton();
			CycleGroupIndicatorNERadioButton = new RadioButton();
			CycleGroupIndicatorWRadioButton = new RadioButton();
			CycleGroupIndicatorSERadioButton = new RadioButton();
			CycleGroupIndicatorCRadioButton = new RadioButton();
			CycleGroupIndicatorSRadioButton = new RadioButton();
			CycleGroupIndicatorERadioButton = new RadioButton();
			CycleGroupIndicatorSWRadioButton = new RadioButton();
			LabelOverlayLabelFont = new Label();
			btnLabelFont = new Button();
			OverlayLabelPositionLabel = new Label();
			OverlayLabelColorLabel = new Label();
			OverlayLabelColorButton = new Panel();
			OverlayLabelAnchorPanel = new Panel();
			OverlayLabelNWRadioButton = new RadioButton();
			OverlayLabelNRadioButton = new RadioButton();
			OverlayLabelNERadioButton = new RadioButton();
			OverlayLabelWRadioButton = new RadioButton();
			OverlayLabelSERadioButton = new RadioButton();
			OverlayLabelCRadioButton = new RadioButton();
			OverlayLabelSRadioButton = new RadioButton();
			OverlayLabelERadioButton = new RadioButton();
			OverlayLabelSWRadioButton = new RadioButton();
			HighlightColorLabel = new Label();
			ActiveFrameThicknessLabel = new Label();
			ShowCycleGroupNameCheckBox = new CheckBox();
			OverlaySubTabControl = new TabControl();
			OverlayGeneralSubPage = new TabPage();
			OverlayWindowNameSubPage = new TabPage();
			OverlayGroupNameSubPage = new TabPage();
			OverlayBorderSubPage = new TabPage();
			ShowClientNameCheckBox = new CheckBox();
			OverlayAlwaysOnTopCheckBox = new CheckBox();
			OverlayLabelFontPreviewPanel = new Panel();
			CycleGroupNameColorButton = new Panel();
			CycleGroupNameColorLabel = new Label();
			btnCycleGroupNameFont = new Button();
			CycleGroupNameFontPreviewPanel = new Panel();
			LabelCycleGroupNameFont = new Label();
			CycleGroupNamePositionLabel = new Label();
			ActiveClientHighlightColorButton = new Panel();
			EnableActiveClientHighlightCheckBox = new CheckBox();
			ShowThumbnailOverlaysCheckBox = new CheckBox();
			ShowThumbnailFramesCheckBox = new CheckBox();
			ThumbnailsList = new CheckedListBox();
			StatusBar = new StatusStrip();
			AddHotkeyButton = new Button();
			EditHotkeyButton = new Button();
			RemoveHotkeyButton = new Button();
			HotkeyStatusLabel = new Label();
			HotkeyBindingsListView = new ListView();
			HotkeyActionColumnHeader = new ColumnHeader();
			HotkeyKeyColumnHeader = new ColumnHeader();
			ClientCycleGroupCombo = new ComboBox();
			CycleGroupSelectCombo = new ComboBox();
			CycleGroupRenameButton = new Button();
			CycleGroupAddGroupButton = new Button();
			CycleGroupRemoveGroupButton = new Button();
			CycleGroupClientsListBox = new ListBox();
			CycleGroupMoveUpButton = new Button();
			CycleGroupMoveDownButton = new Button();
			CycleGroupRemoveClientButton = new Button();
			CycleGroupAddClientCombo = new ComboBox();
			CycleGroupAddClientButton = new Button();
			VersionLabel = new Label();
			DocumentationLink = new LinkLabel();
			NotifyIcon = new NotifyIcon(components);
			TrayMenu = new ContextMenuStrip(components);
			RestoreWindowMenuItem = new ToolStripMenuItem();
			ExitMenuItem = new ToolStripMenuItem();
			TitleMenuItem = new ToolStripMenuItem();
			SeparatorMenuItem = new ToolStripSeparator();
			ContentTabControl = new TabControl();
			GeneralTabPage = new TabPage();
			GeneralSettingsPanel = new TableLayoutPanel();
			ClientWindowsTabPage = new TabPage();
			ClientWindowsPanel = new TableLayoutPanel();
			ThumbnailTabPage = new TabPage();
			ThumbnailSettingsPanel = new Panel();
			PreviewSubTabControl = new TabControl();
			PreviewGeneralSubPage = new TabPage();
			PreviewVisualSubPage = new TabPage();
			PreviewRenderingSubPage = new TabPage();
			PreviewLayoutSubPage = new TabPage();
			PreviewZoomSubPage = new TabPage();
			PreviewGeneralTablePanel = new TableLayoutPanel();
			PreviewVisualTablePanel = new TableLayoutPanel();
			PreviewRenderingTablePanel = new TableLayoutPanel();
			ThumbnailRefreshPeriodLabel = new Label();
			ThumbnailRefreshPeriodNumericEdit = new NumericUpDown();
			MinimizedRefreshPeriodLabel = new Label();
			MinimizedClientsRefreshPeriodNumericEdit = new NumericUpDown();
			MinimizedRefreshHintLabel = new Label();
			MinimizedRenderingNoteLabel = new Label();
			PreviewLayoutTablePanel = new TableLayoutPanel();
			PreviewZoomTablePanel = new TableLayoutPanel();
			HeightLabel = new Label();
			WidthLabel = new Label();
			OpacityLabel = new Label();
			ZoomFactorLabel = new Label();
			ZoomAnchorLabel = new Label();
			OverlayTabPage = new TabPage();
			OverlaySettingsPanel = new Panel();
			ClientsTabPage = new TabPage();
			ClientsPanel = new Panel();
			ThumbnailsListLabel = new Label();
			ClientCycleGroupLabel = new Label();
			CycleGroupsTabPage = new TabPage();
			CycleGroupsPanel = new Panel();
			CycleGroupSelectLabel = new Label();
			CycleGroupClientsLabel = new Label();
			CycleGroupAddClientLabel = new Label();
			HotkeysTabPage = new TabPage();
			HotkeysPanel = new Panel();
			AboutTabPage = new TabPage();
			AboutPanel = new Panel();
			CreditMaintLabel = new Label();
			DocumentationLinkLabel = new Label();
			DescriptionLabel = new Label();
			NameLabel = new Label();
			ContentTabControl.SuspendLayout();
			GeneralTabPage.SuspendLayout();
			GeneralSettingsPanel.SuspendLayout();
			ClientWindowsTabPage.SuspendLayout();
			ClientWindowsPanel.SuspendLayout();
			ThumbnailTabPage.SuspendLayout();
			ThumbnailSettingsPanel.SuspendLayout();
			PreviewSubTabControl.SuspendLayout();
			PreviewGeneralSubPage.SuspendLayout();
			PreviewVisualSubPage.SuspendLayout();
			PreviewRenderingSubPage.SuspendLayout();
			PreviewLayoutSubPage.SuspendLayout();
			PreviewZoomSubPage.SuspendLayout();
			PreviewGeneralTablePanel.SuspendLayout();
			PreviewVisualTablePanel.SuspendLayout();
			PreviewRenderingTablePanel.SuspendLayout();
			PreviewLayoutTablePanel.SuspendLayout();
			PreviewZoomTablePanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)ThumbnailRefreshPeriodNumericEdit).BeginInit();
			((System.ComponentModel.ISupportInitialize)MinimizedClientsRefreshPeriodNumericEdit).BeginInit();
			((System.ComponentModel.ISupportInitialize)ThumbnailSnapToGridSizeYNumericEdit).BeginInit();
			((System.ComponentModel.ISupportInitialize)ActiveClientHighlightThicknessNumericEdit).BeginInit();
			((System.ComponentModel.ISupportInitialize)ThumbnailSnapToGridSizeXNumericEdit).BeginInit();
			((System.ComponentModel.ISupportInitialize)ThumbnailsWidthNumericEdit).BeginInit();
			((System.ComponentModel.ISupportInitialize)ThumbnailsHeightNumericEdit).BeginInit();
			((System.ComponentModel.ISupportInitialize)ThumbnailOpacityTrackBar).BeginInit();
			ZoomAnchorPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)ThumbnailZoomFactorNumericEdit).BeginInit();
			OverlayTabPage.SuspendLayout();
			OverlaySettingsPanel.SuspendLayout();
			OverlaySubTabControl.SuspendLayout();
			OverlayGeneralSubPage.SuspendLayout();
			OverlayWindowNameSubPage.SuspendLayout();
			OverlayLabelFontPreviewPanel.SuspendLayout();
			OverlayGroupNameSubPage.SuspendLayout();
			CycleGroupNameFontPreviewPanel.SuspendLayout();
			OverlayBorderSubPage.SuspendLayout();
			CycleGroupIndicatorPanel.SuspendLayout();
			OverlayLabelAnchorPanel.SuspendLayout();
			ClientsTabPage.SuspendLayout();
			ClientsPanel.SuspendLayout();
			CycleGroupsTabPage.SuspendLayout();
			CycleGroupsPanel.SuspendLayout();
			HotkeysTabPage.SuspendLayout();
			HotkeysPanel.SuspendLayout();
			AboutTabPage.SuspendLayout();
			AboutPanel.SuspendLayout();
			TrayMenu.SuspendLayout();
			SuspendLayout();
			// 
			// RestoreWindowMenuItem
			// 
			RestoreWindowMenuItem.Name = "RestoreWindowMenuItem";
			RestoreWindowMenuItem.Size = new Size(153, 22);
			RestoreWindowMenuItem.Text = "Restore";
			RestoreWindowMenuItem.Click += RestoreMainForm_Handler;
			// 
			// ExitMenuItem
			// 
			ExitMenuItem.Name = "ExitMenuItem";
			ExitMenuItem.Size = new Size(153, 22);
			ExitMenuItem.Text = "Exit";
			ExitMenuItem.Click += ExitMenuItemClick_Handler;
			// 
			// TitleMenuItem
			// 
			TitleMenuItem.Enabled = false;
			TitleMenuItem.Name = "TitleMenuItem";
			TitleMenuItem.Size = new Size(153, 22);
			TitleMenuItem.Text = "EVE-O-Preview";
			// 
			// SeparatorMenuItem
			// 
			SeparatorMenuItem.Name = "SeparatorMenuItem";
			SeparatorMenuItem.Size = new Size(150, 6);
			// 
			// ContentTabControl
			// 
			ContentTabControl.Alignment = TabAlignment.Left;
			ContentTabControl.Controls.Add(GeneralTabPage);
			ContentTabControl.Controls.Add(ThumbnailTabPage);
			ContentTabControl.Controls.Add(OverlayTabPage);
			ContentTabControl.Controls.Add(ClientWindowsTabPage);
			ContentTabControl.Controls.Add(ClientsTabPage);
			ContentTabControl.Controls.Add(CycleGroupsTabPage);
			ContentTabControl.Controls.Add(HotkeysTabPage);
			ContentTabControl.Controls.Add(AboutTabPage);
			ContentTabControl.Dock = DockStyle.Fill;
			ContentTabControl.DrawMode = TabDrawMode.OwnerDrawFixed;
			ContentTabControl.ItemSize = new Size(35, 120);
			ContentTabControl.Location = new Point(0, 0);
			ContentTabControl.Margin = new Padding(4);
			ContentTabControl.Multiline = true;
			ContentTabControl.Name = "ContentTabControl";
			ContentTabControl.SelectedIndex = 0;
			ContentTabControl.Size = new Size(455, 330);
			ContentTabControl.SizeMode = TabSizeMode.Fixed;
			ContentTabControl.TabIndex = 6;
			ContentTabControl.DrawItem += ContentTabControl_DrawItem;
			// 
			// GeneralTabPage
			//
			GeneralTabPage.BackColor = SystemColors.Control;
			GeneralTabPage.Controls.Add(GeneralSettingsPanel);
			GeneralTabPage.Location = new Point(124, 4);
			GeneralTabPage.Margin = new Padding(4);
			GeneralTabPage.Name = "GeneralTabPage";
			GeneralTabPage.Padding = new Padding(4);
			GeneralTabPage.Size = new Size(327, 243);
			GeneralTabPage.TabIndex = 0;
			GeneralTabPage.Text = "General";
			//
			// GeneralSettingsPanel
			//
			GeneralSettingsPanel.ColumnCount = 2;
			GeneralSettingsPanel.ColumnStyles.Add(new ColumnStyle());
			GeneralSettingsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			GeneralSettingsPanel.Controls.Add(MinimizeToTrayCheckBox, 0, 0);
			GeneralSettingsPanel.Controls.Add(LanguageLabel, 0, 1);
			GeneralSettingsPanel.Controls.Add(LanguageCombo, 1, 1);
			GeneralSettingsPanel.Controls.Add(LanguageRestartHintLabel, 0, 2);
			GeneralSettingsPanel.Dock = DockStyle.Fill;
			GeneralSettingsPanel.Location = new Point(4, 4);
			GeneralSettingsPanel.Margin = new Padding(4);
			GeneralSettingsPanel.Name = "GeneralSettingsPanel";
			GeneralSettingsPanel.Padding = new Padding(4);
			GeneralSettingsPanel.RowCount = 4;
			GeneralSettingsPanel.RowStyles.Add(new RowStyle());
			GeneralSettingsPanel.RowStyles.Add(new RowStyle());
			GeneralSettingsPanel.RowStyles.Add(new RowStyle());
			GeneralSettingsPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
			GeneralSettingsPanel.Size = new Size(319, 235);
			GeneralSettingsPanel.TabIndex = 18;
			//
			// MinimizeToTrayCheckBox
			//
			GeneralSettingsPanel.SetColumnSpan(MinimizeToTrayCheckBox, 2);
			MinimizeToTrayCheckBox.AutoSize = true;
			MinimizeToTrayCheckBox.Margin = new Padding(4);
			MinimizeToTrayCheckBox.Name = "MinimizeToTrayCheckBox";
			MinimizeToTrayCheckBox.TabIndex = 0;
			MinimizeToTrayCheckBox.Text = "Minimize to System Tray";
			MinimizeToTrayCheckBox.UseVisualStyleBackColor = true;
			MinimizeToTrayCheckBox.CheckedChanged += OptionChanged_Handler;
			//
			// LanguageLabel
			//
			LanguageLabel.Anchor = AnchorStyles.Left;
			LanguageLabel.AutoSize = true;
			LanguageLabel.Margin = new Padding(4, 12, 8, 0);
			LanguageLabel.Name = "LanguageLabel";
			LanguageLabel.TabIndex = 1;
			LanguageLabel.Text = "Language";
			//
			// LanguageCombo
			//
			LanguageCombo.Anchor = AnchorStyles.Left;
			LanguageCombo.DropDownStyle = ComboBoxStyle.DropDownList;
			LanguageCombo.Margin = new Padding(4, 8, 4, 4);
			LanguageCombo.Name = "LanguageCombo";
			LanguageCombo.Size = new Size(160, 23);
			LanguageCombo.TabIndex = 2;
			LanguageCombo.SelectedIndexChanged += LanguageChanged_Handler;
			//
			// LanguageRestartHintLabel
			//
			GeneralSettingsPanel.SetColumnSpan(LanguageRestartHintLabel, 2);
			LanguageRestartHintLabel.AutoSize = true;
			LanguageRestartHintLabel.ForeColor = SystemColors.GrayText;
			LanguageRestartHintLabel.Margin = new Padding(4, 2, 4, 4);
			LanguageRestartHintLabel.Name = "LanguageRestartHintLabel";
			LanguageRestartHintLabel.TabIndex = 3;
			LanguageRestartHintLabel.Text = "Takes effect after restart";
			LanguageRestartHintLabel.Visible = false;
			//
			// ClientWindowsTabPage
			//
			ClientWindowsTabPage.BackColor = SystemColors.Control;
			ClientWindowsTabPage.Controls.Add(ClientWindowsPanel);
			ClientWindowsTabPage.Location = new Point(124, 4);
			ClientWindowsTabPage.Margin = new Padding(4);
			ClientWindowsTabPage.Name = "ClientWindowsTabPage";
			ClientWindowsTabPage.Padding = new Padding(4);
			ClientWindowsTabPage.Size = new Size(327, 243);
			ClientWindowsTabPage.TabIndex = 8;
			ClientWindowsTabPage.Text = "Client Windows";
			//
			// ClientWindowsPanel
			//
			ClientWindowsPanel.ColumnCount = 1;
			ClientWindowsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			ClientWindowsPanel.Controls.Add(EnableClientLayoutTrackingCheckBox, 0, 0);
			ClientWindowsPanel.Controls.Add(MinimizeInactiveClientsCheckBox, 0, 1);
			ClientWindowsPanel.Controls.Add(HideCaptionOnClientsCheckBox, 0, 2);
			ClientWindowsPanel.Controls.Add(DisableAnimationCheckBox, 0, 3);
			ClientWindowsPanel.Controls.Add(MinimizedRenderingNoteLabel, 0, 4);
			ClientWindowsPanel.Dock = DockStyle.Fill;
			ClientWindowsPanel.Location = new Point(4, 4);
			ClientWindowsPanel.Margin = new Padding(4);
			ClientWindowsPanel.Name = "ClientWindowsPanel";
			ClientWindowsPanel.Padding = new Padding(4);
			ClientWindowsPanel.RowCount = 6;
			ClientWindowsPanel.RowStyles.Add(new RowStyle());
			ClientWindowsPanel.RowStyles.Add(new RowStyle());
			ClientWindowsPanel.RowStyles.Add(new RowStyle());
			ClientWindowsPanel.RowStyles.Add(new RowStyle());
			ClientWindowsPanel.RowStyles.Add(new RowStyle());
			ClientWindowsPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
			ClientWindowsPanel.Size = new Size(319, 235);
			ClientWindowsPanel.TabIndex = 0;
			//
			// EnableClientLayoutTrackingCheckBox
			//
			EnableClientLayoutTrackingCheckBox.AutoSize = true;
			EnableClientLayoutTrackingCheckBox.Margin = new Padding(4);
			EnableClientLayoutTrackingCheckBox.Name = "EnableClientLayoutTrackingCheckBox";
			EnableClientLayoutTrackingCheckBox.TabIndex = 0;
			EnableClientLayoutTrackingCheckBox.Text = "Track client locations";
			EnableClientLayoutTrackingCheckBox.UseVisualStyleBackColor = true;
			EnableClientLayoutTrackingCheckBox.CheckedChanged += OptionChanged_Handler;
			//
			// MinimizeInactiveClientsCheckBox
			//
			MinimizeInactiveClientsCheckBox.AutoSize = true;
			MinimizeInactiveClientsCheckBox.Margin = new Padding(4);
			MinimizeInactiveClientsCheckBox.Name = "MinimizeInactiveClientsCheckBox";
			MinimizeInactiveClientsCheckBox.TabIndex = 1;
			MinimizeInactiveClientsCheckBox.Text = "Minimize inactive EVE clients";
			MinimizeInactiveClientsCheckBox.UseVisualStyleBackColor = true;
			MinimizeInactiveClientsCheckBox.CheckedChanged += OptionChanged_Handler;
			//
			// HideCaptionOnClientsCheckBox
			//
			HideCaptionOnClientsCheckBox.AutoSize = true;
			HideCaptionOnClientsCheckBox.Margin = new Padding(4);
			HideCaptionOnClientsCheckBox.Name = "HideCaptionOnClientsCheckBox";
			HideCaptionOnClientsCheckBox.TabIndex = 2;
			HideCaptionOnClientsCheckBox.Text = "Hide caption bar on clients";
			HideCaptionOnClientsCheckBox.UseVisualStyleBackColor = true;
			HideCaptionOnClientsCheckBox.CheckedChanged += OptionChanged_Handler;
			//
			// DisableAnimationCheckBox
			//
			DisableAnimationCheckBox.AutoSize = true;
			DisableAnimationCheckBox.Margin = new Padding(4);
			DisableAnimationCheckBox.Name = "DisableAnimationCheckBox";
			DisableAnimationCheckBox.TabIndex = 3;
			DisableAnimationCheckBox.Text = "Disable minimize/restore animation";
			DisableAnimationCheckBox.UseVisualStyleBackColor = true;
			DisableAnimationCheckBox.CheckedChanged += OptionChanged_Handler;
			//
			// MinimizedRenderingNoteLabel
			//
			MinimizedRenderingNoteLabel.Anchor = AnchorStyles.Left | AnchorStyles.Right;
			MinimizedRenderingNoteLabel.AutoSize = true;
			MinimizedRenderingNoteLabel.ForeColor = SystemColors.GrayText;
			MinimizedRenderingNoteLabel.Margin = new Padding(4, 12, 4, 0);
			MinimizedRenderingNoteLabel.Name = "MinimizedRenderingNoteLabel";
			MinimizedRenderingNoteLabel.TabIndex = 4;
			MinimizedRenderingNoteLabel.Text = "Windows does not render minimized windows in real time, so their previews are refreshed periodically instead - the interval is set on the Previews > Rendering tab.";
			//
			// ThumbnailTabPage
			// 
			ThumbnailTabPage.BackColor = SystemColors.Control;
			ThumbnailTabPage.Controls.Add(ThumbnailSettingsPanel);
			ThumbnailTabPage.Location = new Point(124, 4);
			ThumbnailTabPage.Margin = new Padding(4);
			ThumbnailTabPage.Name = "ThumbnailTabPage";
			ThumbnailTabPage.Padding = new Padding(4);
			ThumbnailTabPage.Size = new Size(327, 243);
			ThumbnailTabPage.TabIndex = 1;
			ThumbnailTabPage.Text = "Previews";
			// 
			// ThumbnailSettingsPanel
			// 
			ThumbnailSettingsPanel.Controls.Add(PreviewSubTabControl);
			ThumbnailSettingsPanel.Dock = DockStyle.Fill;
			ThumbnailSettingsPanel.Location = new Point(4, 4);
			ThumbnailSettingsPanel.Margin = new Padding(4);
			ThumbnailSettingsPanel.Name = "ThumbnailSettingsPanel";
			ThumbnailSettingsPanel.Padding = new Padding(2, 0, 0, 0);
			ThumbnailSettingsPanel.Size = new Size(319, 235);
			ThumbnailSettingsPanel.TabIndex = 19;
			//
			// PreviewSubTabControl
			//
			PreviewSubTabControl.Controls.Add(PreviewGeneralSubPage);
			PreviewSubTabControl.Controls.Add(PreviewVisualSubPage);
			PreviewSubTabControl.Controls.Add(PreviewRenderingSubPage);
			PreviewSubTabControl.Controls.Add(PreviewLayoutSubPage);
			PreviewSubTabControl.Controls.Add(PreviewZoomSubPage);
			PreviewSubTabControl.Dock = DockStyle.Fill;
			PreviewSubTabControl.Location = new Point(2, 0);
			PreviewSubTabControl.Name = "PreviewSubTabControl";
			PreviewSubTabControl.SelectedIndex = 0;
			PreviewSubTabControl.Size = new Size(317, 235);
			PreviewSubTabControl.TabIndex = 0;
			//
			// PreviewGeneralSubPage
			//
			PreviewGeneralSubPage.BackColor = SystemColors.Control;
			PreviewGeneralSubPage.Controls.Add(PreviewGeneralTablePanel);
			PreviewGeneralSubPage.Location = new Point(4, 24);
			PreviewGeneralSubPage.Name = "PreviewGeneralSubPage";
			PreviewGeneralSubPage.Padding = new Padding(3);
			PreviewGeneralSubPage.Size = new Size(309, 207);
			PreviewGeneralSubPage.TabIndex = 0;
			PreviewGeneralSubPage.Text = "General";
			//
			// PreviewGeneralTablePanel
			//
			PreviewGeneralTablePanel.ColumnCount = 2;
			PreviewGeneralTablePanel.ColumnStyles.Add(new ColumnStyle());
			PreviewGeneralTablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			PreviewGeneralTablePanel.Controls.Add(HideActiveClientThumbnailCheckBox, 0, 0);
			PreviewGeneralTablePanel.Controls.Add(ShowThumbnailsAlwaysOnTopCheckBox, 0, 1);
			PreviewGeneralTablePanel.Controls.Add(HideThumbnailsOnLostFocusCheckBox, 0, 2);
			PreviewGeneralTablePanel.Dock = DockStyle.Fill;
			PreviewGeneralTablePanel.Location = new Point(3, 3);
			PreviewGeneralTablePanel.Name = "PreviewGeneralTablePanel";
			PreviewGeneralTablePanel.Padding = new Padding(6);
			PreviewGeneralTablePanel.RowCount = 4;
			PreviewGeneralTablePanel.RowStyles.Add(new RowStyle());
			PreviewGeneralTablePanel.RowStyles.Add(new RowStyle());
			PreviewGeneralTablePanel.RowStyles.Add(new RowStyle());
			PreviewGeneralTablePanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
			PreviewGeneralTablePanel.Size = new Size(303, 201);
			PreviewGeneralTablePanel.TabIndex = 0;
			//
			// HideActiveClientThumbnailCheckBox
			//
			PreviewGeneralTablePanel.SetColumnSpan(HideActiveClientThumbnailCheckBox, 2);
			HideActiveClientThumbnailCheckBox.AutoSize = true;
			HideActiveClientThumbnailCheckBox.Checked = true;
			HideActiveClientThumbnailCheckBox.CheckState = CheckState.Checked;
			HideActiveClientThumbnailCheckBox.Margin = new Padding(4);
			HideActiveClientThumbnailCheckBox.Name = "HideActiveClientThumbnailCheckBox";
			HideActiveClientThumbnailCheckBox.TabIndex = 0;
			HideActiveClientThumbnailCheckBox.Text = "Hide preview of active EVE client";
			HideActiveClientThumbnailCheckBox.UseVisualStyleBackColor = true;
			HideActiveClientThumbnailCheckBox.CheckedChanged += OptionChanged_Handler;
			//
			// ShowThumbnailsAlwaysOnTopCheckBox
			//
			PreviewGeneralTablePanel.SetColumnSpan(ShowThumbnailsAlwaysOnTopCheckBox, 2);
			ShowThumbnailsAlwaysOnTopCheckBox.AutoSize = true;
			ShowThumbnailsAlwaysOnTopCheckBox.Checked = true;
			ShowThumbnailsAlwaysOnTopCheckBox.CheckState = CheckState.Checked;
			ShowThumbnailsAlwaysOnTopCheckBox.Margin = new Padding(4);
			ShowThumbnailsAlwaysOnTopCheckBox.Name = "ShowThumbnailsAlwaysOnTopCheckBox";
			ShowThumbnailsAlwaysOnTopCheckBox.RightToLeft = RightToLeft.No;
			ShowThumbnailsAlwaysOnTopCheckBox.TabIndex = 1;
			ShowThumbnailsAlwaysOnTopCheckBox.Text = "Previews always on top";
			ShowThumbnailsAlwaysOnTopCheckBox.UseVisualStyleBackColor = true;
			ShowThumbnailsAlwaysOnTopCheckBox.CheckedChanged += OptionChanged_Handler;
			//
			// HideThumbnailsOnLostFocusCheckBox
			//
			PreviewGeneralTablePanel.SetColumnSpan(HideThumbnailsOnLostFocusCheckBox, 2);
			HideThumbnailsOnLostFocusCheckBox.AutoSize = true;
			HideThumbnailsOnLostFocusCheckBox.Checked = true;
			HideThumbnailsOnLostFocusCheckBox.CheckState = CheckState.Checked;
			HideThumbnailsOnLostFocusCheckBox.Margin = new Padding(4);
			HideThumbnailsOnLostFocusCheckBox.Name = "HideThumbnailsOnLostFocusCheckBox";
			HideThumbnailsOnLostFocusCheckBox.TabIndex = 2;
			HideThumbnailsOnLostFocusCheckBox.Text = "Hide previews when EVE client is not active";
			HideThumbnailsOnLostFocusCheckBox.UseVisualStyleBackColor = true;
			HideThumbnailsOnLostFocusCheckBox.CheckedChanged += OptionChanged_Handler;
			//
			// OpacityLabel
			//
			OpacityLabel.Anchor = AnchorStyles.Left;
			OpacityLabel.AutoSize = true;
			OpacityLabel.Margin = new Padding(4, 0, 8, 0);
			OpacityLabel.Name = "OpacityLabel";
			OpacityLabel.TabIndex = 3;
			OpacityLabel.Text = "Opacity";
			//
			// ThumbnailOpacityTrackBar
			//
			ThumbnailOpacityTrackBar.Anchor = AnchorStyles.Left | AnchorStyles.Right;
			ThumbnailOpacityTrackBar.AutoSize = false;
			ThumbnailOpacityTrackBar.LargeChange = 10;
			ThumbnailOpacityTrackBar.Margin = new Padding(4);
			ThumbnailOpacityTrackBar.Maximum = 100;
			ThumbnailOpacityTrackBar.Minimum = 20;
			ThumbnailOpacityTrackBar.Name = "ThumbnailOpacityTrackBar";
			ThumbnailOpacityTrackBar.Size = new Size(180, 28);
			ThumbnailOpacityTrackBar.TabIndex = 4;
			ThumbnailOpacityTrackBar.TickFrequency = 10;
			ThumbnailOpacityTrackBar.Value = 20;
			ThumbnailOpacityTrackBar.ValueChanged += OptionChanged_Handler;
			//
			// PreventPreviewsCheckBox
			//
			PreviewVisualTablePanel.SetColumnSpan(PreventPreviewsCheckBox, 2);
			PreventPreviewsCheckBox.AutoSize = true;
			PreventPreviewsCheckBox.Margin = new Padding(4);
			PreventPreviewsCheckBox.Name = "PreventPreviewsCheckBox";
			PreventPreviewsCheckBox.TabIndex = 5;
			PreventPreviewsCheckBox.Text = "Do not display previews";
			PreventPreviewsCheckBox.UseVisualStyleBackColor = true;
			PreventPreviewsCheckBox.CheckedChanged += OptionChanged_Handler;
			//
			// PreventPreviewColorLabel
			//
			PreventPreviewColorLabel.Anchor = AnchorStyles.Left;
			PreventPreviewColorLabel.AutoSize = true;
			PreventPreviewColorLabel.Margin = new Padding(4, 0, 8, 0);
			PreventPreviewColorLabel.Name = "PreventPreviewColorLabel";
			PreventPreviewColorLabel.TabIndex = 6;
			PreventPreviewColorLabel.Text = "Placeholder color";
			//
			// PreventPreviewColorButton
			//
			PreventPreviewColorButton.Anchor = AnchorStyles.Left;
			PreventPreviewColorButton.BorderStyle = BorderStyle.FixedSingle;
			PreventPreviewColorButton.Margin = new Padding(4);
			PreventPreviewColorButton.Name = "PreventPreviewColorButton";
			PreventPreviewColorButton.Size = new Size(72, 21);
			PreventPreviewColorButton.TabIndex = 7;
			PreventPreviewColorButton.Click += PreventPreviewColorButton_Click;
			//
			// PreviewVisualSubPage
			//
			PreviewVisualSubPage.BackColor = SystemColors.Control;
			PreviewVisualSubPage.Controls.Add(PreviewVisualTablePanel);
			PreviewVisualSubPage.Location = new Point(4, 24);
			PreviewVisualSubPage.Name = "PreviewVisualSubPage";
			PreviewVisualSubPage.Padding = new Padding(3);
			PreviewVisualSubPage.Size = new Size(309, 207);
			PreviewVisualSubPage.TabIndex = 3;
			PreviewVisualSubPage.Text = "Visualization";
			//
			// PreviewVisualTablePanel
			//
			PreviewVisualTablePanel.ColumnCount = 2;
			PreviewVisualTablePanel.ColumnStyles.Add(new ColumnStyle());
			PreviewVisualTablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			PreviewVisualTablePanel.Controls.Add(OpacityLabel, 0, 0);
			PreviewVisualTablePanel.Controls.Add(ThumbnailOpacityTrackBar, 1, 0);
			PreviewVisualTablePanel.Controls.Add(PreventPreviewsCheckBox, 0, 1);
			PreviewVisualTablePanel.Controls.Add(PreventPreviewColorLabel, 0, 2);
			PreviewVisualTablePanel.Controls.Add(PreventPreviewColorButton, 1, 2);
			PreviewVisualTablePanel.Dock = DockStyle.Fill;
			PreviewVisualTablePanel.Location = new Point(3, 3);
			PreviewVisualTablePanel.Name = "PreviewVisualTablePanel";
			PreviewVisualTablePanel.Padding = new Padding(6);
			PreviewVisualTablePanel.RowCount = 4;
			PreviewVisualTablePanel.RowStyles.Add(new RowStyle());
			PreviewVisualTablePanel.RowStyles.Add(new RowStyle());
			PreviewVisualTablePanel.RowStyles.Add(new RowStyle());
			PreviewVisualTablePanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
			PreviewVisualTablePanel.Size = new Size(303, 201);
			PreviewVisualTablePanel.TabIndex = 0;
			//
			// PreviewRenderingSubPage
			//
			PreviewRenderingSubPage.BackColor = SystemColors.Control;
			PreviewRenderingSubPage.Controls.Add(PreviewRenderingTablePanel);
			PreviewRenderingSubPage.Location = new Point(4, 24);
			PreviewRenderingSubPage.Name = "PreviewRenderingSubPage";
			PreviewRenderingSubPage.Padding = new Padding(3);
			PreviewRenderingSubPage.Size = new Size(309, 207);
			PreviewRenderingSubPage.TabIndex = 4;
			PreviewRenderingSubPage.Text = "Rendering";
			//
			// PreviewRenderingTablePanel
			//
			PreviewRenderingTablePanel.ColumnCount = 2;
			PreviewRenderingTablePanel.ColumnStyles.Add(new ColumnStyle());
			PreviewRenderingTablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			PreviewRenderingTablePanel.Controls.Add(ThumbnailRefreshPeriodLabel, 0, 0);
			PreviewRenderingTablePanel.Controls.Add(ThumbnailRefreshPeriodNumericEdit, 1, 0);
			PreviewRenderingTablePanel.Controls.Add(MinimizedRefreshPeriodLabel, 0, 1);
			PreviewRenderingTablePanel.Controls.Add(MinimizedClientsRefreshPeriodNumericEdit, 1, 1);
			PreviewRenderingTablePanel.Controls.Add(MinimizedRefreshHintLabel, 0, 2);
			PreviewRenderingTablePanel.Dock = DockStyle.Fill;
			PreviewRenderingTablePanel.Location = new Point(3, 3);
			PreviewRenderingTablePanel.Name = "PreviewRenderingTablePanel";
			PreviewRenderingTablePanel.Padding = new Padding(6);
			PreviewRenderingTablePanel.RowCount = 4;
			PreviewRenderingTablePanel.RowStyles.Add(new RowStyle());
			PreviewRenderingTablePanel.RowStyles.Add(new RowStyle());
			PreviewRenderingTablePanel.RowStyles.Add(new RowStyle());
			PreviewRenderingTablePanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
			PreviewRenderingTablePanel.Size = new Size(303, 201);
			PreviewRenderingTablePanel.TabIndex = 0;
			//
			// ThumbnailRefreshPeriodLabel
			//
			ThumbnailRefreshPeriodLabel.Anchor = AnchorStyles.Left;
			ThumbnailRefreshPeriodLabel.AutoSize = true;
			ThumbnailRefreshPeriodLabel.Margin = new Padding(4, 0, 8, 0);
			ThumbnailRefreshPeriodLabel.Name = "ThumbnailRefreshPeriodLabel";
			ThumbnailRefreshPeriodLabel.TabIndex = 0;
			ThumbnailRefreshPeriodLabel.Text = "Preview refresh period (ms)";
			//
			// ThumbnailRefreshPeriodNumericEdit
			//
			ThumbnailRefreshPeriodNumericEdit.Anchor = AnchorStyles.Left;
			ThumbnailRefreshPeriodNumericEdit.BackColor = SystemColors.Window;
			ThumbnailRefreshPeriodNumericEdit.BorderStyle = BorderStyle.FixedSingle;
			ThumbnailRefreshPeriodNumericEdit.CausesValidation = false;
			ThumbnailRefreshPeriodNumericEdit.Increment = new decimal(new int[] { 50, 0, 0, 0 });
			ThumbnailRefreshPeriodNumericEdit.Margin = new Padding(4);
			ThumbnailRefreshPeriodNumericEdit.Minimum = new decimal(new int[] { 300, 0, 0, 0 });
			ThumbnailRefreshPeriodNumericEdit.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
			ThumbnailRefreshPeriodNumericEdit.Name = "ThumbnailRefreshPeriodNumericEdit";
			ThumbnailRefreshPeriodNumericEdit.Size = new Size(72, 23);
			ThumbnailRefreshPeriodNumericEdit.TabIndex = 1;
			ThumbnailRefreshPeriodNumericEdit.Value = new decimal(new int[] { 500, 0, 0, 0 });
			ThumbnailRefreshPeriodNumericEdit.ValueChanged += OptionChanged_Handler;
			//
			// MinimizedRefreshPeriodLabel
			//
			MinimizedRefreshPeriodLabel.Anchor = AnchorStyles.Left;
			MinimizedRefreshPeriodLabel.AutoSize = true;
			MinimizedRefreshPeriodLabel.Margin = new Padding(4, 0, 8, 0);
			MinimizedRefreshPeriodLabel.Name = "MinimizedRefreshPeriodLabel";
			MinimizedRefreshPeriodLabel.TabIndex = 2;
			MinimizedRefreshPeriodLabel.Text = "Minimized clients refresh period (s)";
			//
			// MinimizedClientsRefreshPeriodNumericEdit
			//
			MinimizedClientsRefreshPeriodNumericEdit.Anchor = AnchorStyles.Left;
			MinimizedClientsRefreshPeriodNumericEdit.BackColor = SystemColors.Window;
			MinimizedClientsRefreshPeriodNumericEdit.BorderStyle = BorderStyle.FixedSingle;
			MinimizedClientsRefreshPeriodNumericEdit.CausesValidation = false;
			MinimizedClientsRefreshPeriodNumericEdit.Margin = new Padding(4);
			MinimizedClientsRefreshPeriodNumericEdit.Maximum = new decimal(new int[] { 300, 0, 0, 0 });
			MinimizedClientsRefreshPeriodNumericEdit.Name = "MinimizedClientsRefreshPeriodNumericEdit";
			MinimizedClientsRefreshPeriodNumericEdit.Size = new Size(72, 23);
			MinimizedClientsRefreshPeriodNumericEdit.TabIndex = 3;
			MinimizedClientsRefreshPeriodNumericEdit.Value = new decimal(new int[] { 5, 0, 0, 0 });
			MinimizedClientsRefreshPeriodNumericEdit.ValueChanged += OptionChanged_Handler;
			//
			// MinimizedRefreshHintLabel
			//
			PreviewRenderingTablePanel.SetColumnSpan(MinimizedRefreshHintLabel, 2);
			MinimizedRefreshHintLabel.Anchor = AnchorStyles.Left | AnchorStyles.Right;
			MinimizedRefreshHintLabel.AutoSize = true;
			MinimizedRefreshHintLabel.ForeColor = SystemColors.GrayText;
			MinimizedRefreshHintLabel.Margin = new Padding(4, 12, 4, 0);
			MinimizedRefreshHintLabel.Name = "MinimizedRefreshHintLabel";
			MinimizedRefreshHintLabel.TabIndex = 4;
			MinimizedRefreshHintLabel.Text = "These intervals drive the background refresh of minimized clients: Windows cannot render a minimized window in real time, so it is briefly woken up on this schedule to redraw its preview. 0 disables the refresh.";
			//
			// PreviewLayoutSubPage
			//
			PreviewLayoutSubPage.BackColor = SystemColors.Control;
			PreviewLayoutSubPage.Controls.Add(PreviewLayoutTablePanel);
			PreviewLayoutSubPage.Location = new Point(4, 24);
			PreviewLayoutSubPage.Name = "PreviewLayoutSubPage";
			PreviewLayoutSubPage.Padding = new Padding(3);
			PreviewLayoutSubPage.Size = new Size(309, 207);
			PreviewLayoutSubPage.TabIndex = 1;
			PreviewLayoutSubPage.Text = "Layout";
			//
			// PreviewLayoutTablePanel
			//
			PreviewLayoutTablePanel.ColumnCount = 2;
			PreviewLayoutTablePanel.ColumnStyles.Add(new ColumnStyle());
			PreviewLayoutTablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			PreviewLayoutTablePanel.Controls.Add(WidthLabel, 0, 0);
			PreviewLayoutTablePanel.Controls.Add(ThumbnailsWidthNumericEdit, 1, 0);
			PreviewLayoutTablePanel.Controls.Add(HeightLabel, 0, 1);
			PreviewLayoutTablePanel.Controls.Add(ThumbnailsHeightNumericEdit, 1, 1);
			PreviewLayoutTablePanel.Controls.Add(LockThumbnailLocationCheckbox, 0, 2);
			PreviewLayoutTablePanel.Controls.Add(EnablePerClientThumbnailsLayoutsCheckBox, 0, 3);
			PreviewLayoutTablePanel.Controls.Add(ThumbnailSnapToGridCheckBox, 0, 4);
			PreviewLayoutTablePanel.Controls.Add(SnapXLabel, 0, 5);
			PreviewLayoutTablePanel.Controls.Add(ThumbnailSnapToGridSizeXNumericEdit, 1, 5);
			PreviewLayoutTablePanel.Controls.Add(SnapYLabel, 0, 6);
			PreviewLayoutTablePanel.Controls.Add(ThumbnailSnapToGridSizeYNumericEdit, 1, 6);
			PreviewLayoutTablePanel.Dock = DockStyle.Fill;
			PreviewLayoutTablePanel.Location = new Point(3, 3);
			PreviewLayoutTablePanel.Name = "PreviewLayoutTablePanel";
			PreviewLayoutTablePanel.Padding = new Padding(6);
			PreviewLayoutTablePanel.RowCount = 8;
			PreviewLayoutTablePanel.RowStyles.Add(new RowStyle());
			PreviewLayoutTablePanel.RowStyles.Add(new RowStyle());
			PreviewLayoutTablePanel.RowStyles.Add(new RowStyle());
			PreviewLayoutTablePanel.RowStyles.Add(new RowStyle());
			PreviewLayoutTablePanel.RowStyles.Add(new RowStyle());
			PreviewLayoutTablePanel.RowStyles.Add(new RowStyle());
			PreviewLayoutTablePanel.RowStyles.Add(new RowStyle());
			PreviewLayoutTablePanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
			PreviewLayoutTablePanel.Size = new Size(303, 201);
			PreviewLayoutTablePanel.TabIndex = 0;
			//
			// WidthLabel
			//
			WidthLabel.Anchor = AnchorStyles.Left;
			WidthLabel.AutoSize = true;
			WidthLabel.Margin = new Padding(4, 0, 8, 0);
			WidthLabel.Name = "WidthLabel";
			WidthLabel.TabIndex = 0;
			WidthLabel.Text = "Preview width";
			//
			// ThumbnailsWidthNumericEdit
			//
			ThumbnailsWidthNumericEdit.Anchor = AnchorStyles.Left;
			ThumbnailsWidthNumericEdit.BackColor = SystemColors.Window;
			ThumbnailsWidthNumericEdit.BorderStyle = BorderStyle.FixedSingle;
			ThumbnailsWidthNumericEdit.CausesValidation = false;
			ThumbnailsWidthNumericEdit.Increment = new decimal(new int[] { 10, 0, 0, 0 });
			ThumbnailsWidthNumericEdit.Margin = new Padding(4);
			ThumbnailsWidthNumericEdit.Maximum = new decimal(new int[] { 999999, 0, 0, 0 });
			ThumbnailsWidthNumericEdit.Name = "ThumbnailsWidthNumericEdit";
			ThumbnailsWidthNumericEdit.Size = new Size(72, 23);
			ThumbnailsWidthNumericEdit.TabIndex = 1;
			ThumbnailsWidthNumericEdit.Value = new decimal(new int[] { 100, 0, 0, 0 });
			ThumbnailsWidthNumericEdit.ValueChanged += ThumbnailSizeChanged_Handler;
			//
			// HeightLabel
			//
			HeightLabel.Anchor = AnchorStyles.Left;
			HeightLabel.AutoSize = true;
			HeightLabel.Margin = new Padding(4, 0, 8, 0);
			HeightLabel.Name = "HeightLabel";
			HeightLabel.TabIndex = 2;
			HeightLabel.Text = "Preview height";
			//
			// ThumbnailsHeightNumericEdit
			//
			ThumbnailsHeightNumericEdit.Anchor = AnchorStyles.Left;
			ThumbnailsHeightNumericEdit.BackColor = SystemColors.Window;
			ThumbnailsHeightNumericEdit.BorderStyle = BorderStyle.FixedSingle;
			ThumbnailsHeightNumericEdit.CausesValidation = false;
			ThumbnailsHeightNumericEdit.Increment = new decimal(new int[] { 10, 0, 0, 0 });
			ThumbnailsHeightNumericEdit.Margin = new Padding(4);
			ThumbnailsHeightNumericEdit.Maximum = new decimal(new int[] { 99999999, 0, 0, 0 });
			ThumbnailsHeightNumericEdit.Name = "ThumbnailsHeightNumericEdit";
			ThumbnailsHeightNumericEdit.Size = new Size(72, 23);
			ThumbnailsHeightNumericEdit.TabIndex = 3;
			ThumbnailsHeightNumericEdit.Value = new decimal(new int[] { 70, 0, 0, 0 });
			ThumbnailsHeightNumericEdit.ValueChanged += ThumbnailSizeChanged_Handler;
			//
			// LockThumbnailLocationCheckbox
			//
			PreviewLayoutTablePanel.SetColumnSpan(LockThumbnailLocationCheckbox, 2);
			LockThumbnailLocationCheckbox.AutoSize = true;
			LockThumbnailLocationCheckbox.Margin = new Padding(4, 8, 4, 4);
			LockThumbnailLocationCheckbox.Name = "LockThumbnailLocationCheckbox";
			LockThumbnailLocationCheckbox.TabIndex = 4;
			LockThumbnailLocationCheckbox.Text = "Lock preview location";
			LockThumbnailLocationCheckbox.UseVisualStyleBackColor = true;
			LockThumbnailLocationCheckbox.CheckedChanged += OptionChanged_Handler;
			//
			// EnablePerClientThumbnailsLayoutsCheckBox
			//
			PreviewLayoutTablePanel.SetColumnSpan(EnablePerClientThumbnailsLayoutsCheckBox, 2);
			EnablePerClientThumbnailsLayoutsCheckBox.AutoSize = true;
			EnablePerClientThumbnailsLayoutsCheckBox.Checked = true;
			EnablePerClientThumbnailsLayoutsCheckBox.CheckState = CheckState.Checked;
			EnablePerClientThumbnailsLayoutsCheckBox.Margin = new Padding(4);
			EnablePerClientThumbnailsLayoutsCheckBox.Name = "EnablePerClientThumbnailsLayoutsCheckBox";
			EnablePerClientThumbnailsLayoutsCheckBox.TabIndex = 5;
			EnablePerClientThumbnailsLayoutsCheckBox.Text = "Unique layout for each EVE client";
			EnablePerClientThumbnailsLayoutsCheckBox.UseVisualStyleBackColor = true;
			EnablePerClientThumbnailsLayoutsCheckBox.CheckedChanged += OptionChanged_Handler;
			//
			// ThumbnailSnapToGridCheckBox
			//
			PreviewLayoutTablePanel.SetColumnSpan(ThumbnailSnapToGridCheckBox, 2);
			ThumbnailSnapToGridCheckBox.AutoSize = true;
			ThumbnailSnapToGridCheckBox.Margin = new Padding(4);
			ThumbnailSnapToGridCheckBox.Name = "ThumbnailSnapToGridCheckBox";
			ThumbnailSnapToGridCheckBox.TabIndex = 6;
			ThumbnailSnapToGridCheckBox.Text = "Snap previews to grid";
			ThumbnailSnapToGridCheckBox.UseVisualStyleBackColor = true;
			ThumbnailSnapToGridCheckBox.CheckedChanged += OptionChanged_Handler;
			//
			// SnapXLabel
			//
			SnapXLabel.Anchor = AnchorStyles.Left;
			SnapXLabel.AutoSize = true;
			SnapXLabel.Margin = new Padding(20, 0, 8, 0);
			SnapXLabel.Name = "SnapXLabel";
			SnapXLabel.TabIndex = 7;
			SnapXLabel.Text = "Grid step X";
			//
			// ThumbnailSnapToGridSizeXNumericEdit
			//
			ThumbnailSnapToGridSizeXNumericEdit.Anchor = AnchorStyles.Left;
			ThumbnailSnapToGridSizeXNumericEdit.BackColor = SystemColors.Window;
			ThumbnailSnapToGridSizeXNumericEdit.BorderStyle = BorderStyle.FixedSingle;
			ThumbnailSnapToGridSizeXNumericEdit.CausesValidation = false;
			ThumbnailSnapToGridSizeXNumericEdit.Increment = new decimal(new int[] { 10, 0, 0, 0 });
			ThumbnailSnapToGridSizeXNumericEdit.Margin = new Padding(4);
			ThumbnailSnapToGridSizeXNumericEdit.Maximum = new decimal(new int[] { 999999, 0, 0, 0 });
			ThumbnailSnapToGridSizeXNumericEdit.Name = "ThumbnailSnapToGridSizeXNumericEdit";
			ThumbnailSnapToGridSizeXNumericEdit.Size = new Size(72, 23);
			ThumbnailSnapToGridSizeXNumericEdit.TabIndex = 8;
			ThumbnailSnapToGridSizeXNumericEdit.Value = new decimal(new int[] { 100, 0, 0, 0 });
			ThumbnailSnapToGridSizeXNumericEdit.ValueChanged += OptionChanged_Handler;
			//
			// SnapYLabel
			//
			SnapYLabel.Anchor = AnchorStyles.Left;
			SnapYLabel.AutoSize = true;
			SnapYLabel.Margin = new Padding(20, 0, 8, 0);
			SnapYLabel.Name = "SnapYLabel";
			SnapYLabel.TabIndex = 9;
			SnapYLabel.Text = "Grid step Y";
			//
			// ThumbnailSnapToGridSizeYNumericEdit
			//
			ThumbnailSnapToGridSizeYNumericEdit.Anchor = AnchorStyles.Left;
			ThumbnailSnapToGridSizeYNumericEdit.BackColor = SystemColors.Window;
			ThumbnailSnapToGridSizeYNumericEdit.BorderStyle = BorderStyle.FixedSingle;
			ThumbnailSnapToGridSizeYNumericEdit.CausesValidation = false;
			ThumbnailSnapToGridSizeYNumericEdit.Increment = new decimal(new int[] { 10, 0, 0, 0 });
			ThumbnailSnapToGridSizeYNumericEdit.Margin = new Padding(4);
			ThumbnailSnapToGridSizeYNumericEdit.Maximum = new decimal(new int[] { 999999, 0, 0, 0 });
			ThumbnailSnapToGridSizeYNumericEdit.Name = "ThumbnailSnapToGridSizeYNumericEdit";
			ThumbnailSnapToGridSizeYNumericEdit.Size = new Size(72, 23);
			ThumbnailSnapToGridSizeYNumericEdit.TabIndex = 10;
			ThumbnailSnapToGridSizeYNumericEdit.Value = new decimal(new int[] { 100, 0, 0, 0 });
			ThumbnailSnapToGridSizeYNumericEdit.ValueChanged += OptionChanged_Handler;
			//
			// PreviewZoomSubPage
			//
			PreviewZoomSubPage.BackColor = SystemColors.Control;
			PreviewZoomSubPage.Controls.Add(PreviewZoomTablePanel);
			PreviewZoomSubPage.Location = new Point(4, 24);
			PreviewZoomSubPage.Name = "PreviewZoomSubPage";
			PreviewZoomSubPage.Padding = new Padding(3);
			PreviewZoomSubPage.Size = new Size(309, 207);
			PreviewZoomSubPage.TabIndex = 2;
			PreviewZoomSubPage.Text = "Zoom";
			//
			// PreviewZoomTablePanel
			//
			PreviewZoomTablePanel.ColumnCount = 2;
			PreviewZoomTablePanel.ColumnStyles.Add(new ColumnStyle());
			PreviewZoomTablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			PreviewZoomTablePanel.Controls.Add(EnableThumbnailZoomCheckBox, 0, 0);
			PreviewZoomTablePanel.Controls.Add(ZoomFactorLabel, 0, 1);
			PreviewZoomTablePanel.Controls.Add(ThumbnailZoomFactorNumericEdit, 1, 1);
			PreviewZoomTablePanel.Controls.Add(ZoomAnchorLabel, 0, 2);
			PreviewZoomTablePanel.Controls.Add(ZoomAnchorPanel, 1, 2);
			PreviewZoomTablePanel.Dock = DockStyle.Fill;
			PreviewZoomTablePanel.Location = new Point(3, 3);
			PreviewZoomTablePanel.Name = "PreviewZoomTablePanel";
			PreviewZoomTablePanel.Padding = new Padding(6);
			PreviewZoomTablePanel.RowCount = 4;
			PreviewZoomTablePanel.RowStyles.Add(new RowStyle());
			PreviewZoomTablePanel.RowStyles.Add(new RowStyle());
			PreviewZoomTablePanel.RowStyles.Add(new RowStyle());
			PreviewZoomTablePanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
			PreviewZoomTablePanel.Size = new Size(303, 201);
			PreviewZoomTablePanel.TabIndex = 0;
			//
			// EnableThumbnailZoomCheckBox
			//
			PreviewZoomTablePanel.SetColumnSpan(EnableThumbnailZoomCheckBox, 2);
			EnableThumbnailZoomCheckBox.AutoSize = true;
			EnableThumbnailZoomCheckBox.Checked = true;
			EnableThumbnailZoomCheckBox.CheckState = CheckState.Checked;
			EnableThumbnailZoomCheckBox.Margin = new Padding(4);
			EnableThumbnailZoomCheckBox.Name = "EnableThumbnailZoomCheckBox";
			EnableThumbnailZoomCheckBox.RightToLeft = RightToLeft.No;
			EnableThumbnailZoomCheckBox.TabIndex = 0;
			EnableThumbnailZoomCheckBox.Text = "Zoom on hover";
			EnableThumbnailZoomCheckBox.UseVisualStyleBackColor = true;
			EnableThumbnailZoomCheckBox.CheckedChanged += OptionChanged_Handler;
			//
			// ZoomFactorLabel
			//
			ZoomFactorLabel.Anchor = AnchorStyles.Left;
			ZoomFactorLabel.AutoSize = true;
			ZoomFactorLabel.Margin = new Padding(4, 0, 8, 0);
			ZoomFactorLabel.Name = "ZoomFactorLabel";
			ZoomFactorLabel.TabIndex = 1;
			ZoomFactorLabel.Text = "Zoom factor";
			//
			// ThumbnailZoomFactorNumericEdit
			//
			ThumbnailZoomFactorNumericEdit.Anchor = AnchorStyles.Left;
			ThumbnailZoomFactorNumericEdit.BackColor = SystemColors.Window;
			ThumbnailZoomFactorNumericEdit.BorderStyle = BorderStyle.FixedSingle;
			ThumbnailZoomFactorNumericEdit.Margin = new Padding(4);
			ThumbnailZoomFactorNumericEdit.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
			ThumbnailZoomFactorNumericEdit.Minimum = new decimal(new int[] { 2, 0, 0, 0 });
			ThumbnailZoomFactorNumericEdit.Name = "ThumbnailZoomFactorNumericEdit";
			ThumbnailZoomFactorNumericEdit.Size = new Size(72, 23);
			ThumbnailZoomFactorNumericEdit.TabIndex = 2;
			ThumbnailZoomFactorNumericEdit.Value = new decimal(new int[] { 2, 0, 0, 0 });
			ThumbnailZoomFactorNumericEdit.ValueChanged += OptionChanged_Handler;
			//
			// ZoomAnchorLabel
			//
			ZoomAnchorLabel.Anchor = AnchorStyles.Left;
			ZoomAnchorLabel.AutoSize = true;
			ZoomAnchorLabel.Margin = new Padding(4, 0, 8, 0);
			ZoomAnchorLabel.Name = "ZoomAnchorLabel";
			ZoomAnchorLabel.TabIndex = 3;
			ZoomAnchorLabel.Text = "Anchor";
			//
			// ZoomAnchorPanel
			//
			ZoomAnchorPanel.Anchor = AnchorStyles.Left;
			ZoomAnchorPanel.BorderStyle = BorderStyle.FixedSingle;
			ZoomAnchorPanel.Controls.Add(ZoomAnchorNWRadioButton);
			ZoomAnchorPanel.Controls.Add(ZoomAnchorNRadioButton);
			ZoomAnchorPanel.Controls.Add(ZoomAnchorNERadioButton);
			ZoomAnchorPanel.Controls.Add(ZoomAnchorWRadioButton);
			ZoomAnchorPanel.Controls.Add(ZoomAnchorSERadioButton);
			ZoomAnchorPanel.Controls.Add(ZoomAnchorCRadioButton);
			ZoomAnchorPanel.Controls.Add(ZoomAnchorSRadioButton);
			ZoomAnchorPanel.Controls.Add(ZoomAnchorERadioButton);
			ZoomAnchorPanel.Controls.Add(ZoomAnchorSWRadioButton);
			ZoomAnchorPanel.Margin = new Padding(4);
			ZoomAnchorPanel.Name = "ZoomAnchorPanel";
			ZoomAnchorPanel.Size = new Size(90, 84);
			ZoomAnchorPanel.TabIndex = 4;
			// 
			// ZoomAnchorNWRadioButton
			// 
			ZoomAnchorNWRadioButton.AutoSize = true;
			ZoomAnchorNWRadioButton.Location = new Point(4, 4);
			ZoomAnchorNWRadioButton.Margin = new Padding(4);
			ZoomAnchorNWRadioButton.Name = "ZoomAnchorNWRadioButton";
			ZoomAnchorNWRadioButton.Size = new Size(14, 13);
			ZoomAnchorNWRadioButton.TabIndex = 0;
			ZoomAnchorNWRadioButton.TabStop = true;
			ZoomAnchorNWRadioButton.UseVisualStyleBackColor = true;
			ZoomAnchorNWRadioButton.CheckedChanged += OptionChanged_Handler;
			// 
			// ZoomAnchorNRadioButton
			// 
			ZoomAnchorNRadioButton.AutoSize = true;
			ZoomAnchorNRadioButton.Location = new Point(36, 4);
			ZoomAnchorNRadioButton.Margin = new Padding(4);
			ZoomAnchorNRadioButton.Name = "ZoomAnchorNRadioButton";
			ZoomAnchorNRadioButton.Size = new Size(14, 13);
			ZoomAnchorNRadioButton.TabIndex = 1;
			ZoomAnchorNRadioButton.TabStop = true;
			ZoomAnchorNRadioButton.UseVisualStyleBackColor = true;
			ZoomAnchorNRadioButton.CheckedChanged += OptionChanged_Handler;
			// 
			// ZoomAnchorNERadioButton
			// 
			ZoomAnchorNERadioButton.AutoSize = true;
			ZoomAnchorNERadioButton.Location = new Point(69, 4);
			ZoomAnchorNERadioButton.Margin = new Padding(4);
			ZoomAnchorNERadioButton.Name = "ZoomAnchorNERadioButton";
			ZoomAnchorNERadioButton.Size = new Size(14, 13);
			ZoomAnchorNERadioButton.TabIndex = 2;
			ZoomAnchorNERadioButton.TabStop = true;
			ZoomAnchorNERadioButton.UseVisualStyleBackColor = true;
			ZoomAnchorNERadioButton.CheckedChanged += OptionChanged_Handler;
			// 
			// ZoomAnchorWRadioButton
			// 
			ZoomAnchorWRadioButton.AutoSize = true;
			ZoomAnchorWRadioButton.Location = new Point(4, 34);
			ZoomAnchorWRadioButton.Margin = new Padding(4);
			ZoomAnchorWRadioButton.Name = "ZoomAnchorWRadioButton";
			ZoomAnchorWRadioButton.Size = new Size(14, 13);
			ZoomAnchorWRadioButton.TabIndex = 3;
			ZoomAnchorWRadioButton.TabStop = true;
			ZoomAnchorWRadioButton.UseVisualStyleBackColor = true;
			ZoomAnchorWRadioButton.CheckedChanged += OptionChanged_Handler;
			// 
			// ZoomAnchorSERadioButton
			// 
			ZoomAnchorSERadioButton.AutoSize = true;
			ZoomAnchorSERadioButton.Location = new Point(69, 64);
			ZoomAnchorSERadioButton.Margin = new Padding(4);
			ZoomAnchorSERadioButton.Name = "ZoomAnchorSERadioButton";
			ZoomAnchorSERadioButton.Size = new Size(14, 13);
			ZoomAnchorSERadioButton.TabIndex = 8;
			ZoomAnchorSERadioButton.TabStop = true;
			ZoomAnchorSERadioButton.UseVisualStyleBackColor = true;
			ZoomAnchorSERadioButton.CheckedChanged += OptionChanged_Handler;
			// 
			// ZoomAnchorCRadioButton
			// 
			ZoomAnchorCRadioButton.AutoSize = true;
			ZoomAnchorCRadioButton.Location = new Point(36, 34);
			ZoomAnchorCRadioButton.Margin = new Padding(4);
			ZoomAnchorCRadioButton.Name = "ZoomAnchorCRadioButton";
			ZoomAnchorCRadioButton.Size = new Size(14, 13);
			ZoomAnchorCRadioButton.TabIndex = 4;
			ZoomAnchorCRadioButton.TabStop = true;
			ZoomAnchorCRadioButton.UseVisualStyleBackColor = true;
			ZoomAnchorCRadioButton.CheckedChanged += OptionChanged_Handler;
			// 
			// ZoomAnchorSRadioButton
			// 
			ZoomAnchorSRadioButton.AutoSize = true;
			ZoomAnchorSRadioButton.Location = new Point(36, 64);
			ZoomAnchorSRadioButton.Margin = new Padding(4);
			ZoomAnchorSRadioButton.Name = "ZoomAnchorSRadioButton";
			ZoomAnchorSRadioButton.Size = new Size(14, 13);
			ZoomAnchorSRadioButton.TabIndex = 7;
			ZoomAnchorSRadioButton.TabStop = true;
			ZoomAnchorSRadioButton.UseVisualStyleBackColor = true;
			ZoomAnchorSRadioButton.CheckedChanged += OptionChanged_Handler;
			// 
			// ZoomAnchorERadioButton
			// 
			ZoomAnchorERadioButton.AutoSize = true;
			ZoomAnchorERadioButton.Location = new Point(69, 34);
			ZoomAnchorERadioButton.Margin = new Padding(4);
			ZoomAnchorERadioButton.Name = "ZoomAnchorERadioButton";
			ZoomAnchorERadioButton.Size = new Size(14, 13);
			ZoomAnchorERadioButton.TabIndex = 5;
			ZoomAnchorERadioButton.TabStop = true;
			ZoomAnchorERadioButton.UseVisualStyleBackColor = true;
			ZoomAnchorERadioButton.CheckedChanged += OptionChanged_Handler;
			// 
			// ZoomAnchorSWRadioButton
			// 
			ZoomAnchorSWRadioButton.AutoSize = true;
			ZoomAnchorSWRadioButton.Location = new Point(4, 64);
			ZoomAnchorSWRadioButton.Margin = new Padding(4);
			ZoomAnchorSWRadioButton.Name = "ZoomAnchorSWRadioButton";
			ZoomAnchorSWRadioButton.Size = new Size(14, 13);
			ZoomAnchorSWRadioButton.TabIndex = 6;
			ZoomAnchorSWRadioButton.TabStop = true;
			ZoomAnchorSWRadioButton.UseVisualStyleBackColor = true;
			ZoomAnchorSWRadioButton.CheckedChanged += OptionChanged_Handler;
			//
			// OverlayTabPage
			// 
			OverlayTabPage.BackColor = SystemColors.Control;
			OverlayTabPage.Controls.Add(OverlaySettingsPanel);
			OverlayTabPage.Location = new Point(124, 4);
			OverlayTabPage.Margin = new Padding(4);
			OverlayTabPage.Name = "OverlayTabPage";
			OverlayTabPage.Size = new Size(327, 243);
			OverlayTabPage.TabIndex = 3;
			OverlayTabPage.Text = "Overlay";
			// 
			// OverlaySettingsPanel
			// 
			OverlaySettingsPanel.Controls.Add(OverlaySubTabControl);
			OverlaySettingsPanel.Dock = DockStyle.Fill;
			OverlaySettingsPanel.Location = new Point(0, 0);
			OverlaySettingsPanel.Margin = new Padding(4);
			OverlaySettingsPanel.Name = "OverlaySettingsPanel";
			OverlaySettingsPanel.Padding = new Padding(6, 4, 4, 4);
			OverlaySettingsPanel.Size = new Size(327, 322);
			OverlaySettingsPanel.TabIndex = 25;
			//
			// OverlaySubTabControl
			//
			OverlaySubTabControl.Controls.Add(OverlayGeneralSubPage);
			OverlaySubTabControl.Controls.Add(OverlayWindowNameSubPage);
			OverlaySubTabControl.Controls.Add(OverlayGroupNameSubPage);
			OverlaySubTabControl.Controls.Add(OverlayBorderSubPage);
			OverlaySubTabControl.Dock = DockStyle.Fill;
			OverlaySubTabControl.Location = new Point(6, 4);
			OverlaySubTabControl.Name = "OverlaySubTabControl";
			OverlaySubTabControl.SelectedIndex = 0;
			OverlaySubTabControl.Size = new Size(317, 314);
			OverlaySubTabControl.TabIndex = 0;
			//
			// OverlayGeneralSubPage
			//
			OverlayGeneralSubPage.BackColor = SystemColors.Control;
			OverlayGeneralSubPage.Controls.Add(ShowThumbnailOverlaysCheckBox);
			OverlayGeneralSubPage.Controls.Add(ShowThumbnailFramesCheckBox);
			OverlayGeneralSubPage.Controls.Add(OverlayAlwaysOnTopCheckBox);
			OverlayGeneralSubPage.Location = new Point(4, 24);
			OverlayGeneralSubPage.Name = "OverlayGeneralSubPage";
			OverlayGeneralSubPage.Padding = new Padding(3);
			OverlayGeneralSubPage.Size = new Size(309, 286);
			OverlayGeneralSubPage.TabIndex = 0;
			OverlayGeneralSubPage.Text = "General";
			//
			// OverlayWindowNameSubPage
			//
			OverlayWindowNameSubPage.BackColor = SystemColors.Control;
			OverlayWindowNameSubPage.Controls.Add(ShowClientNameCheckBox);
			OverlayWindowNameSubPage.Controls.Add(OverlayLabelColorLabel);
			OverlayWindowNameSubPage.Controls.Add(OverlayLabelColorButton);
			OverlayWindowNameSubPage.Controls.Add(btnLabelFont);
			OverlayWindowNameSubPage.Controls.Add(OverlayLabelPositionLabel);
			OverlayWindowNameSubPage.Controls.Add(OverlayLabelAnchorPanel);
			OverlayWindowNameSubPage.Controls.Add(OverlayLabelFontPreviewPanel);
			OverlayLabelFontPreviewPanel.Controls.Add(LabelOverlayLabelFont);
			OverlayWindowNameSubPage.Location = new Point(4, 24);
			OverlayWindowNameSubPage.Name = "OverlayWindowNameSubPage";
			OverlayWindowNameSubPage.Padding = new Padding(3);
			OverlayWindowNameSubPage.Size = new Size(309, 286);
			OverlayWindowNameSubPage.TabIndex = 1;
			OverlayWindowNameSubPage.Text = "Window Name";
			//
			// OverlayGroupNameSubPage
			//
			OverlayGroupNameSubPage.BackColor = SystemColors.Control;
			OverlayGroupNameSubPage.Controls.Add(ShowCycleGroupNameCheckBox);
			OverlayGroupNameSubPage.Controls.Add(CycleGroupNameColorLabel);
			OverlayGroupNameSubPage.Controls.Add(CycleGroupNameColorButton);
			OverlayGroupNameSubPage.Controls.Add(btnCycleGroupNameFont);
			OverlayGroupNameSubPage.Controls.Add(CycleGroupNamePositionLabel);
			OverlayGroupNameSubPage.Controls.Add(CycleGroupIndicatorPanel);
			OverlayGroupNameSubPage.Controls.Add(CycleGroupNameFontPreviewPanel);
			CycleGroupNameFontPreviewPanel.Controls.Add(LabelCycleGroupNameFont);
			OverlayGroupNameSubPage.Location = new Point(4, 24);
			OverlayGroupNameSubPage.Name = "OverlayGroupNameSubPage";
			OverlayGroupNameSubPage.Padding = new Padding(3);
			OverlayGroupNameSubPage.Size = new Size(309, 286);
			OverlayGroupNameSubPage.TabIndex = 2;
			OverlayGroupNameSubPage.Text = "Group Name";
			//
			// ShowClientNameCheckBox
			//
			ShowClientNameCheckBox.AutoSize = true;
			ShowClientNameCheckBox.Location = new Point(12, 12);
			ShowClientNameCheckBox.Margin = new Padding(4);
			ShowClientNameCheckBox.Name = "ShowClientNameCheckBox";
			ShowClientNameCheckBox.Size = new Size(130, 19);
			ShowClientNameCheckBox.TabIndex = 0;
			ShowClientNameCheckBox.Text = "Show window name";
			ShowClientNameCheckBox.UseVisualStyleBackColor = true;
			ShowClientNameCheckBox.CheckedChanged += OptionChanged_Handler;
			//
			// OverlayAlwaysOnTopCheckBox
			//
			OverlayAlwaysOnTopCheckBox.AutoSize = true;
			OverlayAlwaysOnTopCheckBox.Location = new Point(12, 70);
			OverlayAlwaysOnTopCheckBox.Margin = new Padding(4);
			OverlayAlwaysOnTopCheckBox.Name = "OverlayAlwaysOnTopCheckBox";
			OverlayAlwaysOnTopCheckBox.Size = new Size(200, 19);
			OverlayAlwaysOnTopCheckBox.TabIndex = 2;
			OverlayAlwaysOnTopCheckBox.Text = "Overlay always above previews";
			OverlayAlwaysOnTopCheckBox.UseVisualStyleBackColor = true;
			OverlayAlwaysOnTopCheckBox.CheckedChanged += OptionChanged_Handler;
			//
			// OverlayLabelFontPreviewPanel
			//
			OverlayLabelFontPreviewPanel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			OverlayLabelFontPreviewPanel.BackColor = SystemColors.ControlDarkDark;
			OverlayLabelFontPreviewPanel.BorderStyle = BorderStyle.FixedSingle;
			OverlayLabelFontPreviewPanel.Location = new Point(12, 200);
			OverlayLabelFontPreviewPanel.Name = "OverlayLabelFontPreviewPanel";
			OverlayLabelFontPreviewPanel.Size = new Size(285, 62);
			OverlayLabelFontPreviewPanel.TabIndex = 6;
			//
			// CycleGroupNameColorLabel
			//
			CycleGroupNameColorLabel.AutoSize = true;
			CycleGroupNameColorLabel.Location = new Point(12, 47);
			CycleGroupNameColorLabel.Margin = new Padding(4, 0, 4, 0);
			CycleGroupNameColorLabel.Name = "CycleGroupNameColorLabel";
			CycleGroupNameColorLabel.Size = new Size(36, 15);
			CycleGroupNameColorLabel.TabIndex = 1;
			CycleGroupNameColorLabel.Text = "Color";
			//
			// CycleGroupNameColorButton
			//
			CycleGroupNameColorButton.BorderStyle = BorderStyle.FixedSingle;
			CycleGroupNameColorButton.Location = new Point(12, 66);
			CycleGroupNameColorButton.Margin = new Padding(4);
			CycleGroupNameColorButton.Name = "CycleGroupNameColorButton";
			CycleGroupNameColorButton.Size = new Size(110, 23);
			CycleGroupNameColorButton.TabIndex = 2;
			CycleGroupNameColorButton.Click += CycleGroupNameColorButton_Click;
			//
			// btnCycleGroupNameFont
			//
			btnCycleGroupNameFont.Location = new Point(12, 97);
			btnCycleGroupNameFont.Margin = new Padding(4);
			btnCycleGroupNameFont.Name = "btnCycleGroupNameFont";
			btnCycleGroupNameFont.Size = new Size(110, 27);
			btnCycleGroupNameFont.TabIndex = 3;
			btnCycleGroupNameFont.Text = "Font...";
			btnCycleGroupNameFont.UseVisualStyleBackColor = true;
			btnCycleGroupNameFont.Click += btnCycleGroupNameFont_Click;
			//
			// CycleGroupNamePositionLabel
			//
			CycleGroupNamePositionLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			CycleGroupNamePositionLabel.AutoSize = true;
			CycleGroupNamePositionLabel.Location = new Point(224, 12);
			CycleGroupNamePositionLabel.Margin = new Padding(4, 0, 4, 0);
			CycleGroupNamePositionLabel.Name = "CycleGroupNamePositionLabel";
			CycleGroupNamePositionLabel.Size = new Size(50, 15);
			CycleGroupNamePositionLabel.TabIndex = 4;
			CycleGroupNamePositionLabel.Text = "Position";
			//
			// CycleGroupNameFontPreviewPanel
			//
			CycleGroupNameFontPreviewPanel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			CycleGroupNameFontPreviewPanel.BackColor = SystemColors.ControlDarkDark;
			CycleGroupNameFontPreviewPanel.BorderStyle = BorderStyle.FixedSingle;
			CycleGroupNameFontPreviewPanel.Location = new Point(12, 200);
			CycleGroupNameFontPreviewPanel.Name = "CycleGroupNameFontPreviewPanel";
			CycleGroupNameFontPreviewPanel.Size = new Size(285, 62);
			CycleGroupNameFontPreviewPanel.TabIndex = 6;
			//
			// LabelCycleGroupNameFont
			//
			LabelCycleGroupNameFont.AutoSize = true;
			LabelCycleGroupNameFont.BackColor = Color.Transparent;
			LabelCycleGroupNameFont.Location = new Point(8, 8);
			LabelCycleGroupNameFont.Margin = new Padding(4, 0, 4, 0);
			LabelCycleGroupNameFont.Name = "LabelCycleGroupNameFont";
			LabelCycleGroupNameFont.Size = new Size(60, 15);
			LabelCycleGroupNameFont.TabIndex = 0;
			LabelCycleGroupNameFont.Text = "Group 1";
			//
			// OverlayBorderSubPage
			//
			OverlayBorderSubPage.BackColor = SystemColors.Control;
			OverlayBorderSubPage.Controls.Add(EnableActiveClientHighlightCheckBox);
			OverlayBorderSubPage.Controls.Add(HighlightColorLabel);
			OverlayBorderSubPage.Controls.Add(ActiveClientHighlightColorButton);
			OverlayBorderSubPage.Controls.Add(ActiveFrameThicknessLabel);
			OverlayBorderSubPage.Controls.Add(ActiveClientHighlightThicknessNumericEdit);
			OverlayBorderSubPage.Location = new Point(4, 24);
			OverlayBorderSubPage.Name = "OverlayBorderSubPage";
			OverlayBorderSubPage.Padding = new Padding(3);
			OverlayBorderSubPage.Size = new Size(309, 286);
			OverlayBorderSubPage.TabIndex = 3;
			OverlayBorderSubPage.Text = "Border";
			//
			// CycleGroupIndicatorPanel
			// 
			CycleGroupIndicatorPanel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			CycleGroupIndicatorPanel.BorderStyle = BorderStyle.FixedSingle;
			CycleGroupIndicatorPanel.Controls.Add(CycleGroupIndicatorNWRadioButton);
			CycleGroupIndicatorPanel.Controls.Add(CycleGroupIndicatorNRadioButton);
			CycleGroupIndicatorPanel.Controls.Add(CycleGroupIndicatorNERadioButton);
			CycleGroupIndicatorPanel.Controls.Add(CycleGroupIndicatorWRadioButton);
			CycleGroupIndicatorPanel.Controls.Add(CycleGroupIndicatorSERadioButton);
			CycleGroupIndicatorPanel.Controls.Add(CycleGroupIndicatorCRadioButton);
			CycleGroupIndicatorPanel.Controls.Add(CycleGroupIndicatorSRadioButton);
			CycleGroupIndicatorPanel.Controls.Add(CycleGroupIndicatorERadioButton);
			CycleGroupIndicatorPanel.Controls.Add(CycleGroupIndicatorSWRadioButton);
			CycleGroupIndicatorPanel.Location = new Point(224, 31);
			CycleGroupIndicatorPanel.Margin = new Padding(4);
			CycleGroupIndicatorPanel.Name = "CycleGroupIndicatorPanel";
			CycleGroupIndicatorPanel.Size = new Size(73, 66);
			CycleGroupIndicatorPanel.TabIndex = 46;
			// 
			// CycleGroupIndicatorNWRadioButton
			// 
			CycleGroupIndicatorNWRadioButton.AutoSize = true;
			CycleGroupIndicatorNWRadioButton.Location = new Point(4, 4);
			CycleGroupIndicatorNWRadioButton.Margin = new Padding(4);
			CycleGroupIndicatorNWRadioButton.Name = "CycleGroupIndicatorNWRadioButton";
			CycleGroupIndicatorNWRadioButton.Size = new Size(14, 13);
			CycleGroupIndicatorNWRadioButton.TabIndex = 0;
			CycleGroupIndicatorNWRadioButton.TabStop = true;
			CycleGroupIndicatorNWRadioButton.UseVisualStyleBackColor = true;
			CycleGroupIndicatorNWRadioButton.CheckedChanged += OptionChanged_Handler;
			// 
			// CycleGroupIndicatorNRadioButton
			// 
			CycleGroupIndicatorNRadioButton.AutoSize = true;
			CycleGroupIndicatorNRadioButton.Location = new Point(27, 4);
			CycleGroupIndicatorNRadioButton.Margin = new Padding(4);
			CycleGroupIndicatorNRadioButton.Name = "CycleGroupIndicatorNRadioButton";
			CycleGroupIndicatorNRadioButton.Size = new Size(14, 13);
			CycleGroupIndicatorNRadioButton.TabIndex = 1;
			CycleGroupIndicatorNRadioButton.TabStop = true;
			CycleGroupIndicatorNRadioButton.UseVisualStyleBackColor = true;
			CycleGroupIndicatorNRadioButton.CheckedChanged += OptionChanged_Handler;
			// 
			// CycleGroupIndicatorNERadioButton
			// 
			CycleGroupIndicatorNERadioButton.AutoSize = true;
			CycleGroupIndicatorNERadioButton.Location = new Point(50, 4);
			CycleGroupIndicatorNERadioButton.Margin = new Padding(4);
			CycleGroupIndicatorNERadioButton.Name = "CycleGroupIndicatorNERadioButton";
			CycleGroupIndicatorNERadioButton.Size = new Size(14, 13);
			CycleGroupIndicatorNERadioButton.TabIndex = 2;
			CycleGroupIndicatorNERadioButton.TabStop = true;
			CycleGroupIndicatorNERadioButton.UseVisualStyleBackColor = true;
			CycleGroupIndicatorNERadioButton.CheckedChanged += OptionChanged_Handler;
			// 
			// CycleGroupIndicatorWRadioButton
			// 
			CycleGroupIndicatorWRadioButton.AutoSize = true;
			CycleGroupIndicatorWRadioButton.Location = new Point(4, 25);
			CycleGroupIndicatorWRadioButton.Margin = new Padding(4);
			CycleGroupIndicatorWRadioButton.Name = "CycleGroupIndicatorWRadioButton";
			CycleGroupIndicatorWRadioButton.Size = new Size(14, 13);
			CycleGroupIndicatorWRadioButton.TabIndex = 3;
			CycleGroupIndicatorWRadioButton.TabStop = true;
			CycleGroupIndicatorWRadioButton.UseVisualStyleBackColor = true;
			CycleGroupIndicatorWRadioButton.CheckedChanged += OptionChanged_Handler;
			// 
			// CycleGroupIndicatorSERadioButton
			// 
			CycleGroupIndicatorSERadioButton.AutoSize = true;
			CycleGroupIndicatorSERadioButton.Location = new Point(50, 46);
			CycleGroupIndicatorSERadioButton.Margin = new Padding(4);
			CycleGroupIndicatorSERadioButton.Name = "CycleGroupIndicatorSERadioButton";
			CycleGroupIndicatorSERadioButton.Size = new Size(14, 13);
			CycleGroupIndicatorSERadioButton.TabIndex = 8;
			CycleGroupIndicatorSERadioButton.TabStop = true;
			CycleGroupIndicatorSERadioButton.UseVisualStyleBackColor = true;
			CycleGroupIndicatorSERadioButton.CheckedChanged += OptionChanged_Handler;
			// 
			// CycleGroupIndicatorCRadioButton
			// 
			CycleGroupIndicatorCRadioButton.AutoSize = true;
			CycleGroupIndicatorCRadioButton.Location = new Point(27, 25);
			CycleGroupIndicatorCRadioButton.Margin = new Padding(4);
			CycleGroupIndicatorCRadioButton.Name = "CycleGroupIndicatorCRadioButton";
			CycleGroupIndicatorCRadioButton.Size = new Size(14, 13);
			CycleGroupIndicatorCRadioButton.TabIndex = 4;
			CycleGroupIndicatorCRadioButton.TabStop = true;
			CycleGroupIndicatorCRadioButton.UseVisualStyleBackColor = true;
			CycleGroupIndicatorCRadioButton.CheckedChanged += OptionChanged_Handler;
			// 
			// CycleGroupIndicatorSRadioButton
			// 
			CycleGroupIndicatorSRadioButton.AutoSize = true;
			CycleGroupIndicatorSRadioButton.Location = new Point(27, 46);
			CycleGroupIndicatorSRadioButton.Margin = new Padding(4);
			CycleGroupIndicatorSRadioButton.Name = "CycleGroupIndicatorSRadioButton";
			CycleGroupIndicatorSRadioButton.Size = new Size(14, 13);
			CycleGroupIndicatorSRadioButton.TabIndex = 7;
			CycleGroupIndicatorSRadioButton.TabStop = true;
			CycleGroupIndicatorSRadioButton.UseVisualStyleBackColor = true;
			CycleGroupIndicatorSRadioButton.CheckedChanged += OptionChanged_Handler;
			// 
			// CycleGroupIndicatorERadioButton
			// 
			CycleGroupIndicatorERadioButton.AutoSize = true;
			CycleGroupIndicatorERadioButton.Location = new Point(50, 25);
			CycleGroupIndicatorERadioButton.Margin = new Padding(4);
			CycleGroupIndicatorERadioButton.Name = "CycleGroupIndicatorERadioButton";
			CycleGroupIndicatorERadioButton.Size = new Size(14, 13);
			CycleGroupIndicatorERadioButton.TabIndex = 5;
			CycleGroupIndicatorERadioButton.TabStop = true;
			CycleGroupIndicatorERadioButton.UseVisualStyleBackColor = true;
			CycleGroupIndicatorERadioButton.CheckedChanged += OptionChanged_Handler;
			// 
			// CycleGroupIndicatorSWRadioButton
			// 
			CycleGroupIndicatorSWRadioButton.AutoSize = true;
			CycleGroupIndicatorSWRadioButton.Location = new Point(4, 46);
			CycleGroupIndicatorSWRadioButton.Margin = new Padding(4);
			CycleGroupIndicatorSWRadioButton.Name = "CycleGroupIndicatorSWRadioButton";
			CycleGroupIndicatorSWRadioButton.Size = new Size(14, 13);
			CycleGroupIndicatorSWRadioButton.TabIndex = 6;
			CycleGroupIndicatorSWRadioButton.TabStop = true;
			CycleGroupIndicatorSWRadioButton.UseVisualStyleBackColor = true;
			CycleGroupIndicatorSWRadioButton.CheckedChanged += OptionChanged_Handler;
			// 
			// LabelOverlayLabelFont
			// 
			LabelOverlayLabelFont.AutoSize = true;
			LabelOverlayLabelFont.Location = new Point(8, 8);
			LabelOverlayLabelFont.Margin = new Padding(4, 0, 4, 0);
			LabelOverlayLabelFont.Name = "LabelOverlayLabelFont";
			LabelOverlayLabelFont.Size = new Size(47, 15);
			LabelOverlayLabelFont.TabIndex = 45;
			LabelOverlayLabelFont.Text = "EVE - Character Name";
			// 
			// btnLabelFont
			// 
			btnLabelFont.Location = new Point(12, 97);
			btnLabelFont.Margin = new Padding(2);
			btnLabelFont.Name = "btnLabelFont";
			btnLabelFont.Size = new Size(110, 27);
			btnLabelFont.TabIndex = 44;
			btnLabelFont.Text = "Font...";
			btnLabelFont.UseVisualStyleBackColor = true;
			btnLabelFont.Click += btnLabelFont_Click;
			// 
			// OverlayLabelPositionLabel
			// 
			OverlayLabelPositionLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			OverlayLabelPositionLabel.AutoSize = true;
			OverlayLabelPositionLabel.Location = new Point(224, 12);
			OverlayLabelPositionLabel.Margin = new Padding(4, 0, 4, 0);
			OverlayLabelPositionLabel.Name = "OverlayLabelPositionLabel";
			OverlayLabelPositionLabel.Size = new Size(50, 15);
			OverlayLabelPositionLabel.TabIndex = 43;
			OverlayLabelPositionLabel.Text = "Position";
			// 
			// OverlayLabelColorLabel
			// 
			OverlayLabelColorLabel.AutoSize = true;
			OverlayLabelColorLabel.Location = new Point(12, 47);
			OverlayLabelColorLabel.Margin = new Padding(4, 0, 4, 0);
			OverlayLabelColorLabel.Name = "OverlayLabelColorLabel";
			OverlayLabelColorLabel.Size = new Size(36, 15);
			OverlayLabelColorLabel.TabIndex = 42;
			OverlayLabelColorLabel.Text = "Color";
			// 
			// OverlayLabelColorButton
			// 
			OverlayLabelColorButton.BorderStyle = BorderStyle.FixedSingle;
			OverlayLabelColorButton.Location = new Point(12, 66);
			OverlayLabelColorButton.Margin = new Padding(4);
			OverlayLabelColorButton.Name = "OverlayLabelColorButton";
			OverlayLabelColorButton.Size = new Size(110, 23);
			OverlayLabelColorButton.TabIndex = 41;
			OverlayLabelColorButton.Click += OverlayLabelColorButton_Click;
			// 
			// OverlayLabelAnchorPanel
			// 
			OverlayLabelAnchorPanel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			OverlayLabelAnchorPanel.BorderStyle = BorderStyle.FixedSingle;
			OverlayLabelAnchorPanel.Controls.Add(OverlayLabelNWRadioButton);
			OverlayLabelAnchorPanel.Controls.Add(OverlayLabelNRadioButton);
			OverlayLabelAnchorPanel.Controls.Add(OverlayLabelNERadioButton);
			OverlayLabelAnchorPanel.Controls.Add(OverlayLabelWRadioButton);
			OverlayLabelAnchorPanel.Controls.Add(OverlayLabelSERadioButton);
			OverlayLabelAnchorPanel.Controls.Add(OverlayLabelCRadioButton);
			OverlayLabelAnchorPanel.Controls.Add(OverlayLabelSRadioButton);
			OverlayLabelAnchorPanel.Controls.Add(OverlayLabelERadioButton);
			OverlayLabelAnchorPanel.Controls.Add(OverlayLabelSWRadioButton);
			OverlayLabelAnchorPanel.Location = new Point(224, 31);
			OverlayLabelAnchorPanel.Margin = new Padding(4);
			OverlayLabelAnchorPanel.Name = "OverlayLabelAnchorPanel";
			OverlayLabelAnchorPanel.Size = new Size(73, 66);
			OverlayLabelAnchorPanel.TabIndex = 39;
			// 
			// OverlayLabelNWRadioButton
			// 
			OverlayLabelNWRadioButton.AutoSize = true;
			OverlayLabelNWRadioButton.Location = new Point(4, 4);
			OverlayLabelNWRadioButton.Margin = new Padding(4);
			OverlayLabelNWRadioButton.Name = "OverlayLabelNWRadioButton";
			OverlayLabelNWRadioButton.Size = new Size(14, 13);
			OverlayLabelNWRadioButton.TabIndex = 0;
			OverlayLabelNWRadioButton.TabStop = true;
			OverlayLabelNWRadioButton.UseVisualStyleBackColor = true;
			OverlayLabelNWRadioButton.CheckedChanged += OptionChanged_Handler;
			// 
			// OverlayLabelNRadioButton
			// 
			OverlayLabelNRadioButton.AutoSize = true;
			OverlayLabelNRadioButton.Location = new Point(27, 4);
			OverlayLabelNRadioButton.Margin = new Padding(4);
			OverlayLabelNRadioButton.Name = "OverlayLabelNRadioButton";
			OverlayLabelNRadioButton.Size = new Size(14, 13);
			OverlayLabelNRadioButton.TabIndex = 1;
			OverlayLabelNRadioButton.TabStop = true;
			OverlayLabelNRadioButton.UseVisualStyleBackColor = true;
			OverlayLabelNRadioButton.CheckedChanged += OptionChanged_Handler;
			// 
			// OverlayLabelNERadioButton
			// 
			OverlayLabelNERadioButton.AutoSize = true;
			OverlayLabelNERadioButton.Location = new Point(50, 4);
			OverlayLabelNERadioButton.Margin = new Padding(4);
			OverlayLabelNERadioButton.Name = "OverlayLabelNERadioButton";
			OverlayLabelNERadioButton.Size = new Size(14, 13);
			OverlayLabelNERadioButton.TabIndex = 2;
			OverlayLabelNERadioButton.TabStop = true;
			OverlayLabelNERadioButton.UseVisualStyleBackColor = true;
			OverlayLabelNERadioButton.CheckedChanged += OptionChanged_Handler;
			// 
			// OverlayLabelWRadioButton
			// 
			OverlayLabelWRadioButton.AutoSize = true;
			OverlayLabelWRadioButton.Location = new Point(4, 25);
			OverlayLabelWRadioButton.Margin = new Padding(4);
			OverlayLabelWRadioButton.Name = "OverlayLabelWRadioButton";
			OverlayLabelWRadioButton.Size = new Size(14, 13);
			OverlayLabelWRadioButton.TabIndex = 3;
			OverlayLabelWRadioButton.TabStop = true;
			OverlayLabelWRadioButton.UseVisualStyleBackColor = true;
			OverlayLabelWRadioButton.CheckedChanged += OptionChanged_Handler;
			// 
			// OverlayLabelSERadioButton
			// 
			OverlayLabelSERadioButton.AutoSize = true;
			OverlayLabelSERadioButton.Location = new Point(50, 46);
			OverlayLabelSERadioButton.Margin = new Padding(4);
			OverlayLabelSERadioButton.Name = "OverlayLabelSERadioButton";
			OverlayLabelSERadioButton.Size = new Size(14, 13);
			OverlayLabelSERadioButton.TabIndex = 8;
			OverlayLabelSERadioButton.TabStop = true;
			OverlayLabelSERadioButton.UseVisualStyleBackColor = true;
			OverlayLabelSERadioButton.CheckedChanged += OptionChanged_Handler;
			// 
			// OverlayLabelCRadioButton
			// 
			OverlayLabelCRadioButton.AutoSize = true;
			OverlayLabelCRadioButton.Location = new Point(27, 25);
			OverlayLabelCRadioButton.Margin = new Padding(4);
			OverlayLabelCRadioButton.Name = "OverlayLabelCRadioButton";
			OverlayLabelCRadioButton.Size = new Size(14, 13);
			OverlayLabelCRadioButton.TabIndex = 4;
			OverlayLabelCRadioButton.TabStop = true;
			OverlayLabelCRadioButton.UseVisualStyleBackColor = true;
			OverlayLabelCRadioButton.CheckedChanged += OptionChanged_Handler;
			// 
			// OverlayLabelSRadioButton
			// 
			OverlayLabelSRadioButton.AutoSize = true;
			OverlayLabelSRadioButton.Location = new Point(27, 46);
			OverlayLabelSRadioButton.Margin = new Padding(4);
			OverlayLabelSRadioButton.Name = "OverlayLabelSRadioButton";
			OverlayLabelSRadioButton.Size = new Size(14, 13);
			OverlayLabelSRadioButton.TabIndex = 7;
			OverlayLabelSRadioButton.TabStop = true;
			OverlayLabelSRadioButton.UseVisualStyleBackColor = true;
			OverlayLabelSRadioButton.CheckedChanged += OptionChanged_Handler;
			// 
			// OverlayLabelERadioButton
			// 
			OverlayLabelERadioButton.AutoSize = true;
			OverlayLabelERadioButton.Location = new Point(50, 25);
			OverlayLabelERadioButton.Margin = new Padding(4);
			OverlayLabelERadioButton.Name = "OverlayLabelERadioButton";
			OverlayLabelERadioButton.Size = new Size(14, 13);
			OverlayLabelERadioButton.TabIndex = 5;
			OverlayLabelERadioButton.TabStop = true;
			OverlayLabelERadioButton.UseVisualStyleBackColor = true;
			OverlayLabelERadioButton.CheckedChanged += OptionChanged_Handler;
			// 
			// OverlayLabelSWRadioButton
			// 
			OverlayLabelSWRadioButton.AutoSize = true;
			OverlayLabelSWRadioButton.Location = new Point(4, 46);
			OverlayLabelSWRadioButton.Margin = new Padding(4);
			OverlayLabelSWRadioButton.Name = "OverlayLabelSWRadioButton";
			OverlayLabelSWRadioButton.Size = new Size(14, 13);
			OverlayLabelSWRadioButton.TabIndex = 6;
			OverlayLabelSWRadioButton.TabStop = true;
			OverlayLabelSWRadioButton.UseVisualStyleBackColor = true;
			OverlayLabelSWRadioButton.CheckedChanged += OptionChanged_Handler;
			//
			// ActiveFrameThicknessLabel
			//
			ActiveFrameThicknessLabel.AutoSize = true;
			ActiveFrameThicknessLabel.Location = new Point(12, 78);
			ActiveFrameThicknessLabel.Margin = new Padding(4, 0, 4, 0);
			ActiveFrameThicknessLabel.Name = "ActiveFrameThicknessLabel";
			ActiveFrameThicknessLabel.Size = new Size(60, 15);
			ActiveFrameThicknessLabel.TabIndex = 48;
			ActiveFrameThicknessLabel.Text = "Thickness";
			//
			// ActiveClientHighlightThicknessNumericEdit
			//
			ActiveClientHighlightThicknessNumericEdit.BackColor = SystemColors.Window;
			ActiveClientHighlightThicknessNumericEdit.BorderStyle = BorderStyle.FixedSingle;
			ActiveClientHighlightThicknessNumericEdit.CausesValidation = false;
			ActiveClientHighlightThicknessNumericEdit.Location = new Point(90, 76);
			ActiveClientHighlightThicknessNumericEdit.Margin = new Padding(4);
			ActiveClientHighlightThicknessNumericEdit.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
			ActiveClientHighlightThicknessNumericEdit.Maximum = new decimal(new int[] { 6, 0, 0, 0 });
			ActiveClientHighlightThicknessNumericEdit.Name = "ActiveClientHighlightThicknessNumericEdit";
			ActiveClientHighlightThicknessNumericEdit.Size = new Size(72, 23);
			ActiveClientHighlightThicknessNumericEdit.TabIndex = 49;
			ActiveClientHighlightThicknessNumericEdit.Value = new decimal(new int[] { 3, 0, 0, 0 });
			ActiveClientHighlightThicknessNumericEdit.ValueChanged += OptionChanged_Handler;
			//
			// ShowCycleGroupNameCheckBox
			//
			ShowCycleGroupNameCheckBox.AutoSize = true;
			ShowCycleGroupNameCheckBox.Location = new Point(12, 14);
			ShowCycleGroupNameCheckBox.Margin = new Padding(4);
			ShowCycleGroupNameCheckBox.Name = "ShowCycleGroupNameCheckBox";
			ShowCycleGroupNameCheckBox.Size = new Size(160, 19);
			ShowCycleGroupNameCheckBox.TabIndex = 50;
			ShowCycleGroupNameCheckBox.Text = "Show cycle group name";
			ShowCycleGroupNameCheckBox.UseVisualStyleBackColor = true;
			ShowCycleGroupNameCheckBox.CheckedChanged += OptionChanged_Handler;
			//
			// HighlightColorLabel
			//
			HighlightColorLabel.AutoSize = true;
			HighlightColorLabel.Location = new Point(12, 48);
			HighlightColorLabel.Margin = new Padding(4, 0, 4, 0);
			HighlightColorLabel.Name = "HighlightColorLabel";
			HighlightColorLabel.Size = new Size(36, 15);
			HighlightColorLabel.TabIndex = 29;
			HighlightColorLabel.Text = "Color";
			// 
			// ActiveClientHighlightColorButton
			// 
			ActiveClientHighlightColorButton.BorderStyle = BorderStyle.FixedSingle;
			ActiveClientHighlightColorButton.Location = new Point(90, 47);
			ActiveClientHighlightColorButton.Margin = new Padding(4);
			ActiveClientHighlightColorButton.Name = "ActiveClientHighlightColorButton";
			ActiveClientHighlightColorButton.Size = new Size(108, 19);
			ActiveClientHighlightColorButton.TabIndex = 28;
			ActiveClientHighlightColorButton.Click += ActiveClientHighlightColorButton_Click;
			// 
			// EnableActiveClientHighlightCheckBox
			// 
			EnableActiveClientHighlightCheckBox.AutoSize = true;
			EnableActiveClientHighlightCheckBox.Checked = true;
			EnableActiveClientHighlightCheckBox.CheckState = CheckState.Checked;
			EnableActiveClientHighlightCheckBox.Location = new Point(12, 14);
			EnableActiveClientHighlightCheckBox.Margin = new Padding(4);
			EnableActiveClientHighlightCheckBox.Name = "EnableActiveClientHighlightCheckBox";
			EnableActiveClientHighlightCheckBox.RightToLeft = RightToLeft.No;
			EnableActiveClientHighlightCheckBox.Size = new Size(142, 19);
			EnableActiveClientHighlightCheckBox.TabIndex = 27;
			EnableActiveClientHighlightCheckBox.Text = "Highlight active client";
			EnableActiveClientHighlightCheckBox.UseVisualStyleBackColor = true;
			EnableActiveClientHighlightCheckBox.CheckedChanged += OptionChanged_Handler;
			// 
			// ShowThumbnailOverlaysCheckBox
			// 
			ShowThumbnailOverlaysCheckBox.AutoSize = true;
			ShowThumbnailOverlaysCheckBox.Checked = true;
			ShowThumbnailOverlaysCheckBox.CheckState = CheckState.Checked;
			ShowThumbnailOverlaysCheckBox.Location = new Point(12, 14);
			ShowThumbnailOverlaysCheckBox.Margin = new Padding(4);
			ShowThumbnailOverlaysCheckBox.Name = "ShowThumbnailOverlaysCheckBox";
			ShowThumbnailOverlaysCheckBox.RightToLeft = RightToLeft.No;
			ShowThumbnailOverlaysCheckBox.Size = new Size(96, 19);
			ShowThumbnailOverlaysCheckBox.TabIndex = 25;
			ShowThumbnailOverlaysCheckBox.Text = "Show overlay";
			ShowThumbnailOverlaysCheckBox.UseVisualStyleBackColor = true;
			ShowThumbnailOverlaysCheckBox.CheckedChanged += OptionChanged_Handler;
			// 
			// ShowThumbnailFramesCheckBox
			// 
			ShowThumbnailFramesCheckBox.AutoSize = true;
			ShowThumbnailFramesCheckBox.Checked = true;
			ShowThumbnailFramesCheckBox.CheckState = CheckState.Checked;
			ShowThumbnailFramesCheckBox.Location = new Point(12, 42);
			ShowThumbnailFramesCheckBox.Margin = new Padding(4);
			ShowThumbnailFramesCheckBox.Name = "ShowThumbnailFramesCheckBox";
			ShowThumbnailFramesCheckBox.RightToLeft = RightToLeft.No;
			ShowThumbnailFramesCheckBox.Size = new Size(94, 19);
			ShowThumbnailFramesCheckBox.TabIndex = 26;
			ShowThumbnailFramesCheckBox.Text = "Show frames";
			ShowThumbnailFramesCheckBox.UseVisualStyleBackColor = true;
			ShowThumbnailFramesCheckBox.CheckedChanged += OptionChanged_Handler;
			// 
			// ClientsTabPage
			// 
			ClientsTabPage.BackColor = SystemColors.Control;
			ClientsTabPage.Controls.Add(ClientsPanel);
			ClientsTabPage.Location = new Point(124, 4);
			ClientsTabPage.Margin = new Padding(4);
			ClientsTabPage.Name = "ClientsTabPage";
			ClientsTabPage.Size = new Size(327, 322);
			ClientsTabPage.TabIndex = 4;
			ClientsTabPage.Text = "Active Clients";
			//
			// ClientsPanel
			//
			ClientsPanel.Controls.Add(ThumbnailsList);
			ClientsPanel.Controls.Add(ThumbnailsListLabel);
			ClientsPanel.Controls.Add(ClientCycleGroupLabel);
			ClientsPanel.Controls.Add(ClientCycleGroupCombo);
			ClientsPanel.Dock = DockStyle.Fill;
			ClientsPanel.Location = new Point(0, 0);
			ClientsPanel.Margin = new Padding(4);
			ClientsPanel.Name = "ClientsPanel";
			ClientsPanel.Size = new Size(327, 322);
			ClientsPanel.TabIndex = 32;
			// 
			// ThumbnailsList
			// 
			ThumbnailsList.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			ThumbnailsList.BackColor = SystemColors.Window;
			ThumbnailsList.BorderStyle = BorderStyle.FixedSingle;
			ThumbnailsList.CheckOnClick = true;
			ThumbnailsList.FormattingEnabled = true;
			ThumbnailsList.IntegralHeight = false;
			ThumbnailsList.Location = new Point(0, 34);
			ThumbnailsList.Margin = new Padding(4);
			ThumbnailsList.Name = "ThumbnailsList";
			ThumbnailsList.Size = new Size(325, 252);
			ThumbnailsList.TabIndex = 34;
			ThumbnailsList.ItemCheck += ThumbnailsList_ItemCheck_Handler;
			//
			// ClientCycleGroupLabel
			//
			ClientCycleGroupLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			ClientCycleGroupLabel.AutoSize = true;
			ClientCycleGroupLabel.Location = new Point(9, 296);
			ClientCycleGroupLabel.Margin = new Padding(4, 0, 4, 0);
			ClientCycleGroupLabel.Name = "ClientCycleGroupLabel";
			ClientCycleGroupLabel.Size = new Size(70, 15);
			ClientCycleGroupLabel.TabIndex = 35;
			ClientCycleGroupLabel.Text = "Cycle group";
			//
			// ClientCycleGroupCombo
			//
			ClientCycleGroupCombo.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			ClientCycleGroupCombo.DropDownStyle = ComboBoxStyle.DropDownList;
			ClientCycleGroupCombo.Location = new Point(95, 292);
			ClientCycleGroupCombo.Margin = new Padding(4);
			ClientCycleGroupCombo.Name = "ClientCycleGroupCombo";
			ClientCycleGroupCombo.Size = new Size(221, 23);
			ClientCycleGroupCombo.TabIndex = 36;
			ClientCycleGroupCombo.SelectedIndexChanged += ClientCycleGroupCombo_SelectedIndexChanged_Handler;
			// 
			// ThumbnailsListLabel
			// 
			ThumbnailsListLabel.AutoSize = true;
			ThumbnailsListLabel.Location = new Point(9, 10);
			ThumbnailsListLabel.Margin = new Padding(4, 0, 4, 0);
			ThumbnailsListLabel.Name = "ThumbnailsListLabel";
			ThumbnailsListLabel.Size = new Size(182, 15);
			ThumbnailsListLabel.TabIndex = 33;
			ThumbnailsListLabel.Text = "Thumbnails";
			//
			// CycleGroupsTabPage
			//
			CycleGroupsTabPage.BackColor = SystemColors.Control;
			CycleGroupsTabPage.Controls.Add(CycleGroupsPanel);
			CycleGroupsTabPage.Location = new Point(124, 4);
			CycleGroupsTabPage.Margin = new Padding(4);
			CycleGroupsTabPage.Name = "CycleGroupsTabPage";
			CycleGroupsTabPage.Size = new Size(327, 322);
			CycleGroupsTabPage.TabIndex = 7;
			CycleGroupsTabPage.Text = "Cycle Groups";
			//
			// CycleGroupsPanel
			//
			CycleGroupsPanel.Controls.Add(CycleGroupSelectLabel);
			CycleGroupsPanel.Controls.Add(CycleGroupSelectCombo);
			CycleGroupsPanel.Controls.Add(CycleGroupRenameButton);
			CycleGroupsPanel.Controls.Add(CycleGroupAddGroupButton);
			CycleGroupsPanel.Controls.Add(CycleGroupRemoveGroupButton);
			CycleGroupsPanel.Controls.Add(CycleGroupClientsLabel);
			CycleGroupsPanel.Controls.Add(CycleGroupClientsListBox);
			CycleGroupsPanel.Controls.Add(CycleGroupMoveUpButton);
			CycleGroupsPanel.Controls.Add(CycleGroupMoveDownButton);
			CycleGroupsPanel.Controls.Add(CycleGroupRemoveClientButton);
			CycleGroupsPanel.Controls.Add(CycleGroupAddClientLabel);
			CycleGroupsPanel.Controls.Add(CycleGroupAddClientCombo);
			CycleGroupsPanel.Controls.Add(CycleGroupAddClientButton);
			CycleGroupsPanel.Dock = DockStyle.Fill;
			CycleGroupsPanel.Location = new Point(0, 0);
			CycleGroupsPanel.Margin = new Padding(4);
			CycleGroupsPanel.Name = "CycleGroupsPanel";
			CycleGroupsPanel.Size = new Size(327, 322);
			CycleGroupsPanel.TabIndex = 0;
			//
			// CycleGroupSelectLabel
			//
			CycleGroupSelectLabel.AutoSize = true;
			CycleGroupSelectLabel.Location = new Point(9, 10);
			CycleGroupSelectLabel.Margin = new Padding(4, 0, 4, 0);
			CycleGroupSelectLabel.Name = "CycleGroupSelectLabel";
			CycleGroupSelectLabel.Size = new Size(70, 15);
			CycleGroupSelectLabel.TabIndex = 0;
			CycleGroupSelectLabel.Text = "Cycle group";
			//
			// CycleGroupSelectCombo
			//
			CycleGroupSelectCombo.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			CycleGroupSelectCombo.DropDownStyle = ComboBoxStyle.DropDownList;
			CycleGroupSelectCombo.Location = new Point(95, 7);
			CycleGroupSelectCombo.Margin = new Padding(4);
			CycleGroupSelectCombo.Name = "CycleGroupSelectCombo";
			CycleGroupSelectCombo.Size = new Size(103, 23);
			CycleGroupSelectCombo.TabIndex = 1;
			CycleGroupSelectCombo.SelectedIndexChanged += CycleGroupSelectCombo_SelectedIndexChanged_Handler;
			//
			// CycleGroupRenameButton
			//
			CycleGroupRenameButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			CycleGroupRenameButton.Location = new Point(204, 6);
			CycleGroupRenameButton.Margin = new Padding(4);
			CycleGroupRenameButton.Name = "CycleGroupRenameButton";
			CycleGroupRenameButton.Size = new Size(32, 25);
			CycleGroupRenameButton.TabIndex = 9;
			CycleGroupRenameButton.Text = "✎";
			CycleGroupRenameButton.UseVisualStyleBackColor = true;
			CycleGroupRenameButton.Click += CycleGroupRenameButton_Click_Handler;
			//
			// CycleGroupAddGroupButton
			//
			CycleGroupAddGroupButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			CycleGroupAddGroupButton.Location = new Point(240, 6);
			CycleGroupAddGroupButton.Margin = new Padding(4);
			CycleGroupAddGroupButton.Name = "CycleGroupAddGroupButton";
			CycleGroupAddGroupButton.Size = new Size(36, 25);
			CycleGroupAddGroupButton.TabIndex = 10;
			CycleGroupAddGroupButton.Text = "+";
			CycleGroupAddGroupButton.UseVisualStyleBackColor = true;
			CycleGroupAddGroupButton.Click += CycleGroupAddGroupButton_Click_Handler;
			//
			// CycleGroupRemoveGroupButton
			//
			CycleGroupRemoveGroupButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			CycleGroupRemoveGroupButton.Location = new Point(280, 6);
			CycleGroupRemoveGroupButton.Margin = new Padding(4);
			CycleGroupRemoveGroupButton.Name = "CycleGroupRemoveGroupButton";
			CycleGroupRemoveGroupButton.Size = new Size(36, 25);
			CycleGroupRemoveGroupButton.TabIndex = 11;
			CycleGroupRemoveGroupButton.Text = "−";
			CycleGroupRemoveGroupButton.UseVisualStyleBackColor = true;
			CycleGroupRemoveGroupButton.Click += CycleGroupRemoveGroupButton_Click_Handler;
			//
			// CycleGroupClientsLabel
			//
			CycleGroupClientsLabel.AutoSize = true;
			CycleGroupClientsLabel.Location = new Point(9, 38);
			CycleGroupClientsLabel.Margin = new Padding(4, 0, 4, 0);
			CycleGroupClientsLabel.Name = "CycleGroupClientsLabel";
			CycleGroupClientsLabel.Size = new Size(160, 15);
			CycleGroupClientsLabel.TabIndex = 2;
			CycleGroupClientsLabel.Text = "Clients in group (cycle order)";
			//
			// CycleGroupClientsListBox
			//
			CycleGroupClientsListBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			CycleGroupClientsListBox.IntegralHeight = false;
			CycleGroupClientsListBox.Location = new Point(9, 56);
			CycleGroupClientsListBox.Margin = new Padding(4);
			CycleGroupClientsListBox.Name = "CycleGroupClientsListBox";
			CycleGroupClientsListBox.Size = new Size(240, 224);
			CycleGroupClientsListBox.TabIndex = 3;
			//
			// CycleGroupMoveUpButton
			//
			CycleGroupMoveUpButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			CycleGroupMoveUpButton.Location = new Point(255, 56);
			CycleGroupMoveUpButton.Margin = new Padding(4);
			CycleGroupMoveUpButton.Name = "CycleGroupMoveUpButton";
			CycleGroupMoveUpButton.Size = new Size(61, 27);
			CycleGroupMoveUpButton.TabIndex = 4;
			CycleGroupMoveUpButton.Text = "Up";
			CycleGroupMoveUpButton.UseVisualStyleBackColor = true;
			CycleGroupMoveUpButton.Click += CycleGroupMoveUpButton_Click_Handler;
			//
			// CycleGroupMoveDownButton
			//
			CycleGroupMoveDownButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			CycleGroupMoveDownButton.Location = new Point(255, 89);
			CycleGroupMoveDownButton.Margin = new Padding(4);
			CycleGroupMoveDownButton.Name = "CycleGroupMoveDownButton";
			CycleGroupMoveDownButton.Size = new Size(61, 27);
			CycleGroupMoveDownButton.TabIndex = 5;
			CycleGroupMoveDownButton.Text = "Down";
			CycleGroupMoveDownButton.UseVisualStyleBackColor = true;
			CycleGroupMoveDownButton.Click += CycleGroupMoveDownButton_Click_Handler;
			//
			// CycleGroupRemoveClientButton
			//
			CycleGroupRemoveClientButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			CycleGroupRemoveClientButton.Location = new Point(255, 122);
			CycleGroupRemoveClientButton.Margin = new Padding(4);
			CycleGroupRemoveClientButton.Name = "CycleGroupRemoveClientButton";
			CycleGroupRemoveClientButton.Size = new Size(61, 27);
			CycleGroupRemoveClientButton.TabIndex = 6;
			CycleGroupRemoveClientButton.Text = "Remove";
			CycleGroupRemoveClientButton.UseVisualStyleBackColor = true;
			CycleGroupRemoveClientButton.Click += CycleGroupRemoveClientButton_Click_Handler;
			//
			// CycleGroupAddClientLabel
			//
			CycleGroupAddClientLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			CycleGroupAddClientLabel.AutoSize = true;
			CycleGroupAddClientLabel.Location = new Point(9, 296);
			CycleGroupAddClientLabel.Margin = new Padding(4, 0, 4, 0);
			CycleGroupAddClientLabel.Name = "CycleGroupAddClientLabel";
			CycleGroupAddClientLabel.Size = new Size(29, 15);
			CycleGroupAddClientLabel.TabIndex = 7;
			CycleGroupAddClientLabel.Text = "Add";
			//
			// CycleGroupAddClientCombo
			//
			CycleGroupAddClientCombo.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			CycleGroupAddClientCombo.DropDownStyle = ComboBoxStyle.DropDownList;
			CycleGroupAddClientCombo.Location = new Point(45, 292);
			CycleGroupAddClientCombo.Margin = new Padding(4);
			CycleGroupAddClientCombo.Name = "CycleGroupAddClientCombo";
			CycleGroupAddClientCombo.Size = new Size(204, 23);
			CycleGroupAddClientCombo.TabIndex = 8;
			//
			// CycleGroupAddClientButton
			//
			CycleGroupAddClientButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			CycleGroupAddClientButton.Location = new Point(255, 290);
			CycleGroupAddClientButton.Margin = new Padding(4);
			CycleGroupAddClientButton.Name = "CycleGroupAddClientButton";
			CycleGroupAddClientButton.Size = new Size(61, 27);
			CycleGroupAddClientButton.TabIndex = 9;
			CycleGroupAddClientButton.Text = "Add";
			CycleGroupAddClientButton.UseVisualStyleBackColor = true;
			CycleGroupAddClientButton.Click += CycleGroupAddClientButton_Click_Handler;
			//
			// HotkeysTabPage
			//
			HotkeysTabPage.BackColor = SystemColors.Control;
			HotkeysTabPage.Controls.Add(HotkeysPanel);
			HotkeysTabPage.Location = new Point(124, 4);
			HotkeysTabPage.Margin = new Padding(4);
			HotkeysTabPage.Name = "HotkeysTabPage";
			HotkeysTabPage.Size = new Size(327, 322);
			HotkeysTabPage.TabIndex = 6;
			HotkeysTabPage.Text = "Hotkeys";
			//
			// HotkeysPanel
			//
			HotkeysPanel.Controls.Add(HotkeyBindingsListView);
			HotkeysPanel.Controls.Add(AddHotkeyButton);
			HotkeysPanel.Controls.Add(EditHotkeyButton);
			HotkeysPanel.Controls.Add(RemoveHotkeyButton);
			HotkeysPanel.Controls.Add(HotkeyStatusLabel);
			HotkeysPanel.Dock = DockStyle.Fill;
			HotkeysPanel.Location = new Point(0, 0);
			HotkeysPanel.Margin = new Padding(4);
			HotkeysPanel.Name = "HotkeysPanel";
			HotkeysPanel.Size = new Size(327, 322);
			HotkeysPanel.TabIndex = 0;
			//
			// HotkeyBindingsListView
			//
			HotkeyBindingsListView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			HotkeyBindingsListView.Columns.AddRange(new ColumnHeader[] { HotkeyActionColumnHeader, HotkeyKeyColumnHeader });
			HotkeyBindingsListView.FullRowSelect = true;
			HotkeyBindingsListView.HeaderStyle = ColumnHeaderStyle.Nonclickable;
			HotkeyBindingsListView.Location = new Point(9, 8);
			HotkeyBindingsListView.Margin = new Padding(4);
			HotkeyBindingsListView.MultiSelect = false;
			HotkeyBindingsListView.Name = "HotkeyBindingsListView";
			HotkeyBindingsListView.Size = new Size(307, 248);
			HotkeyBindingsListView.TabIndex = 0;
			HotkeyBindingsListView.UseCompatibleStateImageBehavior = false;
			HotkeyBindingsListView.View = System.Windows.Forms.View.Details;
			//
			// AddHotkeyButton
			//
			AddHotkeyButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			AddHotkeyButton.Location = new Point(9, 264);
			AddHotkeyButton.Margin = new Padding(4);
			AddHotkeyButton.Name = "AddHotkeyButton";
			AddHotkeyButton.Size = new Size(95, 27);
			AddHotkeyButton.TabIndex = 1;
			AddHotkeyButton.Text = "Add...";
			AddHotkeyButton.UseVisualStyleBackColor = true;
			AddHotkeyButton.Click += AddHotkeyButton_Click_Handler;
			//
			// EditHotkeyButton
			//
			EditHotkeyButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			EditHotkeyButton.Location = new Point(111, 264);
			EditHotkeyButton.Margin = new Padding(4);
			EditHotkeyButton.Name = "EditHotkeyButton";
			EditHotkeyButton.Size = new Size(95, 27);
			EditHotkeyButton.TabIndex = 2;
			EditHotkeyButton.Text = "Edit...";
			EditHotkeyButton.UseVisualStyleBackColor = true;
			EditHotkeyButton.Click += EditHotkeyButton_Click_Handler;
			//
			// RemoveHotkeyButton
			//
			RemoveHotkeyButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			RemoveHotkeyButton.Location = new Point(213, 264);
			RemoveHotkeyButton.Margin = new Padding(4);
			RemoveHotkeyButton.Name = "RemoveHotkeyButton";
			RemoveHotkeyButton.Size = new Size(95, 27);
			RemoveHotkeyButton.TabIndex = 3;
			RemoveHotkeyButton.Text = "Remove";
			RemoveHotkeyButton.UseVisualStyleBackColor = true;
			RemoveHotkeyButton.Click += RemoveHotkeyButton_Click_Handler;
			//
			// HotkeyStatusLabel
			//
			HotkeyStatusLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			HotkeyStatusLabel.ForeColor = SystemColors.GrayText;
			HotkeyStatusLabel.Location = new Point(9, 298);
			HotkeyStatusLabel.Margin = new Padding(4, 0, 4, 0);
			HotkeyStatusLabel.Name = "HotkeyStatusLabel";
			HotkeyStatusLabel.Size = new Size(307, 15);
			HotkeyStatusLabel.TabIndex = 3;
			//
			// HotkeyActionColumnHeader
			//
			HotkeyActionColumnHeader.Text = "Action";
			HotkeyActionColumnHeader.Width = 190;
			//
			// HotkeyKeyColumnHeader
			//
			HotkeyKeyColumnHeader.Text = "Hotkey";
			HotkeyKeyColumnHeader.Width = 110;
			//
			// AboutTabPage
			//
			AboutTabPage.BackColor = SystemColors.Control;
			AboutTabPage.Controls.Add(AboutPanel);
			AboutTabPage.Location = new Point(124, 4);
			AboutTabPage.Margin = new Padding(4);
			AboutTabPage.Name = "AboutTabPage";
			AboutTabPage.Size = new Size(327, 243);
			AboutTabPage.TabIndex = 5;
			AboutTabPage.Text = "About";
			// 
			// AboutPanel
			// 
			AboutPanel.BackColor = Color.Transparent;
			AboutPanel.Controls.Add(CreditMaintLabel);
			AboutPanel.Controls.Add(DocumentationLinkLabel);
			AboutPanel.Controls.Add(DescriptionLabel);
			AboutPanel.Controls.Add(VersionLabel);
			AboutPanel.Controls.Add(NameLabel);
			AboutPanel.Controls.Add(DocumentationLink);
			AboutPanel.Dock = DockStyle.Fill;
			AboutPanel.Location = new Point(0, 0);
			AboutPanel.Margin = new Padding(4);
			AboutPanel.Name = "AboutPanel";
			AboutPanel.Size = new Size(327, 243);
			AboutPanel.TabIndex = 2;
			// 
			// CreditMaintLabel
			// 
			CreditMaintLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			CreditMaintLabel.AutoSize = true;
			CreditMaintLabel.Location = new Point(0, 165);
			CreditMaintLabel.Margin = new Padding(4, 0, 4, 0);
			CreditMaintLabel.Name = "CreditMaintLabel";
			CreditMaintLabel.Padding = new Padding(9, 4, 9, 4);
			CreditMaintLabel.Size = new Size(292, 23);
			CreditMaintLabel.TabIndex = 7;
			CreditMaintLabel.Text = "Credit to previous maintainer: Phrynohyas Tig-Rah";
			// 
			// DocumentationLinkLabel
			// 
			DocumentationLinkLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			DocumentationLinkLabel.AutoSize = true;
			DocumentationLinkLabel.Location = new Point(0, 188);
			DocumentationLinkLabel.Margin = new Padding(4, 0, 4, 0);
			DocumentationLinkLabel.Name = "DocumentationLinkLabel";
			DocumentationLinkLabel.Padding = new Padding(9, 4, 9, 4);
			DocumentationLinkLabel.Size = new Size(259, 23);
			DocumentationLinkLabel.TabIndex = 6;
			DocumentationLinkLabel.Text = "For more information visit the forum thread:";
			// 
			// DescriptionLabel
			// 
			DescriptionLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			DescriptionLabel.BackColor = Color.Transparent;
			DescriptionLabel.Location = new Point(0, 34);
			DescriptionLabel.Margin = new Padding(4, 0, 4, 0);
			DescriptionLabel.Name = "DescriptionLabel";
			DescriptionLabel.Padding = new Padding(9, 4, 9, 4);
			DescriptionLabel.Size = new Size(304, 167);
			DescriptionLabel.TabIndex = 5;
			// The text comes from the localization resources
			// 
			// VersionLabel
			// 
			VersionLabel.AutoSize = true;
			VersionLabel.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 204);
			VersionLabel.Location = new Point(155, 10);
			VersionLabel.Margin = new Padding(4, 0, 4, 0);
			VersionLabel.Name = "VersionLabel";
			VersionLabel.Size = new Size(49, 20);
			VersionLabel.TabIndex = 4;
			VersionLabel.Text = "1.0.0";
			// 
			// NameLabel
			// 
			NameLabel.AutoSize = true;
			NameLabel.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 204);
			NameLabel.Location = new Point(5, 10);
			NameLabel.Margin = new Padding(4, 0, 4, 0);
			NameLabel.Name = "NameLabel";
			NameLabel.Size = new Size(131, 20);
			NameLabel.TabIndex = 3;
			NameLabel.Text = "EVE-O-Preview";
			// 
			// DocumentationLink
			// 
			DocumentationLink.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			DocumentationLink.Location = new Point(0, 204);
			DocumentationLink.Margin = new Padding(35, 4, 4, 4);
			DocumentationLink.Name = "DocumentationLink";
			DocumentationLink.Padding = new Padding(9, 4, 9, 4);
			DocumentationLink.Size = new Size(306, 38);
			DocumentationLink.TabIndex = 2;
			DocumentationLink.TabStop = true;
			// The URL itself is filled in by the presenter
			DocumentationLink.LinkClicked += DocumentationLinkClicked_Handler;
			// 
			// NotifyIcon
			// 
			NotifyIcon.ContextMenuStrip = TrayMenu;
			NotifyIcon.Icon = (Icon)resources.GetObject("NotifyIcon.Icon");
			NotifyIcon.Text = "EVE-O-Preview";
			NotifyIcon.Visible = true;
			NotifyIcon.MouseDoubleClick += RestoreMainForm_Handler;
			// 
			// TrayMenu
			// 
			TrayMenu.ImageScalingSize = new Size(24, 24);
			TrayMenu.Items.AddRange(new ToolStripItem[] { TitleMenuItem, RestoreWindowMenuItem, SeparatorMenuItem, ExitMenuItem });
			TrayMenu.Name = "contextMenuStrip1";
			TrayMenu.Size = new Size(154, 76);
			//
			// StatusBar
			//
			// Hosts the standard Windows sizing grip: the tab control fills the form
			// and would cover the grip drawn by the form itself
			StatusBar.BackColor = SystemColors.Control;
			StatusBar.Dock = DockStyle.Bottom;
			StatusBar.GripStyle = ToolStripGripStyle.Hidden;
			StatusBar.Location = new Point(0, 308);
			StatusBar.Name = "StatusBar";
			StatusBar.Size = new Size(455, 22);
			StatusBar.SizingGrip = true;
			StatusBar.TabIndex = 7;
			//
			// MainForm
			//
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			BackColor = SystemColors.Control;
			ClientSize = new Size(455, 330);
			Controls.Add(ContentTabControl);
			Controls.Add(StatusBar);
			FormBorderStyle = FormBorderStyle.Sizable;
			Icon = (Icon)resources.GetObject("$this.Icon");
			Margin = new Padding(0);
			MaximizeBox = true;
			SizeGripStyle = SizeGripStyle.Show;
			Name = "MainForm";
			Text = "EVE-O-Preview";
			TopMost = true;
			FormClosing += MainFormClosing_Handler;
			Load += MainFormResize_Handler;
			Resize += MainFormResize_Handler;
			ContentTabControl.ResumeLayout(false);
			GeneralTabPage.ResumeLayout(false);
			GeneralSettingsPanel.ResumeLayout(false);
			GeneralSettingsPanel.PerformLayout();
			ClientWindowsTabPage.ResumeLayout(false);
			ClientWindowsPanel.ResumeLayout(false);
			ClientWindowsPanel.PerformLayout();
			ThumbnailTabPage.ResumeLayout(false);
			ThumbnailSettingsPanel.ResumeLayout(false);
			PreviewSubTabControl.ResumeLayout(false);
			PreviewGeneralSubPage.ResumeLayout(false);
			PreviewGeneralTablePanel.ResumeLayout(false);
			PreviewGeneralTablePanel.PerformLayout();
			PreviewVisualSubPage.ResumeLayout(false);
			PreviewVisualTablePanel.ResumeLayout(false);
			PreviewVisualTablePanel.PerformLayout();
			PreviewRenderingSubPage.ResumeLayout(false);
			PreviewRenderingTablePanel.ResumeLayout(false);
			PreviewRenderingTablePanel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)ThumbnailRefreshPeriodNumericEdit).EndInit();
			((System.ComponentModel.ISupportInitialize)MinimizedClientsRefreshPeriodNumericEdit).EndInit();
			PreviewLayoutSubPage.ResumeLayout(false);
			PreviewLayoutTablePanel.ResumeLayout(false);
			PreviewLayoutTablePanel.PerformLayout();
			PreviewZoomSubPage.ResumeLayout(false);
			PreviewZoomTablePanel.ResumeLayout(false);
			PreviewZoomTablePanel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)ThumbnailSnapToGridSizeYNumericEdit).EndInit();
			((System.ComponentModel.ISupportInitialize)ActiveClientHighlightThicknessNumericEdit).EndInit();
			((System.ComponentModel.ISupportInitialize)ThumbnailSnapToGridSizeXNumericEdit).EndInit();
			((System.ComponentModel.ISupportInitialize)ThumbnailsWidthNumericEdit).EndInit();
			((System.ComponentModel.ISupportInitialize)ThumbnailsHeightNumericEdit).EndInit();
			((System.ComponentModel.ISupportInitialize)ThumbnailOpacityTrackBar).EndInit();
			ZoomAnchorPanel.ResumeLayout(false);
			ZoomAnchorPanel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)ThumbnailZoomFactorNumericEdit).EndInit();
			OverlayTabPage.ResumeLayout(false);
			OverlaySettingsPanel.ResumeLayout(false);
			OverlaySubTabControl.ResumeLayout(false);
			OverlayGeneralSubPage.ResumeLayout(false);
			OverlayGeneralSubPage.PerformLayout();
			OverlayWindowNameSubPage.ResumeLayout(false);
			OverlayWindowNameSubPage.PerformLayout();
			OverlayLabelFontPreviewPanel.ResumeLayout(false);
			OverlayLabelFontPreviewPanel.PerformLayout();
			OverlayGroupNameSubPage.ResumeLayout(false);
			OverlayGroupNameSubPage.PerformLayout();
			CycleGroupNameFontPreviewPanel.ResumeLayout(false);
			CycleGroupNameFontPreviewPanel.PerformLayout();
			OverlayBorderSubPage.ResumeLayout(false);
			OverlayBorderSubPage.PerformLayout();
			CycleGroupIndicatorPanel.ResumeLayout(false);
			CycleGroupIndicatorPanel.PerformLayout();
			OverlayLabelAnchorPanel.ResumeLayout(false);
			OverlayLabelAnchorPanel.PerformLayout();
			ClientsTabPage.ResumeLayout(false);
			ClientsPanel.ResumeLayout(false);
			ClientsPanel.PerformLayout();
			CycleGroupsTabPage.ResumeLayout(false);
			CycleGroupsPanel.ResumeLayout(false);
			CycleGroupsPanel.PerformLayout();
			HotkeysTabPage.ResumeLayout(false);
			HotkeysPanel.ResumeLayout(false);
			HotkeysPanel.PerformLayout();
			AboutTabPage.ResumeLayout(false);
			AboutPanel.ResumeLayout(false);
			AboutPanel.PerformLayout();
			TrayMenu.ResumeLayout(false);
			ResumeLayout(false);

		}

		#endregion
		private NotifyIcon NotifyIcon;
		private ContextMenuStrip TrayMenu;
		private CheckBox EnableClientLayoutTrackingCheckBox;
		private CheckBox HideActiveClientThumbnailCheckBox;
		private CheckBox ShowThumbnailsAlwaysOnTopCheckBox;
		private CheckBox HideThumbnailsOnLostFocusCheckBox;
		private CheckBox EnablePerClientThumbnailsLayoutsCheckBox;
		private CheckBox MinimizeToTrayCheckBox;
		private NumericUpDown ThumbnailsWidthNumericEdit;
		private NumericUpDown ThumbnailRefreshPeriodNumericEdit;
		private NumericUpDown MinimizedClientsRefreshPeriodNumericEdit;
		private NumericUpDown ThumbnailsHeightNumericEdit;
		private TrackBar ThumbnailOpacityTrackBar;
		private Panel ZoomAnchorPanel;
		private RadioButton ZoomAnchorNWRadioButton;
		private RadioButton ZoomAnchorNRadioButton;
		private RadioButton ZoomAnchorNERadioButton;
		private RadioButton ZoomAnchorWRadioButton;
		private RadioButton ZoomAnchorSERadioButton;
		private RadioButton ZoomAnchorCRadioButton;
		private RadioButton ZoomAnchorSRadioButton;
		private RadioButton ZoomAnchorERadioButton;
		private RadioButton ZoomAnchorSWRadioButton;
		private CheckBox EnableThumbnailZoomCheckBox;
		private NumericUpDown ThumbnailZoomFactorNumericEdit;
		private Label HighlightColorLabel;
		private Panel ActiveClientHighlightColorButton;
		private CheckBox EnableActiveClientHighlightCheckBox;
		private CheckBox ShowThumbnailOverlaysCheckBox;
		private CheckBox ShowThumbnailFramesCheckBox;
		private CheckedListBox ThumbnailsList;
		private LinkLabel DocumentationLink;
		private Label VersionLabel;
		private CheckBox MinimizeInactiveClientsCheckBox;
        private CheckBox LockThumbnailLocationCheckbox;
        private NumericUpDown ThumbnailSnapToGridSizeYNumericEdit;
        private Label SnapYLabel;
        private NumericUpDown ThumbnailSnapToGridSizeXNumericEdit;
        private Label SnapXLabel;
        private CheckBox ThumbnailSnapToGridCheckBox;
        private Label OverlayLabelPositionLabel;
        private Label OverlayLabelColorLabel;
        private Panel OverlayLabelColorButton;
        private Panel OverlayLabelAnchorPanel;
        private RadioButton OverlayLabelNWRadioButton;
        private RadioButton OverlayLabelNRadioButton;
        private RadioButton OverlayLabelNERadioButton;
        private RadioButton OverlayLabelWRadioButton;
        private RadioButton OverlayLabelSERadioButton;
        private RadioButton OverlayLabelCRadioButton;
        private RadioButton OverlayLabelSRadioButton;
        private RadioButton OverlayLabelERadioButton;
        private RadioButton OverlayLabelSWRadioButton;
		private CheckBox DisableAnimationCheckBox;
		private Label LanguageLabel;
		private ComboBox LanguageCombo;
		private Label LanguageRestartHintLabel;
		private CheckBox HideCaptionOnClientsCheckBox;
		private Button btnLabelFont;
		private Label LabelOverlayLabelFont;
		private CheckBox PreventPreviewsCheckBox;
		private Label PreventPreviewColorLabel;
		private Panel PreventPreviewColorButton;
		private Panel CycleGroupIndicatorPanel;
		private RadioButton CycleGroupIndicatorNWRadioButton;
		private RadioButton CycleGroupIndicatorNRadioButton;
		private RadioButton CycleGroupIndicatorNERadioButton;
		private RadioButton CycleGroupIndicatorWRadioButton;
		private RadioButton CycleGroupIndicatorSERadioButton;
		private RadioButton CycleGroupIndicatorCRadioButton;
		private RadioButton CycleGroupIndicatorSRadioButton;
		private RadioButton CycleGroupIndicatorERadioButton;
		private RadioButton CycleGroupIndicatorSWRadioButton;
		private Button AddHotkeyButton;
		private Button EditHotkeyButton;
		private NumericUpDown ActiveClientHighlightThicknessNumericEdit;
		private CheckBox ShowCycleGroupNameCheckBox;
		private Button CycleGroupRenameButton;
		private StatusStrip StatusBar;
		private TabControl OverlaySubTabControl;
		private TabPage OverlayGeneralSubPage;
		private TabPage OverlayWindowNameSubPage;
		private TabPage OverlayGroupNameSubPage;
		private TabPage OverlayBorderSubPage;
		private CheckBox ShowClientNameCheckBox;
		private CheckBox OverlayAlwaysOnTopCheckBox;
		private Panel OverlayLabelFontPreviewPanel;
		private Panel CycleGroupNameColorButton;
		private Label CycleGroupNameColorLabel;
		private Button btnCycleGroupNameFont;
		private Panel CycleGroupNameFontPreviewPanel;
		private Label LabelCycleGroupNameFont;
		private Label CycleGroupNamePositionLabel;
		private Button RemoveHotkeyButton;
		private Label HotkeyStatusLabel;
		private ListView HotkeyBindingsListView;
		private ColumnHeader HotkeyActionColumnHeader;
		private ColumnHeader HotkeyKeyColumnHeader;
		private ComboBox ClientCycleGroupCombo;
		private ComboBox CycleGroupSelectCombo;
		private Button CycleGroupAddGroupButton;
		private Button CycleGroupRemoveGroupButton;
		private ListBox CycleGroupClientsListBox;
		private Button CycleGroupMoveUpButton;
		private Button CycleGroupMoveDownButton;
		private Button CycleGroupRemoveClientButton;
		private ComboBox CycleGroupAddClientCombo;
		private Button CycleGroupAddClientButton;
	}
}