using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using EveOPreview.Localization;
using EveOPreview.UI.Hotkeys;
using Newtonsoft.Json;

namespace EveOPreview.Configuration.Implementation
{
	sealed class ThumbnailConfiguration : IThumbnailConfiguration
	{
		#region Private constants
		/// <summary>Configuration layout version that introduced the character registry</summary>
		private const int CONFIG_VERSION_CHARACTER_REGISTRY = 2;

		/// <summary>Sample per-client entries older builds seeded fresh configurations with</summary>
		private const string EXAMPLE_CLIENT_TITLE_PREFIX = "EVE - Example Toon";

		/// <summary>Colors handed out to the character groups in turn</summary>
		private static readonly Color[] GROUP_COLOR_PALETTE =
		{
			Color.FromArgb(0x4F, 0x9D, 0xE0), Color.FromArgb(0x5C, 0xC2, 0x8A), Color.FromArgb(0xE0, 0xA8, 0x3E),
			Color.FromArgb(0xC0, 0x7C, 0xD8), Color.FromArgb(0xE0, 0x7B, 0x5A), Color.FromArgb(0x3F, 0xC0, 0xC0),
			Color.FromArgb(0xB0, 0xC0, 0x4F), Color.FromArgb(0xD8, 0x7C, 0xA8)
		};
		#endregion

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

			// Legacy per-client dictionaries: they are still read from existing
			// configuration files and migrated into the character registry below,
			// but they are never written back
			this.PerClientActiveClientHighlightColor = new Dictionary<string, Color>();
			this.PerClientPreventPreviewColor = new Dictionary<string, Color>();
			this.PerClientPreventPreviews = new Dictionary<string, bool>();
			this.PerClientThumbnailSize = new Dictionary<string, Size>();
			this.PerClientZoomAnchor = new Dictionary<string, ZoomAnchor>();

			this.Characters = new List<CharacterInfo>();
			this.CharacterGroups = new List<CharacterGroup>();
			this.ClientPreviewSettings = new Dictionary<string, PreviewSettings>();

			this.PerClientLayout = new Dictionary<string, Dictionary<string, Point>>();
			this.FlatLayout = new Dictionary<string, Point>();
			this.ClientLayout = new Dictionary<string, ClientLayout>();
			this.ClientHotkey = new Dictionary<string, string>();
			this.ClientHotkeys = new Dictionary<string, List<string>>();
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

			this.EnableGameLogMonitor = false;
			this.GameLogsFolder = "";
			this.EnableAggroFrames = true;
			this.AggroYellowColor = Color.Gold;
			this.AggroRedColor = Color.Red;
			this.AggroFillPercent = 20;

			// Clicks on a preview. These are the combinations the application has always
			// used, so an existing configuration keeps behaving the way it did
			this.PreviewClickMinimize = PreviewClickBinding.Compose(Keys.Control, MouseButtons.Left);
			this.PreviewClickSwitchOut = PreviewClickBinding.Compose(Keys.Control | Keys.Shift, MouseButtons.Left);
			this.PreviewClickToggleCycleGroup = PreviewClickBinding.Compose(Keys.Shift, MouseButtons.Left);

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

		#region Character registry
		/// <summary>Every character that has been seen logged in at least once</summary>
		[JsonProperty("Characters")]
		private List<CharacterInfo> Characters { get; set; }

		/// <summary>Character groups (accounts). Membership lives in CharacterInfo.GroupId</summary>
		[JsonProperty("CharacterGroups")]
		private List<CharacterGroup> CharacterGroups { get; set; }

		/// <summary>Per-window preview configuration, keyed by the client title</summary>
		[JsonProperty("PreviewSettings")]
		private Dictionary<string, PreviewSettings> ClientPreviewSettings { get; set; }
		#endregion

		#region Legacy per-client entries (read for migration, never written back)
		[JsonProperty("PerClientPreventPreviewColor")]
		private Dictionary<string, Color> PerClientPreventPreviewColor { get; set; }
		public bool ShouldSerializePerClientPreventPreviewColor() => false;

		[JsonProperty("PerClientActiveClientHighlightColor")]
		private Dictionary<string, Color> PerClientActiveClientHighlightColor { get; set; }
		public bool ShouldSerializePerClientActiveClientHighlightColor() => false;

		[JsonProperty("PerClientPreventPreviews")]
		private Dictionary<string, bool> PerClientPreventPreviews { get; set; }
		public bool ShouldSerializePerClientPreventPreviews() => false;

		[JsonProperty("PerClientThumbnailSize")]
		private Dictionary<string, Size> PerClientThumbnailSize { get; set; }
		public bool ShouldSerializePerClientThumbnailSize() => false;

		[JsonProperty("PerClientZoomAnchor")]
		private Dictionary<string, ZoomAnchor> PerClientZoomAnchor { get; set; }
		public bool ShouldSerializePerClientZoomAnchor() => false;
		#endregion
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

		/// <summary>Click on a preview that minimizes its client</summary>
		public string PreviewClickMinimize { get; set; }

