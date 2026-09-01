using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using EveOPreview.Configuration;
using EveOPreview.Localization;
using EveOPreview.Mediator.Messages;
using EveOPreview.UI.Hotkeys;
using EveOPreview.View;
using MediatR;

namespace EveOPreview.Presenters
{
	public class MainFormPresenter : Presenter<IMainFormView>, IMainFormPresenter
	{
		#region Private constants
		private const string FORUM_URL = @"https://forums.eveonline.com/t/eve-o-preview-v8-0-2-0";

		private const string HOTKEY_ACTION_CLIENT_PREFIX = "client:";
		private const string HOTKEY_ACTION_ACCOUNT_PREFIX = "account:";
		private const string HOTKEY_ACTION_MINIMIZE_ALL = "minimizeall";
		private const string HOTKEY_ACTION_TOGGLE_ALL_PREVIEWS = "toggleallpreviews";
		private const string HOTKEY_ACTION_CLICK_THROUGH = "toggleclickthrough";
		#endregion

		#region Private fields
		private readonly IMediator _mediator;
		private readonly IThumbnailConfiguration _configuration;
		private readonly IConfigurationStorage _configurationStorage;
		private readonly IDictionary<string, IThumbnailDescription> _descriptionsCache;
		private bool _suppressSizeNotifications;
		private bool _settingsLoaded;

		private bool _exitApplication;
		#endregion

		public MainFormPresenter(IApplicationController controller, IMainFormView view, IMediator mediator, IThumbnailConfiguration configuration, IConfigurationStorage configurationStorage)
			: base(controller, view)
		{
			this._mediator = mediator;
			this._configuration = configuration;
			this._configurationStorage = configurationStorage;

			this._descriptionsCache = new Dictionary<string, IThumbnailDescription>();

			this._suppressSizeNotifications = false;
			this._settingsLoaded = false;
			this._exitApplication = false;

			this.View.FormActivated = this.Activate;
			this.View.FormMinimized = this.Minimize;
			this.View.FormCloseRequested = this.Close;
			this.View.ApplicationSettingsChanged = this.SaveApplicationSettings;
			this.View.ThumbnailsSizeChanged = this.UpdateThumbnailsSize;
			this.View.ThumbnailStateChanged = this.UpdateThumbnailState;
			this.View.DocumentationLinkActivated = this.OpenDocumentationLink;
			this.View.ApplicationExitRequested = this.ExitApplication;
			this.View.HotkeyBindingsChanged = this.ChangeHotkeyBindings;
			this.View.HotkeyBindingsRemoved = this.RemoveHotkeyBindings;
			this.View.CycleGroupClientsChanged = this.ChangeCycleGroupClients;
			this.View.ThumbnailCycleGroupChanged = this.ChangeThumbnailCycleGroup;
			this.View.CycleGroupAddRequested = this.AddCycleGroup;
			this.View.CycleGroupRemoveRequested = this.RemoveCycleGroup;
			this.View.CycleGroupRenameRequested = this.RenameCycleGroup;
			this.View.HotkeyCaptureModeChanged = this.ChangeHotkeyCaptureMode;
			this.View.WindowSizeChanged = this.SaveWindowSize;
			this.View.AggroTestRequested = this.TestAggroFrames;
			this.View.CharacterGroupChanged = this.ChangeCharacterGroup;
			this.View.CharacterGroupCreateRequested = this.CreateCharacterGroup;
			this.View.CharacterGroupRenameRequested = this.RenameCharacterGroup;
			this.View.CharacterGroupRemoveRequested = this.RemoveCharacterGroup;
			this.View.CharacterGroupManageAsWholeChanged = this.ChangeCharacterGroupManagement;
			this.View.CharacterForgetRequested = this.ForgetCharacter;
			this.View.CharacterGroupsSuggestionRequested = this.SuggestCharacterGroups;
			this.View.CharacterIgnoreChanged = this.ChangeCharacterIgnored;
			this.View.CharacterGroupColorChanged = this.ChangeCharacterGroupColor;
			this.View.CharacterPreviewSettingsRequested = this.EditPreviewSettings;
			this.View.CharacterPreviewSettingsChanged = this.ApplyPreviewSettings;

			this.View.IconName = this._configuration.IconName;
		}

		private void Activate()
		{
			this._suppressSizeNotifications = true;
			this.LoadApplicationSettings();
			this.ReportBrokenConfiguration();
			this.RefreshHotkeyActions();
			this.RefreshHotkeyBindings();
			this.RefreshCycleGroupData();
			this.View.SetDocumentationUrl(MainFormPresenter.FORUM_URL);
			this.View.SetVersionInfo(this.GetApplicationVersion());
			if (this._configuration.MinimizeToTray)
			{
				this.View.Minimize();
			}

			this._mediator.Send(new StartService());
			this._suppressSizeNotifications = false;
		}

		private void Minimize()
		{
			if (!this._configuration.MinimizeToTray)
			{
				return;
			}

			this.View.Hide();
		}

		private void Close(ViewCloseRequest request)
		{
			if (this._exitApplication || !this.View.MinimizeToTray)
			{
				this._mediator.Send(new StopService()).Wait();

				this._configurationStorage.Save();

				// Settings are written on a worker task - wait for the write to land
				// before letting the process exit
				this._configurationStorage.Flush();
				request.Allow = true;
				return;
			}

			request.Allow = false;
			this.View.Minimize();
		}

		private async void UpdateThumbnailsSize()
		{
			if (!this._suppressSizeNotifications)
			{
				this.SaveApplicationSettings();
				await this._mediator.Publish(new ThumbnailConfiguredSizeUpdated());
			}
		}

