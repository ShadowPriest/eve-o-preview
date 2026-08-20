using System.Drawing;
using Newtonsoft.Json;

namespace EveOPreview.Configuration
{
	/// <summary>
	/// Preview configuration of a single client window (keyed by the client title).
	/// Every value is optional: a value left unset falls back to the global setting, so
	/// options added to the overlay later are inherited by the existing entries instead of
	/// silently resetting them to a hardcoded default.
	/// UseCustomSettings is the master switch shown as a single checkbox in the UI - with it
	/// unset the whole entry is ignored and the client uses the global appearance
	/// </summary>
	[JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
	public sealed class PreviewSettings
	{
		public bool UseCustomSettings { get; set; }

		public Size? ThumbnailSize { get; set; }
		public double? ThumbnailOpacity { get; set; }

		public bool? PreventPreviews { get; set; }
		public Color? PreventPreviewColor { get; set; }

		public bool? EnableActiveClientHighlight { get; set; }
		public Color? ActiveClientHighlightColor { get; set; }
		public int? ActiveClientHighlightThickness { get; set; }

		public bool? ShowThumbnailOverlays { get; set; }
		public bool? OverlayAlwaysOnTop { get; set; }
		public bool? ShowThumbnailFrames { get; set; }

		public bool? ShowClientName { get; set; }
		public ZoomAnchor? OverlayLabelAnchor { get; set; }
		public Color? OverlayLabelColor { get; set; }
		public Font OverlayLabelFont { get; set; }
		public bool? OverlayLabelOutlineEnabled { get; set; }
		public int? OverlayLabelOutlineThickness { get; set; }
		public Color? OverlayLabelOutlineColor { get; set; }

		public bool? ShowCycleGroupName { get; set; }
		public ZoomAnchor? CycleGroupIndicatorAnchor { get; set; }
		public Color? CycleGroupNameColor { get; set; }
		public Font CycleGroupNameFont { get; set; }
		public bool? CycleGroupNameOutlineEnabled { get; set; }
		public int? CycleGroupNameOutlineThickness { get; set; }
		public Color? CycleGroupNameOutlineColor { get; set; }

		public bool? ThumbnailZoomEnabled { get; set; }
		public int? ThumbnailZoomFactor { get; set; }
		public ZoomAnchor? ThumbnailZoomAnchor { get; set; }

		/// <summary>True when the entry carries at least one stored value</summary>
		[JsonIgnore]
		public bool IsEmpty => !this.UseCustomSettings
							&& (this.ThumbnailSize == null) && (this.ThumbnailOpacity == null)
							&& (this.PreventPreviews == null) && (this.PreventPreviewColor == null)
							&& (this.EnableActiveClientHighlight == null) && (this.ActiveClientHighlightColor == null)
							&& (this.ActiveClientHighlightThickness == null)
							&& (this.ShowThumbnailOverlays == null) && (this.OverlayAlwaysOnTop == null)
							&& (this.ShowThumbnailFrames == null)
							&& (this.ShowClientName == null) && (this.OverlayLabelAnchor == null)
							&& (this.OverlayLabelColor == null) && (this.OverlayLabelFont == null)
							&& (this.OverlayLabelOutlineEnabled == null) && (this.OverlayLabelOutlineThickness == null)
							&& (this.OverlayLabelOutlineColor == null)
							&& (this.ShowCycleGroupName == null) && (this.CycleGroupIndicatorAnchor == null)
							&& (this.CycleGroupNameColor == null) && (this.CycleGroupNameFont == null)
							&& (this.CycleGroupNameOutlineEnabled == null) && (this.CycleGroupNameOutlineThickness == null)
							&& (this.CycleGroupNameOutlineColor == null)
							&& (this.ThumbnailZoomEnabled == null) && (this.ThumbnailZoomFactor == null)
							&& (this.ThumbnailZoomAnchor == null);

		public PreviewSettings Clone()
		{
			return (PreviewSettings)this.MemberwiseClone();
		}
	}
}