		/// <summary>Click on a preview that switches to the last non-client application</summary>
		public string PreviewClickSwitchOut { get; set; }

		/// <summary>Click on a preview that excludes its client from the cycle groups (and back)</summary>
		public string PreviewClickToggleCycleGroup { get; set; }

		[JsonProperty("LoginThumbnailLocation")]
		public Point LoginThumbnailLocation { get; set; }

		[JsonProperty("MainWindowSize")]
		public Size MainWindowSize { get; set; }

		public bool EnableGameLogMonitor { get; set; }
		public string GameLogsFolder { get; set; }
		public bool EnableAggroFrames { get; set; }
		public Color AggroYellowColor { get; set; }
		public Color AggroRedColor { get; set; }
		public int AggroFillPercent { get; set; }

		[JsonProperty]
		private Dictionary<string, Dictionary<string, Point>> PerClientLayout { get; set; }
		[JsonProperty]
		private Dictionary<string, Point> FlatLayout { get; set; }
		[JsonProperty]
		private Dictionary<string, ClientLayout> ClientLayout { get; set; }
		/// <summary>Legacy single hotkey per client, read for migration and never written back</summary>
		[JsonProperty]
		private Dictionary<string, string> ClientHotkey { get; set; }
		public bool ShouldSerializeClientHotkey() => false;

		/// <summary>Hotkeys of a client: an action can be reached by more than one of them</summary>
		[JsonProperty]
		private Dictionary<string, List<string>> ClientHotkeys { get; set; }
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

		#region Character registry
		private readonly object _registrySyncRoot = new object();

		// Rebuilt on every load by SanitizeCharacterRegistry()
		private Dictionary<string, CharacterInfo> _characterIndex = new Dictionary<string, CharacterInfo>(StringComparer.Ordinal);

		public IReadOnlyList<CharacterInfo> GetCharacters()
		{
			lock (this._registrySyncRoot)
			{
				return new List<CharacterInfo>(this.Characters);
			}
		}

		public CharacterInfo GetCharacter(string title)
		{
			lock (this._registrySyncRoot)
			{
				return this.FindCharacterLocked(title);
			}
		}

		/// <summary>
		/// Adds the character to the registry (or refreshes its 'last seen' stamp).
		/// Returns true when the registry has actually changed and has to be saved
		/// </summary>
		public bool RegisterCharacter(string title)
		{
			if (!CharacterInfo.IsCharacterTitle(title))
			{
				return false;
			}

			lock (this._registrySyncRoot)
			{
				DateTime now = DateTime.UtcNow;
				CharacterInfo character = this.FindCharacterLocked(title);

				if (character != null)
				{
					// A refreshed 'last seen' stamp alone is not worth a configuration write
					character.LastSeen = now;
					return false;
				}

				this.AddCharacterLocked(title, now);
				return true;
			}
		}

		/// <summary>Drops the character and every setting stored for it</summary>
		/// <summary>
		/// Blacklists the character (or takes it off the blacklist). A blacklisted character
		/// keeps its settings but disappears from every list the application offers
		/// </summary>
		public void SetCharacterIgnored(string title, bool ignored)
		{
			lock (this._registrySyncRoot)
			{
				CharacterInfo character = this.FindCharacterLocked(title);

				if (character == null)
				{
					return;
				}

				character.Ignored = ignored;
			}

			if (!ignored)
			{
				return;
			}

			// A blacklisted character leaves the cycle order and gives up its hotkey.
			// Its preview settings and position stay, so taking it off the blacklist
			// brings it back the way it was
			this.ClientHotkeys.Remove(title);

			foreach (CycleGroup group in this.CycleGroups)
			{
				group.ClientsOrder.Remove(title);
			}
		}

		public void SetCharacterGroupColor(string groupId, Color color)
		{
			lock (this._registrySyncRoot)
			{
				CharacterGroup group = this.FindGroupLocked(groupId);

				if (group != null)
				{
					group.Color = color;
				}
			}
		}

		public bool ForgetCharacter(string title)
		{
			if (string.IsNullOrEmpty(title))
			{
				return false;
			}

			lock (this._registrySyncRoot)
			{
				CharacterInfo character = this.FindCharacterLocked(title);

				if (character == null)
				{
					return false;
				}

				this.Characters.Remove(character);
				this._characterIndex.Remove(title);
				this.ClientPreviewSettings.Remove(title);
			}

			// Everything else that is keyed by the client title
			this.FlatLayout.Remove(title);
			this.ClientLayout.Remove(title);
			this.ClientHotkeys.Remove(title);
			this.DisableThumbnail.Remove(title);
			this.PriorityClients.Remove(title);
			this.PerClientLayout.Remove(title);

			foreach (Dictionary<string, Point> layout in this.PerClientLayout.Values)
			{
				layout.Remove(title);
			}

			foreach (CycleGroup group in this.CycleGroups)
			{
				group.ClientsOrder.Remove(title);
			}

			this.SanitizeCharacterRegistry();

			return true;
		}

		public IReadOnlyList<CharacterGroup> GetCharacterGroups()
		{
			lock (this._registrySyncRoot)
			{
				return new List<CharacterGroup>(this.CharacterGroups);
			}
		}

