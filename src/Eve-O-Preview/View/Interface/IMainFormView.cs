using System;
using System.Collections.Generic;
using System.Drawing;

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

		bool LockThumbnailLocation { get; set; }
		bool ThumbnailSnapToGrid { get; set; }
		int ThumbnailSnapToGridSizeX { get; set; }
		int ThumbnailSnapToGridSizeY { get; set; }

		bool EnableActiveClientHighlight { get; set; }
		int ActiveClientHighlightThickness { get; set; }
		Color ActiveClientHighlightColor { get; set; }
		Color PreventPreviewColor { get; set; }
		Color OverlayLabelColor { get; set; }
		Font OverlayLabelFont { get; set; }

		string IconName { get; set; }

		Size WindowSize { get; set; }

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
		Action WindowSizeChanged { get; set; }
	}
}