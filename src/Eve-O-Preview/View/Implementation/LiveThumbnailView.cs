using System;
using System.Drawing;
using EveOPreview.Configuration;
using EveOPreview.Services;

namespace EveOPreview.View
{
	sealed class LiveThumbnailView : ThumbnailView
	{
		#region Private constants
		// Re-creating a DWM thumbnail is expensive and briefly renders black.
		// Under rapid client switching (f.e. cycling with the mouse wheel) forced
		// re-registrations are throttled down to a cheap properties update
		private const int FORCED_REFRESH_THROTTLE_MS = 500;
		#endregion

		#region Private fields
		private IDwmThumbnail _thumbnail;
		private Point _startLocation;
		private Point _endLocation;
		private IThumbnailConfiguration _config;
		private DateTime _lastRegistrationTimestamp = DateTime.MinValue;
		#endregion

		public LiveThumbnailView(IWindowManager windowManager, IThumbnailConfiguration config, IThumbnailManager thumbnailManager)
			: base(windowManager, config, thumbnailManager)
		{
			this._startLocation = new Point(0, 0);
			this._endLocation = new Point(this.ClientSize);
			this._config = config;
		}

		protected override void RefreshThumbnail(bool forceRefresh)
		{
			// 'Do not display previews' releases the DWM capture entirely instead of just
			// covering the picture with the placeholder: no capture - no rendering load
			if (this.IsPreventPreviews())
			{
				if (this._thumbnail != null)
				{
					this._thumbnail.Unregister();
					this._thumbnail = null;
				}

				return;
			}

			if (this._thumbnail == null)
			{
				this.RegisterThumbnail();
				return;
			}

			if (!forceRefresh)
			{
				return;
			}

			if ((DateTime.UtcNow - this._lastRegistrationTimestamp).TotalMilliseconds < LiveThumbnailView.FORCED_REFRESH_THROTTLE_MS)
			{
				this._thumbnail.Update();
				return;
			}

			// To prevent flickering the old broken thumbnail is removed AFTER the new shiny one is created
			IDwmThumbnail obsoleteThumbnail = this._thumbnail;
			this.RegisterThumbnail();
			obsoleteThumbnail.Unregister();
		}

		protected override void ResizeThumbnail(int baseWidth, int baseHeight, int highlightWidthTop, int highlightWidthRight, int highlightWidthBottom, int highlightWidthLeft)
		{
			var left = 0 + highlightWidthLeft;
			var top = 0 + highlightWidthTop;
			var right = baseWidth - highlightWidthRight;
			var bottom = baseHeight - highlightWidthBottom;

			if ((this._startLocation.X == left) && (this._startLocation.Y == top) && (this._endLocation.X == right) && (this._endLocation.Y == bottom))
			{
				return; // No update required
			}
			this._startLocation = new Point(left, top);
			this._endLocation = new Point(right, bottom);

			// The thumbnail is unloaded while 'do not display previews' is active;
			// the stored locations will be applied when it is registered again
			this._thumbnail?.Move(left, top, right, bottom);
			this._thumbnail?.Update();
		}

		private void RegisterThumbnail()
		{
			this._thumbnail = this.WindowManager.GetLiveThumbnail(this.Handle, this.Id);
			this._thumbnail.Move(this._startLocation.X, this._startLocation.Y, this._endLocation.X, this._endLocation.Y);
			this._thumbnail.Update();

			this._lastRegistrationTimestamp = DateTime.UtcNow;
		}
	}
}