		private void LoadApplicationSettings()
		{
			this._configurationStorage.Load();

			this.View.MinimizeToTray = this._configuration.MinimizeToTray;
			this.View.Language = this._configuration.Language;

			this.View.ThumbnailOpacity = this._configuration.ThumbnailOpacity;
			this.View.ThumbnailRefreshPeriod = this._configuration.ThumbnailRefreshPeriod;
			this.View.MinimizedClientsRefreshPeriod = this._configuration.MinimizedClientsRefreshPeriod;
			this.View.EnableMinimizedClientsRefresh = this._configuration.EnableMinimizedClientsRefresh;

			this.View.EnableClientLayoutTracking = this._configuration.EnableClientLayoutTracking;
			this.View.HideActiveClientThumbnail = this._configuration.HideActiveClientThumbnail;
			this.View.MinimizeInactiveClients = this._configuration.MinimizeInactiveClients;
			this.View.HideCaptionOnClients = this._configuration.HideCaptionOnClients;
			this.View.WindowsAnimationStyle = ViewAnimationStyleConverter.Convert(this._configuration.WindowsAnimationStyle);
			this.View.ShowThumbnailsAlwaysOnTop = this._configuration.ShowThumbnailsAlwaysOnTop;
			this.View.PreventPreviews = this._configuration.PreventPreviews;
			this.View.HideThumbnailsOnLostFocus = this._configuration.HideThumbnailsOnLostFocus;
			this.View.EnablePerClientThumbnailLayouts = this._configuration.EnablePerClientThumbnailLayouts;

			this.View.SetThumbnailSizeLimitations(this._configuration.ThumbnailMinimumSize, this._configuration.ThumbnailMaximumSize);
			this.View.ThumbnailSize = this._configuration.ThumbnailSize;

			this.View.EnableThumbnailZoom = this._configuration.ThumbnailZoomEnabled;
			this.View.ThumbnailZoomFactor = this._configuration.ThumbnailZoomFactor;
			this.View.ThumbnailZoomAnchor = ViewZoomAnchorConverter.Convert(this._configuration.ThumbnailZoomAnchor);
			this.View.OverlayLabelAnchor = ViewZoomAnchorConverter.Convert(this._configuration.OverlayLabelAnchor);
			this.View.CycleGroupIndicatorAnchor = ViewZoomAnchorConverter.Convert(this._configuration.CycleGroupIndicatorAnchor);

			this.View.ShowThumbnailOverlays = this._configuration.ShowThumbnailOverlays;
			this.View.ShowClientName = this._configuration.ShowClientName;
			this.View.ShowCycleGroupName = this._configuration.ShowCycleGroupName;
			this.View.OverlayAlwaysOnTop = this._configuration.OverlayAlwaysOnTop;
			this.View.CycleGroupNameColor = this._configuration.CycleGroupNameColor;
			this.View.CycleGroupNameFont = this._configuration.CycleGroupNameFont;
			this.View.ShowThumbnailFrames = this._configuration.ShowThumbnailFrames;
			this.View.LockThumbnailLocation = this._configuration.LockThumbnailLocation;

			this.View.PreviewClickMinimize = this._configuration.PreviewClickMinimize;
			this.View.PreviewClickSwitchOut = this._configuration.PreviewClickSwitchOut;
			this.View.PreviewClickToggleCycleGroup = this._configuration.PreviewClickToggleCycleGroup;
			this.View.ThumbnailSnapToGrid = this._configuration.ThumbnailSnapToGrid;
			this.View.ThumbnailSnapToGridSizeX = this._configuration.ThumbnailSnapToGridSizeX;
			this.View.ThumbnailSnapToGridSizeY = this._configuration.ThumbnailSnapToGridSizeY;
			this.View.ThumbnailSnapToGridOffsetX = this._configuration.ThumbnailSnapToGridOffsetX;
			this.View.ThumbnailSnapToGridOffsetY = this._configuration.ThumbnailSnapToGridOffsetY;
			this.View.ThumbnailSnapToGridFillCell = this._configuration.ThumbnailSnapToGridFillCell;
			this.View.ThumbnailSnapToGridCellPadding = this._configuration.ThumbnailSnapToGridCellPadding;
			this.View.EnableActiveClientHighlight = this._configuration.EnableActiveClientHighlight;
			this.View.ActiveClientHighlightThickness = this._configuration.ActiveClientHighlightThickness;
			this.View.ActiveClientHighlightColor = this._configuration.ActiveClientHighlightColor;
			this.View.PreventPreviewColor = this._configuration.PreventPreviewColor;

			this.View.OverlayLabelColor = this._configuration.OverlayLabelColor;
			this.View.OverlayLabelFont = this._configuration.OverlayLabelFont;
			this.View.OverlayLabelOutlineEnabled = this._configuration.OverlayLabelOutlineEnabled;
			this.View.OverlayLabelOutlineThickness = this._configuration.OverlayLabelOutlineThickness;
			this.View.OverlayLabelOutlineColor = this._configuration.OverlayLabelOutlineColor;
			this.View.CycleGroupNameOutlineEnabled = this._configuration.CycleGroupNameOutlineEnabled;
			this.View.CycleGroupNameOutlineThickness = this._configuration.CycleGroupNameOutlineThickness;
			this.View.CycleGroupNameOutlineColor = this._configuration.CycleGroupNameOutlineColor;


			this.View.EnableGameLogMonitor = this._configuration.EnableGameLogMonitor;
			this.View.GameLogsFolder = this._configuration.GameLogsFolder;
			this.View.EnableAggroFrames = this._configuration.EnableAggroFrames;
			this.View.AggroYellowColor = this._configuration.AggroYellowColor;
			this.View.AggroRedColor = this._configuration.AggroRedColor;
			this.View.AggroFillPercent = this._configuration.AggroFillPercent;

			this.View.IconName = this._configuration.IconName;

			this.View.WindowSize = this._configuration.MainWindowSize;

			this._settingsLoaded = true;
		}

