using EveOPreview.View;

namespace EveOPreview.Services
{
	public interface IThumbnailManager
	{
		void Start();
		void Stop();

		void UpdateActiveColour();
		void UpdateCycleGroupIndicator();
		void UpdateThumbnailsSize();
		void UpdateThumbnailFrames();

		IThumbnailView GetClientByTitle(string title);
		IThumbnailView GetClientByPointer(System.IntPtr ptr);
		IThumbnailView GetActiveClient();
	}
}