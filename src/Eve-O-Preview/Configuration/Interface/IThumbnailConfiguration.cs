using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace EveOPreview.Configuration
{
	public interface IThumbnailConfiguration
	{
		List<CycleGroup> CycleGroups { get; set; }

		Dictionary<string, bool> CycleGroupExclusions { get; set; }

		bool MinimizeToTray { get; set; }
		int ThumbnailRefreshPeriod { get; set; }
		int ThumbnailResizeTimeoutPeriod { get; set; }
		bool EnableMinimizedClientsRefresh { get; set; }
		int MinimizedClientsRefreshPeriod { get; set; }
		bool EnableWineCompatibilityMode { get; set; }

		double ThumbnailOpacity { get; set; }

		bool EnableClientLayoutTracking { get; set; }
		bool HideActiveClientThumbnail { get; set; }
		bool HideLoginClientThumbnail { get; set; }
		bool MinimizeInactiveClients { get; set; }
		bool HideCaptionOnClients { get; set; }
		AnimationStyle WindowsAnimationStyle { get; set; }
		bool ShowThumbnailsAlwaysOnTop { get; set; }
		bool EnablePerClientThumbnailLayouts { get; set; }

		bool PreventPreviews { get; set; }
		bool HideThumbnailsOnLostFocus { get; set; }
		int HideThumbnailsDelay { get; set; }

		Size ThumbnailSize { get; set; }
		Size ThumbnailMinimumSize { get; set; }
		Size ThumbnailMaximumSize { get; set; }

		bool EnableThumbnailSnap { get; set; }

		bool ThumbnailZoomEnabled { get; set; }
		int ThumbnailZoomFactor { get; set; }
		ZoomAnchor ThumbnailZoomAnchor { get; set; }
		ZoomAnchor OverlayLabelAnchor { get; set; }
		ZoomAnchor CycleGroupIndicatorAnchor { get; set; }

		bool ShowThumbnailOverlays { get; set; }
		bool ShowClientName { get; set; }
		bool ShowCycleGroupName { get; set; }
		bool OverlayAlwaysOnTop { get; set; }
		bool ShowThumbnailFrames { get; set; }

		Color CycleGroupNameColor { get; set; }
		Font CycleGroupNameFont { get; set; }
		bool LockThumbnailLocation { get; set; }
		bool ThumbnailSnapToGrid {  get; set; }
		bool ThumbnailSnapToGridFillCell { get; set; }
		int ThumbnailSnapToGridOffsetX { get; set; }
		int ThumbnailSnapToGridOffsetY { get; set; }
		int ThumbnailSnapToGridCellPadding { get; set; }
		int ThumbnailSnapToGridSizeX { get; set; }
		int ThumbnailSnapToGridSizeY { get; set; }

		bool EnableActiveClientHighlight { get; set; }
		Color ActiveClientHighlightColor { get; set; }
		Color PreventPreviewColor { get; set; }
		int ActiveClientHighlightThickness { get; set; }
		Color OverlayLabelColor { get; set; }
		Font OverlayLabelFont { get; set; }

		string IconName { get; set; }
		string Language { get; set; }
		List<string> MinimizeAllClientsHotkeys { get; set; }
		List<string> ToggleAllPreviewsHotkeys { get; set; }
		List<string> ClickThroughHotkeys { get; set; }
		bool OverlayLabelOutlineEnabled { get; set; }
		int OverlayLabelOutlineThickness { get; set; }
		Color OverlayLabelOutlineColor { get; set; }
		bool CycleGroupNameOutlineEnabled { get; set; }
		int CycleGroupNameOutlineThickness { get; set; }
		Color CycleGroupNameOutlineColor { get; set; }

		/// <summary>Clicks on a preview window, stored as "Shift+LButton" and the like</summary>
		string PreviewClickMinimize { get; set; }
		string PreviewClickSwitchOut { get; set; }
		string PreviewClickToggleCycleGroup { get; set; }

		Point LoginThumbnailLocation { get; set; }
		Size MainWindowSize { get; set; }

		/// <summary>Master switch of the game log reading (Documents\EVE\logs\Gamelogs)</summary>
		bool EnableGameLogMonitor { get; set; }

		/// <summary>Game log folder override; an empty value means the default Documents\EVE\logs\Gamelogs</summary>
		string GameLogsFolder { get; set; }

		/// <summary>Flashing yellow/red frames on the previews of clients under NPC attack</summary>
		bool EnableAggroFrames { get; set; }
		Color AggroYellowColor { get; set; }
		Color AggroRedColor { get; set; }

		/// <summary>How deep the edge-to-center gradient reaches: 100 fills the whole preview</summary>
		int AggroFillPercent { get; set; }

		#region Character registry
		/// <summary>Every character that has been seen logged in at least once</summary>
		IReadOnlyList<CharacterInfo> GetCharacters();

		CharacterInfo GetCharacter(string title);

		/// <summary>Adds the character to the registry. True when the registry has changed</summary>
		bool RegisterCharacter(string title);

		/// <summary>Drops the character and every setting stored for it</summary>
		bool ForgetCharacter(string title);

		/// <summary>Blacklists the character: it stays stored but is hidden from every list</summary>
		void SetCharacterIgnored(string title, bool ignored);

		void SetCharacterGroupColor(string groupId, Color color);

		IReadOnlyList<CharacterGroup> GetCharacterGroups();
		CharacterGroup GetCharacterGroupById(string groupId);
		CharacterGroup GetCharacterGroupOf(string title);
		IReadOnlyList<string> GetGroupMembers(string groupId);

		/// <summary>Clients a setting written for this one has to be written for as well</summary>
		IReadOnlyList<string> GetLinkedClients(string title);

		/// <summary>Puts the characters into one group. True when the registry has changed</summary>
		bool LinkCharacters(IEnumerable<string> titles);

		void SetCharacterGroup(string title, string groupId);
		CharacterGroup CreateCharacterGroup(string name);
		void RenameCharacterGroup(string groupId, string name);
		void SetGroupManageAsWhole(string groupId, bool value);
		void RemoveCharacterGroup(string groupId);
		#endregion

		#region Per-window preview settings
		PreviewSettings GetPreviewSettings(string title);
		void SetPreviewSettings(string title, PreviewSettings settings);

		/// <summary>Settings of the client with the global values filled in where it has none</summary>
		PreviewSettings ResolvePreviewSettings(string title);

		bool GetPreventPreviews(string currentClient, bool defaultValue);
		Color GetPreventPreviewColor(string currentClient, Color defaultColor);
		Color GetActiveClientHighlightColor(string currentClient, Color defaultColor);
		#endregion

		Point GetThumbnailLocation(string currentClient, string activeClient, Point defaultLocation);
		Size GetThumbnailSize(string currentClient, string activeClient, Size defaultSize);
		ZoomAnchor GetZoomAnchor(string currentClient, ZoomAnchor defaultZoomAnchor);
		void SetThumbnailLocation(string currentClient, string activeClient, Point location);

		ClientLayout GetClientLayout(string currentClient);
		void SetClientLayout(string currentClient, ClientLayout layout);

		/// <summary>Hotkeys of one client - an action can be reached by more than one of them</summary>
		IReadOnlyList<string> GetClientHotkeys(string currentClient);

		IReadOnlyDictionary<string, IReadOnlyList<string>> GetClientHotkeys();
		void AddClientHotkey(string currentClient, string hotkey);
		void SetClientHotkeys(string currentClient, IEnumerable<string> hotkeys);
		void RemoveClientHotkey(string currentClient, string hotkey);
		Keys StringToKey(string hotkey);

		/// <summary>Canonical form used to compare and store hotkeys</summary>
		string NormalizeHotkey(string hotkey);
		bool IsPriorityClient(string currentClient);
		bool IsExecutableToPreview(string processName);

		bool IsThumbnailDisabled(string currentClient);
		void ToggleThumbnail(string currentClient, bool isDisabled);

		void ApplyRestrictions();
	}
}