		// Losing every setting without a word looks like the app forgot them on its own,
		// so an unreadable settings file is reported instead of being silently replaced
		private void ReportBrokenConfiguration()
		{
			string brokenFile = this._configurationStorage.BrokenConfigurationFileName;

			if (brokenFile == null)
			{
				return;
			}

			string message = string.Format(
				this._configurationStorage.IsSaveBlocked ? Strings.Config_BrokenKept : Strings.Config_BrokenMovedAside,
				brokenFile);

			this.View.ShowWarning(Strings.Config_BrokenTitle, message);
		}

		private async void SaveWindowSize()
		{
			Size windowSize = this.View.WindowSize;

			if (this._configuration.MainWindowSize == windowSize)
			{
				return;
			}

			this._configuration.MainWindowSize = windowSize;
			await this._mediator.Send(new SaveConfiguration());
		}

		private async void SaveApplicationSettings()
		{
			// Saving copies the view state into the configuration, so a save triggered before
			// the settings reached the view would overwrite the stored config with empty values
			if (!this._settingsLoaded)
			{
				return;
			}

			this._configuration.MinimizeToTray = this.View.MinimizeToTray;
			this._configuration.Language = this.View.Language;

			this._configuration.ThumbnailOpacity = (float)this.View.ThumbnailOpacity;
			this._configuration.ThumbnailRefreshPeriod = this.View.ThumbnailRefreshPeriod;
			this._configuration.MinimizedClientsRefreshPeriod = this.View.MinimizedClientsRefreshPeriod;
			this._configuration.EnableMinimizedClientsRefresh = this.View.EnableMinimizedClientsRefresh;

			this._configuration.EnableClientLayoutTracking = this.View.EnableClientLayoutTracking;
			this._configuration.HideActiveClientThumbnail = this.View.HideActiveClientThumbnail;
			this._configuration.MinimizeInactiveClients = this.View.MinimizeInactiveClients;

			if (this._configuration.HideCaptionOnClients != this.View.HideCaptionOnClients ) {
				this._configuration.HideCaptionOnClients = this.View.HideCaptionOnClients;
				await this._mediator.Publish(new ThumbnailFrameSettingsUpdated());
			}
			this._configuration.WindowsAnimationStyle = ViewAnimationStyleConverter.Convert(this.View.WindowsAnimationStyle); 
            this._configuration.ShowThumbnailsAlwaysOnTop = this.View.ShowThumbnailsAlwaysOnTop;

			if (this._configuration.PreventPreviews != this.View.PreventPreviews)
			{
				this._configuration.PreventPreviews = this.View.PreventPreviews;
				await this._mediator.Publish(new ThumbnailFrameSettingsUpdated());
			}

			this._configuration.HideThumbnailsOnLostFocus = this.View.HideThumbnailsOnLostFocus;
			this._configuration.EnablePerClientThumbnailLayouts = this.View.EnablePerClientThumbnailLayouts;

			this._configuration.ThumbnailSize = this.View.ThumbnailSize;

			this._configuration.ThumbnailZoomEnabled = this.View.EnableThumbnailZoom;
			this._configuration.ThumbnailZoomFactor = this.View.ThumbnailZoomFactor;
			this._configuration.ThumbnailZoomAnchor = ViewZoomAnchorConverter.Convert(this.View.ThumbnailZoomAnchor);
			this._configuration.OverlayLabelAnchor = ViewZoomAnchorConverter.Convert(this.View.OverlayLabelAnchor);

			if (this._configuration.CycleGroupIndicatorAnchor != ViewZoomAnchorConverter.Convert(this.View.CycleGroupIndicatorAnchor))
			{
				this._configuration.CycleGroupIndicatorAnchor = ViewZoomAnchorConverter.Convert(this.View.CycleGroupIndicatorAnchor);
				await this._mediator.Publish(new ThumbnailCycleGroupIndicatorUpdated());
			}

			this._configuration.ShowThumbnailOverlays = this.View.ShowThumbnailOverlays;
			this._configuration.ShowClientName = this.View.ShowClientName;
			this._configuration.ShowCycleGroupName = this.View.ShowCycleGroupName;
			this._configuration.OverlayAlwaysOnTop = this.View.OverlayAlwaysOnTop;
			this._configuration.CycleGroupNameColor = this.View.CycleGroupNameColor;
			this._configuration.CycleGroupNameFont = this.View.CycleGroupNameFont;
			if (this._configuration.ShowThumbnailFrames != this.View.ShowThumbnailFrames)
			{
				this._configuration.ShowThumbnailFrames = this.View.ShowThumbnailFrames;
				await this._mediator.Publish(new ThumbnailFrameSettingsUpdated());
			}

            this._configuration.LockThumbnailLocation = this.View.LockThumbnailLocation;

			this._configuration.PreviewClickMinimize = this.View.PreviewClickMinimize;
			this._configuration.PreviewClickSwitchOut = this.View.PreviewClickSwitchOut;
			this._configuration.PreviewClickToggleCycleGroup = this.View.PreviewClickToggleCycleGroup;
			this._configuration.ThumbnailSnapToGrid = this.View.ThumbnailSnapToGrid;
			this._configuration.ThumbnailSnapToGridSizeX = this.View.ThumbnailSnapToGridSizeX;
            this._configuration.ThumbnailSnapToGridSizeY = this.View.ThumbnailSnapToGridSizeY;
			this._configuration.ThumbnailSnapToGridOffsetX = this.View.ThumbnailSnapToGridOffsetX;
			this._configuration.ThumbnailSnapToGridOffsetY = this.View.ThumbnailSnapToGridOffsetY;
			this._configuration.ThumbnailSnapToGridFillCell = this.View.ThumbnailSnapToGridFillCell;
			this._configuration.ThumbnailSnapToGridCellPadding = this.View.ThumbnailSnapToGridCellPadding;

            this._configuration.EnableActiveClientHighlight = this.View.EnableActiveClientHighlight;

			// The border color is cached per thumbnail, so a change has to be pushed to the views
			if (this._configuration.ActiveClientHighlightColor != this.View.ActiveClientHighlightColor)
			{
				this._configuration.ActiveClientHighlightColor = this.View.ActiveClientHighlightColor;
				await this._mediator.Publish(new ThumbnailFrameSettingsUpdated());
			}

			this._configuration.ActiveClientHighlightThickness = this.View.ActiveClientHighlightThickness;

			if (this._configuration.PreventPreviewColor != this.View.PreventPreviewColor)
			{
				this._configuration.PreventPreviewColor = this.View.PreventPreviewColor;
				await this._mediator.Publish(new ThumbnailFrameSettingsUpdated());
			}

			this._configuration.OverlayLabelColor = this.View.OverlayLabelColor;
			this._configuration.OverlayLabelFont = this.View.OverlayLabelFont;
			this._configuration.OverlayLabelOutlineEnabled = this.View.OverlayLabelOutlineEnabled;
			this._configuration.OverlayLabelOutlineThickness = this.View.OverlayLabelOutlineThickness;
			this._configuration.OverlayLabelOutlineColor = this.View.OverlayLabelOutlineColor;
			this._configuration.CycleGroupNameOutlineEnabled = this.View.CycleGroupNameOutlineEnabled;
			this._configuration.CycleGroupNameOutlineThickness = this.View.CycleGroupNameOutlineThickness;
			this._configuration.CycleGroupNameOutlineColor = this.View.CycleGroupNameOutlineColor;

			this._configuration.EnableGameLogMonitor = this.View.EnableGameLogMonitor;
			this._configuration.GameLogsFolder = this.View.GameLogsFolder;
			this._configuration.EnableAggroFrames = this.View.EnableAggroFrames;
			this._configuration.AggroYellowColor = this.View.AggroYellowColor;
			this._configuration.AggroRedColor = this.View.AggroRedColor;
			this._configuration.AggroFillPercent = this.View.AggroFillPercent;

			this._configuration.IconName = this.View.IconName;

			this._configurationStorage.Save();

			this.View.RefreshZoomSettings();

			await this._mediator.Send(new SaveConfiguration());
		}