		public CharacterGroup GetCharacterGroupById(string groupId)
		{
			lock (this._registrySyncRoot)
			{
				return this.FindGroupLocked(groupId);
			}
		}

		/// <summary>Group (account) the character belongs to, null for an ungrouped one</summary>
		public CharacterGroup GetCharacterGroupOf(string title)
		{
			lock (this._registrySyncRoot)
			{
				CharacterInfo character = this.FindCharacterLocked(title);

				return (character == null) ? null : this.FindGroupLocked(character.GroupId);
			}
		}

		public IReadOnlyList<string> GetGroupMembers(string groupId)
		{
			lock (this._registrySyncRoot)
			{
				return this.GetGroupMembersLocked(groupId);
			}
		}

		/// <summary>
		/// Clients a setting written for this one has to be written for as well.
		/// The client itself always comes first; the rest of its group follows it
		/// when that group is managed as a whole
		/// </summary>
		public IReadOnlyList<string> GetLinkedClients(string title)
		{
			List<string> result = new List<string> { title };

			if (!CharacterInfo.IsCharacterTitle(title))
			{
				return result;
			}

			lock (this._registrySyncRoot)
			{
				CharacterInfo character = this.FindCharacterLocked(title);
				CharacterGroup group = (character == null) ? null : this.FindGroupLocked(character.GroupId);

				if ((group == null) || !group.ManageAsWhole)
				{
					return result;
				}

				foreach (string member in this.GetGroupMembersLocked(group.Id))
				{
					if (!string.Equals(member, title, StringComparison.Ordinal))
					{
						result.Add(member);
					}
				}
			}

			return result;
		}

		/// <summary>
		/// Puts the characters into one group. Two characters that already belong to
		/// different groups are never merged automatically: a client cannot switch the
		/// account without a restart, so that combination means a stale or a bogus entry
		/// and only the user can tell which one it is.
		/// Returns true when the registry has changed
		/// </summary>
		public bool LinkCharacters(IEnumerable<string> titles)
		{
			List<string> members = titles.Where(CharacterInfo.IsCharacterTitle).Distinct(StringComparer.Ordinal).ToList();

			if (members.Count < 2)
			{
				return false;
			}

			lock (this._registrySyncRoot)
			{
				DateTime now = DateTime.UtcNow;

				foreach (string title in members)
				{
					if (this.FindCharacterLocked(title) == null)
					{
						this.AddCharacterLocked(title, now);
					}
				}

				List<string> groupIds = members.Select(title => this._characterIndex[title].GroupId)
												.Where(id => this.FindGroupLocked(id) != null)
												.Distinct(StringComparer.Ordinal)
												.ToList();

				if (groupIds.Count > 1)
				{
					return false;
				}

				string groupId = groupIds.FirstOrDefault()
									?? this.AddGroupLocked(CharacterInfo.GetDisplayName(members[0])).Id;

				bool changed = false;

				foreach (string title in members)
				{
					CharacterInfo character = this._characterIndex[title];

					if (!string.Equals(character.GroupId, groupId, StringComparison.Ordinal))
					{
						character.GroupId = groupId;
						changed = true;
					}
				}

				if (changed)
				{
					this.PropagateGroupSettingsLocked(groupId);
				}

				return changed;
			}
		}

		/// <summary>
		/// Hands the stored preview settings and the stored preview position of a group over
		/// to the members that have none yet. Done once, when a character joins the group,
		/// so that the lookups stay a plain dictionary hit
		/// </summary>
		private void PropagateGroupSettingsLocked(string groupId)
		{
			CharacterGroup group = this.FindGroupLocked(groupId);

			if ((group == null) || !group.ManageAsWhole)
			{
				return;
			}

			List<string> members = this.GetGroupMembersLocked(groupId);

			PreviewSettings settings = members.Select(member => this.ClientPreviewSettings.TryGetValue(member, out PreviewSettings stored) ? stored : null)
												.FirstOrDefault(stored => stored != null);

			bool hasLocation = false;
			Point location = Point.Empty;

			foreach (string member in members)
			{
				if (this.FlatLayout.TryGetValue(member, out location))
				{
					hasLocation = true;
					break;
				}
			}

			foreach (string member in members)
			{
				if ((settings != null) && !this.ClientPreviewSettings.ContainsKey(member))
				{
					this.ClientPreviewSettings[member] = settings.Clone();
				}

				if (hasLocation && !this.FlatLayout.ContainsKey(member))
				{
					this.FlatLayout[member] = location;
				}
			}
		}

		/// <summary>Moves the character into another group. A null group id detaches it</summary>
		public void SetCharacterGroup(string title, string groupId)
		{
			lock (this._registrySyncRoot)
			{
				CharacterInfo character = this.FindCharacterLocked(title);

				if (character == null)
				{
					return;
				}

				character.GroupId = (this.FindGroupLocked(groupId) != null) ? groupId : null;

				this.PropagateGroupSettingsLocked(character.GroupId);
			}

			this.SanitizeCharacterRegistry();
		}

