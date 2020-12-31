namespace SiteStatus.Domains.Settings
{
    public interface ISettingsRepository
    {
        string ReadRawJsonFromFile(string path);
    }
}