		public void AddThumbnails(IList<string> thumbnailTitles)
		{
			IList<IThumbnailDescription> descriptions = new List<IThumbnailDescription>(thumbnailTitles.Count);

			lock (this._descriptionsCache)
			{
				foreach (string title in thumbnailTitles)
				{
					IThumbnailDescription description = this.CreateThumbnailDescription(title);
					this._descriptionsCache[title] = description;

					descriptions.Add(description);
				}
			}

			this.View.AddThumbnails(descriptions);
			this.RefreshHotkeyActions();
			this.RefreshCycleGroupData();
		}

		public void RemoveThumbnails(IList<string> thumbnailTitles)
		{
			IList<IThumbnailDescription> descriptions = new List<IThumbnailDescription>(thumbnailTitles.Count);

			lock (this._descriptionsCache)
			{
				foreach (string title in thumbnailTitles)
				{
					if (!this._descriptionsCache.TryGetValue(title, out IThumbnailDescription description))
					{
						continue;
					}

					this._descriptionsCache.Remove(title);
					descriptions.Add(description);
				}
			}

			this.View.RemoveThumbnails(descriptions);
			this.RefreshHotkeyActions();
			this.RefreshCycleGroupData();
		}

		#region Hotkey management
		// Known clients: every character the registry has ever seen, the currently running
		// ones, the ones with a configured hotkey and the cycle group members
		private List<string> GetKnownClients()
		{
			SortedSet<string> clients = new SortedSet<string>(StringComparer.OrdinalIgnoreCase);

			foreach (CharacterInfo character in this._configuration.GetCharacters())
			{
				// A blacklisted character is not offered for hotkeys or cycle groups
				if (!character.Ignored)
				{
					clients.Add(character.Title);
				}
			}

			lock (this._descriptionsCache)
			{
				foreach (string title in this._descriptionsCache.Keys)
				{
					clients.Add(title);
				}
			}

			foreach (string title in this._configuration.GetClientHotkeys().Keys)
			{
				clients.Add(title);
			}

			foreach (CycleGroup group in this._configuration.CycleGroups)
			{
				foreach (string title in group.ClientsOrder.Keys)
				{
					clients.Add(title);
				}
			}

			return clients.ToList();
		}

		private void RefreshHotkeyActions()
		{
			List<(string ActionId, string DisplayName)> actions = new List<(string, string)>();

			foreach (string client in this.GetKnownClients())
			{
				actions.Add((MainFormPresenter.HOTKEY_ACTION_CLIENT_PREFIX + client, string.Format(Strings.Hotkey_ActivateClient, client)));
			}

			foreach (CharacterGroup characterGroup in this._configuration.GetCharacterGroups())
			{
				actions.Add((MainFormPresenter.HOTKEY_ACTION_ACCOUNT_PREFIX + characterGroup.Id,
								string.Format(Strings.Hotkey_ActivateAccount, characterGroup.Name)));
			}

			foreach (CycleGroup group in this._configuration.CycleGroups)
			{
				actions.Add((MainFormPresenter.GetCycleGroupActionId(group.Name, true), string.Format(Strings.Hotkey_CycleForward, group.Name)));
				actions.Add((MainFormPresenter.GetCycleGroupActionId(group.Name, false), string.Format(Strings.Hotkey_CycleBackward, group.Name)));
			}

			actions.Add((MainFormPresenter.HOTKEY_ACTION_MINIMIZE_ALL, Strings.Hotkey_MinimizeAll));
			actions.Add((MainFormPresenter.HOTKEY_ACTION_TOGGLE_ALL_PREVIEWS, Strings.Hotkey_ToggleAllPreviews));
			actions.Add((MainFormPresenter.HOTKEY_ACTION_CLICK_THROUGH, Strings.Hotkey_ClickThrough));

			this.View.SetHotkeyActions(actions);
		}

