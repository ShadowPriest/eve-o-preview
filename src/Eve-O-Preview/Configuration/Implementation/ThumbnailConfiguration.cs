using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using EveOPreview.Localization;
using Newtonsoft.Json;

namespace EveOPreview.Configuration.Implementation
{
	sealed class ThumbnailConfiguration : IThumbnailConfiguration
	{
		#region Private fields
		private bool _enablePerClientThumbnailLayouts;
		private bool _enableClientLayoutTracking;
		#endregion

		public ThumbnailConfiguration()
		{
			this.ConfigVersion = 1;

			// Cycle groups stay null here: they are either read from the config file,
			// migrated from the legacy CycleGroupN* entries or defaulted in ApplyRestrictions()
			this.CycleGroups = null;

			this.PerClientActiveClientHighlightColor = new Dictionary<string, Color>
			{
				{"EVE - Example Toon 1", Color.Red},
				{"EVE - Example Toon 2", Color.Green}
			};
			this.PerClientPreventPreviewColor = new Dictionary<string, Color>
			{
				{"EVE - Example Toon 1", Color.Red},
				{"EVE - Example Toon 2", Color.Green}
			};
			this.PerClientPreventPreviews = new Dictionary<string, bool>
			{
				{"EVE - Example Toon 1", false},
				{"EVE - Example Toon 2", true}
			};

			this.PerClientThumbnailSize = new Dictionary<string, Size>
			{
				{"EVE - Example Toon 1", new Size(200, 200)},
				{"EVE - Example Toon 2", new Size(200, 200)}
			};

			this.PerClientZoomAnchor = new Dictionary<string, ZoomAnchor>
			{
				{"EVE - Example Toon 1", ZoomAnchor.N },
				{"EVE - Example Toon 2", ZoomAnchor.S}
			};

			this.PerClientLayout = new Dictionary<string, Dictionary<string, Point>>();
			this.FlatLayout = new Dictionary<string, Point>();
			this.ClientLayout = new Dictionary<string, ClientLayout>();
			this.ClientHotkey = new Dictionary<string, string>();
			this.MinimizeAllClientsHotkeys = new List<string> { "Control+F22" };
			this.ToggleAllPreviewsHotkeys = new List<string>();
			this.ClickThroughHotkeys = new List<string>();
			this.DisableThumbnail = new Dictionary<string, bool>();
			this.PriorityClients = new List<string>();

			this.ExecutablesToPreview = new List<string> { "exefile" };

			this.MinimizeToTray = false;
			this.ThumbnailRefreshPeriod = 500;
			this.ThumbnailResizeTimeoutPeriod = 500;
			this.EnableMinimizedClientsRefresh = true;
			this.MinimizedClientsRefreshPeriod = 5;

#if LINUX
			this.EnableWineCompatibilityMode = true;
#else
			this.EnableWineCompatibilityMode = false;
#endif

			this.ThumbnailOpacity = 0.5;

			this.EnableClientLayoutTracking = false;
			this.HideActiveClientThumbnail = false;
			this.HideLoginClientThumbnail = false;
			this.MinimizeInactiveClients = false;
			this.HideCaptionOnClients = false;
			this.WindowsAnimationStyle = AnimationStyle.NoAnimation;
			this.ShowThumbnailsAlwaysOnTop = true;
			this.EnablePerClientThumbnailLayouts = false;

			this.HideThumbnailsOnLostFocus = false;
			this.PreventPreviews = false;
			this.HideThumbnailsDelay = 2; // 2 thumbnails refresh cycles (1.0 sec)

			this.ThumbnailSize = new Size(384, 216);
			this.ThumbnailMinimumSize = new Size(192, 108);
			this.ThumbnailMaximumSize = new Size(960, 540);

			this.EnableThumbnailSnap = true;

			this.ThumbnailZoomEnabled = false;
			this.ThumbnailZoomFactor = 2;
			this.ThumbnailZoomAnchor = ZoomAnchor.NW;
			this.OverlayLabelAnchor = ZoomAnchor.NW;
			this.CycleGroupIndicatorAnchor = ZoomAnchor.NW;

			this.ShowThumbnailOverlays = true;
			this.OverlayLabelOutlineEnabled = true;
			this.OverlayLabelOutlineThickness = 1;
			this.OverlayLabelOutlineColor = Color.Black;
			this.CycleGroupNameOutlineEnabled = true;
			this.CycleGroupNameOutlineThickness = 1;
			this.CycleGroupNameOutlineColor = Color.Black;
			this.ShowClientName = true;
			this.ShowCycleGroupName = false;
			this.OverlayAlwaysOnTop = true;
			this.ShowThumbnailFrames = false;

			this.CycleGroupNameColor = Color.Orange;
			this.CycleGroupNameFont = new Font(FontFamily.GenericSansSerif, 10.0F, FontStyle.Bold);
			this.LockThumbnailLocation = false;

			this.ThumbnailSnapToGrid = true;
			this.ThumbnailSnapToGridFillCell = false;
			this.ThumbnailSnapToGridOffsetX = 0;
			this.ThumbnailSnapToGridOffsetY = 0;
			this.ThumbnailSnapToGridCellPadding = 2;
			this.ThumbnailSnapToGridSizeX = 100;
			this.ThumbnailSnapToGridSizeY = 50;

            this.EnableActiveClientHighlight = false;
			this.ActiveClientHighlightColor = Color.GreenYellow;
			this.PreventPreviewColor = Color.Purple;
			this.ActiveClientHighlightThickness = 3;

			this.OverlayLabelColor = Color.Orange;
			this.OverlayLabelFont = new Font(FontFamily.GenericSansSerif,10.0F, FontStyle.Bold);

			this.IconName = "";
			this.Language = LanguageManager.SYSTEM_LANGUAGE;

			this.LoginThumbnailLocation = new Point(5, 5);

			this.MainWindowSize = Size.Empty;
		}


