namespace EveOPreview.Configuration
{
	public interface IConfigurationStorage
	{
		string BrokenConfigurationFileName { get; }
		bool IsSaveBlocked { get; }

		void Load();
		string ReadLanguage();
		void Save();
	}
}