		private void RefreshHotkeyBindings()
		{
			// The list shows one entry per action with all of its combinations in a row
			List<(string ActionId, string ActionName, IList<string> Hotkeys)> bindings = new List<(string, string, IList<string>)>();

			foreach ((string actionId, string actionName, string hotkey) in this.GetHotkeyBindings())
			{
				int index = bindings.FindIndex(binding => binding.ActionId == actionId);

				if (index < 0)
				{
					bindings.Add((actionId, actionName, new List<string> { hotkey }));
					continue;
				}

				bindings[index].Hotkeys.Add(hotkey);
			}

			this.View.SetHotkeyBindings(bindings);
		}

		private List<(string ActionId, string ActionName, string Hotkey)> GetHotkeyBindings()
		{
			List<(string ActionId, string ActionName, string Hotkey)> bindings = new List<(string, string, string)>();

			foreach (KeyValuePair<string, IReadOnlyList<string>> entry in this._configuration.GetClientHotkeys().OrderBy(x => x.Key, StringComparer.OrdinalIgnoreCase))
			{
				// One row per combination: a client can be reached by several of them
				foreach (string hotkey in entry.Value.Where(x => !string.IsNullOrEmpty(x)))
				{
					bindings.Add((MainFormPresenter.HOTKEY_ACTION_CLIENT_PREFIX + entry.Key,
									string.Format(Strings.Hotkey_ActivateClient, entry.Key), hotkey));
				}
			}

			foreach (CharacterGroup characterGroup in this._configuration.GetCharacterGroups())
			{
				foreach (string hotkey in characterGroup.Hotkeys.Where(x => !string.IsNullOrEmpty(x)))
				{
					bindings.Add((MainFormPresenter.HOTKEY_ACTION_ACCOUNT_PREFIX + characterGroup.Id,
									string.Format(Strings.Hotkey_ActivateAccount, characterGroup.Name), hotkey));
				}
			}

			foreach (CycleGroup group in this._configuration.CycleGroups)
			{
				foreach (string hotkey in group.ForwardHotkeys.Where(x => !string.IsNullOrEmpty(x)))
				{
					bindings.Add((MainFormPresenter.GetCycleGroupActionId(group.Name, true), string.Format(Strings.Hotkey_CycleForward, group.Name), hotkey));
				}

				foreach (string hotkey in group.BackwardHotkeys.Where(x => !string.IsNullOrEmpty(x)))
				{
					bindings.Add((MainFormPresenter.GetCycleGroupActionId(group.Name, false), string.Format(Strings.Hotkey_CycleBackward, group.Name), hotkey));
				}
			}

			foreach (string hotkey in this._configuration.MinimizeAllClientsHotkeys.Where(x => !string.IsNullOrEmpty(x)))
			{
				bindings.Add((MainFormPresenter.HOTKEY_ACTION_MINIMIZE_ALL, Strings.Hotkey_MinimizeAll, hotkey));
			}

			foreach (string hotkey in this._configuration.ToggleAllPreviewsHotkeys.Where(x => !string.IsNullOrEmpty(x)))
			{
				bindings.Add((MainFormPresenter.HOTKEY_ACTION_TOGGLE_ALL_PREVIEWS, Strings.Hotkey_ToggleAllPreviews, hotkey));
			}

			foreach (string hotkey in this._configuration.ClickThroughHotkeys.Where(x => !string.IsNullOrEmpty(x)))
			{
				bindings.Add((MainFormPresenter.HOTKEY_ACTION_CLICK_THROUGH, Strings.Hotkey_ClickThrough, hotkey));
			}

			return bindings;
		}

		// Canonical hotkey form used to compare bindings: covers both keyboard and mouse ones
		private string NormalizeHotkey(string hotkey)
		{
			return this._configuration.NormalizeHotkey(hotkey);
		}

		// Checks that the combination is not taken by ANOTHER action. The bindings of the
		// action itself are skipped: the editor hands over its complete set, so whatever it
		// had before is about to be replaced
		private bool ValidateHotkeyBinding(string actionId, string normalizedHotkey)
		{
			foreach ((string boundActionId, string boundActionName, string boundHotkey) in this.GetHotkeyBindings())
			{
				if (boundActionId == actionId)
				{
					continue;
				}

				if (this.NormalizeHotkey(boundHotkey) != normalizedHotkey)
				{
					continue;
				}

				this.View.SetHotkeyStatus(string.Format(Strings.Hotkey_UsedBy, boundActionName));
				return false;
			}

			return true;
		}