		public CharacterGroup CreateCharacterGroup(string name)
		{
			lock (this._registrySyncRoot)
			{
				return this.AddGroupLocked(name);
			}
		}

		public void RenameCharacterGroup(string groupId, string name)
		{
			lock (this._registrySyncRoot)
			{
				CharacterGroup group = this.FindGroupLocked(groupId);

				if ((group != null) && !string.IsNullOrWhiteSpace(name))
				{
					group.Name = name.Trim();
				}
			}
		}

		public void SetGroupManageAsWhole(string groupId, bool value)
		{
			lock (this._registrySyncRoot)
			{
				CharacterGroup group = this.FindGroupLocked(groupId);

				if (group != null)
				{
					group.ManageAsWhole = value;
				}
			}
		}

		public void RemoveCharacterGroup(string groupId)
		{
			lock (this._registrySyncRoot)
			{
				CharacterGroup group = this.FindGroupLocked(groupId);

				if (group == null)
				{
					return;
				}

				foreach (CharacterInfo character in this.Characters)
				{
					if (string.Equals(character.GroupId, groupId, StringComparison.Ordinal))
					{
						character.GroupId = null;
					}
				}

				this.CharacterGroups.Remove(group);
			}
		}
		#endregion

		#region Per-window preview settings
		/// <summary>A copy of the stored entry of this client, null when it has none</summary>
		public PreviewSettings GetPreviewSettings(string title)
		{
			if (string.IsNullOrEmpty(title))
			{
				return null;
			}

			lock (this._registrySyncRoot)
			{
				return this.ClientPreviewSettings.TryGetValue(title, out PreviewSettings settings) ? settings.Clone() : null;
			}
		}

		/// <summary>
		/// Stores the preview settings of the client. They are applied to the whole group
		/// of that client when the group is managed as a whole
		/// </summary>
		public void SetPreviewSettings(string title, PreviewSettings settings)
		{
			if (string.IsNullOrEmpty(title))
			{
				return;
			}

			foreach (string client in this.GetLinkedClients(title))
			{
				lock (this._registrySyncRoot)
				{
					if ((settings == null) || settings.IsEmpty)
					{
						this.ClientPreviewSettings.Remove(client);
						continue;
					}

					this.ClientPreviewSettings[client] = settings.Clone();
				}
			}
		}

		/// <summary>
		/// Preview settings of the client with every value filled in: the stored ones where
		/// the client has its own settings enabled, the global ones everywhere else
		/// </summary>
		public PreviewSettings ResolvePreviewSettings(string title)
		{
			PreviewSettings custom = this.FindCustomSettings(title);

			return new PreviewSettings
			{
				UseCustomSettings = custom != null,

				ThumbnailSize = custom?.ThumbnailSize ?? this.ThumbnailSize,
				ThumbnailOpacity = custom?.ThumbnailOpacity ?? this.ThumbnailOpacity,

				PreventPreviews = custom?.PreventPreviews ?? this.PreventPreviews,
				PreventPreviewColor = custom?.PreventPreviewColor ?? this.PreventPreviewColor,

				EnableActiveClientHighlight = custom?.EnableActiveClientHighlight ?? this.EnableActiveClientHighlight,
				ActiveClientHighlightColor = custom?.ActiveClientHighlightColor ?? this.ActiveClientHighlightColor,
				ActiveClientHighlightThickness = custom?.ActiveClientHighlightThickness ?? this.ActiveClientHighlightThickness,

				ShowThumbnailOverlays = custom?.ShowThumbnailOverlays ?? this.ShowThumbnailOverlays,
				OverlayAlwaysOnTop = custom?.OverlayAlwaysOnTop ?? this.OverlayAlwaysOnTop,
				ShowThumbnailFrames = custom?.ShowThumbnailFrames ?? this.ShowThumbnailFrames,

				ShowClientName = custom?.ShowClientName ?? this.ShowClientName,
				OverlayLabelAnchor = custom?.OverlayLabelAnchor ?? this.OverlayLabelAnchor,
				OverlayLabelColor = custom?.OverlayLabelColor ?? this.OverlayLabelColor,
				OverlayLabelFont = custom?.OverlayLabelFont ?? this.OverlayLabelFont,
				OverlayLabelOutlineEnabled = custom?.OverlayLabelOutlineEnabled ?? this.OverlayLabelOutlineEnabled,
				OverlayLabelOutlineThickness = custom?.OverlayLabelOutlineThickness ?? this.OverlayLabelOutlineThickness,
				OverlayLabelOutlineColor = custom?.OverlayLabelOutlineColor ?? this.OverlayLabelOutlineColor,

				ShowCycleGroupName = custom?.ShowCycleGroupName ?? this.ShowCycleGroupName,
				CycleGroupIndicatorAnchor = custom?.CycleGroupIndicatorAnchor ?? this.CycleGroupIndicatorAnchor,
				CycleGroupNameColor = custom?.CycleGroupNameColor ?? this.CycleGroupNameColor,
				CycleGroupNameFont = custom?.CycleGroupNameFont ?? this.CycleGroupNameFont,
				CycleGroupNameOutlineEnabled = custom?.CycleGroupNameOutlineEnabled ?? this.CycleGroupNameOutlineEnabled,
				CycleGroupNameOutlineThickness = custom?.CycleGroupNameOutlineThickness ?? this.CycleGroupNameOutlineThickness,
				CycleGroupNameOutlineColor = custom?.CycleGroupNameOutlineColor ?? this.CycleGroupNameOutlineColor,

				ThumbnailZoomEnabled = custom?.ThumbnailZoomEnabled ?? this.ThumbnailZoomEnabled,
				ThumbnailZoomFactor = custom?.ThumbnailZoomFactor ?? this.ThumbnailZoomFactor,
				ThumbnailZoomAnchor = custom?.ThumbnailZoomAnchor ?? this.ThumbnailZoomAnchor
			};
		}

