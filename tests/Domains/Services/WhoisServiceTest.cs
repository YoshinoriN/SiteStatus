using System.IO;
using System.Text.Json;
using SiteStatus.Domains.Services;
using SiteStatus.Infrastructures.Settings;
using SiteStatus.Infrastructures.Whois;
using SiteStatus.Infrastructures.Whois.Storage;
using Xunit;

namespace SiteStatus.Tests.Utils
{
    public class WhoisServiceTest
    {
        [Fact]
        public void WhoisLookupSuccessTest()
        {
            var settingsRepo = new SettingsRepository();
            var settingsService = new SettingsService(settingsRepo);
            var settings = settingsService.Get(Path.Join(Directory.GetCurrentDirectory(), "settings.json"));

            var whoisStorage = WhoisStorageFactory.CreateWhoisStorage(settings);
            var whoisRepository = new WhoisRepository(whoisStorage);
            var whoisService = new WhoisService(whoisRepository);

            var whoisResult = whoisService.Lookup("example.com")
                            .GetAwaiter()
                            .GetResult();
            var prsedResponse = JsonSerializer.Serialize(whoisResult.ParsedResponse);
            Assert.StartsWith("{", prsedResponse);
            Assert.EndsWith("}", prsedResponse);
            Assert.Contains("DomainName", prsedResponse);
        }
    }
}