		/// <summary>Stores the complete set of combinations of the action</summary>
		private bool ReplaceActionHotkeys(string actionId, IList<string> hotkeys)
		{
			if (actionId.StartsWith(MainFormPresenter.HOTKEY_ACTION_CLIENT_PREFIX, StringComparison.Ordinal))
			{
				this._configuration.SetClientHotkeys(actionId.Substring(MainFormPresenter.HOTKEY_ACTION_CLIENT_PREFIX.Length), hotkeys);
				return true;
			}

			if (actionId.StartsWith(MainFormPresenter.HOTKEY_ACTION_ACCOUNT_PREFIX, StringComparison.Ordinal))
			{
				CharacterGroup characterGroup = this._configuration.GetCharacterGroupById(actionId.Substring(MainFormPresenter.HOTKEY_ACTION_ACCOUNT_PREFIX.Length));

				if (characterGroup == null)
				{
					return false;
				}

				MainFormPresenter.ReplaceHotkeyList(characterGroup.Hotkeys, hotkeys);
				return true;
			}

			if (actionId == MainFormPresenter.HOTKEY_ACTION_MINIMIZE_ALL)
			{
				MainFormPresenter.ReplaceHotkeyList(this._configuration.MinimizeAllClientsHotkeys, hotkeys);
				return true;
			}

			if (actionId == MainFormPresenter.HOTKEY_ACTION_TOGGLE_ALL_PREVIEWS)
			{
				MainFormPresenter.ReplaceHotkeyList(this._configuration.ToggleAllPreviewsHotkeys, hotkeys);
				return true;
			}

			if (actionId == MainFormPresenter.HOTKEY_ACTION_CLICK_THROUGH)
			{
				MainFormPresenter.ReplaceHotkeyList(this._configuration.ClickThroughHotkeys, hotkeys);
				return true;
			}

			if (MainFormPresenter.TryParseCycleGroupActionId(actionId, out bool isForward, out string groupName))
			{
				CycleGroup group = this.FindCycleGroup(groupName);

				if (group == null)
				{
					return false;
				}

				MainFormPresenter.ReplaceHotkeyList(isForward ? group.ForwardHotkeys : group.BackwardHotkeys, hotkeys);
				return true;
			}

			return false;
		}

		private static void ReplaceHotkeyList(List<string> stored, IList<string> hotkeys)
		{
			stored.Clear();
			stored.AddRange(hotkeys);
		}

		private bool IsUsableHotkey(string hotkey)
		{
			return MouseBinding.IsMouseBinding(hotkey) || (this._configuration.StringToKey(hotkey) != Keys.None);
		}

		/// <summary>
		/// The editor hands over the complete set of combinations of one action, so the
		/// stored ones are replaced rather than added to
		/// </summary>
		private async void ChangeHotkeyBindings(string actionId, IList<string> hotkeys)
		{
			List<string> normalized = new List<string>();

			foreach (string hotkey in hotkeys ?? new List<string>())
			{
				if (!this.IsUsableHotkey(hotkey))
				{
					this.View.SetHotkeyStatus(Strings.Hotkey_Unsupported);
					return;
				}

				string value = this.NormalizeHotkey(hotkey);

				if (normalized.Contains(value, StringComparer.OrdinalIgnoreCase))
				{
					this.View.SetHotkeyStatus(Strings.Hotkey_AlreadyAssigned);
					return;
				}

				// Nothing that another action already answers to
				if (!this.ValidateHotkeyBinding(actionId, value))
				{
					return;
				}

				normalized.Add(value);
			}

			if (!this.ReplaceActionHotkeys(actionId, normalized))
			{
				return;
			}

			await this.ApplyHotkeySettings();
			this.View.SetHotkeyStatus(string.Format(Strings.Hotkey_Assigned, string.Join(", ", normalized)));
		}

		private async void RemoveHotkeyBindings(string actionId)
		{
			if (!this.ReplaceActionHotkeys(actionId, new List<string>()))
			{
				return;
			}

			await this.ApplyHotkeySettings();
			this.View.SetHotkeyStatus(Strings.Hotkey_RemovedAll);
		}

		private async Task ApplyHotkeySettings()
		{
			await this._mediator.Publish(new ThumbnailHotkeysUpdated());
			await this._mediator.Send(new SaveConfiguration());

			this.RefreshHotkeyActions();
			this.RefreshHotkeyBindings();
		}

		private void RefreshCycleGroupData()
		{
			// Every character the registry knows can be put into a cycle group, not only
			// the ones that happen to be running right now
			List<string> activeClients = this.GetKnownClients();

			List<(string Name, IList<string> Clients)> groups = new List<(string, IList<string>)>();
			foreach (CycleGroup group in this._configuration.CycleGroups)
			{
				groups.Add((group.Name, group.ClientsOrder.OrderBy(x => x.Value).Select(x => x.Key).ToList()));
			}

			Dictionary<string, IList<string>> clientGroups = new Dictionary<string, IList<string>>(StringComparer.OrdinalIgnoreCase);
			foreach (string client in this.GetKnownClients())
			{
				clientGroups[client] = this._configuration.CycleGroups.Where(g => g.ClientsOrder.ContainsKey(client)).Select(g => g.Name).ToList();
			}

			this.View.SetActiveClients(activeClients);
			this.View.SetCycleGroups(groups);
			this.View.SetClientCycleGroups(clientGroups);

			this.RefreshCharacters();
		}

		#region Character registry
		private void RefreshCharacters()
		{
			List<CharacterGroupViewItem> groups = this._configuration.GetCharacterGroups()
											.Select(group => new CharacterGroupViewItem(group.Id, group.Name, group.ManageAsWhole, group.Color))
											.OrderBy(group => group.Name, StringComparer.CurrentCultureIgnoreCase)
											.ToList();

			HashSet<string> onlineClients;

			lock (this._descriptionsCache)
			{
				onlineClients = new HashSet<string>(this._descriptionsCache.Keys, StringComparer.Ordinal);
			}

			List<CharacterViewItem> characters = this._configuration.GetCharacters()
											.Select(character => new CharacterViewItem(character.Title, character.Name, character.GroupId,
																		onlineClients.Contains(character.Title), character.Ignored,
																		MainFormPresenter.FormatLastSeen(character.LastSeen)))
											.OrderBy(character => character.Name, StringComparer.CurrentCultureIgnoreCase)
											.ToList();

			this.View.SetCharacters(groups, characters);
		}