		public bool GetPreventPreviews(string currentClient, bool defaultValue)
		{
			return this.FindCustomSettings(currentClient)?.PreventPreviews ?? defaultValue;
		}

		public Color GetPreventPreviewColor(string currentClient, Color defaultColor)
		{
			return this.FindCustomSettings(currentClient)?.PreventPreviewColor ?? defaultColor;
		}

		public Color GetActiveClientHighlightColor(string currentClient, Color defaultColor)
		{
			return this.FindCustomSettings(currentClient)?.ActiveClientHighlightColor ?? defaultColor;
		}

		/// <summary>Stored entry of the client, but only when its own settings are enabled</summary>
		private PreviewSettings FindCustomSettings(string title)
		{
			if (string.IsNullOrEmpty(title))
			{
				return null;
			}

			lock (this._registrySyncRoot)
			{
				return this.ClientPreviewSettings.TryGetValue(title, out PreviewSettings settings) && settings.UseCustomSettings
						? settings
						: null;
			}
		}
		#endregion

		#region Character registry internals
		private CharacterInfo FindCharacterLocked(string title)
		{
			return (!string.IsNullOrEmpty(title) && this._characterIndex.TryGetValue(title, out CharacterInfo character))
					? character
					: null;
		}

		private CharacterInfo AddCharacterLocked(string title, DateTime? timestamp)
		{
			CharacterInfo character = new CharacterInfo(title, timestamp, timestamp);

			this.Characters.Add(character);
			this._characterIndex[title] = character;

			return character;
		}

		private CharacterGroup FindGroupLocked(string groupId)
		{
			if (string.IsNullOrEmpty(groupId))
			{
				return null;
			}

			return this.CharacterGroups.FirstOrDefault(group => string.Equals(group.Id, groupId, StringComparison.Ordinal));
		}

		private List<string> GetGroupMembersLocked(string groupId)
		{
			List<string> members = new List<string>();

			if (string.IsNullOrEmpty(groupId))
			{
				return members;
			}

			foreach (CharacterInfo character in this.Characters)
			{
				if (string.Equals(character.GroupId, groupId, StringComparison.Ordinal))
				{
					members.Add(character.Title);
				}
			}

			return members;
		}

		private CharacterGroup AddGroupLocked(string name)
		{
			int index = 1;

			while (this.CharacterGroups.Any(group => group.Id == "g" + index))
			{
				index++;
			}

			CharacterGroup newGroup = new CharacterGroup
			{
				Id = "g" + index,
				Name = string.IsNullOrWhiteSpace(name) ? "g" + index : name.Trim(),
				ManageAsWhole = true,
				Color = ThumbnailConfiguration.GROUP_COLOR_PALETTE[this.CharacterGroups.Count % ThumbnailConfiguration.GROUP_COLOR_PALETTE.Length]
			};

			this.CharacterGroups.Add(newGroup);

			return newGroup;
		}
		#endregion

		public Point GetThumbnailLocation(string currentClient, string activeClient, Point defaultLocation)
		{
			// What this code does:
			// If Per-Client layouts are enabled
			//    and client name is known
			//    and there is a separate thumbnails layout for this client
			//    and this layout contains an entry for the current client
			// then return that entry
			// otherwise try to get client layout from the flat all-clients layout
			// If there is no layout too then use the default one
			// A client that shares its position with the rest of its group also picks up
			// the stored position of any of its group mates - that is what puts a character
			// logging in for the first time into the slot of its account right away
			IReadOnlyList<string> clients = this.GetLinkedClients(currentClient);

			if (this.EnablePerClientThumbnailLayouts && !string.IsNullOrEmpty(activeClient))
			{
				if (this.PerClientLayout.TryGetValue(activeClient, out Dictionary<string, Point> layoutSource))
				{
					foreach (string client in clients)
					{
						if (layoutSource.TryGetValue(client, out Point perClientLocation))
						{
							return perClientLocation;
						}
					}
				}
			}

			foreach (string client in clients)
			{
				if (this.FlatLayout.TryGetValue(client, out Point location))
				{
					return location;
				}
			}

			return defaultLocation;
		}

