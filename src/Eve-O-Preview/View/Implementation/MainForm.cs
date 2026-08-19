using EveOPreview.Configuration;
using EveOPreview.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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
		private List<(string ActionId, string DisplayName)> _hotkeyActions;
		private List<(string ActionId, string ActionName, string Hotkey)> _hotkeyBindings;
		private List<string> _activeClients;
		private Point? _thumbnailsListClickLocation;
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
			this._hotkeyActions = new List<(string ActionId, string DisplayName)>();
			this._hotkeyBindings = new List<(string ActionId, string ActionName, string Hotkey)>();
			this._activeClients = new List<string>();

			InitializeComponent();

			this.InitTabSeparator();

			this.ThumbnailsList.DisplayMember = "Title";
			this.ThumbnailsList.Format += this.ThumbnailsList_Format_Handler;
			this.ThumbnailsList.MouseDown += this.ThumbnailsList_MouseDown_Handler;
			this.ThumbnailsList.SelectedIndexChanged += this.ThumbnailsList_SelectedIndexChanged_Handler;
			this.ClientCycleGroupCombo.Enabled = false;

			this.HotkeyBindingsListView.ClientSizeChanged += this.HotkeyBindingsListViewResize_Handler;
			this.HotkeyBindingsListView.DoubleClick += this.HotkeyBindingsListView_DoubleClick_Handler;
			this.HotkeyBindingsListViewResize_Handler(this.HotkeyBindingsListView, EventArgs.Empty);

			this.ResizeEnd += this.MainFormResizeEnd_Handler;

			this.InitZoomAnchorMap();
			this.InitOverlayLabelMap();
			this.InitCycleGroupIndicatorMap();
			this.InitFormSize();

			this.AnimationStyleCombo.DataSource = Enum.GetValues(typeof(AnimationStyle));
		}

		public bool MinimizeToTray
		{
			get => this.MinimizeToTrayCheckBox.Checked;
			set => this.MinimizeToTrayCheckBox.Checked = value;
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
				catch (Exception ex)
				{
					// Log ?
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
			set => this.MinimizeInactiveClientsCheckBox.Checked = value;
		}
		public bool HideCaptionOnClients
		{
			get => this.HideCaptionOnClientsCheckBox.Checked;
			set => this.HideCaptionOnClientsCheckBox.Checked = value;
		}
		public ViewAnimationStyle WindowsAnimationStyle
		{
			get => (ViewAnimationStyle)this.AnimationStyleCombo.SelectedItem;
			set => this.AnimationStyleCombo.SelectedIndex = (int)value;
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
			set => this.ThumbnailSnapToGridCheckBox.Checked = value;
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

		public void SetHotkeyBindings(IList<(string ActionId, string ActionName, string Hotkey)> bindings)
		{
			this._hotkeyBindings = new List<(string ActionId, string ActionName, string Hotkey)>(bindings);

			this.HotkeyBindingsListView.BeginUpdate();
			this.HotkeyBindingsListView.Items.Clear();

			foreach ((string actionId, string actionName, string hotkey) in bindings)
			{
				ListViewItem item = new ListViewItem(new[] { actionName, hotkey });
				item.Tag = (actionId, hotkey);
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
			this.ClientCycleGroupCombo.Items.Add("None");
			foreach (string name in this._cycleGroupNames)
			{
				this.ClientCycleGroupCombo.Items.Add(name);
			}
			this.ClientCycleGroupCombo.SelectedIndex = Math.Max(0, this.ClientCycleGroupCombo.Items.IndexOf(selectedClientGroup ?? "None"));
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

		public Action ApplicationExitRequested { get; set; }

		public Action FormActivated { get; set; }

		public Action FormMinimized { get; set; }

		public Action<ViewCloseRequest> FormCloseRequested { get; set; }

		public Action ApplicationSettingsChanged { get; set; }

		public Action ThumbnailsSizeChanged { get; set; }

		public Action<string> ThumbnailStateChanged { get; set; }

		public Action DocumentationLinkActivated { get; set; }

		public Action<string, string> HotkeyBindingAssigned { get; set; }

		public Action<string, string> HotkeyBindingRemoved { get; set; }

		public Action<string, string, string, string> HotkeyBindingEdited { get; set; }

		public Action<string, IList<string>> CycleGroupClientsChanged { get; set; }

		public Action<string, string> ThumbnailCycleGroupChanged { get; set; }

		public Action CycleGroupAddRequested { get; set; }

		public Action<string> CycleGroupRemoveRequested { get; set; }

		public Action<string, string> CycleGroupRenameRequested { get; set; }

		public Action<bool> HotkeyCaptureModeChanged { get; set; }

		public Action WindowSizeChanged { get; set; }

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

				this.HotkeyBindingAssigned?.Invoke(dialog.SelectedActionId, dialog.HotkeyString);
			}
		}

		private void EditHotkeyButton_Click_Handler(object sender, EventArgs e)
		{
			if (this.HotkeyBindingsListView.SelectedItems.Count == 0)
			{
				this.SetHotkeyStatus("Select a binding in the list first");
				return;
			}

			(string actionId, string hotkey) = ((string, string))this.HotkeyBindingsListView.SelectedItems[0].Tag;

			using (HotkeyEditDialog dialog = new HotkeyEditDialog(this._hotkeyActions, this._activeClients, this._hotkeyBindings, actionId, hotkey))
			{
				if (this.ShowHotkeyDialog(dialog) != DialogResult.OK)
				{
					return;
				}

				this.HotkeyBindingEdited?.Invoke(actionId, hotkey, dialog.SelectedActionId, dialog.HotkeyString);
			}
		}

		private void CycleGroupRenameButton_Click_Handler(object sender, EventArgs e)
		{
			string groupName = this.SelectedCycleGroupName;

			if (groupName == null)
			{
				return;
			}

			using (TextPromptDialog dialog = new TextPromptDialog("Rename cycle group", "Group name", groupName))
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
				this.SetHotkeyStatus("Select a binding in the list first");
				return;
			}

			(string actionId, string hotkey) = ((string, string))this.HotkeyBindingsListView.SelectedItems[0].Tag;

			this.HotkeyBindingRemoved?.Invoke(actionId, hotkey);
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

		private void MoveSelectedCycleGroupClient(int direction)
		{
			List<string> clients = this.GetSelectedCycleGroupClients();
			int index = this.CycleGroupClientsListBox.SelectedIndex;
			int newIndex = index + direction;

			if ((clients == null) || (index < 0) || (newIndex < 0) || (newIndex >= clients.Count))
			{
				return;
			}

			(clients[index], clients[newIndex]) = (clients[newIndex], clients[index]);

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

			clients.RemoveAt(index);

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

			if (clients.Contains(client, StringComparer.OrdinalIgnoreCase))
			{
				return;
			}

			clients.Add(client);

			this.RenderSelectedCycleGroup();

			this.CycleGroupClientsChanged?.Invoke(this.SelectedCycleGroupName, new List<string>(clients));
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

			this.ClientCycleGroupCombo.SelectedIndex = Math.Max(0, this.ClientCycleGroupCombo.Items.IndexOf(groupName ?? "None"));
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

		private void InitZoomAnchorMap()
		{
			this._zoomAnchorMap[ViewZoomAnchor.NW] = this.ZoomAanchorNWRadioButton;
			this._zoomAnchorMap[ViewZoomAnchor.N] = this.ZoomAanchorNRadioButton;
			this._zoomAnchorMap[ViewZoomAnchor.NE] = this.ZoomAanchorNERadioButton;
			this._zoomAnchorMap[ViewZoomAnchor.W] = this.ZoomAanchorWRadioButton;
			this._zoomAnchorMap[ViewZoomAnchor.C] = this.ZoomAanchorCRadioButton;
			this._zoomAnchorMap[ViewZoomAnchor.E] = this.ZoomAanchorERadioButton;
			this._zoomAnchorMap[ViewZoomAnchor.SW] = this.ZoomAanchorSWRadioButton;
			this._zoomAnchorMap[ViewZoomAnchor.S] = this.ZoomAanchorSRadioButton;
			this._zoomAnchorMap[ViewZoomAnchor.SE] = this.ZoomAanchorSERadioButton;
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