		private static string FormatLastSeen(DateTime? lastSeen)
		{
			return lastSeen.HasValue
					? string.Format(Strings.Characters_LastSeen, lastSeen.Value.ToLocalTime().ToString("g"))
					: Strings.Characters_LastSeenUnknown;
		}

		private async void ChangeCharacterGroup(string title, string groupId)
		{
			this._configuration.SetCharacterGroup(title, groupId);

			await this.ApplyCharacterRegistryChange();
		}

		private async void CreateCharacterGroup(string title, string name)
		{
			CharacterGroup group = this._configuration.CreateCharacterGroup(name);
			this._configuration.SetCharacterGroup(title, group.Id);

			await this.ApplyCharacterRegistryChange();
		}

		private async void RenameCharacterGroup(string groupId, string name)
		{
			this._configuration.RenameCharacterGroup(groupId, name);

			await this.ApplyCharacterRegistryChange();
		}

		private async void RemoveCharacterGroup(string groupId)
		{
			this._configuration.RemoveCharacterGroup(groupId);

			await this.ApplyCharacterRegistryChange();
		}

		private async void ChangeCharacterGroupManagement(string groupId, bool manageAsWhole)
		{
			this._configuration.SetGroupManageAsWhole(groupId, manageAsWhole);

			await this.ApplyCharacterRegistryChange();
		}

		private async void ChangeCharacterIgnored(string title, bool ignored)
		{
			this._configuration.SetCharacterIgnored(title, ignored);

			// A blacklisted character leaves the hotkey and cycle group lists
			await this.ApplyCharacterRegistryChange();
		}

		private async void ChangeCharacterGroupColor(string groupId, Color color)
		{
			this._configuration.SetCharacterGroupColor(groupId, color);

			await this.ApplyCharacterRegistryChange();
		}

		private async void ForgetCharacter(string title)
		{
			this._configuration.ForgetCharacter(title);

			// Forgetting a character also drops its hotkey and its cycle group entries
			await this.ApplyCycleGroupSettings();
		}

		/// <summary>
		/// Groups the characters that share one stored preview position. Only one character
		/// of an account can be online at a time, so the ones the user has put into the same
		/// spot are almost always the characters of one account
		/// </summary>
		private async void SuggestCharacterGroups()
		{
			List<List<string>> suggestions = this.BuildCharacterGroupSuggestions();

			if (suggestions.Count == 0)
			{
				this.View.ShowWarning(Strings.Characters_SuggestTitle, Strings.Characters_SuggestNothing);
				return;
			}

			string summary = string.Join(Environment.NewLine,
								suggestions.Select(suggestion => string.Join(" + ", suggestion.Select(CharacterInfo.GetDisplayName))));

			if (!this.View.ShowQuestion(Strings.Characters_SuggestTitle, string.Format(Strings.Characters_SuggestPrompt, summary)))
			{
				return;
			}

			foreach (List<string> suggestion in suggestions)
			{
				this._configuration.LinkCharacters(suggestion);
			}

			await this.ApplyCharacterRegistryChange();
		}

		private List<List<string>> BuildCharacterGroupSuggestions()
		{
			// Positions dragged by hand are never pixel-identical, so previews standing
			// this close to each other count as one spot. A grid slot is at least a
			// preview wide, so the tolerance cannot merge two neighboring slots
			const int LOCATION_TOLERANCE = 12;

			Point unknownLocation = new Point(int.MinValue, int.MinValue);
			List<(Point Location, List<string> Characters)> spots = new List<(Point, List<string>)>();

			foreach (CharacterInfo character in this._configuration.GetCharacters())
			{
				// Characters that are already grouped or blacklisted are left alone
				if (!string.IsNullOrEmpty(character.GroupId) || character.Ignored)
				{
					continue;
				}

				Point location = this._configuration.GetThumbnailLocation(character.Title, null, unknownLocation);

				if (location == unknownLocation)
				{
					continue;
				}

				int index = spots.FindIndex(spot => (Math.Abs(spot.Location.X - location.X) <= LOCATION_TOLERANCE)
													&& (Math.Abs(spot.Location.Y - location.Y) <= LOCATION_TOLERANCE));

				if (index < 0)
				{
					spots.Add((location, new List<string> { character.Title }));
					continue;
				}

				spots[index].Characters.Add(character.Title);
			}

			return spots.Where(spot => spot.Characters.Count > 1).Select(spot => spot.Characters).ToList();
		}

		private void EditPreviewSettings(string title)
		{
			CharacterGroup group = this._configuration.GetCharacterGroupOf(title);

			// Editing one member of a group managed as a whole edits all of its members
			string groupHint = ((group != null) && group.ManageAsWhole)
								? string.Format(Strings.Characters_PreviewSettingsGroupHint, group.Name)
								: null;

			this.View.ShowPreviewSettings(title, CharacterInfo.GetDisplayName(title), groupHint,
											this._configuration.ResolvePreviewSettings(title),
											this._configuration.ResolvePreviewSettings(null));
		}

		private async void ApplyPreviewSettings(string title, PreviewSettings settings)
		{
			this._configuration.SetPreviewSettings(title, settings);

			await this.ApplyCharacterRegistryChange();
		}

		private async Task ApplyCharacterRegistryChange()
		{
			// Account hotkeys are stored in the group entries, so a changed registry
			// (a renamed, created or dropped group) has to re-register them
			await this._mediator.Publish(new ThumbnailHotkeysUpdated());
			await this._mediator.Send(new SaveConfiguration());

			this.RefreshHotkeyActions();
			this.RefreshHotkeyBindings();
			this.RefreshCharacters();
		}
		#endregion

