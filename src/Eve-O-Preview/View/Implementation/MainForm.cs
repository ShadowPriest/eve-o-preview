using EveOPreview.Configuration;
using EveOPreview.Localization;
using EveOPreview.UI.Hotkeys;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace EveOPreview.View
{
	public partial class MainForm : Form, IMainFormView
	{
		#region Private fields
		private readonly ApplicationContext _context;
		private readonly Dictionary<ViewZoomAnchor, RadioButton> _zoomAnchorMap;
		private readonly Dictionary<ViewZoomAnchor, RadioButton> _overlayLabelMap;
		private readonly Dictionary<ViewZoomAnchor, RadioButton> _cycleGroupIndicatorMap;
		private ViewZoomAnchor _cachedThumbnailZoomAnchor;
		private ViewZoomAnchor _cachedOverlayLabelAnchor;
		private ViewZoomAnchor _cachedCycleGroupIndicatorAnchor;
		private bool _suppressEvents;
		private Size _minimumSize;
		private Size _maximumSize;
		private string _iconName;
		private readonly List<string> _cycleGroupNames;
		private readonly Dictionary<string, List<string>> _cycleGroups;
		private Dictionary<string, IList<string>> _clientCycleGroups;
		private IList<CharacterGroupViewItem> _characterGroups;
		private IList<CharacterViewItem> _characters;
		private List<(string ActionId, string DisplayName)> _hotkeyActions;
		private List<(string ActionId, string ActionName, string Hotkey)> _hotkeyBindings;
		private List<string> _activeClients;
		private Point? _thumbnailsListClickLocation;
		private readonly List<string> _languages;
		private string _loadedLanguage;
		#endregion

		public MainForm(ApplicationContext context)
		{
			this._context = context;
			this._zoomAnchorMap = new Dictionary<ViewZoomAnchor, RadioButton>();
			this._overlayLabelMap = new Dictionary<ViewZoomAnchor, RadioButton>();
			this._cycleGroupIndicatorMap = new Dictionary<ViewZoomAnchor, RadioButton>();
			this._cachedThumbnailZoomAnchor = ViewZoomAnchor.NW;
			this._suppressEvents = false;
			this._minimumSize = new Size(20, 20);
			this._maximumSize = new Size(20, 20);

			this._cycleGroupNames = new List<string>();
			this._cycleGroups = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
			this._clientCycleGroups = new Dictionary<string, IList<string>>(StringComparer.OrdinalIgnoreCase);
			this._characterGroups = new List<CharacterGroupViewItem>();
			this._characters = new List<CharacterViewItem>();
			this._hotkeyActions = new List<(string ActionId, string DisplayName)>();
			this._hotkeyBindings = new List<(string ActionId, string ActionName, string Hotkey)>();
			this._activeClients = new List<string>();
			this._languages = new List<string>();
			this._loadedLanguage = LanguageManager.SYSTEM_LANGUAGE;

			InitializeComponent();

			this.ApplyLocalization();
			this.InitLanguages();
			this.InitTabSeparator();

			this.ThumbnailsList.DisplayMember = "Title";
			this.ThumbnailsList.Format += this.ThumbnailsList_Format_Handler;
			this.ThumbnailsList.MouseDown += this.ThumbnailsList_MouseDown_Handler;
			this.ThumbnailsList.SelectedIndexChanged += this.ThumbnailsList_SelectedIndexChanged_Handler;
			this.ClientCycleGroupCombo.Enabled = false;

			this.InitCharactersContextMenu();
			this.InitPreviewClicks();

			this.HotkeyBindingsListView.ClientSizeChanged += this.HotkeyBindingsListViewResize_Handler;
			this.HotkeyBindingsListView.DoubleClick += this.HotkeyBindingsListView_DoubleClick_Handler;
			this.HotkeyBindingsListViewResize_Handler(this.HotkeyBindingsListView, EventArgs.Empty);

			this.ResizeEnd += this.MainFormResizeEnd_Handler;

			// The background rendering availability follows the minimize-inactive setting
			this.MinimizeInactiveClientsCheckBox.CheckedChanged += (s, e) => this.RefreshRenderingSettings();

			// The 'fill the grid cell' option availability follows the snap checkbox,
			// and the size fields lock while the fill-cell mode is active
			this.ThumbnailSnapToGridCheckBox.CheckedChanged += (s, e) => this.RefreshSnapSettings();
			this.SnapFillCellCheckBox.CheckedChanged += (s, e) => this.RefreshSnapSettings();

			// The snap grid overlay follows the grid step / offset edits while it is visible
			EventHandler gridStepChanged = (s, e) => this._gridOverlay?.SetGridStep(
				(int)this.ThumbnailSnapToGridSizeXNumericEdit.Value, (int)this.ThumbnailSnapToGridSizeYNumericEdit.Value,
				(int)this.GridOffsetXNumericEdit.Value, (int)this.GridOffsetYNumericEdit.Value);
			this.ThumbnailSnapToGridSizeXNumericEdit.ValueChanged += gridStepChanged;
			this.ThumbnailSnapToGridSizeYNumericEdit.ValueChanged += gridStepChanged;
			this.GridOffsetXNumericEdit.ValueChanged += gridStepChanged;
			this.GridOffsetYNumericEdit.ValueChanged += gridStepChanged;

			this.InitZoomAnchorMap();
			this.InitOverlayLabelMap();
			this.InitCycleGroupIndicatorMap();
			this.InitFormSize();
		}

		public bool MinimizeToTray
		{
			get => this.MinimizeToTrayCheckBox.Checked;
			set => this.MinimizeToTrayCheckBox.Checked = value;
		}

		public string Language
		{
			get
			{
				int index = this.LanguageCombo.SelectedIndex;
				return (index >= 0) && (index < this._languages.Count) ? this._languages[index] : LanguageManager.SYSTEM_LANGUAGE;
			}
			set
			{
				this._loadedLanguage = LanguageManager.Normalize(value);
				this.LanguageCombo.SelectedIndex = Math.Max(0, this._languages.IndexOf(this._loadedLanguage));
			}
		}

		public string IconName
		{
			get => this._iconName;
			set
			{


				this._iconName = value;

				// Set Icon 
				System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
				if (this._iconName == null || ((resources.GetObject(this._iconName))) == null)
				{
					this._iconName = "IconOriginal";
				}

				// pull icon from resources
				try
				{
					var iconBytes = (byte[])resources.GetObject(this._iconName);
					using (MemoryStream ms = new MemoryStream(iconBytes))
					{
						this.Icon = new Icon(ms);
						this.NotifyIcon.Icon = this.Icon;
					}
				}
				catch (Exception)
				{
					// A missing or unreadable icon resource leaves the default one in place
				}

				if (value != "")
				{
					this.ApplicationSettingsChanged?.Invoke();
				}
			}
		}

		public double ThumbnailOpacity
		{
			get => Math.Min(this.ThumbnailOpacityTrackBar.Value / 100.00, 1.00);
			set
			{
				int barValue = (int)(100.0 * value);
				if (barValue > 100)
				{
					barValue = 100;
				}
				else if (barValue < 10)
				{
					barValue = 10;
				}

				this.ThumbnailOpacityTrackBar.Value = barValue;
			}
		}

		public bool EnableMinimizedClientsRefresh
		{
			get => this.EnableBackgroundRenderingCheckBox.Checked;
			set
			{
				this.EnableBackgroundRenderingCheckBox.Checked = value;
				this.RefreshRenderingSettings();
			}
		}

		// Background rendering only applies to clients minimized by the app, so the
		// checkbox is available only while 'Minimize inactive clients' is on, and the
		// interval fields only while the background rendering itself is on
		private void RefreshRenderingSettings()
		{
			bool minimizeInactive = this.MinimizeInactiveClientsCheckBox.Checked;
			this.EnableBackgroundRenderingCheckBox.Enabled = minimizeInactive;

			bool intervalsEnabled = minimizeInactive && this.EnableBackgroundRenderingCheckBox.Checked;
			this.ThumbnailRefreshPeriodNumericEdit.Enabled = intervalsEnabled;
			this.MinimizedClientsRefreshPeriodNumericEdit.Enabled = intervalsEnabled;
		}

		private void EnableBackgroundRenderingCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			this.RefreshRenderingSettings();
			this.OptionChanged_Handler(sender, e);
		}

		public int ThumbnailRefreshPeriod
		{
			get => (int)this.ThumbnailRefreshPeriodNumericEdit.Value;
			set => this.ThumbnailRefreshPeriodNumericEdit.Value = Math.Max(this.ThumbnailRefreshPeriodNumericEdit.Minimum, Math.Min(this.ThumbnailRefreshPeriodNumericEdit.Maximum, value));
		}

		public int MinimizedClientsRefreshPeriod
		{
			get => (int)this.MinimizedClientsRefreshPeriodNumericEdit.Value;
			set => this.MinimizedClientsRefreshPeriodNumericEdit.Value = Math.Max(this.MinimizedClientsRefreshPeriodNumericEdit.Minimum, Math.Min(this.MinimizedClientsRefreshPeriodNumericEdit.Maximum, value));
		}

		public bool EnableClientLayoutTracking
		{
			get => this.EnableClientLayoutTrackingCheckBox.Checked;
			set => this.EnableClientLayoutTrackingCheckBox.Checked = value;
		}

		public bool HideActiveClientThumbnail
		{
			get => this.HideActiveClientThumbnailCheckBox.Checked;
			set => this.HideActiveClientThumbnailCheckBox.Checked = value;
		}

		public bool MinimizeInactiveClients
		{
			get => this.MinimizeInactiveClientsCheckBox.Checked;
			set
			{
				this.MinimizeInactiveClientsCheckBox.Checked = value;
				this.RefreshRenderingSettings();
			}
		}
		public bool HideCaptionOnClients
		{
			get => this.HideCaptionOnClientsCheckBox.Checked;
			set => this.HideCaptionOnClientsCheckBox.Checked = value;
		}
		public ViewAnimationStyle WindowsAnimationStyle
		{
			get => this.DisableAnimationCheckBox.Checked ? ViewAnimationStyle.NoAnimation : ViewAnimationStyle.OriginalAnimation;
			set => this.DisableAnimationCheckBox.Checked = value == ViewAnimationStyle.NoAnimation;
		}

		public bool ShowThumbnailsAlwaysOnTop
		{
			get => this.ShowThumbnailsAlwaysOnTopCheckBox.Checked;
			set => this.ShowThumbnailsAlwaysOnTopCheckBox.Checked = value;
		}
		public bool PreventPreviews
		{
			get => this.PreventPreviewsCheckBox.Checked;
			set => this.PreventPreviewsCheckBox.Checked = value;
		}

		public bool HideThumbnailsOnLostFocus
		{
			get => this.HideThumbnailsOnLostFocusCheckBox.Checked;
			set => this.HideThumbnailsOnLostFocusCheckBox.Checked = value;
		}

		public bool EnablePerClientThumbnailLayouts
		{
			get => this.EnablePerClientThumbnailsLayoutsCheckBox.Checked;
			set => this.EnablePerClientThumbnailsLayoutsCheckBox.Checked = value;
		}

		public Size ThumbnailSize
		{
			get => new Size((int)this.ThumbnailsWidthNumericEdit.Value, (int)this.ThumbnailsHeightNumericEdit.Value);
			set
			{
				this.ThumbnailsWidthNumericEdit.Value = value.Width;
				this.ThumbnailsHeightNumericEdit.Value = value.Height;
			}
		}

		public bool EnableThumbnailZoom
		{
			get => this.EnableThumbnailZoomCheckBox.Checked;
			set
			{
				this.EnableThumbnailZoomCheckBox.Checked = value;
				this.RefreshZoomSettings();
			}
		}

		public int ThumbnailZoomFactor
		{
			get => (int)this.ThumbnailZoomFactorNumericEdit.Value;
			set => this.ThumbnailZoomFactorNumericEdit.Value = value;
		}

		public ViewZoomAnchor ThumbnailZoomAnchor
		{
			get
			{
				if (this._zoomAnchorMap[this._cachedThumbnailZoomAnchor].Checked)
				{
					return this._cachedThumbnailZoomAnchor;
				}

				foreach (KeyValuePair<ViewZoomAnchor, RadioButton> valuePair in this._zoomAnchorMap)
				{
					if (!valuePair.Value.Checked)
					{
						continue;
					}

					this._cachedThumbnailZoomAnchor = valuePair.Key;
					return this._cachedThumbnailZoomAnchor;
				}

				// Default value
				return ViewZoomAnchor.NW;
			}
			set
			{
				this._cachedThumbnailZoomAnchor = value;
				this._zoomAnchorMap[this._cachedThumbnailZoomAnchor].Checked = true;
			}
		}

		public ViewZoomAnchor OverlayLabelAnchor
		{
			get
			{
				if (this._overlayLabelMap[this._cachedOverlayLabelAnchor].Checked)
				{
					return this._cachedOverlayLabelAnchor;
				}

				foreach (KeyValuePair<ViewZoomAnchor, RadioButton> valuePair in this._overlayLabelMap)
				{
					if (!valuePair.Value.Checked)
					{
						continue;
					}

					this._cachedOverlayLabelAnchor = valuePair.Key;
					return this._cachedOverlayLabelAnchor;
				}

				// Default Value
				return ViewZoomAnchor.NW;
			}
			set
			{
				this._cachedOverlayLabelAnchor = value;
				this._overlayLabelMap[this._cachedOverlayLabelAnchor].Checked = true;
			}
		}

		public ViewZoomAnchor CycleGroupIndicatorAnchor
		{
			get
			{
				if (this._cycleGroupIndicatorMap[this._cachedCycleGroupIndicatorAnchor].Checked)
				{
					return this._cachedCycleGroupIndicatorAnchor;
				}

				foreach (KeyValuePair<ViewZoomAnchor, RadioButton> valuePair in this._cycleGroupIndicatorMap)
				{
					if (!valuePair.Value.Checked)
					{
						continue;
					}

					this._cachedCycleGroupIndicatorAnchor = valuePair.Key;
					return this._cachedCycleGroupIndicatorAnchor;
				}

				// Default Value
				return ViewZoomAnchor.NW;
			}
			set
			{
				this._cachedCycleGroupIndicatorAnchor = value;
				this._cycleGroupIndicatorMap[this._cachedCycleGroupIndicatorAnchor].Checked = true;
			}
		}

		public bool ShowThumbnailOverlays
		{
			get => this.ShowThumbnailOverlaysCheckBox.Checked;
			set
			{
				this.ShowThumbnailOverlaysCheckBox.Checked = value;
				this.RefreshOverlaySubPages();
			}
		}

		// The overlay sub-pages only make sense while the overlay itself is enabled.
		// TabPage.Enabled does not block the tab header, so the pages are removed instead
		private void RefreshOverlaySubPages()
		{
			bool isEnabled = this.ShowThumbnailOverlaysCheckBox.Checked;
			TabPage[] optionalPages = { this.OverlayWindowNameSubPage, this.OverlayGroupNameSubPage, this.OverlayBorderSubPage };

			this.OverlaySubTabControl.SuspendLayout();

			foreach (TabPage page in optionalPages)
			{
				bool isPresent = this.OverlaySubTabControl.TabPages.Contains(page);

				if (isEnabled && !isPresent)
				{
					this.OverlaySubTabControl.TabPages.Add(page);
				}
				else if (!isEnabled && isPresent)
				{
					this.OverlaySubTabControl.TabPages.Remove(page);
				}
			}

			this.OverlaySubTabControl.ResumeLayout();
		}

		public bool ShowCycleGroupName
		{
			get => this.ShowCycleGroupNameCheckBox.Checked;
			set => this.ShowCycleGroupNameCheckBox.Checked = value;
		}

		public bool ShowClientName
		{
			get => this.ShowClientNameCheckBox.Checked;
			set => this.ShowClientNameCheckBox.Checked = value;
		}

		public bool OverlayAlwaysOnTop
		{
			get => this.OverlayAlwaysOnTopCheckBox.Checked;
			set => this.OverlayAlwaysOnTopCheckBox.Checked = value;
		}

		public Color CycleGroupNameColor
		{
			get => this._cycleGroupNameColor;
			set
			{
				this._cycleGroupNameColor = value;
				this.CycleGroupNameColorButton.BackColor = value;
				this.LabelCycleGroupNameFont.ForeColor = value;
			}
		}
		private Color _cycleGroupNameColor;

		public Font CycleGroupNameFont
		{
			get => this._cycleGroupNameFont;
			set
			{
				this._cycleGroupNameFont = value;
				this.LabelCycleGroupNameFont.Font = value;
			}
		}
		private Font _cycleGroupNameFont;

		public bool ShowThumbnailFrames
		{
			get => this.ShowThumbnailFramesCheckBox.Checked;
			set => this.ShowThumbnailFramesCheckBox.Checked = value;
		}
		public bool LockThumbnailLocation
		{
			get => this.LockThumbnailLocationCheckbox.Checked;
			set => this.LockThumbnailLocationCheckbox.Checked = value;
		}
		public bool ThumbnailSnapToGrid
		{
			get => this.ThumbnailSnapToGridCheckBox.Checked;
			set
			{
				this.ThumbnailSnapToGridCheckBox.Checked = value;
				this.RefreshSnapSettings();
			}
		}
		public int ThumbnailSnapToGridSizeX
		{
			get => (int)ThumbnailSnapToGridSizeXNumericEdit.Value;
			set => ThumbnailSnapToGridSizeXNumericEdit.Value = value;
		}
		public int ThumbnailSnapToGridSizeY
		{
			get => (int)ThumbnailSnapToGridSizeYNumericEdit.Value;
			set => ThumbnailSnapToGridSizeYNumericEdit.Value = value;
		}

		public bool ThumbnailSnapToGridFillCell
		{
			get => this.SnapFillCellCheckBox.Checked;
			set
			{
				this.SnapFillCellCheckBox.Checked = value;
				this.RefreshSnapSettings();
			}
		}

		public int ThumbnailSnapToGridOffsetX
		{
			get => (int)this.GridOffsetXNumericEdit.Value;
			set => this.GridOffsetXNumericEdit.Value = Math.Max(this.GridOffsetXNumericEdit.Minimum, Math.Min(this.GridOffsetXNumericEdit.Maximum, value));
		}

		public int ThumbnailSnapToGridOffsetY
		{
			get => (int)this.GridOffsetYNumericEdit.Value;
			set => this.GridOffsetYNumericEdit.Value = Math.Max(this.GridOffsetYNumericEdit.Minimum, Math.Min(this.GridOffsetYNumericEdit.Maximum, value));
		}

		public int ThumbnailSnapToGridCellPadding
		{
			get => (int)this.SnapPaddingNumericEdit.Value;
			set => this.SnapPaddingNumericEdit.Value = Math.Max(this.SnapPaddingNumericEdit.Minimum, Math.Min(this.SnapPaddingNumericEdit.Maximum, value));
		}

		// 'Fill the grid cell' only makes sense while the snap itself is enabled.
		// While it is active, the preview size is dictated by the grid cell, so the
		// manual size fields are locked
		private void RefreshSnapSettings()
		{
			bool snapEnabled = this.ThumbnailSnapToGridCheckBox.Checked;
			this.SnapFillCellCheckBox.Enabled = snapEnabled;
			this.SnapPaddingNumericEdit.Enabled = snapEnabled;

			bool fillCell = snapEnabled && this.SnapFillCellCheckBox.Checked;
			this.ThumbnailsWidthNumericEdit.Enabled = !fillCell;
			this.ThumbnailsHeightNumericEdit.Enabled = !fillCell;
		}

		public bool EnableActiveClientHighlight
		{
			get => this.EnableActiveClientHighlightCheckBox.Checked;
			set => this.EnableActiveClientHighlightCheckBox.Checked = value;
		}

		public int ActiveClientHighlightThickness
		{
			get => (int)this.ActiveClientHighlightThicknessNumericEdit.Value;
			set => this.ActiveClientHighlightThicknessNumericEdit.Value = Math.Min(Math.Max(value, (int)this.ActiveClientHighlightThicknessNumericEdit.Minimum), (int)this.ActiveClientHighlightThicknessNumericEdit.Maximum);
		}

		public Color ActiveClientHighlightColor
		{
			get => this._activeClientHighlightColor;
			set
			{
				this._activeClientHighlightColor = value;
				this.ActiveClientHighlightColorButton.BackColor = value;
			}
		}
		private Color _activeClientHighlightColor;

		public Color PreventPreviewColor
		{
			get => this._preventPreviewColor;
			set
			{
				this._preventPreviewColor = value;
				this.PreventPreviewColorButton.BackColor = value;
			}
		}
		private Color _preventPreviewColor;

		public Color OverlayLabelColor
		{
			get => this._OverlayLabelColor;
			set
			{
				this._OverlayLabelColor = value;
				this.OverlayLabelColorButton.BackColor = value;
				this.LabelOverlayLabelFont.ForeColor = value;
			}
		}
		private Color _OverlayLabelColor;

		public Font OverlayLabelFont
		{
			get => (Font)this._OverlayLabelFont;
			set
			{
				this._OverlayLabelFont = value;
				this.LabelOverlayLabelFont.Font = value;
			}
		}
		private Font _OverlayLabelFont;

		public bool EnableGameLogMonitor
		{
			get => this.EnableGameLogMonitorCheckBox.Checked;
			set
			{
				this.EnableGameLogMonitorCheckBox.Checked = value;
				this.RefreshAggroSubPage();
			}
		}

		public string GameLogsFolder
		{
			get => this.GameLogsFolderTextBox.Text.Trim();
			set => this.GameLogsFolderTextBox.Text = value ?? "";
		}

		public bool EnableAggroFrames
		{
			get => this.EnableAggroFramesCheckBox.Checked;
			set => this.EnableAggroFramesCheckBox.Checked = value;
		}

		public Color AggroYellowColor
		{
			get => this._aggroYellowColor;
			set
			{
				this._aggroYellowColor = value;
				this.AggroYellowColorButton.BackColor = value;
			}
		}
		private Color _aggroYellowColor;

		public Color AggroRedColor
		{
			get => this._aggroRedColor;
			set
			{
				this._aggroRedColor = value;
				this.AggroRedColorButton.BackColor = value;
			}
		}
		private Color _aggroRedColor;

		public int AggroFillPercent
		{
			get => (int)this.AggroFillPercentNumericEdit.Value;
			set => this.AggroFillPercentNumericEdit.Value = Math.Max(this.AggroFillPercentNumericEdit.Minimum, Math.Min(this.AggroFillPercentNumericEdit.Maximum, value));
		}

		// The aggro settings only make sense while the log reading is on; otherwise the
		// page explains the dependency and offers a shortcut to the log settings
		private void RefreshAggroSubPage()
		{
			bool logsEnabled = this.EnableGameLogMonitorCheckBox.Checked;

			this.AggroSettingsPanel.Visible = logsEnabled;
			this.AggroDisabledPanel.Visible = !logsEnabled;
		}

		private void EnableGameLogMonitorCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			this.RefreshAggroSubPage();
			this.OptionChanged_Handler(sender, e);
		}

		private void GameLogsFolderBrowseButton_Click(object sender, EventArgs e)
		{
			using (FolderBrowserDialog dialog = new FolderBrowserDialog())
			{
				string currentFolder = this.GameLogsFolder;
				if ((currentFolder.Length > 0) && Directory.Exists(currentFolder))
				{
					dialog.SelectedPath = currentFolder;
				}

				if (this.ShowModalDialog(dialog) != DialogResult.OK)
				{
					return;
				}

				// Assigning the text raises TextChanged, which saves the settings
				this.GameLogsFolderTextBox.Text = dialog.SelectedPath;
			}
		}

		private void AggroYellowColorButton_Click(object sender, EventArgs e)
		{
			using (ColorDialog dialog = new ColorDialog { Color = this.AggroYellowColor })
			{
				if (this.ShowModalDialog(dialog) != DialogResult.OK)
				{
					return;
				}

				this.AggroYellowColor = dialog.Color;
			}

			this.OptionChanged_Handler(sender, e);
		}

		private void AggroRedColorButton_Click(object sender, EventArgs e)
		{
			using (ColorDialog dialog = new ColorDialog { Color = this.AggroRedColor })
			{
				if (this.ShowModalDialog(dialog) != DialogResult.OK)
				{
					return;
				}

				this.AggroRedColor = dialog.Color;
			}

			this.OptionChanged_Handler(sender, e);
		}

		private void AggroTestButton_Click(object sender, EventArgs e)
		{
			this.AggroTestRequested?.Invoke();
		}

		private void AggroGoToLogsButton_Click(object sender, EventArgs e)
		{
			TabControl tabControl = (TabControl)this.Controls.Find("ContentTabControl", false).First();
			tabControl.SelectedTab = this.GameLogsTabPage;
		}

		public Size WindowSize
		{
			get => this.WindowState == FormWindowState.Normal ? this.Size : this.RestoreBounds.Size;
			set
			{
				if (value.IsEmpty)
				{
					return;
				}

				this.Size = new Size(Math.Max(value.Width, this.MinimumSize.Width), Math.Max(value.Height, this.MinimumSize.Height));
			}
		}

		public new void Show()
		{
			// Registers the current instance as the application's Main Form
			this._context.MainForm = this;

			this._suppressEvents = true;
			this.FormActivated?.Invoke();
			this._suppressEvents = false;

			Application.Run(this._context);
		}

		// Raised while the main form is still being brought up, so the dialog is shown
		// without an owner - the form has no handle to parent it to yet
		public void ShowWarning(string title, string message)
		{
			MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
		}

		public bool ShowQuestion(string title, string message)
		{
			// The always-on-top main window would cover a message box of its own
			bool wasTopMost = this.TopMost;
			this.TopMost = false;

			try
			{
				return MessageBox.Show(this, message, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
			}
			finally
			{
				this.TopMost = wasTopMost;
			}
		}

		public void SetThumbnailSizeLimitations(Size minimumSize, Size maximumSize)
		{
			this._minimumSize = minimumSize;
			this._maximumSize = maximumSize;
		}

		public void Minimize()
		{
			this.WindowState = FormWindowState.Minimized;
		}

		public void SetVersionInfo(string version)
		{
			this.VersionLabel.Text = version;
		}

		public void SetDocumentationUrl(string url)
		{
			this.DocumentationLink.Text = url;
		}

		public void AddThumbnails(IList<IThumbnailDescription> thumbnails)
		{
			this.ThumbnailsList.BeginUpdate();

			foreach (IThumbnailDescription view in thumbnails)
			{
				// The checkbox is checked when the preview is enabled
				this.ThumbnailsList.SetItemChecked(this.ThumbnailsList.Items.Add(view), !view.IsDisabled);
			}

			this.ThumbnailsList.EndUpdate();
		}

		public void RemoveThumbnails(IList<IThumbnailDescription> thumbnails)
		{
			this.ThumbnailsList.BeginUpdate();

			foreach (IThumbnailDescription view in thumbnails)
			{
				this.ThumbnailsList.Items.Remove(view);
			}

			this.ThumbnailsList.EndUpdate();
		}

		public void RefreshZoomSettings()
		{
			bool enableControls = this.EnableThumbnailZoom;
			this.ThumbnailZoomFactorNumericEdit.Enabled = enableControls;
			this.ZoomAnchorPanel.Enabled = enableControls;
		}

		public void SetHotkeyActions(IList<(string ActionId, string DisplayName)> actions)
		{
			this._hotkeyActions = new List<(string ActionId, string DisplayName)>(actions);
		}

		public void SetHotkeyBindings(IList<(string ActionId, string ActionName, IList<string> Hotkeys)> bindings)
		{
			// The dialog needs them one by one to spot a combination taken by another action
			this._hotkeyBindings = bindings
									.SelectMany(binding => binding.Hotkeys.Select(hotkey => (binding.ActionId, binding.ActionName, hotkey)))
									.ToList();

			this.HotkeyBindingsListView.BeginUpdate();
			this.HotkeyBindingsListView.Items.Clear();

			foreach ((string actionId, string actionName, IList<string> hotkeys) in bindings)
			{
				// Every combination of the action stands in one row, joined into one string
				ListViewItem item = new ListViewItem(new[] { actionName, string.Join(", ", hotkeys) });
				item.Tag = (actionId, hotkeys);
				this.HotkeyBindingsListView.Items.Add(item);
			}

			this.HotkeyBindingsListView.EndUpdate();
		}

		public void SetHotkeyStatus(string status)
		{
			this.HotkeyStatusLabel.Text = status;
		}

		public void SetActiveClients(IList<string> clients)
		{
			this._activeClients = new List<string>(clients);

			string selectedClient = this.CycleGroupAddClientCombo.SelectedItem as string;

			this.CycleGroupAddClientCombo.BeginUpdate();
			this.CycleGroupAddClientCombo.Items.Clear();

			foreach (string client in clients)
			{
				int index = this.CycleGroupAddClientCombo.Items.Add(client);

				if (client == selectedClient)
				{
					this.CycleGroupAddClientCombo.SelectedIndex = index;
				}
			}

			this.CycleGroupAddClientCombo.EndUpdate();
		}

		public void SetCycleGroups(IList<(string Name, IList<string> Clients)> groups)
		{
			string selectedGroup = this.SelectedCycleGroupName;

			this._cycleGroupNames.Clear();
			this._cycleGroups.Clear();

			foreach ((string name, IList<string> clients) in groups)
			{
				this._cycleGroupNames.Add(name);
				this._cycleGroups[name] = new List<string>(clients);
			}

			bool suppressed = this._suppressEvents;
			this._suppressEvents = true;

			// The group selector on the Cycle Groups tab
			this.CycleGroupSelectCombo.BeginUpdate();
			this.CycleGroupSelectCombo.Items.Clear();
			foreach (string name in this._cycleGroupNames)
			{
				int index = this.CycleGroupSelectCombo.Items.Add(name);
				if (name == selectedGroup)
				{
					this.CycleGroupSelectCombo.SelectedIndex = index;
				}
			}
			if ((this.CycleGroupSelectCombo.SelectedIndex < 0) && (this.CycleGroupSelectCombo.Items.Count > 0))
			{
				this.CycleGroupSelectCombo.SelectedIndex = 0;
			}
			this.CycleGroupSelectCombo.EndUpdate();

			// The per-client group selector on the Active Clients tab
			string selectedClientGroup = this.ClientCycleGroupCombo.SelectedItem as string;
			this.ClientCycleGroupCombo.BeginUpdate();
			this.ClientCycleGroupCombo.Items.Clear();
			this.ClientCycleGroupCombo.Items.Add(Strings.Clients_NoCycleGroup);
			foreach (string name in this._cycleGroupNames)
			{
				this.ClientCycleGroupCombo.Items.Add(name);
			}
			this.ClientCycleGroupCombo.SelectedIndex = Math.Max(0, this.ClientCycleGroupCombo.Items.IndexOf(selectedClientGroup ?? Strings.Clients_NoCycleGroup));
			this.ClientCycleGroupCombo.EndUpdate();

			this._suppressEvents = suppressed;

			this.RenderSelectedCycleGroup();
			this.RefreshSelectedThumbnailCycleGroup();
		}

		public void SelectCycleGroup(string groupName)
		{
			int index = this.CycleGroupSelectCombo.Items.IndexOf(groupName);

			if (index >= 0)
			{
				this.CycleGroupSelectCombo.SelectedIndex = index;
			}
		}

		public void SetClientCycleGroups(IDictionary<string, IList<string>> clientGroups)
		{
			this._clientCycleGroups = new Dictionary<string, IList<string>>(clientGroups, StringComparer.OrdinalIgnoreCase);
			this.RefreshSelectedThumbnailCycleGroup();

			// The group membership is rendered as a suffix in the Active Clients list
			this.ThumbnailsList.Invalidate();
		}

		#region Preview clicks
		private PreviewClickRow _previewClickMinimizeRow;
		private PreviewClickRow _previewClickSwitchOutRow;
		private PreviewClickRow _previewClickToggleGroupRow;
		private Label _previewClickStatusLabel;

		public string PreviewClickMinimize
		{
			get => this._previewClickMinimizeRow.Value;
			set => this._previewClickMinimizeRow.Value = value;
		}

		public string PreviewClickSwitchOut
		{
			get => this._previewClickSwitchOutRow.Value;
			set => this._previewClickSwitchOutRow.Value = value;
		}

		public string PreviewClickToggleCycleGroup
		{
			get => this._previewClickToggleGroupRow.Value;
			set => this._previewClickToggleGroupRow.Value = value;
		}

		/// <summary>
		/// The rows are built here instead of the designer: they are three copies of the
		/// same modifiers-plus-button editor and the list is going to grow
		/// </summary>
		private void InitPreviewClicks()
		{
			Label hintLabel = new Label
			{
				Text = Strings.PreviewClicks_Hint,
				Location = new Point(6, 8),
				Size = new Size(301, 48),
				ForeColor = SystemColors.GrayText
			};

			this.PreviewClicksPanel.Controls.Add(hintLabel);

			int top = 62;

			this._previewClickToggleGroupRow = this.AddPreviewClickRow(Strings.PreviewClicks_ToggleCycleGroup, ref top);
			this._previewClickMinimizeRow = this.AddPreviewClickRow(Strings.PreviewClicks_Minimize, ref top);
			this._previewClickSwitchOutRow = this.AddPreviewClickRow(Strings.PreviewClicks_SwitchOut, ref top);

			this._previewClickStatusLabel = new Label
			{
				Location = new Point(6, top + 6),
				Size = new Size(301, 32),
				ForeColor = SystemColors.GrayText
			};

			this.PreviewClicksPanel.Controls.Add(this._previewClickStatusLabel);
		}

		private PreviewClickRow AddPreviewClickRow(string caption, ref int top)
		{
			Label captionLabel = new Label
			{
				Text = caption,
				Location = new Point(6, top),
				Size = new Size(301, 15),
				AutoEllipsis = true
			};

			PreviewClickRow row = new PreviewClickRow(this.PreviewClicksPanel, top + 20, this.PreviewClickChanged_Handler);

			this.PreviewClicksPanel.Controls.Add(captionLabel);

			top += 56;

			return row;
		}

		private void PreviewClickChanged_Handler(object sender, EventArgs e)
		{
			if (this._suppressEvents)
			{
				return;
			}

			PreviewClickRow[] rows = { this._previewClickToggleGroupRow, this._previewClickMinimizeRow, this._previewClickSwitchOutRow };
			PreviewClickRow changedRow = rows.FirstOrDefault(row => row.Owns(sender));

			if (changedRow == null)
			{
				return;
			}

			string value = changedRow.Value;

			// The plain left click activates the client and is not up for grabs
			if (PreviewClickBinding.IsReservedForActivation(value))
			{
				this.RejectPreviewClick(changedRow, Strings.PreviewClicks_LeftClickReserved);
				return;
			}

			if (rows.Any(row => (row != changedRow) && (row.Value.Length > 0) && (row.Value == value)))
			{
				this.RejectPreviewClick(changedRow, Strings.PreviewClicks_AlreadyAssigned);
				return;
			}

			this._previewClickStatusLabel.Text = "";
			changedRow.Accept();

			this.ApplicationSettingsChanged?.Invoke();
		}

		private void RejectPreviewClick(PreviewClickRow row, string message)
		{
			this._previewClickStatusLabel.Text = message;

			bool suppressed = this._suppressEvents;
			this._suppressEvents = true;

			try
			{
				row.Revert();
			}
			finally
			{
				this._suppressEvents = suppressed;
			}
		}

		/// <summary>One assignable click: the modifier check boxes and the mouse button</summary>
		private sealed class PreviewClickRow
		{
			private readonly CheckBox _control;
			private readonly CheckBox _shift;
			private readonly CheckBox _alt;
			private readonly ComboBox _button;

			private string _acceptedValue;

			public PreviewClickRow(Control parent, int top, EventHandler changedHandler)
			{
				this._control = PreviewClickRow.CreateModifier("Ctrl", 20, top);
				this._shift = PreviewClickRow.CreateModifier("Shift", 76, top);
				this._alt = PreviewClickRow.CreateModifier("Alt", 138, top);

				this._button = new ComboBox
				{
					Location = new Point(190, top - 2),
					Size = new Size(117, 23),
					DropDownStyle = ComboBoxStyle.DropDownList
				};

				this._button.Items.Add(Strings.PreviewClicks_NoButton);
				this._button.Items.AddRange(PreviewClickBinding.GetButtonNames());
				this._button.SelectedIndex = 0;

				this._acceptedValue = "";

				parent.Controls.Add(this._control);
				parent.Controls.Add(this._shift);
				parent.Controls.Add(this._alt);
				parent.Controls.Add(this._button);

				this._control.CheckedChanged += changedHandler;
				this._shift.CheckedChanged += changedHandler;
				this._alt.CheckedChanged += changedHandler;
				this._button.SelectedIndexChanged += changedHandler;
			}

			public bool Owns(object control)
			{
				return object.ReferenceEquals(control, this._control) || object.ReferenceEquals(control, this._shift)
						|| object.ReferenceEquals(control, this._alt) || object.ReferenceEquals(control, this._button);
			}

			public string Value
			{
				get
				{
					if (this._button.SelectedIndex <= 0)
					{
						return "";
					}

					Keys modifiers = (this._control.Checked ? Keys.Control : Keys.None)
									| (this._shift.Checked ? Keys.Shift : Keys.None)
									| (this._alt.Checked ? Keys.Alt : Keys.None);

					return PreviewClickBinding.Compose(modifiers, PreviewClickRow.GetButton((string)this._button.SelectedItem));
				}

				set
				{
					string normalized = PreviewClickBinding.Normalize(value);

					PreviewClickBinding.TryParse(normalized, out Keys modifiers, out MouseButtons button);

					this._control.Checked = (modifiers & Keys.Control) == Keys.Control;
					this._shift.Checked = (modifiers & Keys.Shift) == Keys.Shift;
					this._alt.Checked = (modifiers & Keys.Alt) == Keys.Alt;

					string name = PreviewClickBinding.GetButtonName(button);
					this._button.SelectedIndex = (name == null) ? 0 : Math.Max(0, this._button.Items.IndexOf(name));

					this._acceptedValue = normalized;
				}
			}

			/// <summary>Remembers the current combination as the one that got through</summary>
			public void Accept()
			{
				this._acceptedValue = this.Value;
			}

			/// <summary>Puts the controls back to the last combination that got through</summary>
			public void Revert()
			{
				this.Value = this._acceptedValue;
			}

			private static CheckBox CreateModifier(string caption, int left, int top)
			{
				return new CheckBox
				{
					Text = caption,
					Location = new Point(left, top),
					AutoSize = true
				};
			}

			private static MouseButtons GetButton(string name)
			{
				switch (name)
				{
					case PreviewClickBinding.MIDDLE_BUTTON:
						return MouseButtons.Middle;
					case PreviewClickBinding.RIGHT_BUTTON:
						return MouseButtons.Right;
					case PreviewClickBinding.X_BUTTON_1:
						return MouseButtons.XButton1;
					case PreviewClickBinding.X_BUTTON_2:
						return MouseButtons.XButton2;
					default:
						return MouseButtons.Left;
				}
			}
		}
		#endregion

		#region Character registry
		private ToolStripMenuItem _characterPreviewSettingsMenuItem;
		private ToolStripMenuItem _characterIgnoreMenuItem;
		private ToolStripMenuItem _characterForgetMenuItem;

		private const string CHARACTER_NODE_PREFIX = "c:";
		private const string CHARACTER_GROUP_NODE_PREFIX = "g:";

		// Marks the 'create a new group' entry of the group combo box
		private const string NEW_CHARACTER_GROUP_ID = "\u0001new";

		public void SetCharacters(IList<CharacterGroupViewItem> groups, IList<CharacterViewItem> characters)
		{
			this._characterGroups = groups ?? new List<CharacterGroupViewItem>();
			this._characters = characters ?? new List<CharacterViewItem>();

			this.RenderCharacters();
		}

		private void RenderCharacters()
		{
			string selectedTag = this.CharactersTree.SelectedNode?.Tag as string;
			string filter = this.CharacterFilterEdit.Text.Trim();

			this.CharactersTree.BeginUpdate();
			this.CharactersTree.Nodes.Clear();

			foreach (CharacterGroupViewItem group in this._characterGroups)
			{
				List<CharacterViewItem> members = this._characters.Where(character => string.Equals(character.GroupId, group.Id, StringComparison.Ordinal)
																						&& !character.IsIgnored).ToList();

				// A group matching the filter is shown with all of its members
				bool groupMatches = MainForm.MatchesCharacterFilter(group.Name, filter);
				List<CharacterViewItem> visibleMembers = groupMatches
														? members
														: members.Where(character => MainForm.MatchesCharacterFilter(character.Name, filter)).ToList();

				if (visibleMembers.Count == 0)
				{
					continue;
				}

				TreeNode groupNode = new TreeNode(group.Name + " (" + members.Count + ")")
				{
					Tag = MainForm.CHARACTER_GROUP_NODE_PREFIX + group.Id,
					ForeColor = group.Color.IsEmpty ? this.CharactersTree.ForeColor : group.Color
				};

				foreach (CharacterViewItem character in visibleMembers)
				{
					groupNode.Nodes.Add(this.CreateCharacterNode(character));
				}

				this.CharactersTree.Nodes.Add(groupNode);
			}

			List<CharacterViewItem> ungrouped = this._characters
													.Where(character => string.IsNullOrEmpty(character.GroupId) && !character.IsIgnored
																		&& MainForm.MatchesCharacterFilter(character.Name, filter))
													.ToList();

			if (ungrouped.Count > 0)
			{
				TreeNode ungroupedNode = new TreeNode(Strings.Characters_NoGroup + " (" + ungrouped.Count + ")");

				foreach (CharacterViewItem character in ungrouped)
				{
					ungroupedNode.Nodes.Add(this.CreateCharacterNode(character));
				}

				this.CharactersTree.Nodes.Add(ungroupedNode);
			}

			// The blacklist sits at the bottom, collapsed: those characters are done with
			List<CharacterViewItem> ignored = this._characters
													.Where(character => character.IsIgnored && MainForm.MatchesCharacterFilter(character.Name, filter))
													.ToList();

			TreeNode ignoredNode = null;

			if (ignored.Count > 0)
			{
				ignoredNode = new TreeNode(Strings.Characters_Blacklist + " (" + ignored.Count + ")")
				{
					ForeColor = SystemColors.GrayText
				};

				foreach (CharacterViewItem character in ignored)
				{
					ignoredNode.Nodes.Add(this.CreateCharacterNode(character));
				}

				this.CharactersTree.Nodes.Add(ignoredNode);
			}

			this.CharactersTree.ExpandAll();

			ignoredNode?.Collapse();

			TreeNode nodeToSelect = MainForm.FindCharacterNode(this.CharactersTree.Nodes, selectedTag);

			if (nodeToSelect != null)
			{
				this.CharactersTree.SelectedNode = nodeToSelect;
				nodeToSelect.EnsureVisible();
			}

			this.CharactersTree.EndUpdate();

			this.RefreshCharacterDetails();
		}

		private TreeNode CreateCharacterNode(CharacterViewItem character)
		{
			return new TreeNode(character.IsOnline ? "\u25cf " + character.Name : character.Name)
			{
				Tag = MainForm.CHARACTER_NODE_PREFIX + character.Title,
				ToolTipText = character.Title + Environment.NewLine + character.LastSeen
			};
		}

		private static bool MatchesCharacterFilter(string value, string filter)
		{
			return (filter.Length == 0)
					|| ((value != null) && (value.IndexOf(filter, StringComparison.CurrentCultureIgnoreCase) >= 0));
		}

		private static TreeNode FindCharacterNode(TreeNodeCollection nodes, string tag)
		{
			if (tag == null)
			{
				return null;
			}

			foreach (TreeNode node in nodes)
			{
				if (string.Equals(node.Tag as string, tag, StringComparison.Ordinal))
				{
					return node;
				}

				TreeNode childNode = MainForm.FindCharacterNode(node.Nodes, tag);

				if (childNode != null)
				{
					return childNode;
				}
			}

			return null;
		}

		private void RefreshCharacterDetails()
		{
			CharacterViewItem character = this.SelectedCharacter;
			CharacterGroupViewItem group = this.SelectedCharacterGroup;

			bool suppressed = this._suppressEvents;
			this._suppressEvents = true;

			try
			{
				this.CharacterGroupCombo.BeginUpdate();
				this.CharacterGroupCombo.Items.Clear();
				this.CharacterGroupCombo.Items.Add(new CharacterGroupComboItem(null, Strings.Characters_NoGroup));

				foreach (CharacterGroupViewItem item in this._characterGroups)
				{
					this.CharacterGroupCombo.Items.Add(new CharacterGroupComboItem(item.Id, item.Name));
				}

				this.CharacterGroupCombo.Items.Add(new CharacterGroupComboItem(MainForm.NEW_CHARACTER_GROUP_ID, Strings.Characters_NewGroup));
				this.CharacterGroupCombo.EndUpdate();

				int selectedIndex = 0;

				for (int i = 0; i < this.CharacterGroupCombo.Items.Count; i++)
				{
					CharacterGroupComboItem item = (CharacterGroupComboItem)this.CharacterGroupCombo.Items[i];

					if (string.Equals(item.Id, character?.GroupId, StringComparison.Ordinal))
					{
						selectedIndex = i;
						break;
					}
				}

				this.CharacterGroupCombo.SelectedIndex = selectedIndex;
				this.CharacterGroupCombo.Enabled = character != null;

				this.CharacterManageAsWholeCheckBox.Checked = (group != null) && group.ManageAsWhole;
				this.CharacterManageAsWholeCheckBox.Enabled = group != null;
				this.CharacterRenameGroupButton.Enabled = group != null;
				this.CharacterUngroupButton.Enabled = group != null;
				this.CharacterForgetButton.Enabled = character != null;
				this.CharacterPreviewSettingsButton.Enabled = character != null;

				this.CharacterGroupColorButton.BackColor = (group == null) ? SystemColors.Control : group.Color;
				this.CharacterGroupColorButton.Enabled = group != null;
			}
			finally
			{
				this._suppressEvents = suppressed;
			}
		}

		private CharacterViewItem SelectedCharacter
		{
			get
			{
				string tag = this.CharactersTree.SelectedNode?.Tag as string;

				if ((tag == null) || !tag.StartsWith(MainForm.CHARACTER_NODE_PREFIX, StringComparison.Ordinal))
				{
					return null;
				}

				string title = tag.Substring(MainForm.CHARACTER_NODE_PREFIX.Length);

				return this._characters.FirstOrDefault(character => string.Equals(character.Title, title, StringComparison.Ordinal));
			}
		}

		/// <summary>Group of the selected node: the group itself or the group of the character</summary>
		private CharacterGroupViewItem SelectedCharacterGroup
		{
			get
			{
				string tag = this.CharactersTree.SelectedNode?.Tag as string;

				if ((tag != null) && tag.StartsWith(MainForm.CHARACTER_GROUP_NODE_PREFIX, StringComparison.Ordinal))
				{
					string groupId = tag.Substring(MainForm.CHARACTER_GROUP_NODE_PREFIX.Length);

					return this._characterGroups.FirstOrDefault(group => string.Equals(group.Id, groupId, StringComparison.Ordinal));
				}

				string characterGroupId = this.SelectedCharacter?.GroupId;

				return string.IsNullOrEmpty(characterGroupId)
						? null
						: this._characterGroups.FirstOrDefault(group => string.Equals(group.Id, characterGroupId, StringComparison.Ordinal));
			}
		}

		private void InitCharactersContextMenu()
		{
			this._characterPreviewSettingsMenuItem = new ToolStripMenuItem(Strings.Characters_PreviewSettings, null,
																			this.CharacterPreviewSettingsButton_Click_Handler);
			this._characterIgnoreMenuItem = new ToolStripMenuItem(Strings.Characters_Blacklist_Add, null, this.CharacterIgnoreMenuItem_Click_Handler);
			this._characterForgetMenuItem = new ToolStripMenuItem(Strings.Characters_Forget, null, this.CharacterForgetButton_Click_Handler);

			ContextMenuStrip menu = new ContextMenuStrip();
			menu.Items.Add(this._characterPreviewSettingsMenuItem);
			menu.Items.Add(new ToolStripSeparator());
			menu.Items.Add(this._characterIgnoreMenuItem);
			menu.Items.Add(this._characterForgetMenuItem);
			menu.Opening += this.CharactersContextMenu_Opening_Handler;

			this.CharactersTree.ContextMenuStrip = menu;
		}

		private void CharactersContextMenu_Opening_Handler(object sender, System.ComponentModel.CancelEventArgs e)
		{
			// The right click does not move the selection on its own
			TreeNode node = this.CharactersTree.GetNodeAt(this.CharactersTree.PointToClient(Cursor.Position));

			if (node != null)
			{
				this.CharactersTree.SelectedNode = node;
			}

			CharacterViewItem character = this.SelectedCharacter;

			if (character == null)
			{
				e.Cancel = true;
				return;
			}

			this._characterIgnoreMenuItem.Text = character.IsIgnored ? Strings.Characters_Blacklist_Remove : Strings.Characters_Blacklist_Add;
		}

		private void CharacterIgnoreMenuItem_Click_Handler(object sender, EventArgs e)
		{
			CharacterViewItem character = this.SelectedCharacter;

			if (character == null)
			{
				return;
			}

			if (!character.IsIgnored
				&& !this.ShowQuestion(Strings.Characters_Blacklist_Add, string.Format(Strings.Characters_BlacklistPrompt, character.Name)))
			{
				return;
			}

			this.CharacterIgnoreChanged?.Invoke(character.Title, !character.IsIgnored);
		}

		private void CharacterGroupColorButton_Click_Handler(object sender, EventArgs e)
		{
			CharacterGroupViewItem group = this.SelectedCharacterGroup;

			if (group == null)
			{
				return;
			}

			using (ColorDialog dialog = new ColorDialog { Color = group.Color })
			{
				if (this.ShowModalDialog(dialog) != DialogResult.OK)
				{
					return;
				}

				this.CharacterGroupColorChanged?.Invoke(group.Id, dialog.Color);
			}
		}

		private void CharacterFilterEdit_TextChanged_Handler(object sender, EventArgs e)
		{
			this.RenderCharacters();
		}

		private void CharactersTree_AfterSelect_Handler(object sender, TreeViewEventArgs e)
		{
			if (this._suppressEvents)
			{
				return;
			}

			this.RefreshCharacterDetails();
		}

		private void CharacterGroupCombo_SelectedIndexChanged_Handler(object sender, EventArgs e)
		{
			if (this._suppressEvents)
			{
				return;
			}

			CharacterViewItem character = this.SelectedCharacter;

			if ((character == null) || !(this.CharacterGroupCombo.SelectedItem is CharacterGroupComboItem item))
			{
				return;
			}

			if (item.Id == MainForm.NEW_CHARACTER_GROUP_ID)
			{
				using (TextPromptDialog dialog = new TextPromptDialog(Strings.Characters_NewGroupTitle, Strings.Characters_NamePrompt, character.Name))
				{
					if ((this.ShowModalDialog(dialog) != DialogResult.OK) || (dialog.Value.Length == 0))
					{
						// Put the combo box back to the group the character is in
						this.RefreshCharacterDetails();
						return;
					}

					this.CharacterGroupCreateRequested?.Invoke(character.Title, dialog.Value);
				}

				return;
			}

			if (string.Equals(item.Id, character.GroupId, StringComparison.Ordinal))
			{
				return;
			}

			this.CharacterGroupChanged?.Invoke(character.Title, item.Id);
		}

		private void CharacterManageAsWholeCheckBox_CheckedChanged_Handler(object sender, EventArgs e)
		{
			if (this._suppressEvents)
			{
				return;
			}

			CharacterGroupViewItem group = this.SelectedCharacterGroup;

			if (group == null)
			{
				return;
			}

			this.CharacterGroupManageAsWholeChanged?.Invoke(group.Id, this.CharacterManageAsWholeCheckBox.Checked);
		}

		private void CharacterRenameGroupButton_Click_Handler(object sender, EventArgs e)
		{
			CharacterGroupViewItem group = this.SelectedCharacterGroup;

			if (group == null)
			{
				return;
			}

			using (TextPromptDialog dialog = new TextPromptDialog(Strings.Characters_RenameTitle, Strings.Characters_NamePrompt, group.Name))
			{
				if ((this.ShowModalDialog(dialog) != DialogResult.OK) || (dialog.Value.Length == 0) || (dialog.Value == group.Name))
				{
					return;
				}

				this.CharacterGroupRenameRequested?.Invoke(group.Id, dialog.Value);
			}
		}

		private void CharacterUngroupButton_Click_Handler(object sender, EventArgs e)
		{
			CharacterGroupViewItem group = this.SelectedCharacterGroup;

			if (group == null)
			{
				return;
			}

			if (!this.ShowQuestion(Strings.Characters_UngroupTitle, string.Format(Strings.Characters_UngroupPrompt, group.Name)))
			{
				return;
			}

			this.CharacterGroupRemoveRequested?.Invoke(group.Id);
		}

		private void CharacterForgetButton_Click_Handler(object sender, EventArgs e)
		{
			CharacterViewItem character = this.SelectedCharacter;

			if (character == null)
			{
				return;
			}

			if (!this.ShowQuestion(Strings.Characters_ForgetTitle, string.Format(Strings.Characters_ForgetPrompt, character.Name)))
			{
				return;
			}

			this.CharacterForgetRequested?.Invoke(character.Title);
		}

		private void CharacterSuggestGroupsButton_Click_Handler(object sender, EventArgs e)
		{
			this.CharacterGroupsSuggestionRequested?.Invoke();
		}

		private void CharacterPreviewSettingsButton_Click_Handler(object sender, EventArgs e)
		{
			CharacterViewItem character = this.SelectedCharacter;

			if (character == null)
			{
				return;
			}

			this.CharacterPreviewSettingsRequested?.Invoke(character.Title);
		}

		public void ShowPreviewSettings(string title, string caption, string groupHint, PreviewSettings values, PreviewSettings globals)
		{
			using (PreviewSettingsDialog dialog = new PreviewSettingsDialog(caption, groupHint, values, globals))
			{
				if (this.ShowModalDialog(dialog) != DialogResult.OK)
				{
					return;
				}

				this.CharacterPreviewSettingsChanged?.Invoke(title, dialog.Value);
			}
		}

		private sealed class CharacterGroupComboItem
		{
			public CharacterGroupComboItem(string id, string name)
			{
				this.Id = id;
				this.Name = name;
			}

			public string Id { get; }
			public string Name { get; }

			public override string ToString()
			{
				return this.Name;
			}
		}
		#endregion

		public Action ApplicationExitRequested { get; set; }

		public Action FormActivated { get; set; }

		public Action FormMinimized { get; set; }

		public Action<ViewCloseRequest> FormCloseRequested { get; set; }

		public Action ApplicationSettingsChanged { get; set; }

		public Action ThumbnailsSizeChanged { get; set; }

		public Action<string> ThumbnailStateChanged { get; set; }

		public Action DocumentationLinkActivated { get; set; }

		public Action<string, IList<string>> HotkeyBindingsChanged { get; set; }

		public Action<string> HotkeyBindingsRemoved { get; set; }

		public Action<string, IList<string>> CycleGroupClientsChanged { get; set; }

		public Action<string, string> ThumbnailCycleGroupChanged { get; set; }

		public Action CycleGroupAddRequested { get; set; }

		public Action<string> CycleGroupRemoveRequested { get; set; }

		public Action<string, string> CycleGroupRenameRequested { get; set; }

		public Action<bool> HotkeyCaptureModeChanged { get; set; }

		public Action WindowSizeChanged { get; set; }

		public Action AggroTestRequested { get; set; }

		public Action<string, string> CharacterGroupChanged { get; set; }

		public Action<string, string> CharacterGroupCreateRequested { get; set; }

		public Action<string, string> CharacterGroupRenameRequested { get; set; }

		public Action<string> CharacterGroupRemoveRequested { get; set; }

		public Action<string, bool> CharacterGroupManageAsWholeChanged { get; set; }

		public Action<string> CharacterForgetRequested { get; set; }

		public Action CharacterGroupsSuggestionRequested { get; set; }

		public Action<string> CharacterPreviewSettingsRequested { get; set; }

		public Action<string, PreviewSettings> CharacterPreviewSettingsChanged { get; set; }

		public Action<string, bool> CharacterIgnoreChanged { get; set; }

		public Action<string, Color> CharacterGroupColorChanged { get; set; }

		#region UI events
		private void ContentTabControl_DrawItem(object sender, DrawItemEventArgs e)
		{
			TabControl control = (TabControl)sender;
			TabPage page = control.TabPages[e.Index];
			Rectangle bounds = control.GetTabRect(e.Index);

			Graphics graphics = e.Graphics;
			bool isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;

			// Flat look: unselected tabs blend with the form, the selected one
			// is highlighted with a lighter background and an accent bar
			using (Brush backgroundBrush = new SolidBrush(isSelected ? SystemColors.Window : SystemColors.Control))
			{
				graphics.FillRectangle(backgroundBrush, bounds);
			}

			if (isSelected)
			{
				using (Brush accentBrush = new SolidBrush(SystemColors.Highlight))
				{
					graphics.FillRectangle(accentBrush, new Rectangle(bounds.X, bounds.Y, this.LogicalToDeviceUnits(4), bounds.Height));
				}
			}

			using (Font font = new Font(this.Font, isSelected ? FontStyle.Bold : FontStyle.Regular))
			using (Brush textBrush = new SolidBrush(SystemColors.ControlText))
			using (StringFormat stringFormat = new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center, Trimming = StringTrimming.EllipsisCharacter })
			{
				Rectangle textBounds = new Rectangle(bounds.X + this.LogicalToDeviceUnits(12), bounds.Y, bounds.Width - this.LogicalToDeviceUnits(14), bounds.Height);
				graphics.DrawString(page.Text, font, textBrush, textBounds, stringFormat);
			}
		}

		private void MainFormResizeEnd_Handler(object sender, EventArgs e)
		{
			if (this._suppressEvents || (this.WindowState == FormWindowState.Minimized))
			{
				return;
			}

			this.WindowSizeChanged?.Invoke();
		}

		private void OptionChanged_Handler(object sender, EventArgs e)
		{
			if (this._suppressEvents)
			{
				return;
			}

			if (sender == this.ShowThumbnailOverlaysCheckBox)
			{
				this.RefreshOverlaySubPages();
			}

			this.ApplicationSettingsChanged?.Invoke();
		}

		// Forms are built with the culture that was active at startup, so a language picked
		// here only takes effect on the next run
		private void LanguageChanged_Handler(object sender, EventArgs e)
		{
			if (this._suppressEvents)
			{
				return;
			}

			this.LanguageRestartHintLabel.Visible = this.Language != this._loadedLanguage;

			this.ApplicationSettingsChanged?.Invoke();
		}

		private void ThumbnailSizeChanged_Handler(object sender, EventArgs e)
		{
			if (this._suppressEvents)
			{
				return;
			}

			// Perform some View work that is not properly done in the Control
			this._suppressEvents = true;
			Size thumbnailSize = this.ThumbnailSize;
			thumbnailSize.Width = Math.Min(Math.Max(thumbnailSize.Width, this._minimumSize.Width), this._maximumSize.Width);
			thumbnailSize.Height = Math.Min(Math.Max(thumbnailSize.Height, this._minimumSize.Height), this._maximumSize.Height);
			this.ThumbnailSize = thumbnailSize;
			this._suppressEvents = false;

			this.ThumbnailsSizeChanged?.Invoke();
		}

		private void ActiveClientHighlightColorButton_Click(object sender, EventArgs e)
		{
			using (ColorDialog dialog = new ColorDialog())
			{
				dialog.Color = this.ActiveClientHighlightColor;

				if (this.ShowModalDialog(dialog) != DialogResult.OK)
				{
					return;
				}

				this.ActiveClientHighlightColor = dialog.Color;
			}

			this.OptionChanged_Handler(sender, e);
		}

		public bool OverlayLabelOutlineEnabled
		{
			get => this.LabelOutlineCheckBox.Checked;
			set => this.LabelOutlineCheckBox.Checked = value;
		}

		public int OverlayLabelOutlineThickness
		{
			get => (int)this.LabelOutlineThicknessNumericEdit.Value;
			set => this.LabelOutlineThicknessNumericEdit.Value = Math.Max(this.LabelOutlineThicknessNumericEdit.Minimum, Math.Min(this.LabelOutlineThicknessNumericEdit.Maximum, value));
		}

		public Color OverlayLabelOutlineColor
		{
			get => this.LabelOutlineColorButton.BackColor;
			set => this.LabelOutlineColorButton.BackColor = value;
		}

		public bool CycleGroupNameOutlineEnabled
		{
			get => this.GroupNameOutlineCheckBox.Checked;
			set => this.GroupNameOutlineCheckBox.Checked = value;
		}

		public int CycleGroupNameOutlineThickness
		{
			get => (int)this.GroupNameOutlineThicknessNumericEdit.Value;
			set => this.GroupNameOutlineThicknessNumericEdit.Value = Math.Max(this.GroupNameOutlineThicknessNumericEdit.Minimum, Math.Min(this.GroupNameOutlineThicknessNumericEdit.Maximum, value));
		}

		public Color CycleGroupNameOutlineColor
		{
			get => this.GroupNameOutlineColorButton.BackColor;
			set => this.GroupNameOutlineColorButton.BackColor = value;
		}

		private void LabelOutlineColorButton_Click(object sender, EventArgs e)
		{
			using (ColorDialog dialog = new ColorDialog())
			{
				dialog.Color = this.OverlayLabelOutlineColor;

				if (this.ShowModalDialog(dialog) != DialogResult.OK)
				{
					return;
				}
				this.OverlayLabelOutlineColor = dialog.Color;
			}

			this.OptionChanged_Handler(sender, e);
		}

		private void GroupNameOutlineColorButton_Click(object sender, EventArgs e)
		{
			using (ColorDialog dialog = new ColorDialog())
			{
				dialog.Color = this.CycleGroupNameOutlineColor;

				if (this.ShowModalDialog(dialog) != DialogResult.OK)
				{
					return;
				}
				this.CycleGroupNameOutlineColor = dialog.Color;
			}

			this.OptionChanged_Handler(sender, e);
		}

		private void OverlayLabelColorButton_Click(object sender, EventArgs e)
		{
			using (ColorDialog dialog = new ColorDialog())
			{
				dialog.Color = this.OverlayLabelColor;

				if (this.ShowModalDialog(dialog) != DialogResult.OK)
				{
					return;
				}
				this.OverlayLabelColor = dialog.Color;
			}

			this.OptionChanged_Handler(sender, e);
		}

		private void ThumbnailsList_MouseDown_Handler(object sender, MouseEventArgs e)
		{
			this._thumbnailsListClickLocation = e.Location;
		}

		private void ThumbnailsList_Format_Handler(object sender, ListControlConvertEventArgs e)
		{
			if (!(e.ListItem is IThumbnailDescription description))
			{
				return;
			}

			// Render the cycle groups the client belongs to as a row suffix
			if (this._clientCycleGroups.TryGetValue(description.Title, out IList<string> groups) && (groups.Count > 0))
			{
				e.Value = description.Title + "   [" + string.Join(", ", groups) + "]";
			}
			else
			{
				e.Value = description.Title;
			}
		}

		private void ThumbnailsList_ItemCheck_Handler(object sender, ItemCheckEventArgs e)
		{
			// A mouse click toggles the checkbox only when it lands on the checkbox itself,
			// clicking the row text just selects the row
			if (this._thumbnailsListClickLocation is Point clickLocation)
			{
				this._thumbnailsListClickLocation = null;

				if (clickLocation.X > this.LogicalToDeviceUnits(16))
				{
					e.NewValue = e.CurrentValue;
					return;
				}
			}

			if (!(this.ThumbnailsList.Items[e.Index] is IThumbnailDescription selectedItem))
			{
				return;
			}

			// The checkbox is checked when the preview is enabled
			selectedItem.IsDisabled = (e.NewValue != CheckState.Checked);

			this.ThumbnailStateChanged?.Invoke(selectedItem.Title);
		}

		private void DocumentationLinkClicked_Handler(object sender, LinkLabelLinkClickedEventArgs e)
		{
			this.DocumentationLinkActivated?.Invoke();
		}

		private void MainFormResize_Handler(object sender, EventArgs e)
		{
			if (this.WindowState != FormWindowState.Minimized)
			{
				return;
			}

			this.FormMinimized?.Invoke();
		}

		private void MainFormClosing_Handler(object sender, FormClosingEventArgs e)
		{
			ViewCloseRequest request = new ViewCloseRequest();

			this.FormCloseRequested?.Invoke(request);

			e.Cancel = !request.Allow;
		}

		// Visual aid: dims the desktop and shows the snap grid; not persisted anywhere
		private GridOverlayForm _gridOverlay;

		private void ShowGridCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (this.ShowGridCheckBox.Checked)
			{
				this._gridOverlay = this._gridOverlay ?? new GridOverlayForm();
				this._gridOverlay.SetGridStep(
					(int)this.ThumbnailSnapToGridSizeXNumericEdit.Value, (int)this.ThumbnailSnapToGridSizeYNumericEdit.Value,
					(int)this.GridOffsetXNumericEdit.Value, (int)this.GridOffsetYNumericEdit.Value);
				this._gridOverlay.Show();

				// The grid enters the topmost band on top - push it below the previews
				// and this window right away
				this._gridOverlay.SinkBelowOtherTopmostWindows();
			}
			else
			{
				this._gridOverlay?.Hide();
			}
		}

		private void RestoreMainForm_Handler(object sender, EventArgs e)
		{
			// This is form's GUI lifecycle event that is invariant to the Form data
			base.Show();
			this.WindowState = FormWindowState.Normal;
			this.BringToFront();
		}

		// A second app instance broadcasts this message before quitting (see Program.Main),
		// the running instance responds by restoring its main window from the tray
		private static readonly uint RESTORE_INSTANCE_MESSAGE =
			Services.Interop.User32NativeMethods.RegisterWindowMessage(Program.RESTORE_INSTANCE_MESSAGE_NAME);

		protected override void WndProc(ref Message m)
		{
			if ((MainForm.RESTORE_INSTANCE_MESSAGE != 0) && (m.Msg == MainForm.RESTORE_INSTANCE_MESSAGE))
			{
				this.RestoreMainForm_Handler(this, EventArgs.Empty);
				return;
			}

			base.WndProc(ref m);
		}

		private void ExitMenuItemClick_Handler(object sender, EventArgs e)
		{
			this.ApplicationExitRequested?.Invoke();
		}

		// The main form is TopMost, so a non-TopMost modal dialog would open behind it.
		// TopMost is dropped for the time the dialog is on the screen instead
		private DialogResult ShowModalDialog(Form dialog)
		{
			bool wasTopMost = this.TopMost;
			this.TopMost = false;

			try
			{
				return dialog.ShowDialog(this);
			}
			finally
			{
				this.TopMost = wasTopMost;
			}
		}

		private DialogResult ShowModalDialog(CommonDialog dialog)
		{
			bool wasTopMost = this.TopMost;
			this.TopMost = false;

			try
			{
				return dialog.ShowDialog(this);
			}
			finally
			{
				this.TopMost = wasTopMost;
			}
		}

		// Registered hotkeys are released while the editor is open, otherwise an already
		// bound combination would be swallowed by its handler and never reach the capture box
		private DialogResult ShowHotkeyDialog(HotkeyEditDialog dialog)
		{
			this.HotkeyCaptureModeChanged?.Invoke(true);

			try
			{
				return this.ShowModalDialog(dialog);
			}
			finally
			{
				this.HotkeyCaptureModeChanged?.Invoke(false);
			}
		}

		private void AddHotkeyButton_Click_Handler(object sender, EventArgs e)
		{
			using (HotkeyEditDialog dialog = new HotkeyEditDialog(this._hotkeyActions, this._activeClients, this._hotkeyBindings))
			{
				if (this.ShowHotkeyDialog(dialog) != DialogResult.OK)
				{
					return;
				}

				this.HotkeyBindingsChanged?.Invoke(dialog.SelectedActionId, dialog.HotkeyStrings);
			}
		}

		private void EditHotkeyButton_Click_Handler(object sender, EventArgs e)
		{
			if (this.HotkeyBindingsListView.SelectedItems.Count == 0)
			{
				this.SetHotkeyStatus(Strings.Hotkeys_SelectBindingFirst);
				return;
			}

			(string actionId, IList<string> hotkeys) = ((string, IList<string>))this.HotkeyBindingsListView.SelectedItems[0].Tag;

			using (HotkeyEditDialog dialog = new HotkeyEditDialog(this._hotkeyActions, this._activeClients, this._hotkeyBindings, actionId, hotkeys))
			{
				if (this.ShowHotkeyDialog(dialog) != DialogResult.OK)
				{
					return;
				}

				this.HotkeyBindingsChanged?.Invoke(dialog.SelectedActionId, dialog.HotkeyStrings);
			}
		}

		private void CycleGroupRenameButton_Click_Handler(object sender, EventArgs e)
		{
			string groupName = this.SelectedCycleGroupName;

			if (groupName == null)
			{
				return;
			}

			using (TextPromptDialog dialog = new TextPromptDialog(Strings.CycleGroups_RenameTitle, Strings.CycleGroups_NamePrompt, groupName))
			{
				if (this.ShowModalDialog(dialog) != DialogResult.OK)
				{
					return;
				}

				if ((dialog.Value.Length == 0) || (dialog.Value == groupName))
				{
					return;
				}

				this.CycleGroupRenameRequested?.Invoke(groupName, dialog.Value);
			}
		}

		private void HotkeyBindingsListView_DoubleClick_Handler(object sender, EventArgs e)
		{
			this.EditHotkeyButton_Click_Handler(sender, e);
		}

		private void RemoveHotkeyButton_Click_Handler(object sender, EventArgs e)
		{
			if (this.HotkeyBindingsListView.SelectedItems.Count == 0)
			{
				this.SetHotkeyStatus(Strings.Hotkeys_SelectBindingFirst);
				return;
			}

			(string actionId, IList<string> hotkeys) = ((string, IList<string>))this.HotkeyBindingsListView.SelectedItems[0].Tag;

			this.HotkeyBindingsRemoved?.Invoke(actionId);
		}

		private string SelectedCycleGroupName => this.CycleGroupSelectCombo.SelectedItem as string;

		private List<string> GetSelectedCycleGroupClients()
		{
			string groupName = this.SelectedCycleGroupName;

			if (groupName == null)
			{
				return null;
			}

			if (!this._cycleGroups.TryGetValue(groupName, out List<string> clients))
			{
				clients = new List<string>();
				this._cycleGroups[groupName] = clients;
			}

			return clients;
		}

		private void RenderSelectedCycleGroup()
		{
			List<string> clients = this.GetSelectedCycleGroupClients() ?? new List<string>();
			string selectedClient = this.CycleGroupClientsListBox.SelectedItem as string;

			this.CycleGroupClientsListBox.BeginUpdate();
			this.CycleGroupClientsListBox.Items.Clear();

			foreach (string client in clients)
			{
				this.CycleGroupClientsListBox.Items.Add(client);
			}

			if (selectedClient != null)
			{
				int index = this.CycleGroupClientsListBox.Items.IndexOf(selectedClient);
				if (index >= 0)
				{
					this.CycleGroupClientsListBox.SelectedIndex = index;
				}
			}

			this.CycleGroupClientsListBox.EndUpdate();
		}

		private void CycleGroupSelectCombo_SelectedIndexChanged_Handler(object sender, EventArgs e)
		{
			if (this._suppressEvents)
			{
				return;
			}

			this.RenderSelectedCycleGroup();
		}

		private void CycleGroupMoveUpButton_Click_Handler(object sender, EventArgs e)
		{
			this.MoveSelectedCycleGroupClient(-1);
		}

		private void CycleGroupMoveDownButton_Click_Handler(object sender, EventArgs e)
		{
			this.MoveSelectedCycleGroupClient(1);
		}

		// Characters of one account travel through the cycle order as one block: they are
		// added, removed and moved together. Only one of them can be online at a time, so
		// the cycle stops at the account once, whichever character is logged in
		private void MoveSelectedCycleGroupClient(int direction)
		{
			List<string> clients = this.GetSelectedCycleGroupClients();
			int index = this.CycleGroupClientsListBox.SelectedIndex;

			if ((clients == null) || (index < 0) || (index >= clients.Count))
			{
				return;
			}

			(int start, int count) = this.GetSelectedRange(clients, index);
			int neighborIndex = (direction < 0) ? (start - 1) : (start + count);

			if ((neighborIndex < 0) || (neighborIndex >= clients.Count))
			{
				return;
			}

			(int neighborStart, int neighborCount) = this.GetSelectedRange(clients, neighborIndex);

			List<string> block = clients.GetRange(start, count);
			List<string> neighbor = clients.GetRange(neighborStart, neighborCount);
			int selectionOffset = index - start;
			int newIndex;

			if (direction < 0)
			{
				clients.RemoveRange(neighborStart, neighborCount + count);
				clients.InsertRange(neighborStart, block);
				clients.InsertRange(neighborStart + count, neighbor);
				newIndex = neighborStart + selectionOffset;
			}
			else
			{
				clients.RemoveRange(start, count + neighborCount);
				clients.InsertRange(start, neighbor);
				clients.InsertRange(start + neighborCount, block);
				newIndex = start + neighborCount + selectionOffset;
			}

			this.RenderSelectedCycleGroup();
			this.CycleGroupClientsListBox.SelectedIndex = newIndex;

			this.CycleGroupClientsChanged?.Invoke(this.SelectedCycleGroupName, new List<string>(clients));
		}

		private void CycleGroupRemoveClientButton_Click_Handler(object sender, EventArgs e)
		{
			List<string> clients = this.GetSelectedCycleGroupClients();
			int index = this.CycleGroupClientsListBox.SelectedIndex;

			if ((clients == null) || (index < 0) || (index >= clients.Count))
			{
				return;
			}

			(int start, int count) = this.GetSelectedRange(clients, index);
			clients.RemoveRange(start, count);

			this.RenderSelectedCycleGroup();

			this.CycleGroupClientsChanged?.Invoke(this.SelectedCycleGroupName, new List<string>(clients));
		}

		private void CycleGroupAddClientButton_Click_Handler(object sender, EventArgs e)
		{
			string client = (this.CycleGroupAddClientCombo.SelectedItem as string)?.Trim();
			List<string> clients = this.GetSelectedCycleGroupClients();

			if (string.IsNullOrEmpty(client) || (clients == null))
			{
				return;
			}

			// The whole account joins the cycle order unless only the picked character is wanted
			List<string> block = this.CycleGroupWholeAccountCheckBox.Checked
									? this.GetAccountMembers(client)
									: new List<string> { client };
			bool added = false;

			foreach (string member in block)
			{
				if (clients.Contains(member, StringComparer.OrdinalIgnoreCase))
				{
					continue;
				}

				clients.Add(member);
				added = true;
			}

			if (!added)
			{
				return;
			}

			this.RenderSelectedCycleGroup();

			this.CycleGroupClientsChanged?.Invoke(this.SelectedCycleGroupName, new List<string>(clients));
		}

		/// <summary>Account of the character, null when it belongs to none</summary>
		private string GetAccountId(string title)
		{
			return this._characters.FirstOrDefault(character => string.Equals(character.Title, title, StringComparison.OrdinalIgnoreCase))?.GroupId;
		}

		/// <summary>The character itself, or every character of its account</summary>
		private List<string> GetAccountMembers(string title)
		{
			string accountId = this.GetAccountId(title);

			if (string.IsNullOrEmpty(accountId))
			{
				return new List<string> { title };
			}

			return this._characters.Where(character => string.Equals(character.GroupId, accountId, StringComparison.Ordinal))
									.Select(character => character.Title)
									.ToList();
		}

		/// <summary>
		/// The range the list operations work on: the whole account block, or just the
		/// single character when the account switch is off
		/// </summary>
		private (int Start, int Count) GetSelectedRange(List<string> clients, int index)
		{
			return this.CycleGroupWholeAccountCheckBox.Checked ? this.GetAccountBlock(clients, index) : (index, 1);
		}

		/// <summary>Range of the account block the item at this position belongs to</summary>
		private (int Start, int Count) GetAccountBlock(List<string> clients, int index)
		{
			string accountId = this.GetAccountId(clients[index]);

			if (string.IsNullOrEmpty(accountId))
			{
				return (index, 1);
			}

			int start = index;
			while ((start > 0) && string.Equals(this.GetAccountId(clients[start - 1]), accountId, StringComparison.Ordinal))
			{
				start--;
			}

			int end = index;
			while ((end < clients.Count - 1) && string.Equals(this.GetAccountId(clients[end + 1]), accountId, StringComparison.Ordinal))
			{
				end++;
			}

			return (start, end - start + 1);
		}

		private CharacterGroupViewItem FindAccount(string accountId)
		{
			return string.IsNullOrEmpty(accountId)
					? null
					: this._characterGroups.FirstOrDefault(group => string.Equals(group.Id, accountId, StringComparison.Ordinal));
		}

		/// <summary>
		/// Every character of an account carries its color and its name here, so that a
		/// block of three characters reads as one account and not as three names that
		/// happen to stand next to each other
		/// </summary>
		private void CycleGroupClientsListBox_DrawItem_Handler(object sender, DrawItemEventArgs e)
		{
			e.DrawBackground();

			if ((e.Index < 0) || (e.Index >= this.CycleGroupClientsListBox.Items.Count))
			{
				return;
			}

			string title = this.CycleGroupClientsListBox.Items[e.Index] as string;
			CharacterGroupViewItem account = this.FindAccount(this.GetAccountId(title));
			bool isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
			Rectangle bounds = e.Bounds;

			if (account != null)
			{
				using (Brush markerBrush = new SolidBrush(account.Color))
				{
					e.Graphics.FillRectangle(markerBrush, bounds.X + 2, bounds.Y + 1, 4, bounds.Height - 2);
				}
			}

			Rectangle textBounds = new Rectangle(bounds.X + 10, bounds.Y, bounds.Width - 12, bounds.Height);
			Color textColor = isSelected ? SystemColors.HighlightText : SystemColors.ControlText;

			TextRenderer.DrawText(e.Graphics, title, e.Font, textBounds, textColor,
									TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);

			if (account != null)
			{
				Size titleSize = TextRenderer.MeasureText(e.Graphics, title, e.Font);
				Rectangle accountBounds = new Rectangle(textBounds.X + titleSize.Width + 8, bounds.Y,
														textBounds.Width - titleSize.Width - 8, bounds.Height);

				if (accountBounds.Width > 20)
				{
					Color accountColor = isSelected ? SystemColors.HighlightText : account.Color;

					TextRenderer.DrawText(e.Graphics, account.Name, e.Font, accountBounds, accountColor,
											TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
				}
			}

			e.DrawFocusRectangle();
		}

		private void CycleGroupAddGroupButton_Click_Handler(object sender, EventArgs e)
		{
			this.CycleGroupAddRequested?.Invoke();
		}

		private void CycleGroupRemoveGroupButton_Click_Handler(object sender, EventArgs e)
		{
			string groupName = this.SelectedCycleGroupName;

			if (groupName == null)
			{
				return;
			}

			this.CycleGroupRemoveRequested?.Invoke(groupName);
		}

		private void ThumbnailsList_SelectedIndexChanged_Handler(object sender, EventArgs e)
		{
			this.RefreshSelectedThumbnailCycleGroup();
		}

		private void RefreshSelectedThumbnailCycleGroup()
		{
			string title = (this.ThumbnailsList.SelectedItem as IThumbnailDescription)?.Title;

			bool suppressed = this._suppressEvents;
			this._suppressEvents = true;

			string groupName = null;
			if ((title != null) && this._clientCycleGroups.TryGetValue(title, out IList<string> groups) && (groups.Count > 0))
			{
				groupName = groups[0];
			}

			this.ClientCycleGroupCombo.SelectedIndex = Math.Max(0, this.ClientCycleGroupCombo.Items.IndexOf(groupName ?? Strings.Clients_NoCycleGroup));
			this.ClientCycleGroupCombo.Enabled = title != null;

			this._suppressEvents = suppressed;
		}

		private void ClientCycleGroupCombo_SelectedIndexChanged_Handler(object sender, EventArgs e)
		{
			if (this._suppressEvents)
			{
				return;
			}

			string title = (this.ThumbnailsList.SelectedItem as IThumbnailDescription)?.Title;

			if (title == null)
			{
				return;
			}

			string groupName = this.ClientCycleGroupCombo.SelectedIndex > 0 ? this.ClientCycleGroupCombo.SelectedItem as string : null;

			this.ThumbnailCycleGroupChanged?.Invoke(title, groupName);
		}

		private void HotkeyBindingsListViewResize_Handler(object sender, EventArgs e)
		{
			// Let the 'Action' column consume the free width
			this.HotkeyActionColumnHeader.Width = Math.Max(120, this.HotkeyBindingsListView.ClientSize.Width - this.HotkeyKeyColumnHeader.Width);
		}
		#endregion

		/// <summary>
		/// Replaces the English captions the designer assigns with the ones for the current
		/// UI culture. Controls are matched by name, so the designer file stays the single
		/// place where the layout is defined
		/// </summary>
		private void ApplyLocalization()
		{
			Dictionary<string, string> texts = new Dictionary<string, string>(StringComparer.Ordinal)
			{
				["GeneralTabPage"] = Strings.Tab_General,
				["ThumbnailTabPage"] = Strings.Tab_Previews,
				["OverlayTabPage"] = Strings.Tab_Overlay,
				["ClientWindowsTabPage"] = Strings.Tab_ClientWindows,
				["ClientsTabPage"] = Strings.Tab_Clients,
				["ClientsActiveSubPage"] = Strings.Tab_ActiveClients,
				["CharactersSubPage"] = Strings.Tab_Characters,
				["HotkeyBindingsSubPage"] = Strings.Tab_HotkeyBindings,
				["PreviewClicksSubPage"] = Strings.Tab_PreviewClicks,
				["CharacterFilterLabel"] = Strings.Characters_Filter,
				["CharacterGroupLabel"] = Strings.Characters_Group,
				["CharacterManageAsWholeCheckBox"] = Strings.Characters_ManageAsWhole,
				["CharacterForgetButton"] = Strings.Characters_Forget,
				["CharacterPreviewSettingsButton"] = Strings.Characters_PreviewSettings,
				["CharacterSuggestGroupsButton"] = Strings.Characters_SuggestGroups,
				["CycleGroupWholeAccountCheckBox"] = Strings.CycleGroups_WholeAccount,
				["CycleGroupsTabPage"] = Strings.Tab_CycleGroups,
				["HotkeysTabPage"] = Strings.Tab_Hotkeys,
				["GameLogsTabPage"] = Strings.Tab_GameLogs,
				["AboutTabPage"] = Strings.Tab_About,

				["MinimizeToTrayCheckBox"] = Strings.General_MinimizeToTray,
				["LanguageLabel"] = Strings.General_Language,
				["LanguageRestartHintLabel"] = Strings.General_LanguageRestartHint,

				["EnableClientLayoutTrackingCheckBox"] = Strings.ClientWindows_TrackLocations,
				["MinimizeInactiveClientsCheckBox"] = Strings.ClientWindows_MinimizeInactive,
				["HideCaptionOnClientsCheckBox"] = Strings.ClientWindows_HideCaption,
				["DisableAnimationCheckBox"] = Strings.ClientWindows_DisableAnimation,
				["MinimizedRenderingNoteLabel"] = Strings.ClientWindows_MinimizedRenderingNote,

				["PreviewGeneralSubPage"] = Strings.PreviewTab_General,
				["PreviewVisualSubPage"] = Strings.PreviewTab_Visualization,
				["PreviewRenderingSubPage"] = Strings.PreviewTab_Rendering,
				["PreviewLayoutSubPage"] = Strings.PreviewTab_Layout,
				["PreviewZoomSubPage"] = Strings.PreviewTab_Zoom,
				["EnableBackgroundRenderingCheckBox"] = Strings.Preview_EnableBackgroundRendering,
				["ThumbnailRefreshPeriodLabel"] = Strings.Preview_RefreshPeriod,
				["ThumbnailRefreshHintLabel"] = Strings.Preview_RefreshPeriodHint,
				["MinimizedRefreshPeriodLabel"] = Strings.Preview_MinimizedRefreshPeriod,
				["MinimizedRefreshHintLabel"] = Strings.Preview_MinimizedRefreshHint,
				["AlwaysOnTopNoteLabel"] = Strings.Preview_AlwaysOnTopHint,
				["LockLocationNoteLabel"] = Strings.Preview_LockLocationHint,
				["ShowGridCheckBox"] = Strings.Preview_ShowGrid,
				["SnapFillCellCheckBox"] = Strings.Preview_SnapFillCell,
				["GridOffsetXLabel"] = Strings.Preview_GridOffsetX,
				["GridOffsetYLabel"] = Strings.Preview_GridOffsetY,
				["SnapPaddingLabel"] = Strings.Preview_GridCellPadding,
				["PerClientLayoutNoteLabel"] = Strings.Preview_PerClientLayoutsHint,
				["HideActiveClientThumbnailCheckBox"] = Strings.Preview_HideActiveClient,
				["ShowThumbnailsAlwaysOnTopCheckBox"] = Strings.Preview_AlwaysOnTop,
				["HideThumbnailsOnLostFocusCheckBox"] = Strings.Preview_HideOnLostFocus,
				["OpacityLabel"] = Strings.Preview_Opacity,
				["PreventPreviewsCheckBox"] = Strings.Preview_PreventPreviews,
				["PreventPreviewColorLabel"] = Strings.Preview_PlaceholderColor,
				["WidthLabel"] = Strings.Preview_Width,
				["HeightLabel"] = Strings.Preview_Height,
				["LockThumbnailLocationCheckbox"] = Strings.Preview_LockLocation,
				["EnablePerClientThumbnailsLayoutsCheckBox"] = Strings.Preview_PerClientLayouts,
				["ThumbnailSnapToGridCheckBox"] = Strings.Preview_SnapToGrid,
				["SnapXLabel"] = Strings.Preview_GridStepX,
				["SnapYLabel"] = Strings.Preview_GridStepY,
				["EnableThumbnailZoomCheckBox"] = Strings.Preview_ZoomOnHover,
				["ZoomFactorLabel"] = Strings.Preview_ZoomFactor,
				["ZoomAnchorLabel"] = Strings.Preview_ZoomAnchor,

				["OverlayGeneralSubPage"] = Strings.OverlayTab_General,
				["OverlayWindowNameSubPage"] = Strings.OverlayTab_WindowName,
				["OverlayGroupNameSubPage"] = Strings.OverlayTab_GroupName,
				["OverlayBorderSubPage"] = Strings.OverlayTab_Border,
				["ShowThumbnailOverlaysCheckBox"] = Strings.Overlay_ShowOverlay,
				["ShowThumbnailFramesCheckBox"] = Strings.Overlay_ShowFrames,
				["OverlayAlwaysOnTopCheckBox"] = Strings.Overlay_AlwaysOnTop,
				["ShowClientNameCheckBox"] = Strings.Overlay_ShowWindowName,
				["OverlayLabelColorLabel"] = Strings.Overlay_Color,
				["OverlayLabelPositionLabel"] = Strings.Overlay_Position,
				["btnLabelFont"] = Strings.Overlay_Font,
				["LabelOverlayLabelFont"] = Strings.Overlay_WindowNameSample,
				["LabelOutlineCheckBox"] = Strings.Overlay_Outline,
				["LabelOutlineThicknessLabel"] = Strings.Overlay_BorderThickness,
				["GroupNameOutlineCheckBox"] = Strings.Overlay_Outline,
				["GroupNameOutlineThicknessLabel"] = Strings.Overlay_BorderThickness,
				["ShowCycleGroupNameCheckBox"] = Strings.Overlay_ShowGroupName,
				["CycleGroupNameColorLabel"] = Strings.Overlay_Color,
				["CycleGroupNamePositionLabel"] = Strings.Overlay_Position,
				["btnCycleGroupNameFont"] = Strings.Overlay_Font,
				["LabelCycleGroupNameFont"] = Strings.Overlay_GroupNameSample,
				["EnableActiveClientHighlightCheckBox"] = Strings.Overlay_HighlightActiveClient,
				["HighlightColorLabel"] = Strings.Overlay_Color,
				["ActiveFrameThicknessLabel"] = Strings.Overlay_BorderThickness,

				["EnableGameLogMonitorCheckBox"] = Strings.GameLogs_Enable,
				["GameLogsNoteLabel"] = Strings.GameLogs_Note,
				["GameLogsFolderLabel"] = Strings.GameLogs_Folder,
				["GameLogsFolderBrowseButton"] = Strings.GameLogs_Browse,
				["GameLogsFolderHintLabel"] = Strings.GameLogs_FolderHint,

				["OverlayAggroSubPage"] = Strings.OverlayTab_Aggro,
				["EnableAggroFramesCheckBox"] = Strings.Aggro_Enable,
				["AggroYellowColorLabel"] = Strings.Aggro_YellowColor,
				["AggroRedColorLabel"] = Strings.Aggro_RedColor,
				["AggroFillPercentLabel"] = Strings.Aggro_FillPercent,
				["AggroFillHintLabel"] = Strings.Aggro_FillHint,
				["AggroTestButton"] = Strings.Aggro_Test,
				["AggroDisabledLabel"] = Strings.Aggro_LogsDisabledWarning,
				["AggroGoToLogsButton"] = Strings.Aggro_OpenLogSettings,

				["ThumbnailsListLabel"] = Strings.Clients_ListLabel,
				["ClientCycleGroupLabel"] = Strings.Clients_CycleGroup,

				["CycleGroupSelectLabel"] = Strings.CycleGroups_Group,
				["CycleGroupClientsLabel"] = Strings.CycleGroups_Clients,
				["CycleGroupMoveUpButton"] = Strings.CycleGroups_MoveUp,
				["CycleGroupMoveDownButton"] = Strings.CycleGroups_MoveDown,
				["CycleGroupRemoveClientButton"] = Strings.CycleGroups_RemoveClient,
				["CycleGroupAddClientLabel"] = Strings.CycleGroups_AddClient,
				["CycleGroupAddClientButton"] = Strings.CycleGroups_AddClient,

				["AddHotkeyButton"] = Strings.Hotkeys_Add,
				["EditHotkeyButton"] = Strings.Hotkeys_Edit,
				["RemoveHotkeyButton"] = Strings.Hotkeys_Remove,

				["DescriptionLabel"] = Strings.About_Description,
				["CreditMaintLabel"] = Strings.About_Credit,
				["DocumentationLinkLabel"] = Strings.About_ForumHint
			};

			MainForm.ApplyTexts(this, texts);

			this.HotkeyActionColumnHeader.Text = Strings.Hotkeys_ColumnAction;
			this.HotkeyKeyColumnHeader.Text = Strings.Hotkeys_ColumnHotkey;

			foreach (ToolStripItem item in this.TrayMenu.Items)
			{
				switch (item.Name)
				{
					case "RestoreWindowMenuItem":
						item.Text = Strings.Tray_Restore;
						break;
					case "ExitMenuItem":
						item.Text = Strings.Tray_Exit;
						break;
				}
			}
		}

		private static void ApplyTexts(Control root, IDictionary<string, string> texts)
		{
			foreach (Control control in root.Controls)
			{
				if (texts.TryGetValue(control.Name, out string text))
				{
					control.Text = text;
				}

				MainForm.ApplyTexts(control, texts);
			}
		}

		private void InitLanguages()
		{
			this._languages.Add(LanguageManager.SYSTEM_LANGUAGE);

			foreach (string language in LanguageManager.SupportedLanguages)
			{
				this._languages.Add(language);
			}

			this.LanguageCombo.BeginUpdate();
			foreach (string language in this._languages)
			{
				this.LanguageCombo.Items.Add(LanguageManager.GetDisplayName(language));
			}
			this.LanguageCombo.EndUpdate();

			this.LanguageCombo.SelectedIndex = 0;
		}

		private void InitZoomAnchorMap()
		{
			this._zoomAnchorMap[ViewZoomAnchor.NW] = this.ZoomAnchorNWRadioButton;
			this._zoomAnchorMap[ViewZoomAnchor.N] = this.ZoomAnchorNRadioButton;
			this._zoomAnchorMap[ViewZoomAnchor.NE] = this.ZoomAnchorNERadioButton;
			this._zoomAnchorMap[ViewZoomAnchor.W] = this.ZoomAnchorWRadioButton;
			this._zoomAnchorMap[ViewZoomAnchor.C] = this.ZoomAnchorCRadioButton;
			this._zoomAnchorMap[ViewZoomAnchor.E] = this.ZoomAnchorERadioButton;
			this._zoomAnchorMap[ViewZoomAnchor.SW] = this.ZoomAnchorSWRadioButton;
			this._zoomAnchorMap[ViewZoomAnchor.S] = this.ZoomAnchorSRadioButton;
			this._zoomAnchorMap[ViewZoomAnchor.SE] = this.ZoomAnchorSERadioButton;
		}
		private void InitOverlayLabelMap()
		{
			this._overlayLabelMap[ViewZoomAnchor.NW] = this.OverlayLabelNWRadioButton;
			this._overlayLabelMap[ViewZoomAnchor.N] = this.OverlayLabelNRadioButton;
			this._overlayLabelMap[ViewZoomAnchor.NE] = this.OverlayLabelNERadioButton;
			this._overlayLabelMap[ViewZoomAnchor.W] = this.OverlayLabelWRadioButton;
			this._overlayLabelMap[ViewZoomAnchor.C] = this.OverlayLabelCRadioButton;
			this._overlayLabelMap[ViewZoomAnchor.E] = this.OverlayLabelERadioButton;
			this._overlayLabelMap[ViewZoomAnchor.SW] = this.OverlayLabelSWRadioButton;
			this._overlayLabelMap[ViewZoomAnchor.S] = this.OverlayLabelSRadioButton;
			this._overlayLabelMap[ViewZoomAnchor.SE] = this.OverlayLabelSERadioButton;
		}
		private void InitCycleGroupIndicatorMap()
		{
			this._cycleGroupIndicatorMap[ViewZoomAnchor.NW] = this.CycleGroupIndicatorNWRadioButton;
			this._cycleGroupIndicatorMap[ViewZoomAnchor.N] = this.CycleGroupIndicatorNRadioButton;
			this._cycleGroupIndicatorMap[ViewZoomAnchor.NE] = this.CycleGroupIndicatorNERadioButton;
			this._cycleGroupIndicatorMap[ViewZoomAnchor.W] = this.CycleGroupIndicatorWRadioButton;
			this._cycleGroupIndicatorMap[ViewZoomAnchor.C] = this.CycleGroupIndicatorCRadioButton;
			this._cycleGroupIndicatorMap[ViewZoomAnchor.E] = this.CycleGroupIndicatorERadioButton;
			this._cycleGroupIndicatorMap[ViewZoomAnchor.SW] = this.CycleGroupIndicatorSWRadioButton;
			this._cycleGroupIndicatorMap[ViewZoomAnchor.S] = this.CycleGroupIndicatorSRadioButton;
			this._cycleGroupIndicatorMap[ViewZoomAnchor.SE] = this.CycleGroupIndicatorSERadioButton;
		}

		// The content area has no frame of its own; a single line separates it from the tab strip
		private void InitTabSeparator()
		{
			TabControl tabControl = (TabControl)this.Controls.Find("ContentTabControl", false).First();

			foreach (TabPage page in tabControl.TabPages)
			{
				page.Paint += this.TabPage_Paint_Handler;
			}
		}

		private void TabPage_Paint_Handler(object sender, PaintEventArgs e)
		{
			TabPage page = (TabPage)sender;

			using (Pen pen = new Pen(SystemColors.ControlDark))
			{
				e.Graphics.DrawLine(pen, 0, 0, 0, page.Height);
			}
		}

		private void InitFormSize()
		{
			const int BUFFER_PIXEL_AMOUNT = 8;
			// resize form height based on tabbed control item height
			var tabControl = (System.Windows.Forms.TabControl)this.Controls.Find("ContentTabControl", false).First();
			if (tabControl != null)
			{
				var furnitureSize = this.Height - tabControl.Height;
				var calculatedHeight = (tabControl.ItemSize.Width * tabControl.Controls.Count) + furnitureSize + BUFFER_PIXEL_AMOUNT;
				if (this.Height < calculatedHeight)
				{
					this.Height = calculatedHeight;
				}
			}

			// The form is resizable but should not shrink below the initial layout-safe size
			this.MinimumSize = this.Size;
		}

		private void btnLabelFont_Click(object sender, EventArgs e)
		{
			using (FontDialog fontSelector = new FontDialog { Font = this.OverlayLabelFont, ShowColor = false, ShowApply = false, ShowHelp = false })
			{
				if (this.ShowModalDialog(fontSelector) == DialogResult.Cancel)
				{
					return;
				}

				this.OverlayLabelFont = fontSelector.Font;
			}

			this.OptionChanged_Handler(sender, e);
		}

		private void btnCycleGroupNameFont_Click(object sender, EventArgs e)
		{
			using (FontDialog fontSelector = new FontDialog { Font = this.CycleGroupNameFont, ShowColor = false, ShowApply = false, ShowHelp = false })
			{
				if (this.ShowModalDialog(fontSelector) == DialogResult.Cancel)
				{
					return;
				}

				this.CycleGroupNameFont = fontSelector.Font;
			}

			this.OptionChanged_Handler(sender, e);
		}

		private void CycleGroupNameColorButton_Click(object sender, EventArgs e)
		{
			using (ColorDialog dialog = new ColorDialog { Color = this.CycleGroupNameColor })
			{
				if (this.ShowModalDialog(dialog) != DialogResult.OK)
				{
					return;
				}

				this.CycleGroupNameColor = dialog.Color;
			}

			this.OptionChanged_Handler(sender, e);
		}

		private void PreventPreviewColorButton_Click(object sender, EventArgs e)
		{
			using (ColorDialog dialog = new ColorDialog())
			{
				dialog.Color = this.PreventPreviewColor;

				if (this.ShowModalDialog(dialog) != DialogResult.OK)
				{
					return;
				}

				this.PreventPreviewColor = dialog.Color;
			}

			this.OptionChanged_Handler(sender, e);

		}
	}
}