		[JsonProperty("ConfigVersion")]
		public int ConfigVersion { get; set; }

		[JsonIgnore]
		public Dictionary<string, bool> CycleGroupExclusions { get; set; }

		[JsonProperty("CycleGroups")]
		public List<CycleGroup> CycleGroups { get; set; }

		#region Legacy fixed cycle group entries (read for migration, never written back)
		[JsonProperty("CycleGroup1ForwardHotkeys")]
		public List<string> CycleGroup1ForwardHotkeys { get; set; }
		public bool ShouldSerializeCycleGroup1ForwardHotkeys() => false;

		[JsonProperty("CycleGroup1BackwardHotkeys")]
		public List<string> CycleGroup1BackwardHotkeys { get; set; }
		public bool ShouldSerializeCycleGroup1BackwardHotkeys() => false;

		[JsonProperty("CycleGroup1ClientsOrder")]
		public Dictionary<string, int> CycleGroup1ClientsOrder { get; set; }
		public bool ShouldSerializeCycleGroup1ClientsOrder() => false;

		[JsonProperty("CycleGroup2ForwardHotkeys")]
		public List<string> CycleGroup2ForwardHotkeys { get; set; }
		public bool ShouldSerializeCycleGroup2ForwardHotkeys() => false;

		[JsonProperty("CycleGroup2BackwardHotkeys")]
		public List<string> CycleGroup2BackwardHotkeys { get; set; }
		public bool ShouldSerializeCycleGroup2BackwardHotkeys() => false;

		[JsonProperty("CycleGroup2ClientsOrder")]
		public Dictionary<string, int> CycleGroup2ClientsOrder { get; set; }
		public bool ShouldSerializeCycleGroup2ClientsOrder() => false;

		[JsonProperty("CycleGroup3ForwardHotkeys")]
		public List<string> CycleGroup3ForwardHotkeys { get; set; }
		public bool ShouldSerializeCycleGroup3ForwardHotkeys() => false;

		[JsonProperty("CycleGroup3BackwardHotkeys")]
		public List<string> CycleGroup3BackwardHotkeys { get; set; }
		public bool ShouldSerializeCycleGroup3BackwardHotkeys() => false;

		[JsonProperty("CycleGroup3ClientsOrder")]
		public Dictionary<string, int> CycleGroup3ClientsOrder { get; set; }
		public bool ShouldSerializeCycleGroup3ClientsOrder() => false;