		private async void ChangeCycleGroupClients(string groupName, IList<string> orderedClients)
		{
			CycleGroup group = this.FindCycleGroup(groupName);

			if (group == null)
			{
				return;
			}

			group.ClientsOrder.Clear();

			int order = 1;
			foreach (string client in orderedClients)
			{
				group.ClientsOrder[client] = order++;
			}

			await this.ApplyCycleGroupSettings();
		}

		private async void ChangeThumbnailCycleGroup(string title, string groupName)
		{
			foreach (CycleGroup group in this._configuration.CycleGroups)
			{
				group.ClientsOrder.Remove(title);
			}

			if (groupName != null)
			{
				CycleGroup group = this.FindCycleGroup(groupName);

				if (group != null)
				{
					group.ClientsOrder[title] = group.ClientsOrder.Count == 0 ? 1 : group.ClientsOrder.Values.Max() + 1;
				}
			}

			await this.ApplyCycleGroupSettings();
		}

		private async void AddCycleGroup()
		{
			int index = 1;
			while (this.FindCycleGroup(string.Format(Strings.CycleGroups_DefaultName, index)) != null)
			{
				index++;
			}

			this._configuration.CycleGroups.Add(new CycleGroup { Name = string.Format(Strings.CycleGroups_DefaultName, index) });

			await this.ApplyCycleGroupSettings();
		}

		private async void RenameCycleGroup(string oldName, string newName)
		{
			CycleGroup group = this.FindCycleGroup(oldName);

			if (group == null)
			{
				return;
			}

			CycleGroup existingGroup = this.FindCycleGroup(newName);

			if ((existingGroup != null) && (existingGroup != group))
			{
				this.View.SetHotkeyStatus(string.Format(Strings.CycleGroups_NameExists, newName));
				return;
			}

			group.Name = newName;

			await this.ApplyCycleGroupSettings();

			// The selection is restored by the old name during the refresh, so the
			// renamed group has to be re-selected explicitly
			this.View.SelectCycleGroup(newName);
		}

		private async void ChangeHotkeyCaptureMode(bool isCapturing)
		{
			await this._mediator.Publish(new HotkeyCaptureModeChanged(isCapturing));
		}

		private async void TestAggroFrames()
		{
			await this._mediator.Publish(new TestAggroFrames());
		}

		private async void RemoveCycleGroup(string groupName)
		{
			CycleGroup group = this.FindCycleGroup(groupName);

			if (group == null)
			{
				return;
			}

			this._configuration.CycleGroups.Remove(group);

			await this.ApplyCycleGroupSettings();
		}

		private async Task ApplyCycleGroupSettings()
		{
			await this._mediator.Publish(new ThumbnailHotkeysUpdated());
			await this._mediator.Send(new SaveConfiguration());

			this.RefreshHotkeyActions();
			this.RefreshHotkeyBindings();
			this.RefreshCycleGroupData();
		}

		private CycleGroup FindCycleGroup(string groupName)
		{
			return this._configuration.CycleGroups.FirstOrDefault(x => string.Equals(x.Name, groupName, StringComparison.OrdinalIgnoreCase));
		}

		private static string GetCycleGroupActionId(string groupName, bool isForward)
		{
			return "cycle:" + (isForward ? "F" : "B") + ":" + groupName;
		}

		private static bool TryParseCycleGroupActionId(string actionId, out bool isForward, out string groupName)
		{
			isForward = false;
			groupName = null;

			string[] parts = actionId.Split(new[] { ':' }, 3);

			if ((parts.Length != 3) || (parts[0] != "cycle"))
			{
				return false;
			}

			isForward = parts[1] == "F";
			groupName = parts[2];
			return true;
		}
		#endregion

		private IThumbnailDescription CreateThumbnailDescription(string title)
		{
			bool isDisabled = this._configuration.IsThumbnailDisabled(title);
			return new ThumbnailDescription(title, isDisabled);
		}

		private async void UpdateThumbnailState(String title)
		{
			if (this._descriptionsCache.TryGetValue(title, out IThumbnailDescription description))
			{
				this._configuration.ToggleThumbnail(title, description.IsDisabled);
			}

			await this._mediator.Send(new SaveConfiguration());
		}

		public void UpdateThumbnailSize(Size size)
		{
			this._suppressSizeNotifications = true;
			this.View.ThumbnailSize = size;
			this._suppressSizeNotifications = false;
		}

		private void OpenDocumentationLink()
		{
			// funtimes
			// https://brockallen.com/2016/09/24/process-start-for-urls-on-net-core/
			// https://github.com/dotnet/runtime/issues/17938

			// TODO Move out to a separate service / presenter / message handler
#if LINUX
			Process.Start("xdg-open", new Uri(MainFormPresenter.FORUM_URL).AbsoluteUri);
#else
			ProcessStartInfo processStartInfo = new ProcessStartInfo(new Uri(MainFormPresenter.FORUM_URL).AbsoluteUri);
			processStartInfo.UseShellExecute = true;
			Process.Start(processStartInfo);
#endif
		}

		private string GetApplicationVersion()
		{
			System.Reflection.Assembly assembly = System.Reflection.Assembly.GetEntryAssembly();
			Version version = assembly.GetName().Version;
			string target = "Windows";
#if LINUX
  target = "Linux";
#endif

			return $"{version.Major}.{version.Minor}.{version.Build}.{version.Revision} {target}";
		}

		private void ExitApplication()
		{
			this._exitApplication = true;
			this.View.Close();
		}
	}
}