		public Size GetThumbnailSize(string currentClient, string activeClient, Size defaultSize)
		{
			return this.FindCustomSettings(currentClient)?.ThumbnailSize ?? defaultSize;
		}

		public ZoomAnchor GetZoomAnchor(string currentClient, ZoomAnchor defaultZoomAnchor)
		{
			return this.FindCustomSettings(currentClient)?.ThumbnailZoomAnchor ?? defaultZoomAnchor;
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

			// The position of a group managed as a whole is stored for all of its members
			foreach (string client in this.GetLinkedClients(currentClient))
			{
				layoutSource[client] = location;
			}
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

		/// <summary>Every hotkey of this client, keyboard and mouse ones alike</summary>
		public IReadOnlyList<string> GetClientHotkeys(string currentClient)
		{
			if (string.IsNullOrEmpty(currentClient) || !this.ClientHotkeys.TryGetValue(currentClient, out List<string> hotkeys))
			{
				return new List<string>();
			}

			return new List<string>(hotkeys);
		}

		public IReadOnlyDictionary<string, IReadOnlyList<string>> GetClientHotkeys()
		{
			Dictionary<string, IReadOnlyList<string>> result = new Dictionary<string, IReadOnlyList<string>>(this.ClientHotkeys.Count);

			foreach (KeyValuePair<string, List<string>> entry in this.ClientHotkeys)
			{
				result.Add(entry.Key, new List<string>(entry.Value));
			}

			return result;
		}

		/// <summary>Adds one more hotkey to the client; an already stored one is not doubled</summary>
		public void AddClientHotkey(string currentClient, string hotkey)
		{
			if (string.IsNullOrEmpty(currentClient) || string.IsNullOrEmpty(hotkey))
			{
				return;
			}

			if (!this.ClientHotkeys.TryGetValue(currentClient, out List<string> hotkeys))
			{
				hotkeys = new List<string>();
				this.ClientHotkeys[currentClient] = hotkeys;
			}

			hotkeys.RemoveAll(string.IsNullOrEmpty);

			string normalized = this.NormalizeHotkey(hotkey);

			if (!hotkeys.Any(stored => string.Equals(this.NormalizeHotkey(stored), normalized, StringComparison.OrdinalIgnoreCase)))
			{
				hotkeys.Add(normalized);
			}
		}

		/// <summary>Replaces every hotkey of the client at once</summary>
		public void SetClientHotkeys(string currentClient, IEnumerable<string> hotkeys)
		{
			if (string.IsNullOrEmpty(currentClient))
			{
				return;
			}

			this.ClientHotkeys.Remove(currentClient);

			foreach (string hotkey in hotkeys ?? new List<string>())
			{
				this.AddClientHotkey(currentClient, hotkey);
			}
		}

		/// <summary>Drops one hotkey of the client; the rest of them stay</summary>
		public void RemoveClientHotkey(string currentClient, string hotkey)
		{
			if (string.IsNullOrEmpty(currentClient) || !this.ClientHotkeys.TryGetValue(currentClient, out List<string> hotkeys))
			{
				return;
			}

			string normalized = this.NormalizeHotkey(hotkey);

			hotkeys.RemoveAll(stored => string.Equals(this.NormalizeHotkey(stored), normalized, StringComparison.OrdinalIgnoreCase));

			if (hotkeys.Count == 0)
			{
				this.ClientHotkeys.Remove(currentClient);
			}
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

		/// <summary>
		/// Canonical form of a hotkey. A hand written "Control+F1" and the "Ctrl+F1" the
		/// key converter produces are the same combination and have to compare equal
		/// </summary>
		public string NormalizeHotkey(string hotkey)
		{
			if (string.IsNullOrEmpty(hotkey))
			{
				return "";
			}

			if (MouseBinding.IsMouseBinding(hotkey))
			{
				return MouseBinding.Normalize(hotkey);
			}

			Keys keys = this.StringToKey(hotkey);

			return (keys == Keys.None) ? hotkey : (new KeysConverter()).ConvertToInvariantString(keys);
		}

		/// <summary>Brings every hotkey of the list to the canonical form and drops the duplicates</summary>
		private void NormalizeHotkeyList(List<string> hotkeys)
		{
			if (hotkeys == null)
			{
				return;
			}

			List<string> normalized = hotkeys.Where(hotkey => !string.IsNullOrEmpty(hotkey))
											.Select(this.NormalizeHotkey)
											.Distinct(StringComparer.OrdinalIgnoreCase)
											.ToList();

			hotkeys.Clear();
			hotkeys.AddRange(normalized);
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
			this.AggroFillPercent = ThumbnailConfiguration.ApplyRestrictions(this.AggroFillPercent, 1, 100);
			this.GameLogsFolder = this.GameLogsFolder ?? "";

			// An unparsable click binding is dropped: the action then has no click at all
			this.PreviewClickMinimize = PreviewClickBinding.Normalize(this.PreviewClickMinimize);
			this.PreviewClickSwitchOut = PreviewClickBinding.Normalize(this.PreviewClickSwitchOut);
			this.PreviewClickToggleCycleGroup = PreviewClickBinding.Normalize(this.PreviewClickToggleCycleGroup);
			this.Language = LanguageManager.Normalize(this.Language);

			this.EnsureAppearance();
			this.EnsureCycleGroups();
			this.EnsureCharacters();
			this.EnsureClientHotkeys();

			this.NormalizeHotkeyList(this.MinimizeAllClientsHotkeys);
			this.NormalizeHotkeyList(this.ToggleAllPreviewsHotkeys);
			this.NormalizeHotkeyList(this.ClickThroughHotkeys);

			foreach (CycleGroup group in this.CycleGroups)
			{
				this.NormalizeHotkeyList(group.ForwardHotkeys);
				this.NormalizeHotkeyList(group.BackwardHotkeys);
			}

			foreach (CharacterGroup characterGroup in this.GetCharacterGroups())
			{
				this.NormalizeHotkeyList(characterGroup.Hotkeys);
			}
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

			if (this.AggroYellowColor.IsEmpty)
			{
				this.AggroYellowColor = Color.Gold;
			}

			if (this.AggroRedColor.IsEmpty)
			{
				this.AggroRedColor = Color.Red;
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

		/// <summary>
		/// Builds the character registry out of the entries stored by the earlier versions
		/// of the configuration file and keeps the registry consistent afterwards
		/// </summary>
		/// <summary>
		/// Moves the legacy single hotkey of a client into the list of its hotkeys.
		/// The legacy entries are read but never written back, the same way the fixed
		/// cycle group entries are handled
		/// </summary>
		private void EnsureClientHotkeys()
		{
			this.ClientHotkeys = this.ClientHotkeys ?? new Dictionary<string, List<string>>();

			if (this.ClientHotkey != null)
			{
				foreach (KeyValuePair<string, string> entry in this.ClientHotkey)
				{
					this.AddClientHotkey(entry.Key, entry.Value);
				}

				this.ClientHotkey.Clear();
			}

			foreach (string client in this.ClientHotkeys.Keys.ToList())
			{
				// Stored in the canonical form, so that a hand edited file does not end up
				// with the same combination spelled two different ways
				List<string> hotkeys = this.ClientHotkeys[client]
											.Where(hotkey => !string.IsNullOrEmpty(hotkey))
											.Select(this.NormalizeHotkey)
											.Distinct(StringComparer.OrdinalIgnoreCase)
											.ToList();

				if (hotkeys.Count == 0)
				{
					this.ClientHotkeys.Remove(client);
					continue;
				}

				this.ClientHotkeys[client] = hotkeys;
			}
		}

		private void EnsureCharacters()
		{
			this.Characters = this.Characters ?? new List<CharacterInfo>();
			this.CharacterGroups = this.CharacterGroups ?? new List<CharacterGroup>();
			this.ClientPreviewSettings = this.ClientPreviewSettings ?? new Dictionary<string, PreviewSettings>();

			if (this.ConfigVersion < ThumbnailConfiguration.CONFIG_VERSION_CHARACTER_REGISTRY)
			{
				this.MigrateLegacyClientEntries();
				this.ConfigVersion = ThumbnailConfiguration.CONFIG_VERSION_CHARACTER_REGISTRY;
			}

			this.SanitizeCharacterRegistry();
		}

		/// <summary>
		/// Every client title the configuration has ever stored a setting for is a character
		/// the user has logged in at least once, so the registry starts fully populated
		/// instead of waiting for every character to log in once again
		/// </summary>
		private void MigrateLegacyClientEntries()
		{
			foreach (string title in this.CollectStoredClientTitles())
			{
				if (this.Characters.Any(character => string.Equals(character.Title, title, StringComparison.Ordinal)))
				{
					continue;
				}

				// The moment these characters were seen for the first time is unknown
				this.Characters.Add(new CharacterInfo(title, null, null));
			}

			foreach (KeyValuePair<string, Size> entry in this.PerClientThumbnailSize)
			{
				PreviewSettings settings = this.GetLegacySettingsTarget(entry.Key);

				if (settings != null)
				{
					settings.ThumbnailSize = entry.Value;
				}
			}

			foreach (KeyValuePair<string, ZoomAnchor> entry in this.PerClientZoomAnchor)
			{
				PreviewSettings settings = this.GetLegacySettingsTarget(entry.Key);

				if (settings != null)
				{
					settings.ThumbnailZoomAnchor = entry.Value;
				}
			}

			foreach (KeyValuePair<string, bool> entry in this.PerClientPreventPreviews)
			{
				PreviewSettings settings = this.GetLegacySettingsTarget(entry.Key);

				if (settings != null)
				{
					settings.PreventPreviews = entry.Value;
				}
			}

			foreach (KeyValuePair<string, Color> entry in this.PerClientPreventPreviewColor)
			{
				PreviewSettings settings = this.GetLegacySettingsTarget(entry.Key);

				if (settings != null)
				{
					settings.PreventPreviewColor = entry.Value;
				}
			}

			foreach (KeyValuePair<string, Color> entry in this.PerClientActiveClientHighlightColor)
			{
				PreviewSettings settings = this.GetLegacySettingsTarget(entry.Key);

				if (settings != null)
				{
					settings.ActiveClientHighlightColor = entry.Value;
				}
			}
		}

		/// <summary>
		/// Preview settings entry a legacy per-client value is migrated into. Legacy values
		/// were applied unconditionally, so the migrated entry is switched on right away
		/// </summary>
		private PreviewSettings GetLegacySettingsTarget(string title)
		{
			if (!ThumbnailConfiguration.IsStoredClientTitle(title))
			{
				return null;
			}

			if (!this.ClientPreviewSettings.TryGetValue(title, out PreviewSettings settings))
			{
				settings = new PreviewSettings();
				this.ClientPreviewSettings[title] = settings;
			}

			settings.UseCustomSettings = true;

			return settings;
		}

		private HashSet<string> CollectStoredClientTitles()
		{
			HashSet<string> titles = new HashSet<string>(StringComparer.Ordinal);

			void Collect(IEnumerable<string> source)
			{
				if (source == null)
				{
					return;
				}

				foreach (string title in source)
				{
					if (ThumbnailConfiguration.IsStoredClientTitle(title))
					{
						titles.Add(title);
					}
				}
			}

			Collect(this.FlatLayout?.Keys);
			Collect(this.ClientLayout?.Keys);
			Collect(this.ClientHotkey?.Keys);
			Collect(this.ClientHotkeys?.Keys);
			Collect(this.DisableThumbnail?.Keys);
			Collect(this.PriorityClients);
			Collect(this.PerClientThumbnailSize?.Keys);
			Collect(this.PerClientZoomAnchor?.Keys);
			Collect(this.PerClientPreventPreviews?.Keys);
			Collect(this.PerClientPreventPreviewColor?.Keys);
			Collect(this.PerClientActiveClientHighlightColor?.Keys);

			if (this.PerClientLayout != null)
			{
				Collect(this.PerClientLayout.Keys);

				foreach (Dictionary<string, Point> layout in this.PerClientLayout.Values)
				{
					Collect(layout?.Keys);
				}
			}

			if (this.CycleGroups != null)
			{
				foreach (CycleGroup group in this.CycleGroups)
				{
					Collect(group.ClientsOrder?.Keys);
				}
			}

			return titles;
		}

		/// <summary>
		/// Filters out the client titles that do not belong to a character: the login
		/// screen and the sample entries older builds seeded fresh configurations with
		/// </summary>
		private static bool IsStoredClientTitle(string title)
		{
			return CharacterInfo.IsCharacterTitle(title)
					&& !title.StartsWith(ThumbnailConfiguration.EXAMPLE_CLIENT_TITLE_PREFIX, StringComparison.Ordinal);
		}

		private void SanitizeCharacterRegistry()
		{
			lock (this._registrySyncRoot)
			{
				Dictionary<string, CharacterInfo> index = new Dictionary<string, CharacterInfo>(StringComparer.Ordinal);
				List<CharacterInfo> characters = new List<CharacterInfo>(this.Characters.Count);

				foreach (CharacterInfo character in this.Characters)
				{
					if ((character == null) || !ThumbnailConfiguration.IsStoredClientTitle(character.Title)
						|| index.ContainsKey(character.Title))
					{
						continue;
					}

					index.Add(character.Title, character);
					characters.Add(character);
				}

				// Entries edited by hand can carry empty or duplicate group ids
				List<CharacterGroup> groups = new List<CharacterGroup>(this.CharacterGroups.Count);
				HashSet<string> groupIds = new HashSet<string>(StringComparer.Ordinal);

				foreach (CharacterGroup group in this.CharacterGroups)
				{
					if ((group == null) || string.IsNullOrWhiteSpace(group.Id) || !groupIds.Add(group.Id))
					{
						continue;
					}

					if (string.IsNullOrWhiteSpace(group.Name))
					{
						group.Name = group.Id;
					}

					group.Hotkeys = group.Hotkeys ?? new List<string>();

					if (group.Color.IsEmpty)
					{
						group.Color = ThumbnailConfiguration.GROUP_COLOR_PALETTE[groups.Count % ThumbnailConfiguration.GROUP_COLOR_PALETTE.Length];
					}

					groups.Add(group);
				}

				foreach (CharacterInfo character in characters)
				{
					if (!string.IsNullOrEmpty(character.GroupId) && !groupIds.Contains(character.GroupId))
					{
						character.GroupId = null;
					}
				}

				groups.RemoveAll(group => !characters.Any(character => string.Equals(character.GroupId, group.Id, StringComparison.Ordinal)));

				this.Characters = characters;
				this.CharacterGroups = groups;
				this._characterIndex = index;

				foreach (string title in this.ClientPreviewSettings.Keys.ToList())
				{
					PreviewSettings settings = this.ClientPreviewSettings[title];

					if ((settings == null) || settings.IsEmpty || !ThumbnailConfiguration.IsStoredClientTitle(title))
					{
						this.ClientPreviewSettings.Remove(title);
					}
				}
			}
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