		[JsonProperty("CycleGroup4ForwardHotkeys")]
		public List<string> CycleGroup4ForwardHotkeys { get; set; }
		public bool ShouldSerializeCycleGroup4ForwardHotkeys() => false;

		[JsonProperty("CycleGroup4BackwardHotkeys")]
		public List<string> CycleGroup4BackwardHotkeys { get; set; }
		public bool ShouldSerializeCycleGroup4BackwardHotkeys() => false;

		[JsonProperty("CycleGroup4ClientsOrder")]
		public Dictionary<string, int> CycleGroup4ClientsOrder { get; set; }
		public bool ShouldSerializeCycleGroup4ClientsOrder() => false;

		[JsonProperty("CycleGroup5ForwardHotkeys")]
		public List<string> CycleGroup5ForwardHotkeys { get; set; }
		public bool ShouldSerializeCycleGroup5ForwardHotkeys() => false;

		[JsonProperty("CycleGroup5BackwardHotkeys")]
		public List<string> CycleGroup5BackwardHotkeys { get; set; }
		public bool ShouldSerializeCycleGroup5BackwardHotkeys() => false;

		[JsonProperty("CycleGroup5ClientsOrder")]
		public Dictionary<string, int> CycleGroup5ClientsOrder { get; set; }
		public bool ShouldSerializeCycleGroup5ClientsOrder() => false;
		#endregion

		[JsonProperty("PerClientPreventPreviewColor")]
		public Dictionary<string, Color> PerClientPreventPreviewColor { get; set; }

		[JsonProperty("PerClientActiveClientHighlightColor")]
		public Dictionary<string, Color> PerClientActiveClientHighlightColor { get; set; }

		[JsonProperty("PerClientPreventPreviews")]
		public Dictionary<string, bool> PerClientPreventPreviews { get; set; }

		[JsonProperty("PerClientThumbnailSize")]
		public Dictionary<string, Size> PerClientThumbnailSize { get; set; }

		[JsonProperty("PerClientZoomAnchor")]
		public Dictionary<string, ZoomAnchor> PerClientZoomAnchor{ get; set; }
		public bool MinimizeToTray { get; set; }
		public int ThumbnailRefreshPeriod { get; set; }
		public int ThumbnailResizeTimeoutPeriod { get; set; }

		/// <summary>Master switch for the background refresh of minimized clients</summary>
		public bool EnableMinimizedClientsRefresh { get; set; }

		/// <summary>
		/// How often (in seconds) a minimized client is briefly woken up so that its
		/// thumbnail shows fresh content (minimized clients stop rendering, so their
		/// thumbnails freeze otherwise). 0 disables the wake-ups
		/// </summary>
		public int MinimizedClientsRefreshPeriod { get; set; }

		[JsonProperty("WineCompatibilityMode")]
		public bool EnableWineCompatibilityMode { get; set; }

		[JsonProperty("ThumbnailsOpacity")]
		public double ThumbnailOpacity { get; set; }

		public bool EnableClientLayoutTracking
		{
			get => this._enableClientLayoutTracking;
			set
			{
				if (!value)
				{
					this.ClientLayout.Clear();
				}

				this._enableClientLayoutTracking = value;
			}
		}

		public bool HideActiveClientThumbnail { get; set; }
		public bool HideLoginClientThumbnail { get; set; }
		public bool MinimizeInactiveClients { get; set; }
		public bool HideCaptionOnClients { get; set; }
		public AnimationStyle WindowsAnimationStyle { get; set; }
		public bool ShowThumbnailsAlwaysOnTop { get; set; }

		public bool EnablePerClientThumbnailLayouts
		{
			get => this._enablePerClientThumbnailLayouts;
			set
			{
				if (!value)
				{
					this.PerClientLayout.Clear();
				}

				this._enablePerClientThumbnailLayouts = value;
			}
		}

		public bool PreventPreviews { get; set; }
		public bool HideThumbnailsOnLostFocus { get; set; }
		public int HideThumbnailsDelay { get; set; }

		public Size ThumbnailSize { get; set; }
		public Size ThumbnailMaximumSize { get; set; }
		public Size ThumbnailMinimumSize { get; set; }

		public bool EnableThumbnailSnap { get; set; }

