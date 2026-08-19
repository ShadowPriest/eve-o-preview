using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using EveOPreview.Configuration;
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
		private const string HOTKEY_ACTION_MINIMIZE_ALL = "minimizeall";
		private const string HOTKEY_ACTION_TOGGLE_ALL_PREVIEWS = "toggleallpreviews";
		#endregion

		#region Private fields
		private readonly IMediator _mediator;
		private readonly IThumbnailConfiguration _configuration;
		private readonly IConfigurationStorage _configurationStorage;
		private readonly IDictionary<string, IThumbnailDescription> _descriptionsCache;
		private bool _suppressSizeNotifications;

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
			this._exitApplication = false;

			this.View.FormActivated = this.Activate;
			this.View.FormMinimized = this.Minimize;
			this.View.FormCloseRequested = this.Close;
			this.View.ApplicationSettingsChanged = this.SaveApplicationSettings;
			this.View.ThumbnailsSizeChanged = this.UpdateThumbnailsSize;
			this.View.ThumbnailStateChanged = this.UpdateThumbnailState;
			this.View.DocumentationLinkActivated = this.OpenDocumentationLink;
			this.View.ApplicationExitRequested = this.ExitApplication;
			this.View.HotkeyBindingAssigned = this.AssignHotkeyBinding;
			this.View.HotkeyBindingRemoved = this.RemoveHotkeyBinding;
			this.View.HotkeyBindingEdited = this.EditHotkeyBinding;
			this.View.CycleGroupClientsChanged = this.ChangeCycleGroupClients;
			this.View.ThumbnailCycleGroupChanged = this.ChangeThumbnailCycleGroup;
			this.View.CycleGroupAddRequested = this.AddCycleGroup;
			this.View.CycleGroupRemoveRequested = this.RemoveCycleGroup;
			this.View.CycleGroupRenameRequested = this.RenameCycleGroup;
			this.View.HotkeyCaptureModeChanged = this.ChangeHotkeyCaptureMode;
			this.View.WindowSizeChanged = this.SaveWindowSize;

			this.View.IconName = this._configuration.IconName;
		}

		private void Activate()
		{
			this._suppressSizeNotifications = true;
			this.LoadApplicationSettings();
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

			this.View.ThumbnailOpacity = this._configuration.ThumbnailOpacity;

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
			this.View.ThumbnailSnapToGrid = this._configuration.ThumbnailSnapToGrid;
			this.View.ThumbnailSnapToGridSizeX = this._configuration.ThumbnailSnapToGridSizeX;
			this.View.ThumbnailSnapToGridSizeY = this._configuration.ThumbnailSnapToGridSizeY;
			this.View.EnableActiveClientHighlight = this._configuration.EnableActiveClientHighlight;
			this.View.ActiveClientHighlightThickness = this._configuration.ActiveClientHighlightThickness;
			this.View.ActiveClientHighlightColor = this._configuration.ActiveClientHighlightColor;
			this.View.PreventPreviewColor = this._configuration.PreventPreviewColor;

			this.View.OverlayLabelColor = this._configuration.OverlayLabelColor;
			this.View.OverlayLabelFont = this._configuration.OverlayLabelFont;


			this.View.IconName = this._configuration.IconName;

			this.View.WindowSize = this._configuration.MainWindowSize;
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
			this._configuration.MinimizeToTray = this.View.MinimizeToTray;

			this._configuration.ThumbnailOpacity = (float)this.View.ThumbnailOpacity;

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
			this._configuration.ThumbnailSnapToGrid = this.View.ThumbnailSnapToGrid;
			this._configuration.ThumbnailSnapToGridSizeX = this.View.ThumbnailSnapToGridSizeX;
            this._configuration.ThumbnailSnapToGridSizeY = this.View.ThumbnailSnapToGridSizeY;

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
		// Known clients: currently running ones, the ones with a configured hotkey and cycle group members
		private List<string> GetKnownClients()
		{
			SortedSet<string> clients = new SortedSet<string>(StringComparer.OrdinalIgnoreCase);

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
				actions.Add((MainFormPresenter.HOTKEY_ACTION_CLIENT_PREFIX + client, "Activate " + client));
			}

			foreach (CycleGroup group in this._configuration.CycleGroups)
			{
				actions.Add((MainFormPresenter.GetCycleGroupActionId(group.Name, true), "Cycle " + group.Name + " forward"));
				actions.Add((MainFormPresenter.GetCycleGroupActionId(group.Name, false), "Cycle " + group.Name + " backward"));
			}

			actions.Add((MainFormPresenter.HOTKEY_ACTION_MINIMIZE_ALL, "Minimize all clients"));
			actions.Add((MainFormPresenter.HOTKEY_ACTION_TOGGLE_ALL_PREVIEWS, "Show / hide all previews"));

			this.View.SetHotkeyActions(actions);
		}

		private void RefreshHotkeyBindings()
		{
			this.View.SetHotkeyBindings(this.GetHotkeyBindings());
		}

		private List<(string ActionId, string ActionName, string Hotkey)> GetHotkeyBindings()
		{
			List<(string ActionId, string ActionName, string Hotkey)> bindings = new List<(string, string, string)>();

			foreach (KeyValuePair<string, string> entry in this._configuration.GetClientHotkeys().OrderBy(x => x.Key, StringComparer.OrdinalIgnoreCase))
			{
				if (string.IsNullOrEmpty(entry.Value))
				{
					continue;
				}

				bindings.Add((MainFormPresenter.HOTKEY_ACTION_CLIENT_PREFIX + entry.Key, "Activate " + entry.Key, entry.Value));
			}

			foreach (CycleGroup group in this._configuration.CycleGroups)
			{
				foreach (string hotkey in group.ForwardHotkeys.Where(x => !string.IsNullOrEmpty(x)))
				{
					bindings.Add((MainFormPresenter.GetCycleGroupActionId(group.Name, true), "Cycle " + group.Name + " forward", hotkey));
				}

				foreach (string hotkey in group.BackwardHotkeys.Where(x => !string.IsNullOrEmpty(x)))
				{
					bindings.Add((MainFormPresenter.GetCycleGroupActionId(group.Name, false), "Cycle " + group.Name + " backward", hotkey));
				}
			}

			foreach (string hotkey in this._configuration.MinimizeAllClientsHotkeys.Where(x => !string.IsNullOrEmpty(x)))
			{
				bindings.Add((MainFormPresenter.HOTKEY_ACTION_MINIMIZE_ALL, "Minimize all clients", hotkey));
			}

			foreach (string hotkey in this._configuration.ToggleAllPreviewsHotkeys.Where(x => !string.IsNullOrEmpty(x)))
			{
				bindings.Add((MainFormPresenter.HOTKEY_ACTION_TOGGLE_ALL_PREVIEWS, "Show / hide all previews", hotkey));
			}

			return bindings;
		}

		// Canonical hotkey form used to compare bindings: covers both keyboard and mouse ones
		private string NormalizeHotkey(string hotkey)
		{
			if (MouseBinding.IsMouseBinding(hotkey))
			{
				return MouseBinding.Normalize(hotkey);
			}

			Keys keys = this._configuration.StringToKey(hotkey);
			return keys == Keys.None ? hotkey : (new KeysConverter()).ConvertToInvariantString(keys);
		}

		// Checks that the hotkey can be used and that it is not taken by another action.
		// The binding identified by (ignoredActionId, ignoredHotkey) is excluded from the check
		private bool ValidateHotkeyBinding(string actionId, string normalizedHotkey, string ignoredActionId, string ignoredHotkey)
		{
			foreach ((string boundActionId, string boundActionName, string boundHotkey) in this.GetHotkeyBindings())
			{
				string boundNormalized = this.NormalizeHotkey(boundHotkey);

				if ((boundActionId == ignoredActionId) && (boundNormalized == ignoredHotkey))
				{
					continue;
				}

				if (boundNormalized != normalizedHotkey)
				{
					continue;
				}

				this.View.SetHotkeyStatus(boundActionId == actionId ? "This hotkey is already assigned" : "Hotkey is already used: " + boundActionName);
				return false;
			}

			return true;
		}

		private bool TryAddHotkeyToConfig(string actionId, string normalizedHotkey)
		{
			if (actionId.StartsWith(MainFormPresenter.HOTKEY_ACTION_CLIENT_PREFIX, StringComparison.Ordinal))
			{
				this._configuration.SetClientHotkey(actionId.Substring(MainFormPresenter.HOTKEY_ACTION_CLIENT_PREFIX.Length), normalizedHotkey);
				return true;
			}

			if (actionId == MainFormPresenter.HOTKEY_ACTION_MINIMIZE_ALL)
			{
				this._configuration.MinimizeAllClientsHotkeys.RemoveAll(string.IsNullOrEmpty);
				this._configuration.MinimizeAllClientsHotkeys.Add(normalizedHotkey);
				return true;
			}

			if (actionId == MainFormPresenter.HOTKEY_ACTION_TOGGLE_ALL_PREVIEWS)
			{
				this._configuration.ToggleAllPreviewsHotkeys.RemoveAll(string.IsNullOrEmpty);
				this._configuration.ToggleAllPreviewsHotkeys.Add(normalizedHotkey);
				return true;
			}

			if (MainFormPresenter.TryParseCycleGroupActionId(actionId, out bool isForward, out string groupName))
			{
				CycleGroup group = this.FindCycleGroup(groupName);

				if (group == null)
				{
					return false;
				}

				List<string> hotkeys = isForward ? group.ForwardHotkeys : group.BackwardHotkeys;
				hotkeys.RemoveAll(string.IsNullOrEmpty);
				hotkeys.Add(normalizedHotkey);
				return true;
			}

			return false;
		}

		private void RemoveHotkeyFromConfig(string actionId, string normalizedHotkey)
		{
			if (actionId.StartsWith(MainFormPresenter.HOTKEY_ACTION_CLIENT_PREFIX, StringComparison.Ordinal))
			{
				this._configuration.RemoveClientHotkey(actionId.Substring(MainFormPresenter.HOTKEY_ACTION_CLIENT_PREFIX.Length));
			}
			else if (actionId == MainFormPresenter.HOTKEY_ACTION_MINIMIZE_ALL)
			{
				this._configuration.MinimizeAllClientsHotkeys.RemoveAll(x => this.NormalizeHotkey(x) == normalizedHotkey);
			}
			else if (actionId == MainFormPresenter.HOTKEY_ACTION_TOGGLE_ALL_PREVIEWS)
			{
				this._configuration.ToggleAllPreviewsHotkeys.RemoveAll(x => this.NormalizeHotkey(x) == normalizedHotkey);
			}
			else if (MainFormPresenter.TryParseCycleGroupActionId(actionId, out bool isForward, out string groupName))
			{
				CycleGroup group = this.FindCycleGroup(groupName);

				if (group != null)
				{
					(isForward ? group.ForwardHotkeys : group.BackwardHotkeys).RemoveAll(x => this.NormalizeHotkey(x) == normalizedHotkey);
				}
			}
		}

		private bool IsUsableHotkey(string hotkey)
		{
			return MouseBinding.IsMouseBinding(hotkey) || (this._configuration.StringToKey(hotkey) != Keys.None);
		}

		private async void AssignHotkeyBinding(string actionId, string hotkey)
		{
			if (!this.IsUsableHotkey(hotkey))
			{
				this.View.SetHotkeyStatus("Unsupported key combination");
				return;
			}

			string normalized = this.NormalizeHotkey(hotkey);

			if (!this.ValidateHotkeyBinding(actionId, normalized, null, null))
			{
				return;
			}

			if (!this.TryAddHotkeyToConfig(actionId, normalized))
			{
				return;
			}

			await this.ApplyHotkeySettings();
			this.View.SetHotkeyStatus("Assigned: " + normalized);
		}

		private async void EditHotkeyBinding(string oldActionId, string oldHotkey, string newActionId, string newHotkey)
		{
			if (!this.IsUsableHotkey(newHotkey))
			{
				this.View.SetHotkeyStatus("Unsupported key combination");
				return;
			}

			string normalizedOld = this.NormalizeHotkey(oldHotkey);
			string normalizedNew = this.NormalizeHotkey(newHotkey);

			if (!this.ValidateHotkeyBinding(newActionId, normalizedNew, oldActionId, normalizedOld))
			{
				return;
			}

			this.RemoveHotkeyFromConfig(oldActionId, normalizedOld);

			if (!this.TryAddHotkeyToConfig(newActionId, normalizedNew))
			{
				// The removal has already happened; still apply it to keep the UI consistent
				await this.ApplyHotkeySettings();
				return;
			}

			await this.ApplyHotkeySettings();
			this.View.SetHotkeyStatus("Updated: " + normalizedNew);
		}

		private async void RemoveHotkeyBinding(string actionId, string hotkey)
		{
			string normalized = this.NormalizeHotkey(hotkey);

			this.RemoveHotkeyFromConfig(actionId, normalized);

			await this.ApplyHotkeySettings();
			this.View.SetHotkeyStatus("Removed: " + normalized);
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
			List<string> activeClients;
			lock (this._descriptionsCache)
			{
				activeClients = this._descriptionsCache.Keys.OrderBy(x => x, StringComparer.OrdinalIgnoreCase).ToList();
			}

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
		}

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
			while (this.FindCycleGroup("Group " + index) != null)
			{
				index++;
			}

			this._configuration.CycleGroups.Add(new CycleGroup { Name = "Group " + index });

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
				this.View.SetHotkeyStatus("A cycle group named '" + newName + "' already exists");
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

			// The build stamp makes it obvious which build is actually running
			string buildDate = assembly.GetCustomAttributes(typeof(System.Reflection.AssemblyMetadataAttribute), false)
										.Cast<System.Reflection.AssemblyMetadataAttribute>()
										.FirstOrDefault(x => x.Key == "BuildDate")?.Value;

			string versionText = $"{version.Major}.{version.Minor}.{version.Build}.{version.Revision} {target}";

			return string.IsNullOrEmpty(buildDate) ? versionText : $"{versionText} ({buildDate})";
		}

		private void ExitApplication()
		{
			this._exitApplication = true;
			this.View.Close();
		}
	}
}