using SiteStatus.Domains.Settings;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace SiteStatus.Domains.Services
{
    public class SettingsService
    {
        private readonly ISettingsRepository _settingsRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="settingsRepository">ISettingsRepository</param>
        public SettingsService(ISettingsRepository settingsRepository)
        {
            this._settingsRepository = settingsRepository;
        }

        /// <summary>
        /// Desirialize JSON from String
        /// </summary>
        /// <param name="rawJson">Raw JSON</param>
        /// <returns>Settings Instance</returns>
        private Domains.Settings.Settings Desirialize(string rawJson)
        {
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true
            };
            return JsonSerializer.Deserialize<Domains.Settings.Settings>(rawJson, options);
        }

        /// <summary>
        /// Get Settings Instance
        /// </summary>
        /// <param name="path">raw JSON file path</param>
        /// <returns>Settings Instance</returns>
        public Domains.Settings.Settings Get(string path)
        {
            var rawJson = this._settingsRepository.ReadRawJsonFromFile(path);
            return this.Desirialize(rawJson);
        }
    }
}