		[JsonProperty("EnableThumbnailZoom")]
		public bool ThumbnailZoomEnabled { get; set; }
		public int ThumbnailZoomFactor { get; set; }
		public ZoomAnchor ThumbnailZoomAnchor { get; set; }
		public ZoomAnchor OverlayLabelAnchor { get; set; }
		public ZoomAnchor CycleGroupIndicatorAnchor { get; set; }

		public bool ShowThumbnailOverlays { get; set; }

		/// <summary>Outline behind the window name label for readability on bright scenes</summary>
		public bool OverlayLabelOutlineEnabled { get; set; }
		public int OverlayLabelOutlineThickness { get; set; }
		public Color OverlayLabelOutlineColor { get; set; }

		/// <summary>Outline behind the cycle group name label</summary>
		public bool CycleGroupNameOutlineEnabled { get; set; }
		public int CycleGroupNameOutlineThickness { get; set; }
		public Color CycleGroupNameOutlineColor { get; set; }
		public bool ShowClientName { get; set; }
		public bool ShowCycleGroupName { get; set; }
		public bool OverlayAlwaysOnTop { get; set; }
		public bool ShowThumbnailFrames { get; set; }

		public Color CycleGroupNameColor { get; set; }

		[JsonProperty]
		public Font CycleGroupNameFont { get; set; }
		public bool LockThumbnailLocation { get; set; }
		public bool ThumbnailSnapToGrid { get; set; }

		/// <summary>Snapped previews are resized to the full grid cell (SizeX x SizeY)</summary>
		public bool ThumbnailSnapToGridFillCell { get; set; }

		/// <summary>Offset of the snap grid relative to the (0, 0) screen origin</summary>
		public int ThumbnailSnapToGridOffsetX { get; set; }
		public int ThumbnailSnapToGridOffsetY { get; set; }

		/// <summary>
		/// Inset from the cell borders: a snapped preview is placed this many pixels away
		/// from the top/left grid lines (and shrunk accordingly in the fill-cell mode),
		/// so it sits INSIDE the cell instead of covering the lines
		/// </summary>
		public int ThumbnailSnapToGridCellPadding { get; set; }

		public int ThumbnailSnapToGridSizeX {  get; set; }
		public int ThumbnailSnapToGridSizeY { get; set; }

		public bool EnableActiveClientHighlight { get; set; }

		public Color ActiveClientHighlightColor { get; set; }
		public Color PreventPreviewColor { get; set; }
		public Color OverlayLabelColor { get; set; }

		[JsonProperty]
		public Font OverlayLabelFont { get; set; }
		public string IconName { get; set; }

		[JsonProperty("Language")]
		public string Language { get; set; }

		public int ActiveClientHighlightThickness { get; set; }

		[JsonProperty("LoginThumbnailLocation")]
		public Point LoginThumbnailLocation { get; set; }

		[JsonProperty("MainWindowSize")]
		public Size MainWindowSize { get; set; }

		[JsonProperty]
		private Dictionary<string, Dictionary<string, Point>> PerClientLayout { get; set; }
		[JsonProperty]
		private Dictionary<string, Point> FlatLayout { get; set; }
		[JsonProperty]
		private Dictionary<string, ClientLayout> ClientLayout { get; set; }
		[JsonProperty]
		private Dictionary<string, string> ClientHotkey { get; set; }
		[JsonProperty]
		public List<string> MinimizeAllClientsHotkeys { get; set; }
		[JsonProperty]
		public List<string> ToggleAllPreviewsHotkeys { get; set; }

		/// <summary>Hotkeys that toggle the click-through mode of the previews</summary>
		public List<string> ClickThroughHotkeys { get; set; }
		[JsonProperty]
		private Dictionary<string, bool> DisableThumbnail { get; set; }
		[JsonProperty]
		private List<string> PriorityClients { get; set; }
		[JsonProperty]
		private List<string> ExecutablesToPreview { get; set; }

