using System;
using System.IO;
using SiteStatus.Domains.Services;
using SiteStatus.Infrastructures.Settings;
using SiteStatus.Utils;

namespace SiteStatus
{
    class Program
    {
        static void Main(string[] args)
        {
            var settingsRepo = new SettingsRepository();
            var settingsService = new SettingsService(settingsRepo);
            var settings = settingsService.Get(Path.Join(Directory.GetCurrentDirectory(), "settings.json"));

            // TODO: impl validation

            // TODO: exec parallelly
            foreach (string domain in settings.Domains)
            {
                ServerCertificate result = null;
                try
                {
                    result = SiteStatus.Utils.HttpClient.GetServerCertificate(new Uri(domain))
                        .GetAwaiter()
                        .GetResult();
                }
                catch (Exception ex)
                {
                    // TODO: logging
                    Console.WriteLine(ex.Message);
                    continue;
                }
                Console.WriteLine(result.Certificate.Issuer);
            }
            // TODO: put result to storage
        }
    }
}
