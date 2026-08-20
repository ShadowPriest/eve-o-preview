using System.Threading;
using System.Threading.Tasks;
using EveOPreview.Mediator.Messages;
using EveOPreview.Services;
using MediatR;

namespace EveOPreview.Mediator.Handlers.Services
{
	sealed class TestAggroFramesHandler : INotificationHandler<TestAggroFrames>
	{
		private readonly IGameLogMonitor _gameLogMonitor;

		public TestAggroFramesHandler(IGameLogMonitor gameLogMonitor)
		{
			this._gameLogMonitor = gameLogMonitor;
		}

		public Task Handle(TestAggroFrames notification, CancellationToken cancellationToken)
		{
			this._gameLogMonitor.InjectTestSequence();

			return Task.CompletedTask;
		}
	}
}