		public Point GetThumbnailLocation(string currentClient, string activeClient, Point defaultLocation)
		{
			Point location;

			// What this code does:
			// If Per-Client layouts are enabled
			//    and client name is known
			//    and there is a separate thumbnails layout for this client
			//    and this layout contains an entry for the current client
			// then return that entry
			// otherwise try to get client layout from the flat all-clients layout
			// If there is no layout too then use the default one
			if (this.EnablePerClientThumbnailLayouts && !string.IsNullOrEmpty(activeClient))
			{
				Dictionary<string, Point> layoutSource;
				if (this.PerClientLayout.TryGetValue(activeClient, out layoutSource) && layoutSource.TryGetValue(currentClient, out location))
				{
					return location;
				}
			}

			return this.FlatLayout.TryGetValue(currentClient, out location) ? location : defaultLocation;
		}

		public Size GetThumbnailSize(string currentClient, string activeClient, Size defaultSize)
		{
			Size sizeOfThumbnail;
			return this.PerClientThumbnailSize.TryGetValue(currentClient, out sizeOfThumbnail) ? sizeOfThumbnail : defaultSize;
		}
		public ZoomAnchor GetZoomAnchor(string currentClient, ZoomAnchor defaultZoomAnchor)
		{
			ZoomAnchor zoomAnchor;
			return this.PerClientZoomAnchor.TryGetValue(currentClient, out zoomAnchor) ? zoomAnchor : defaultZoomAnchor;
		}

		public void SetThumbnailLocation(string currentClient, string activeClient, Point location)
		{
			Dictionary<string, Point> layoutSource;

			if (this.EnablePerClientThumbnailLayouts)
			{
				if (string.IsNullOrEmpty(activeClient))
				{
					return;
				}

				if (!this.PerClientLayout.TryGetValue(activeClient, out layoutSource))
				{
					layoutSource = new Dictionary<string, Point>();
					this.PerClientLayout[activeClient] = layoutSource;
				}
			}
			else
			{
				layoutSource = this.FlatLayout;
			}

			layoutSource[currentClient] = location;
		}

		public ClientLayout GetClientLayout(string currentClient)
		{
			ClientLayout layout;
			this.ClientLayout.TryGetValue(currentClient, out layout);

			return layout;
		}

		public void SetClientLayout(string currentClient, ClientLayout layout)
		{
			this.ClientLayout[currentClient] = layout;
		}

		public Keys GetClientHotkey(string currentClient)
		{
			string hotkey;
			if (this.ClientHotkey.TryGetValue(currentClient, out hotkey))
			{
				return this.StringToKey(hotkey);
			}

			return Keys.None;
		}

		public string GetClientHotkeyString(string currentClient)
		{
			string hotkey;
			return this.ClientHotkey.TryGetValue(currentClient, out hotkey) ? hotkey : null;
		}

		public void SetClientHotkey(string currentClient, string hotkey)
		{
			this.ClientHotkey[currentClient] = hotkey;
		}

		public void SetClientHotkey(string currentClient, Keys hotkey)
		{
			this.ClientHotkey[currentClient] = (new KeysConverter()).ConvertToInvariantString(hotkey);
		}

		public IReadOnlyDictionary<string, string> GetClientHotkeys()
		{
			return new Dictionary<string, string>(this.ClientHotkey);
		}

		public void RemoveClientHotkey(string currentClient)
		{
			this.ClientHotkey.Remove(currentClient);
		}

		public Keys StringToKey(string hotkey)
		{
			if (string.IsNullOrEmpty(hotkey) || EveOPreview.UI.Hotkeys.MouseBinding.IsMouseBinding(hotkey))
			{
				return Keys.None;
			}

			try
			{
				object rawValue = (new KeysConverter()).ConvertFromInvariantString(hotkey);
				return rawValue != null ? (Keys)rawValue : Keys.None;
			}
			catch (Exception)
			{
				// Protect from incorrect values
				return Keys.None;
			}
		}

		public bool IsPriorityClient(string currentClient)
		{
			return this.PriorityClients.Contains(currentClient);
		}
		public bool IsExecutableToPreview(string processName)
		{
			return this.ExecutablesToPreview.Any(s => s.Equals(processName, StringComparison.OrdinalIgnoreCase));
		}

		public bool IsThumbnailDisabled(string currentClient)
		{
			return this.DisableThumbnail.TryGetValue(currentClient, out bool isDisabled) && isDisabled;
		}

		public void ToggleThumbnail(string currentClient, bool isDisabled)
		{
			this.DisableThumbnail[currentClient] = isDisabled;
		}

