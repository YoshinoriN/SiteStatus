using System;
using System.IO;
using SiteStatus.Domains.Services;
using SiteStatus.Infrastructures.Settings;
using SiteStatus.Utils;
using System.Text.Json;
using Whois.Models;
using System.Collections.Generic;
using SiteStatus.Infrastructures.Certificates;
using SiteStatus.Infrastructures.Certificates.Storage;
using SiteStatus.Domains.Certificates;
using System.Net.Security;

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
            List<Certificate> certificates = new List<Certificate>();
            foreach (string domain in settings.Domains)
            {
                WhoisResponse whoisResult = null;
                try
                {
                    whoisResult = SiteStatus.Utils.Whois.Lookup(domain)
                        .GetAwaiter()
                        .GetResult();
                }
                catch (Exception ex)
                {
                    // TODO: logging
                    Console.WriteLine(ex.Message);
                    continue;
                }
                Console.WriteLine(JsonSerializer.Serialize(whoisResult.ParsedResponse));

                try
                {
                    ServerCertificate result = null;
                    result = SiteStatus.Utils.HttpClient.GetServerCertificate(new Uri("https://" + domain))
                        .GetAwaiter()
                        .GetResult();

                    certificates.Add(new Certificate
                    {
                        Domain = domain,
                        Status = result != null ? (result.SslErrors == SslPolicyErrors.None ? "success" : "error") : "error",
                        Issuer = result != null ? result.Certificate.Issuer : "N/A",
                        // NOTE: timezone
                        ValidFrom = result != null ? ((DateTimeOffset)result.Certificate.NotBefore).ToUnixTimeSeconds() : null,
                        ValidTo = result != null ? ((DateTimeOffset)result.Certificate.NotAfter).ToUnixTimeSeconds() : null,
                        CheckedAt = DateTimeOffset.Now.ToUnixTimeSeconds()
                    });
                }
                catch (Exception ex)
                {
                    // TODO: logging
                    Console.WriteLine(ex.Message);
                    certificates.Add(new Certificate
                    {
                        Domain = domain,
                        Status = "error",
                        Issuer = "N/A",
                        ValidFrom = null,
                        ValidTo = null,
                        CheckedAt = DateTimeOffset.Now.ToUnixTimeSeconds()
                    });
                }
            }

            var certificateStorage = CertificateStorageFactory.CreateCertificateStorage(settings);
            var certificateRepository = new CertificateRepository(certificateStorage);
            var certificateService = new CertificateService(certificateRepository);

            certificateService.Put(certificates);
            // TODO: put result to storage
        }
    }
}
