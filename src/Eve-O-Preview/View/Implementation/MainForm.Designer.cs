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
			Panel GeneralSettingsPanel;
			Label label4;
			TabPage ThumbnailTabPage;
			Panel ThumbnailSettingsPanel;
			Label HeigthLabel;
			Label WidthLabel;
			Label OpacityLabel;
			Panel ZoomSettingsPanel;
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
			AnimationStyleCombo = new ComboBox();
			MinimizeInactiveClientsCheckBox = new CheckBox();
			EnableClientLayoutTrackingCheckBox = new CheckBox();
			HideActiveClientThumbnailCheckBox = new CheckBox();
			ShowThumbnailsAlwaysOnTopCheckBox = new CheckBox();
			HideThumbnailsOnLostFocusCheckBox = new CheckBox();
			EnablePerClientThumbnailsLayoutsCheckBox = new CheckBox();
			MinimizeToTrayCheckBox = new CheckBox();
			label1 = new Label();
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
			ZoomTabPage = new TabPage();
			ZoomAnchorPanel = new Panel();
			ZoomAanchorNWRadioButton = new RadioButton();
			ZoomAanchorNRadioButton = new RadioButton();
			ZoomAanchorNERadioButton = new RadioButton();
			ZoomAanchorWRadioButton = new RadioButton();
			ZoomAanchorSERadioButton = new RadioButton();
			ZoomAanchorCRadioButton = new RadioButton();
			ZoomAanchorSRadioButton = new RadioButton();
			ZoomAanchorERadioButton = new RadioButton();
			ZoomAanchorSWRadioButton = new RadioButton();
			EnableThumbnailZoomCheckBox = new CheckBox();
			ThumbnailZoomFactorNumericEdit = new NumericUpDown();
			label5 = new Label();
			panel2 = new Panel();
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
			label3 = new Label();
			label2 = new Label();
			OverlayLabelColorButton = new Panel();
			panel1 = new Panel();
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
			GeneralSettingsPanel = new Panel();
			label4 = new Label();
			ThumbnailTabPage = new TabPage();
			ThumbnailSettingsPanel = new Panel();
			HeigthLabel = new Label();
			WidthLabel = new Label();
			OpacityLabel = new Label();
			ZoomSettingsPanel = new Panel();
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
			ThumbnailTabPage.SuspendLayout();
			ThumbnailSettingsPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)ThumbnailSnapToGridSizeYNumericEdit).BeginInit();
			((System.ComponentModel.ISupportInitialize)ActiveClientHighlightThicknessNumericEdit).BeginInit();
			((System.ComponentModel.ISupportInitialize)ThumbnailSnapToGridSizeXNumericEdit).BeginInit();
			((System.ComponentModel.ISupportInitialize)ThumbnailsWidthNumericEdit).BeginInit();
			((System.ComponentModel.ISupportInitialize)ThumbnailsHeightNumericEdit).BeginInit();
			((System.ComponentModel.ISupportInitialize)ThumbnailOpacityTrackBar).BeginInit();
			ZoomTabPage.SuspendLayout();
			ZoomSettingsPanel.SuspendLayout();
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
			panel2.SuspendLayout();
			panel1.SuspendLayout();
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
			ContentTabControl.Controls.Add(ZoomTabPage);
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
			GeneralSettingsPanel.Controls.Add(HideCaptionOnClientsCheckBox);
			GeneralSettingsPanel.Controls.Add(label4);
			GeneralSettingsPanel.Controls.Add(AnimationStyleCombo);
			GeneralSettingsPanel.Controls.Add(MinimizeInactiveClientsCheckBox);
			GeneralSettingsPanel.Controls.Add(EnableClientLayoutTrackingCheckBox);
			GeneralSettingsPanel.Controls.Add(HideActiveClientThumbnailCheckBox);
			GeneralSettingsPanel.Controls.Add(ShowThumbnailsAlwaysOnTopCheckBox);
			GeneralSettingsPanel.Controls.Add(HideThumbnailsOnLostFocusCheckBox);
			GeneralSettingsPanel.Controls.Add(EnablePerClientThumbnailsLayoutsCheckBox);
			GeneralSettingsPanel.Controls.Add(MinimizeToTrayCheckBox);
			GeneralSettingsPanel.Dock = DockStyle.Fill;
			GeneralSettingsPanel.Location = new Point(4, 4);
			GeneralSettingsPanel.Margin = new Padding(4);
			GeneralSettingsPanel.Name = "GeneralSettingsPanel";
			GeneralSettingsPanel.Size = new Size(319, 235);
			GeneralSettingsPanel.TabIndex = 18;
			// 
			// HideCaptionOnClientsCheckBox
			// 
			HideCaptionOnClientsCheckBox.AutoSize = true;
			HideCaptionOnClientsCheckBox.Location = new Point(9, 121);
			HideCaptionOnClientsCheckBox.Margin = new Padding(4);
			HideCaptionOnClientsCheckBox.Name = "HideCaptionOnClientsCheckBox";
			HideCaptionOnClientsCheckBox.Size = new Size(168, 19);
			HideCaptionOnClientsCheckBox.TabIndex = 28;
			HideCaptionOnClientsCheckBox.Text = "Hide caption bar on clients";
			HideCaptionOnClientsCheckBox.UseVisualStyleBackColor = true;
			HideCaptionOnClientsCheckBox.CheckedChanged += OptionChanged_Handler;
			// 
			// label4
			// 
			label4.AutoSize = true;
			label4.Location = new Point(6, 96);
			label4.Margin = new Padding(4, 0, 4, 0);
			label4.Name = "label4";
			label4.Size = new Size(91, 15);
			label4.TabIndex = 27;
			label4.Text = "Animation Style";
			// 
			// AnimationStyleCombo
			// 
			AnimationStyleCombo.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			AnimationStyleCombo.DropDownStyle = ComboBoxStyle.DropDownList;
			AnimationStyleCombo.FormattingEnabled = true;
			AnimationStyleCombo.Location = new Point(105, 93);
			AnimationStyleCombo.Margin = new Padding(4);
			AnimationStyleCombo.Name = "AnimationStyleCombo";
			AnimationStyleCombo.Size = new Size(177, 23);
			AnimationStyleCombo.TabIndex = 26;
			AnimationStyleCombo.SelectedIndexChanged += OptionChanged_Handler;
			// 
			// MinimizeInactiveClientsCheckBox
			// 
			MinimizeInactiveClientsCheckBox.AutoSize = true;
			MinimizeInactiveClientsCheckBox.Location = new Point(9, 73);
			MinimizeInactiveClientsCheckBox.Margin = new Padding(4);
			MinimizeInactiveClientsCheckBox.Name = "MinimizeInactiveClientsCheckBox";
			MinimizeInactiveClientsCheckBox.Size = new Size(178, 19);
			MinimizeInactiveClientsCheckBox.TabIndex = 24;
			MinimizeInactiveClientsCheckBox.Text = "Minimize inactive EVE clients";
			MinimizeInactiveClientsCheckBox.UseVisualStyleBackColor = true;
			MinimizeInactiveClientsCheckBox.CheckedChanged += OptionChanged_Handler;
			// 
			// EnableClientLayoutTrackingCheckBox
			// 
			EnableClientLayoutTrackingCheckBox.AutoSize = true;
			EnableClientLayoutTrackingCheckBox.Location = new Point(9, 30);
			EnableClientLayoutTrackingCheckBox.Margin = new Padding(4);
			EnableClientLayoutTrackingCheckBox.Name = "EnableClientLayoutTrackingCheckBox";
			EnableClientLayoutTrackingCheckBox.Size = new Size(137, 19);
			EnableClientLayoutTrackingCheckBox.TabIndex = 19;
			EnableClientLayoutTrackingCheckBox.Text = "Track client locations";
			EnableClientLayoutTrackingCheckBox.UseVisualStyleBackColor = true;
			EnableClientLayoutTrackingCheckBox.CheckedChanged += OptionChanged_Handler;
			// 
			// HideActiveClientThumbnailCheckBox
			// 
			HideActiveClientThumbnailCheckBox.AutoSize = true;
			HideActiveClientThumbnailCheckBox.Checked = true;
			HideActiveClientThumbnailCheckBox.CheckState = CheckState.Checked;
			HideActiveClientThumbnailCheckBox.Location = new Point(9, 52);
			HideActiveClientThumbnailCheckBox.Margin = new Padding(4);
			HideActiveClientThumbnailCheckBox.Name = "HideActiveClientThumbnailCheckBox";
			HideActiveClientThumbnailCheckBox.Size = new Size(197, 19);
			HideActiveClientThumbnailCheckBox.TabIndex = 20;
			HideActiveClientThumbnailCheckBox.Text = "Hide preview of active EVE client";
			HideActiveClientThumbnailCheckBox.UseVisualStyleBackColor = true;
			HideActiveClientThumbnailCheckBox.CheckedChanged += OptionChanged_Handler;
			// 
			// ShowThumbnailsAlwaysOnTopCheckBox
			// 
			ShowThumbnailsAlwaysOnTopCheckBox.AutoSize = true;
			ShowThumbnailsAlwaysOnTopCheckBox.Checked = true;
			ShowThumbnailsAlwaysOnTopCheckBox.CheckState = CheckState.Checked;
			ShowThumbnailsAlwaysOnTopCheckBox.Location = new Point(9, 142);
			ShowThumbnailsAlwaysOnTopCheckBox.Margin = new Padding(4);
			ShowThumbnailsAlwaysOnTopCheckBox.Name = "ShowThumbnailsAlwaysOnTopCheckBox";
			ShowThumbnailsAlwaysOnTopCheckBox.RightToLeft = RightToLeft.No;
			ShowThumbnailsAlwaysOnTopCheckBox.Size = new Size(148, 19);
			ShowThumbnailsAlwaysOnTopCheckBox.TabIndex = 21;
			ShowThumbnailsAlwaysOnTopCheckBox.Text = "Previews always on top";
			ShowThumbnailsAlwaysOnTopCheckBox.UseVisualStyleBackColor = true;
			ShowThumbnailsAlwaysOnTopCheckBox.CheckedChanged += OptionChanged_Handler;
			// 
			// HideThumbnailsOnLostFocusCheckBox
			// 
			HideThumbnailsOnLostFocusCheckBox.AutoSize = true;
			HideThumbnailsOnLostFocusCheckBox.Checked = true;
			HideThumbnailsOnLostFocusCheckBox.CheckState = CheckState.Checked;
			HideThumbnailsOnLostFocusCheckBox.Location = new Point(9, 163);
			HideThumbnailsOnLostFocusCheckBox.Margin = new Padding(4);
			HideThumbnailsOnLostFocusCheckBox.Name = "HideThumbnailsOnLostFocusCheckBox";
			HideThumbnailsOnLostFocusCheckBox.Size = new Size(252, 19);
			HideThumbnailsOnLostFocusCheckBox.TabIndex = 22;
			HideThumbnailsOnLostFocusCheckBox.Text = "Hide previews when EVE client is not active";
			HideThumbnailsOnLostFocusCheckBox.UseVisualStyleBackColor = true;
			HideThumbnailsOnLostFocusCheckBox.CheckedChanged += OptionChanged_Handler;
			// 
			// EnablePerClientThumbnailsLayoutsCheckBox
			// 
			EnablePerClientThumbnailsLayoutsCheckBox.AutoSize = true;
			EnablePerClientThumbnailsLayoutsCheckBox.Checked = true;
			EnablePerClientThumbnailsLayoutsCheckBox.CheckState = CheckState.Checked;
			EnablePerClientThumbnailsLayoutsCheckBox.Location = new Point(9, 185);
			EnablePerClientThumbnailsLayoutsCheckBox.Margin = new Padding(4);
			EnablePerClientThumbnailsLayoutsCheckBox.Name = "EnablePerClientThumbnailsLayoutsCheckBox";
			EnablePerClientThumbnailsLayoutsCheckBox.Size = new Size(200, 19);
			EnablePerClientThumbnailsLayoutsCheckBox.TabIndex = 23;
			EnablePerClientThumbnailsLayoutsCheckBox.Text = "Unique layout for each EVE client";
			EnablePerClientThumbnailsLayoutsCheckBox.UseVisualStyleBackColor = true;
			EnablePerClientThumbnailsLayoutsCheckBox.CheckedChanged += OptionChanged_Handler;
			// 
			// MinimizeToTrayCheckBox
			// 
			MinimizeToTrayCheckBox.AutoSize = true;
			MinimizeToTrayCheckBox.Location = new Point(9, 8);
			MinimizeToTrayCheckBox.Margin = new Padding(4);
			MinimizeToTrayCheckBox.Name = "MinimizeToTrayCheckBox";
			MinimizeToTrayCheckBox.Size = new Size(155, 19);
			MinimizeToTrayCheckBox.TabIndex = 18;
			MinimizeToTrayCheckBox.Text = "Minimize to System Tray";
			MinimizeToTrayCheckBox.UseVisualStyleBackColor = true;
			MinimizeToTrayCheckBox.CheckedChanged += OptionChanged_Handler;
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
			ThumbnailTabPage.Text = "Preview";
			// 
			// ThumbnailSettingsPanel
			// 
			ThumbnailSettingsPanel.Controls.Add(label1);
			ThumbnailSettingsPanel.Controls.Add(PreventPreviewColorButton);
			ThumbnailSettingsPanel.Controls.Add(PreventPreviewsCheckBox);
			ThumbnailSettingsPanel.Controls.Add(ThumbnailSnapToGridCheckBox);
			ThumbnailSettingsPanel.Controls.Add(ThumbnailSnapToGridSizeYNumericEdit);
			ThumbnailSettingsPanel.Controls.Add(SnapYLabel);
			ThumbnailSettingsPanel.Controls.Add(ThumbnailSnapToGridSizeXNumericEdit);
			ThumbnailSettingsPanel.Controls.Add(SnapXLabel);
			ThumbnailSettingsPanel.Controls.Add(LockThumbnailLocationCheckbox);
			ThumbnailSettingsPanel.Controls.Add(HeigthLabel);
			ThumbnailSettingsPanel.Controls.Add(WidthLabel);
			ThumbnailSettingsPanel.Controls.Add(ThumbnailsWidthNumericEdit);
			ThumbnailSettingsPanel.Controls.Add(ThumbnailsHeightNumericEdit);
			ThumbnailSettingsPanel.Controls.Add(ThumbnailOpacityTrackBar);
			ThumbnailSettingsPanel.Controls.Add(OpacityLabel);
			ThumbnailSettingsPanel.Dock = DockStyle.Fill;
			ThumbnailSettingsPanel.Location = new Point(4, 4);
			ThumbnailSettingsPanel.Margin = new Padding(4);
			ThumbnailSettingsPanel.Name = "ThumbnailSettingsPanel";
			ThumbnailSettingsPanel.Size = new Size(319, 235);
			ThumbnailSettingsPanel.TabIndex = 19;
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Location = new Point(175, 169);
			label1.Margin = new Padding(4, 0, 4, 0);
			label1.Name = "label1";
			label1.Size = new Size(36, 15);
			label1.TabIndex = 35;
			label1.Text = "Color";
			// 
			// PreventPreviewColorButton
			// 
			PreventPreviewColorButton.BorderStyle = BorderStyle.FixedSingle;
			PreventPreviewColorButton.Location = new Point(218, 168);
			PreventPreviewColorButton.Margin = new Padding(4);
			PreventPreviewColorButton.Name = "PreventPreviewColorButton";
			PreventPreviewColorButton.Size = new Size(58, 19);
			PreventPreviewColorButton.TabIndex = 34;
			PreventPreviewColorButton.Click += PreventPreviewColorButton_Click;
			// 
			// PreventPreviewsCheckBox
			// 
			PreventPreviewsCheckBox.AutoSize = true;
			PreventPreviewsCheckBox.Location = new Point(13, 168);
			PreventPreviewsCheckBox.Margin = new Padding(4);
			PreventPreviewsCheckBox.Name = "PreventPreviewsCheckBox";
			PreventPreviewsCheckBox.Size = new Size(151, 19);
			PreventPreviewsCheckBox.TabIndex = 33;
			PreventPreviewsCheckBox.Text = "Do not display previews";
			PreventPreviewsCheckBox.UseVisualStyleBackColor = true;
			PreventPreviewsCheckBox.CheckedChanged += OptionChanged_Handler;
			// 
			// ThumbnailSnapToGridCheckBox
			// 
			ThumbnailSnapToGridCheckBox.AutoSize = true;
			ThumbnailSnapToGridCheckBox.Location = new Point(13, 120);
			ThumbnailSnapToGridCheckBox.Margin = new Padding(4);
			ThumbnailSnapToGridCheckBox.Name = "ThumbnailSnapToGridCheckBox";
			ThumbnailSnapToGridCheckBox.Size = new Size(152, 19);
			ThumbnailSnapToGridCheckBox.TabIndex = 32;
			ThumbnailSnapToGridCheckBox.Text = "Thumbnail Snap to Grid";
			ThumbnailSnapToGridCheckBox.UseVisualStyleBackColor = true;
			ThumbnailSnapToGridCheckBox.CheckedChanged += OptionChanged_Handler;
			// 
			// ThumbnailSnapToGridSizeYNumericEdit
			// 
			ThumbnailSnapToGridSizeYNumericEdit.BackColor = SystemColors.Window;
			ThumbnailSnapToGridSizeYNumericEdit.BorderStyle = BorderStyle.FixedSingle;
			ThumbnailSnapToGridSizeYNumericEdit.CausesValidation = false;
			ThumbnailSnapToGridSizeYNumericEdit.Increment = new decimal(new int[] { 10, 0, 0, 0 });
			ThumbnailSnapToGridSizeYNumericEdit.Location = new Point(152, 141);
			ThumbnailSnapToGridSizeYNumericEdit.Margin = new Padding(4);
			ThumbnailSnapToGridSizeYNumericEdit.Maximum = new decimal(new int[] { 999999, 0, 0, 0 });
			ThumbnailSnapToGridSizeYNumericEdit.Name = "ThumbnailSnapToGridSizeYNumericEdit";
			ThumbnailSnapToGridSizeYNumericEdit.Size = new Size(56, 23);
			ThumbnailSnapToGridSizeYNumericEdit.TabIndex = 31;
			ThumbnailSnapToGridSizeYNumericEdit.Value = new decimal(new int[] { 100, 0, 0, 0 });
			ThumbnailSnapToGridSizeYNumericEdit.ValueChanged += OptionChanged_Handler;
			// 
			// SnapYLabel
			// 
			SnapYLabel.AutoSize = true;
			SnapYLabel.Location = new Point(128, 143);
			SnapYLabel.Margin = new Padding(4, 0, 4, 0);
			SnapYLabel.Name = "SnapYLabel";
			SnapYLabel.Size = new Size(14, 15);
			SnapYLabel.TabIndex = 30;
			SnapYLabel.Text = "Y";
			// 
			// ThumbnailSnapToGridSizeXNumericEdit
			// 
			ThumbnailSnapToGridSizeXNumericEdit.BackColor = SystemColors.Window;
			ThumbnailSnapToGridSizeXNumericEdit.BorderStyle = BorderStyle.FixedSingle;
			ThumbnailSnapToGridSizeXNumericEdit.CausesValidation = false;
			ThumbnailSnapToGridSizeXNumericEdit.Increment = new decimal(new int[] { 10, 0, 0, 0 });
			ThumbnailSnapToGridSizeXNumericEdit.Location = new Point(65, 141);
			ThumbnailSnapToGridSizeXNumericEdit.Margin = new Padding(4);
			ThumbnailSnapToGridSizeXNumericEdit.Maximum = new decimal(new int[] { 999999, 0, 0, 0 });
			ThumbnailSnapToGridSizeXNumericEdit.Name = "ThumbnailSnapToGridSizeXNumericEdit";
			ThumbnailSnapToGridSizeXNumericEdit.Size = new Size(56, 23);
			ThumbnailSnapToGridSizeXNumericEdit.TabIndex = 29;
			ThumbnailSnapToGridSizeXNumericEdit.Value = new decimal(new int[] { 100, 0, 0, 0 });
			ThumbnailSnapToGridSizeXNumericEdit.ValueChanged += OptionChanged_Handler;
			// 
			// SnapXLabel
			// 
			SnapXLabel.AutoSize = true;
			SnapXLabel.Location = new Point(9, 143);
			SnapXLabel.Margin = new Padding(4, 0, 4, 0);
			SnapXLabel.Name = "SnapXLabel";
			SnapXLabel.Size = new Size(43, 15);
			SnapXLabel.TabIndex = 28;
			SnapXLabel.Text = "Snap X";
			// 
			// LockThumbnailLocationCheckbox
			// 
			LockThumbnailLocationCheckbox.AutoSize = true;
			LockThumbnailLocationCheckbox.Location = new Point(13, 94);
			LockThumbnailLocationCheckbox.Margin = new Padding(4);
			LockThumbnailLocationCheckbox.Name = "LockThumbnailLocationCheckbox";
			LockThumbnailLocationCheckbox.Size = new Size(161, 19);
			LockThumbnailLocationCheckbox.TabIndex = 26;
			LockThumbnailLocationCheckbox.Text = "Lock Thumbnail Location";
			LockThumbnailLocationCheckbox.UseVisualStyleBackColor = true;
			LockThumbnailLocationCheckbox.CheckedChanged += OptionChanged_Handler;
			// 
			// HeigthLabel
			// 
			HeigthLabel.AutoSize = true;
			HeigthLabel.Location = new Point(9, 66);
			HeigthLabel.Margin = new Padding(4, 0, 4, 0);
			HeigthLabel.Name = "HeigthLabel";
			HeigthLabel.Size = new Size(104, 15);
			HeigthLabel.TabIndex = 24;
			HeigthLabel.Text = "Thumbnail Height";
			// 
			// WidthLabel
			// 
			WidthLabel.AutoSize = true;
			WidthLabel.Location = new Point(9, 38);
			WidthLabel.Margin = new Padding(4, 0, 4, 0);
			WidthLabel.Name = "WidthLabel";
			WidthLabel.Size = new Size(100, 15);
			WidthLabel.TabIndex = 23;
			WidthLabel.Text = "Thumbnail Width";
			// 
			// ThumbnailsWidthNumericEdit
			// 
			ThumbnailsWidthNumericEdit.BackColor = SystemColors.Window;
			ThumbnailsWidthNumericEdit.BorderStyle = BorderStyle.FixedSingle;
			ThumbnailsWidthNumericEdit.CausesValidation = false;
			ThumbnailsWidthNumericEdit.Increment = new decimal(new int[] { 10, 0, 0, 0 });
			ThumbnailsWidthNumericEdit.Location = new Point(122, 36);
			ThumbnailsWidthNumericEdit.Margin = new Padding(4);
			ThumbnailsWidthNumericEdit.Maximum = new decimal(new int[] { 999999, 0, 0, 0 });
			ThumbnailsWidthNumericEdit.Name = "ThumbnailsWidthNumericEdit";
			ThumbnailsWidthNumericEdit.Size = new Size(56, 23);
			ThumbnailsWidthNumericEdit.TabIndex = 21;
			ThumbnailsWidthNumericEdit.Value = new decimal(new int[] { 100, 0, 0, 0 });
			ThumbnailsWidthNumericEdit.ValueChanged += ThumbnailSizeChanged_Handler;
			// 
			// ThumbnailsHeightNumericEdit
			// 
			ThumbnailsHeightNumericEdit.BackColor = SystemColors.Window;
			ThumbnailsHeightNumericEdit.BorderStyle = BorderStyle.FixedSingle;
			ThumbnailsHeightNumericEdit.CausesValidation = false;
			ThumbnailsHeightNumericEdit.Increment = new decimal(new int[] { 10, 0, 0, 0 });
			ThumbnailsHeightNumericEdit.Location = new Point(122, 64);
			ThumbnailsHeightNumericEdit.Margin = new Padding(4);
			ThumbnailsHeightNumericEdit.Maximum = new decimal(new int[] { 99999999, 0, 0, 0 });
			ThumbnailsHeightNumericEdit.Name = "ThumbnailsHeightNumericEdit";
			ThumbnailsHeightNumericEdit.Size = new Size(56, 23);
			ThumbnailsHeightNumericEdit.TabIndex = 22;
			ThumbnailsHeightNumericEdit.Value = new decimal(new int[] { 70, 0, 0, 0 });
			ThumbnailsHeightNumericEdit.ValueChanged += ThumbnailSizeChanged_Handler;
			// 
			// ThumbnailOpacityTrackBar
			// 
			ThumbnailOpacityTrackBar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			ThumbnailOpacityTrackBar.AutoSize = false;
			ThumbnailOpacityTrackBar.LargeChange = 10;
			ThumbnailOpacityTrackBar.Location = new Point(71, 7);
			ThumbnailOpacityTrackBar.Margin = new Padding(4);
			ThumbnailOpacityTrackBar.Maximum = 100;
			ThumbnailOpacityTrackBar.Minimum = 20;
			ThumbnailOpacityTrackBar.Name = "ThumbnailOpacityTrackBar";
			ThumbnailOpacityTrackBar.Size = new Size(223, 25);
			ThumbnailOpacityTrackBar.TabIndex = 20;
			ThumbnailOpacityTrackBar.TickFrequency = 10;
			ThumbnailOpacityTrackBar.Value = 20;
			ThumbnailOpacityTrackBar.ValueChanged += OptionChanged_Handler;
			// 
			// OpacityLabel
			// 
			OpacityLabel.AutoSize = true;
			OpacityLabel.Location = new Point(9, 10);
			OpacityLabel.Margin = new Padding(4, 0, 4, 0);
			OpacityLabel.Name = "OpacityLabel";
			OpacityLabel.Size = new Size(48, 15);
			OpacityLabel.TabIndex = 19;
			OpacityLabel.Text = "Opacity";
			//
			// ZoomTabPage
			//
			ZoomTabPage.BackColor = SystemColors.Control;
			ZoomTabPage.Controls.Add(ZoomSettingsPanel);
			ZoomTabPage.Location = new Point(124, 4);
			ZoomTabPage.Margin = new Padding(4);
			ZoomTabPage.Name = "ZoomTabPage";
			ZoomTabPage.Size = new Size(327, 243);
			ZoomTabPage.TabIndex = 2;
			ZoomTabPage.Text = "Zoom";
			// 
			// ZoomSettingsPanel
			// 
			ZoomSettingsPanel.Controls.Add(ZoomFactorLabel);
			ZoomSettingsPanel.Controls.Add(ZoomAnchorPanel);
			ZoomSettingsPanel.Controls.Add(ZoomAnchorLabel);
			ZoomSettingsPanel.Controls.Add(EnableThumbnailZoomCheckBox);
			ZoomSettingsPanel.Controls.Add(ThumbnailZoomFactorNumericEdit);
			ZoomSettingsPanel.Dock = DockStyle.Fill;
			ZoomSettingsPanel.Location = new Point(0, 0);
			ZoomSettingsPanel.Margin = new Padding(4);
			ZoomSettingsPanel.Name = "ZoomSettingsPanel";
			ZoomSettingsPanel.Size = new Size(327, 243);
			ZoomSettingsPanel.TabIndex = 36;
			// 
			// ZoomFactorLabel
			// 
			ZoomFactorLabel.AutoSize = true;
			ZoomFactorLabel.Location = new Point(9, 38);
			ZoomFactorLabel.Margin = new Padding(4, 0, 4, 0);
			ZoomFactorLabel.Name = "ZoomFactorLabel";
			ZoomFactorLabel.Size = new Size(75, 15);
			ZoomFactorLabel.TabIndex = 39;
			ZoomFactorLabel.Text = "Zoom Factor";
			// 
			// ZoomAnchorPanel
			// 
			ZoomAnchorPanel.BorderStyle = BorderStyle.FixedSingle;
			ZoomAnchorPanel.Controls.Add(ZoomAanchorNWRadioButton);
			ZoomAnchorPanel.Controls.Add(ZoomAanchorNRadioButton);
			ZoomAnchorPanel.Controls.Add(ZoomAanchorNERadioButton);
			ZoomAnchorPanel.Controls.Add(ZoomAanchorWRadioButton);
			ZoomAnchorPanel.Controls.Add(ZoomAanchorSERadioButton);
			ZoomAnchorPanel.Controls.Add(ZoomAanchorCRadioButton);
			ZoomAnchorPanel.Controls.Add(ZoomAanchorSRadioButton);
			ZoomAnchorPanel.Controls.Add(ZoomAanchorERadioButton);
			ZoomAnchorPanel.Controls.Add(ZoomAanchorSWRadioButton);
			ZoomAnchorPanel.Location = new Point(94, 62);
			ZoomAnchorPanel.Margin = new Padding(4);
			ZoomAnchorPanel.Name = "ZoomAnchorPanel";
			ZoomAnchorPanel.Size = new Size(90, 84);
			ZoomAnchorPanel.TabIndex = 38;
			// 
			// ZoomAanchorNWRadioButton
			// 
			ZoomAanchorNWRadioButton.AutoSize = true;
			ZoomAanchorNWRadioButton.Location = new Point(4, 4);
			ZoomAanchorNWRadioButton.Margin = new Padding(4);
			ZoomAanchorNWRadioButton.Name = "ZoomAanchorNWRadioButton";
			ZoomAanchorNWRadioButton.Size = new Size(14, 13);
			ZoomAanchorNWRadioButton.TabIndex = 0;
			ZoomAanchorNWRadioButton.TabStop = true;
			ZoomAanchorNWRadioButton.UseVisualStyleBackColor = true;
			ZoomAanchorNWRadioButton.CheckedChanged += OptionChanged_Handler;
			// 
			// ZoomAanchorNRadioButton
			// 
			ZoomAanchorNRadioButton.AutoSize = true;
			ZoomAanchorNRadioButton.Location = new Point(36, 4);
			ZoomAanchorNRadioButton.Margin = new Padding(4);
			ZoomAanchorNRadioButton.Name = "ZoomAanchorNRadioButton";
			ZoomAanchorNRadioButton.Size = new Size(14, 13);
			ZoomAanchorNRadioButton.TabIndex = 1;
			ZoomAanchorNRadioButton.TabStop = true;
			ZoomAanchorNRadioButton.UseVisualStyleBackColor = true;
			ZoomAanchorNRadioButton.CheckedChanged += OptionChanged_Handler;
			// 
			// ZoomAanchorNERadioButton
			// 
			ZoomAanchorNERadioButton.AutoSize = true;
			ZoomAanchorNERadioButton.Location = new Point(69, 4);
			ZoomAanchorNERadioButton.Margin = new Padding(4);
			ZoomAanchorNERadioButton.Name = "ZoomAanchorNERadioButton";
			ZoomAanchorNERadioButton.Size = new Size(14, 13);
			ZoomAanchorNERadioButton.TabIndex = 2;
			ZoomAanchorNERadioButton.TabStop = true;
			ZoomAanchorNERadioButton.UseVisualStyleBackColor = true;
			ZoomAanchorNERadioButton.CheckedChanged += OptionChanged_Handler;
			// 
			// ZoomAanchorWRadioButton
			// 
			ZoomAanchorWRadioButton.AutoSize = true;
			ZoomAanchorWRadioButton.Location = new Point(4, 34);
			ZoomAanchorWRadioButton.Margin = new Padding(4);
			ZoomAanchorWRadioButton.Name = "ZoomAanchorWRadioButton";
			ZoomAanchorWRadioButton.Size = new Size(14, 13);
			ZoomAanchorWRadioButton.TabIndex = 3;
			ZoomAanchorWRadioButton.TabStop = true;
			ZoomAanchorWRadioButton.UseVisualStyleBackColor = true;
			ZoomAanchorWRadioButton.CheckedChanged += OptionChanged_Handler;
			// 
			// ZoomAanchorSERadioButton
			// 
			ZoomAanchorSERadioButton.AutoSize = true;
			ZoomAanchorSERadioButton.Location = new Point(69, 64);
			ZoomAanchorSERadioButton.Margin = new Padding(4);
			ZoomAanchorSERadioButton.Name = "ZoomAanchorSERadioButton";
			ZoomAanchorSERadioButton.Size = new Size(14, 13);
			ZoomAanchorSERadioButton.TabIndex = 8;
			ZoomAanchorSERadioButton.TabStop = true;
			ZoomAanchorSERadioButton.UseVisualStyleBackColor = true;
			ZoomAanchorSERadioButton.CheckedChanged += OptionChanged_Handler;
			// 
			// ZoomAanchorCRadioButton
			// 
			ZoomAanchorCRadioButton.AutoSize = true;
			ZoomAanchorCRadioButton.Location = new Point(36, 34);
			ZoomAanchorCRadioButton.Margin = new Padding(4);
			ZoomAanchorCRadioButton.Name = "ZoomAanchorCRadioButton";
			ZoomAanchorCRadioButton.Size = new Size(14, 13);
			ZoomAanchorCRadioButton.TabIndex = 4;
			ZoomAanchorCRadioButton.TabStop = true;
			ZoomAanchorCRadioButton.UseVisualStyleBackColor = true;
			ZoomAanchorCRadioButton.CheckedChanged += OptionChanged_Handler;
			// 
			// ZoomAanchorSRadioButton
			// 
			ZoomAanchorSRadioButton.AutoSize = true;
			ZoomAanchorSRadioButton.Location = new Point(36, 64);
			ZoomAanchorSRadioButton.Margin = new Padding(4);
			ZoomAanchorSRadioButton.Name = "ZoomAanchorSRadioButton";
			ZoomAanchorSRadioButton.Size = new Size(14, 13);
			ZoomAanchorSRadioButton.TabIndex = 7;
			ZoomAanchorSRadioButton.TabStop = true;
			ZoomAanchorSRadioButton.UseVisualStyleBackColor = true;
			ZoomAanchorSRadioButton.CheckedChanged += OptionChanged_Handler;
			// 
			// ZoomAanchorERadioButton
			// 
			ZoomAanchorERadioButton.AutoSize = true;
			ZoomAanchorERadioButton.Location = new Point(69, 34);
			ZoomAanchorERadioButton.Margin = new Padding(4);
			ZoomAanchorERadioButton.Name = "ZoomAanchorERadioButton";
			ZoomAanchorERadioButton.Size = new Size(14, 13);
			ZoomAanchorERadioButton.TabIndex = 5;
			ZoomAanchorERadioButton.TabStop = true;
			ZoomAanchorERadioButton.UseVisualStyleBackColor = true;
			ZoomAanchorERadioButton.CheckedChanged += OptionChanged_Handler;
			// 
			// ZoomAanchorSWRadioButton
			// 
			ZoomAanchorSWRadioButton.AutoSize = true;
			ZoomAanchorSWRadioButton.Location = new Point(4, 64);
			ZoomAanchorSWRadioButton.Margin = new Padding(4);
			ZoomAanchorSWRadioButton.Name = "ZoomAanchorSWRadioButton";
			ZoomAanchorSWRadioButton.Size = new Size(14, 13);
			ZoomAanchorSWRadioButton.TabIndex = 6;
			ZoomAanchorSWRadioButton.TabStop = true;
			ZoomAanchorSWRadioButton.UseVisualStyleBackColor = true;
			ZoomAanchorSWRadioButton.CheckedChanged += OptionChanged_Handler;
			// 
			// ZoomAnchorLabel
			// 
			ZoomAnchorLabel.AutoSize = true;
			ZoomAnchorLabel.Location = new Point(9, 66);
			ZoomAnchorLabel.Margin = new Padding(4, 0, 4, 0);
			ZoomAnchorLabel.Name = "ZoomAnchorLabel";
			ZoomAnchorLabel.Size = new Size(46, 15);
			ZoomAnchorLabel.TabIndex = 40;
			ZoomAnchorLabel.Text = "Anchor";
			// 
			// EnableThumbnailZoomCheckBox
			// 
			EnableThumbnailZoomCheckBox.AutoSize = true;
			EnableThumbnailZoomCheckBox.Checked = true;
			EnableThumbnailZoomCheckBox.CheckState = CheckState.Checked;
			EnableThumbnailZoomCheckBox.Location = new Point(9, 8);
			EnableThumbnailZoomCheckBox.Margin = new Padding(4);
			EnableThumbnailZoomCheckBox.Name = "EnableThumbnailZoomCheckBox";
			EnableThumbnailZoomCheckBox.RightToLeft = RightToLeft.No;
			EnableThumbnailZoomCheckBox.Size = new Size(108, 19);
			EnableThumbnailZoomCheckBox.TabIndex = 36;
			EnableThumbnailZoomCheckBox.Text = "Zoom on hover";
			EnableThumbnailZoomCheckBox.UseVisualStyleBackColor = true;
			EnableThumbnailZoomCheckBox.CheckedChanged += OptionChanged_Handler;
			// 
			// ThumbnailZoomFactorNumericEdit
			// 
			ThumbnailZoomFactorNumericEdit.BackColor = SystemColors.Window;
			ThumbnailZoomFactorNumericEdit.BorderStyle = BorderStyle.FixedSingle;
			ThumbnailZoomFactorNumericEdit.Location = new Point(94, 36);
			ThumbnailZoomFactorNumericEdit.Margin = new Padding(4);
			ThumbnailZoomFactorNumericEdit.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
			ThumbnailZoomFactorNumericEdit.Minimum = new decimal(new int[] { 2, 0, 0, 0 });
			ThumbnailZoomFactorNumericEdit.Name = "ThumbnailZoomFactorNumericEdit";
			ThumbnailZoomFactorNumericEdit.Size = new Size(44, 23);
			ThumbnailZoomFactorNumericEdit.TabIndex = 37;
			ThumbnailZoomFactorNumericEdit.Value = new decimal(new int[] { 2, 0, 0, 0 });
			ThumbnailZoomFactorNumericEdit.ValueChanged += OptionChanged_Handler;
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
			OverlayWindowNameSubPage.Controls.Add(label2);
			OverlayWindowNameSubPage.Controls.Add(OverlayLabelColorButton);
			OverlayWindowNameSubPage.Controls.Add(btnLabelFont);
			OverlayWindowNameSubPage.Controls.Add(label3);
			OverlayWindowNameSubPage.Controls.Add(panel1);
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
			OverlayGroupNameSubPage.Controls.Add(panel2);
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
			// label5
			// 
			label5.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			label5.AutoSize = true;
			label5.Location = new Point(224, 12);
			label5.Margin = new Padding(4, 0, 4, 0);
			label5.Name = "label5";
			label5.Size = new Size(168, 15);
			label5.TabIndex = 47;
			label5.Text = "Position";
			// 
			// panel2
			// 
			panel2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			panel2.BorderStyle = BorderStyle.FixedSingle;
			panel2.Controls.Add(CycleGroupIndicatorNWRadioButton);
			panel2.Controls.Add(CycleGroupIndicatorNRadioButton);
			panel2.Controls.Add(CycleGroupIndicatorNERadioButton);
			panel2.Controls.Add(CycleGroupIndicatorWRadioButton);
			panel2.Controls.Add(CycleGroupIndicatorSERadioButton);
			panel2.Controls.Add(CycleGroupIndicatorCRadioButton);
			panel2.Controls.Add(CycleGroupIndicatorSRadioButton);
			panel2.Controls.Add(CycleGroupIndicatorERadioButton);
			panel2.Controls.Add(CycleGroupIndicatorSWRadioButton);
			panel2.Location = new Point(224, 31);
			panel2.Margin = new Padding(4);
			panel2.Name = "panel2";
			panel2.Size = new Size(73, 66);
			panel2.TabIndex = 46;
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
			// label3
			// 
			label3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			label3.AutoSize = true;
			label3.Location = new Point(224, 12);
			label3.Margin = new Padding(4, 0, 4, 0);
			label3.Name = "label3";
			label3.Size = new Size(50, 15);
			label3.TabIndex = 43;
			label3.Text = "Position";
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.Location = new Point(12, 47);
			label2.Margin = new Padding(4, 0, 4, 0);
			label2.Name = "label2";
			label2.Size = new Size(36, 15);
			label2.TabIndex = 42;
			label2.Text = "Color";
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
			// panel1
			// 
			panel1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			panel1.BorderStyle = BorderStyle.FixedSingle;
			panel1.Controls.Add(OverlayLabelNWRadioButton);
			panel1.Controls.Add(OverlayLabelNRadioButton);
			panel1.Controls.Add(OverlayLabelNERadioButton);
			panel1.Controls.Add(OverlayLabelWRadioButton);
			panel1.Controls.Add(OverlayLabelSERadioButton);
			panel1.Controls.Add(OverlayLabelCRadioButton);
			panel1.Controls.Add(OverlayLabelSRadioButton);
			panel1.Controls.Add(OverlayLabelERadioButton);
			panel1.Controls.Add(OverlayLabelSWRadioButton);
			panel1.Location = new Point(224, 31);
			panel1.Margin = new Padding(4);
			panel1.Name = "panel1";
			panel1.Size = new Size(73, 66);
			panel1.TabIndex = 39;
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
			DescriptionLabel.Text = resources.GetString("DescriptionLabel.Text");
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
			DocumentationLink.Text = "to be set from prresenter to be set from prresenter to be set from prresenter to be set from prresenter";
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
			ThumbnailTabPage.ResumeLayout(false);
			ThumbnailSettingsPanel.ResumeLayout(false);
			ThumbnailSettingsPanel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)ThumbnailSnapToGridSizeYNumericEdit).EndInit();
			((System.ComponentModel.ISupportInitialize)ActiveClientHighlightThicknessNumericEdit).EndInit();
			((System.ComponentModel.ISupportInitialize)ThumbnailSnapToGridSizeXNumericEdit).EndInit();
			((System.ComponentModel.ISupportInitialize)ThumbnailsWidthNumericEdit).EndInit();
			((System.ComponentModel.ISupportInitialize)ThumbnailsHeightNumericEdit).EndInit();
			((System.ComponentModel.ISupportInitialize)ThumbnailOpacityTrackBar).EndInit();
			ZoomTabPage.ResumeLayout(false);
			ZoomSettingsPanel.ResumeLayout(false);
			ZoomSettingsPanel.PerformLayout();
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
			panel2.ResumeLayout(false);
			panel2.PerformLayout();
			panel1.ResumeLayout(false);
			panel1.PerformLayout();
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
		private TabPage ZoomTabPage;
		private CheckBox EnableClientLayoutTrackingCheckBox;
		private CheckBox HideActiveClientThumbnailCheckBox;
		private CheckBox ShowThumbnailsAlwaysOnTopCheckBox;
		private CheckBox HideThumbnailsOnLostFocusCheckBox;
		private CheckBox EnablePerClientThumbnailsLayoutsCheckBox;
		private CheckBox MinimizeToTrayCheckBox;
		private NumericUpDown ThumbnailsWidthNumericEdit;
		private NumericUpDown ThumbnailsHeightNumericEdit;
		private TrackBar ThumbnailOpacityTrackBar;
		private Panel ZoomAnchorPanel;
		private RadioButton ZoomAanchorNWRadioButton;
		private RadioButton ZoomAanchorNRadioButton;
		private RadioButton ZoomAanchorNERadioButton;
		private RadioButton ZoomAanchorWRadioButton;
		private RadioButton ZoomAanchorSERadioButton;
		private RadioButton ZoomAanchorCRadioButton;
		private RadioButton ZoomAanchorSRadioButton;
		private RadioButton ZoomAanchorERadioButton;
		private RadioButton ZoomAanchorSWRadioButton;
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
        private Label label3;
        private Label label2;
        private Panel OverlayLabelColorButton;
        private Panel panel1;
        private RadioButton OverlayLabelNWRadioButton;
        private RadioButton OverlayLabelNRadioButton;
        private RadioButton OverlayLabelNERadioButton;
        private RadioButton OverlayLabelWRadioButton;
        private RadioButton OverlayLabelSERadioButton;
        private RadioButton OverlayLabelCRadioButton;
        private RadioButton OverlayLabelSRadioButton;
        private RadioButton OverlayLabelERadioButton;
        private RadioButton OverlayLabelSWRadioButton;
		private ComboBox AnimationStyleCombo;
		private CheckBox HideCaptionOnClientsCheckBox;
		private Button btnLabelFont;
		private Label LabelOverlayLabelFont;
		private CheckBox PreventPreviewsCheckBox;
		private Label label1;
		private Panel PreventPreviewColorButton;
		private Label label5;
		private Panel panel2;
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