		/// <summary>
		/// Applies restrictions to different parameters of the config
		/// </summary>
		public void ApplyRestrictions()
		{
#if LINUX
			this.ThumbnailRefreshPeriod = ThumbnailConfiguration.ApplyRestrictions(this.ThumbnailRefreshPeriod, 10, 1000);
#else
			this.ThumbnailRefreshPeriod = ThumbnailConfiguration.ApplyRestrictions(this.ThumbnailRefreshPeriod, 300, 1000);
#endif
			this.ThumbnailResizeTimeoutPeriod = ThumbnailConfiguration.ApplyRestrictions(this.ThumbnailResizeTimeoutPeriod, 200, 5000);

			this.ThumbnailSnapToGridCellPadding = ThumbnailConfiguration.ApplyRestrictions(this.ThumbnailSnapToGridCellPadding, 0, 100);
			this.OverlayLabelOutlineThickness = ThumbnailConfiguration.ApplyRestrictions(this.OverlayLabelOutlineThickness, 1, 5);
			this.CycleGroupNameOutlineThickness = ThumbnailConfiguration.ApplyRestrictions(this.CycleGroupNameOutlineThickness, 1, 5);

			// 0 keeps the minimized clients wake-up feature disabled
			if (this.MinimizedClientsRefreshPeriod != 0)
			{
				this.MinimizedClientsRefreshPeriod = ThumbnailConfiguration.ApplyRestrictions(this.MinimizedClientsRefreshPeriod, 2, 300);
			}
			this.ThumbnailSize = new Size(ThumbnailConfiguration.ApplyRestrictions(this.ThumbnailSize.Width, this.ThumbnailMinimumSize.Width, this.ThumbnailMaximumSize.Width),
				ThumbnailConfiguration.ApplyRestrictions(this.ThumbnailSize.Height, this.ThumbnailMinimumSize.Height, this.ThumbnailMaximumSize.Height));
			this.ThumbnailOpacity = ThumbnailConfiguration.ApplyRestrictions((int)(this.ThumbnailOpacity * 100.00), 20, 100) / 100.00;
			this.ThumbnailZoomFactor = ThumbnailConfiguration.ApplyRestrictions(this.ThumbnailZoomFactor, 2, 10);
			this.ActiveClientHighlightThickness = ThumbnailConfiguration.ApplyRestrictions(this.ActiveClientHighlightThickness, 1, 6);
			this.Language = LanguageManager.Normalize(this.Language);

			this.EnsureAppearance();
			this.EnsureCycleGroups();
		}

		/// <summary>
		/// Fonts and colors are the only settings the app cannot fall back on at the point of
		/// use - a null font takes the overlay down. A hand-edited config, or one written by
		/// a build that saved before its settings were loaded, can carry empty values here
		/// </summary>
		private void EnsureAppearance()
		{
			this.OverlayLabelFont = this.OverlayLabelFont ?? new Font(FontFamily.GenericSansSerif, 10.0F, FontStyle.Bold);
			this.CycleGroupNameFont = this.CycleGroupNameFont ?? new Font(FontFamily.GenericSansSerif, 10.0F, FontStyle.Bold);

			if (this.OverlayLabelColor.IsEmpty)
			{
				this.OverlayLabelColor = Color.Orange;
			}

			if (this.CycleGroupNameColor.IsEmpty)
			{
				this.CycleGroupNameColor = Color.Orange;
			}

			if (this.ActiveClientHighlightColor.IsEmpty)
			{
				this.ActiveClientHighlightColor = Color.GreenYellow;
			}

			if (this.PreventPreviewColor.IsEmpty)
			{
				this.PreventPreviewColor = Color.Purple;
			}
		}

