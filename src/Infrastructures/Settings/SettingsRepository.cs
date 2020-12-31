using SiteStatus.Domains.Settings;


namespace SiteStatus.Infrastructures.Settings
{
    public class SettingsRepository : ISettingsRepository
    {
        public string ReadRawJsonFromFile(string path)
        {
            return utils.File.ReadFile(path);
        }
    }
}
