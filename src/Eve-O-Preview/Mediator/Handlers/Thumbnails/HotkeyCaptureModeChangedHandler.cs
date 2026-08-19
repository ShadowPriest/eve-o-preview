using System.Threading;
using System.Threading.Tasks;
using EveOPreview.Mediator.Messages;
using EveOPreview.Services;
using MediatR;

namespace EveOPreview.Mediator.Handlers.Thumbnails
{
	sealed class HotkeyCaptureModeChangedHandler : INotificationHandler<HotkeyCaptureModeChanged>
	{
		private readonly IThumbnailManager _manager;

		public HotkeyCaptureModeChangedHandler(IThumbnailManager manager)
		{
			this._manager = manager;
		}

		public Task Handle(HotkeyCaptureModeChanged notification, CancellationToken cancellationToken)
		{
			if (notification.IsCapturing)
			{
				this._manager.SuspendHotkeys();
			}
			else
			{
				this._manager.ResumeHotkeys();
			}

			return Task.CompletedTask;
		}
	}
}