		private void EnsureCycleGroups()
		{
			// Configs written before these options existed have them missing
			this.MinimizeAllClientsHotkeys = this.MinimizeAllClientsHotkeys ?? new List<string>();
			this.ToggleAllPreviewsHotkeys = this.ToggleAllPreviewsHotkeys ?? new List<string>();
			this.ClickThroughHotkeys = this.ClickThroughHotkeys ?? new List<string>();
			this.CycleGroupNameFont = this.CycleGroupNameFont ?? this.OverlayLabelFont;

			if (this.CycleGroups == null)
			{
				// Migrate the legacy fixed CycleGroupN* entries (if any) to the dynamic list
				this.CycleGroups = new List<CycleGroup>();

				this.MigrateLegacyCycleGroup("Group 1", this.CycleGroup1ForwardHotkeys, this.CycleGroup1BackwardHotkeys, this.CycleGroup1ClientsOrder);
				this.MigrateLegacyCycleGroup("Group 2", this.CycleGroup2ForwardHotkeys, this.CycleGroup2BackwardHotkeys, this.CycleGroup2ClientsOrder);
				this.MigrateLegacyCycleGroup("Group 3", this.CycleGroup3ForwardHotkeys, this.CycleGroup3BackwardHotkeys, this.CycleGroup3ClientsOrder);
				this.MigrateLegacyCycleGroup("Group 4", this.CycleGroup4ForwardHotkeys, this.CycleGroup4BackwardHotkeys, this.CycleGroup4ClientsOrder);
				this.MigrateLegacyCycleGroup("Group 5", this.CycleGroup5ForwardHotkeys, this.CycleGroup5BackwardHotkeys, this.CycleGroup5ClientsOrder);

				if (this.CycleGroups.Count == 0)
				{
					this.CycleGroups.Add(new CycleGroup
					{
						Name = "Group 1",
						ForwardHotkeys = new List<string> { "F14", "Control+F14" },
						BackwardHotkeys = new List<string> { "F13", "Control+F13" }
					});
				}
			}

			this.CycleGroup1ForwardHotkeys = null;
			this.CycleGroup1BackwardHotkeys = null;
			this.CycleGroup1ClientsOrder = null;
			this.CycleGroup2ForwardHotkeys = null;
			this.CycleGroup2BackwardHotkeys = null;
			this.CycleGroup2ClientsOrder = null;
			this.CycleGroup3ForwardHotkeys = null;
			this.CycleGroup3BackwardHotkeys = null;
			this.CycleGroup3ClientsOrder = null;
			this.CycleGroup4ForwardHotkeys = null;
			this.CycleGroup4BackwardHotkeys = null;
			this.CycleGroup4ClientsOrder = null;
			this.CycleGroup5ForwardHotkeys = null;
			this.CycleGroup5BackwardHotkeys = null;
			this.CycleGroup5ClientsOrder = null;

			// Sanitize entries possibly edited by hand
			int fallbackIndex = 1;
			foreach (CycleGroup group in this.CycleGroups)
			{
				group.ForwardHotkeys = group.ForwardHotkeys ?? new List<string>();
				group.BackwardHotkeys = group.BackwardHotkeys ?? new List<string>();
				group.ClientsOrder = group.ClientsOrder ?? new Dictionary<string, int>();

				if (string.IsNullOrWhiteSpace(group.Name))
				{
					group.Name = "Group " + fallbackIndex;
				}

				fallbackIndex++;
			}
		}

		private void MigrateLegacyCycleGroup(string name, List<string> forwardHotkeys, List<string> backwardHotkeys, Dictionary<string, int> clientsOrder)
		{
			bool hasHotkeys = ((forwardHotkeys != null) && forwardHotkeys.Any(x => !string.IsNullOrEmpty(x)))
							|| ((backwardHotkeys != null) && backwardHotkeys.Any(x => !string.IsNullOrEmpty(x)));
			bool hasClients = (clientsOrder != null) && (clientsOrder.Count > 0);

			if (!hasHotkeys && !hasClients)
			{
				return;
			}

			this.CycleGroups.Add(new CycleGroup
			{
				Name = name,
				ForwardHotkeys = forwardHotkeys?.Where(x => !string.IsNullOrEmpty(x)).ToList() ?? new List<string>(),
				BackwardHotkeys = backwardHotkeys?.Where(x => !string.IsNullOrEmpty(x)).ToList() ?? new List<string>(),
				ClientsOrder = clientsOrder != null ? new Dictionary<string, int>(clientsOrder) : new Dictionary<string, int>()
			});
		}

		private static int ApplyRestrictions(int value, int minimum, int maximum)
		{
			if (value <= minimum)
			{
				return minimum;
			}

			if (value >= maximum)
			{
				return maximum;
			}

			return value;
		}
	}
}