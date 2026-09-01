using System;
using System.Collections.Generic;
using System.Drawing;
using EveOPreview.Configuration;

namespace EveOPreview.View
{
	/// <summary>
	/// Main view interface
	/// Presenter uses it to access GUI properties
	/// </summary>
	public interface IMainFormView : IView
	{
		bool MinimizeToTray { get; set; }

		double ThumbnailOpacity { get; set; }

		bool EnableMinimizedClientsRefresh { get; set; }
		int ThumbnailRefreshPeriod { get; set; }
		int MinimizedClientsRefreshPeriod { get; set; }

		bool EnableClientLayoutTracking { get; set; }
		bool HideActiveClientThumbnail { get; set; }
		string Language { get; set; }
		void ShowWarning(string title, string message);
		bool MinimizeInactiveClients { get; set; }
		bool HideCaptionOnClients { get; set; }
		ViewAnimationStyle WindowsAnimationStyle { get; set; }
        bool ShowThumbnailsAlwaysOnTop { get; set; }
		bool PreventPreviews { get; set; }
		bool HideThumbnailsOnLostFocus { get; set; }
		bool EnablePerClientThumbnailLayouts { get; set; }

		Size ThumbnailSize { get; set; }

		bool EnableThumbnailZoom { get; set; }
		int ThumbnailZoomFactor { get; set; }
		ViewZoomAnchor ThumbnailZoomAnchor { get; set; }
		ViewZoomAnchor OverlayLabelAnchor { get; set; }
		ViewZoomAnchor CycleGroupIndicatorAnchor { get; set; }

		bool ShowThumbnailOverlays { get; set; }
		bool ShowClientName { get; set; }
		bool ShowCycleGroupName { get; set; }
		bool OverlayAlwaysOnTop { get; set; }
		bool ShowThumbnailFrames { get; set; }

		Color CycleGroupNameColor { get; set; }
		Font CycleGroupNameFont { get; set; }

		/// <summary>Clicks on a preview window, stored as "Shift+LButton" and the like</summary>
		string PreviewClickMinimize { get; set; }
		string PreviewClickSwitchOut { get; set; }
		string PreviewClickToggleCycleGroup { get; set; }

		bool LockThumbnailLocation { get; set; }
		bool ThumbnailSnapToGrid { get; set; }
		bool ThumbnailSnapToGridFillCell { get; set; }
		int ThumbnailSnapToGridSizeX { get; set; }
		int ThumbnailSnapToGridSizeY { get; set; }
		int ThumbnailSnapToGridOffsetX { get; set; }
		int ThumbnailSnapToGridOffsetY { get; set; }
		int ThumbnailSnapToGridCellPadding { get; set; }

		bool EnableActiveClientHighlight { get; set; }
		int ActiveClientHighlightThickness { get; set; }
		Color ActiveClientHighlightColor { get; set; }
		Color PreventPreviewColor { get; set; }
		Color OverlayLabelColor { get; set; }
		Font OverlayLabelFont { get; set; }
		bool OverlayLabelOutlineEnabled { get; set; }
		int OverlayLabelOutlineThickness { get; set; }
		Color OverlayLabelOutlineColor { get; set; }
		bool CycleGroupNameOutlineEnabled { get; set; }
		int CycleGroupNameOutlineThickness { get; set; }
		Color CycleGroupNameOutlineColor { get; set; }

		string IconName { get; set; }

		Size WindowSize { get; set; }

		bool EnableGameLogMonitor { get; set; }
		string GameLogsFolder { get; set; }
		bool EnableAggroFrames { get; set; }
		Color AggroYellowColor { get; set; }
		Color AggroRedColor { get; set; }
		int AggroFillPercent { get; set; }

		void SetDocumentationUrl(string url);
		void SetVersionInfo(string version);
		void SetThumbnailSizeLimitations(Size minimumSize, Size maximumSize);

		void Minimize();

		void AddThumbnails(IList<IThumbnailDescription> thumbnails);
		void RemoveThumbnails(IList<IThumbnailDescription> thumbnails);
		void RefreshZoomSettings();

		void SetHotkeyActions(IList<(string ActionId, string DisplayName)> actions);
		void SetHotkeyBindings(IList<(string ActionId, string ActionName, string Hotkey)> bindings);
		void SetHotkeyStatus(string status);

		/// <summary>Asks the user a yes/no question. True when the answer is yes</summary>
		bool ShowQuestion(string title, string message);

		void SetCharacters(IList<CharacterGroupViewItem> groups, IList<CharacterViewItem> characters);

		/// <summary>
		/// Opens the preview settings editor of one client. 'values' carries the settings
		/// the client uses right now, 'globals' the values it would use without its own ones
		/// </summary>
		void ShowPreviewSettings(string title, string caption, string groupHint, PreviewSettings values, PreviewSettings globals);

		void SetActiveClients(IList<string> clients);
		void SetCycleGroups(IList<(string Name, IList<string> Clients)> groups);
		void SetClientCycleGroups(IDictionary<string, IList<string>> clientGroups);
		void SelectCycleGroup(string groupName);

		Action ApplicationExitRequested { get; set; }
		Action FormActivated { get; set; }
		Action FormMinimized { get; set; }
		Action<ViewCloseRequest> FormCloseRequested { get; set; }
		Action ApplicationSettingsChanged { get; set; }
		Action ThumbnailsSizeChanged { get; set; }
		Action<string> ThumbnailStateChanged { get; set; }
		Action DocumentationLinkActivated { get; set; }
		Action<string, string> HotkeyBindingAssigned { get; set; }
		Action<string, string> HotkeyBindingRemoved { get; set; }
		Action<string, string, string, string> HotkeyBindingEdited { get; set; }
		Action<string, IList<string>> CycleGroupClientsChanged { get; set; }
		Action<string, string> ThumbnailCycleGroupChanged { get; set; }
		Action CycleGroupAddRequested { get; set; }
		Action<string> CycleGroupRemoveRequested { get; set; }
		Action<string, string> CycleGroupRenameRequested { get; set; }
		Action<bool> HotkeyCaptureModeChanged { get; set; }
		/// <summary>(character title, group id or null to detach it)</summary>
		Action<string, string> CharacterGroupChanged { get; set; }

		/// <summary>(character title, name of the group to create for it)</summary>
		Action<string, string> CharacterGroupCreateRequested { get; set; }

		Action<string, string> CharacterGroupRenameRequested { get; set; }
		Action<string> CharacterGroupRemoveRequested { get; set; }
		Action<string, bool> CharacterGroupManageAsWholeChanged { get; set; }
		Action<string> CharacterForgetRequested { get; set; }
		Action CharacterGroupsSuggestionRequested { get; set; }

		/// <summary>(character title, blacklisted)</summary>
		Action<string, bool> CharacterIgnoreChanged { get; set; }

		Action<string, Color> CharacterGroupColorChanged { get; set; }
		Action<string> CharacterPreviewSettingsRequested { get; set; }

		/// <summary>(character title, edited settings or null to follow the global ones)</summary>
		Action<string, PreviewSettings> CharacterPreviewSettingsChanged { get; set; }

		Action WindowSizeChanged { get; set; }
		Action AggroTestRequested { get; set; }
	}
}