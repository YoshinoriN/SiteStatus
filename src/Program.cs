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
using SiteStatus.Domains.Whois;
using System.Net.Security;
using SiteStatus.Infrastructures.Whois.Storage;
using SiteStatus.Infrastructures.Whois;
using System.Linq;

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

            List<Certificate> certificates = new List<Certificate>();
            List<SiteStatus.Domains.Whois.Whois> whoisInfos = new List<SiteStatus.Domains.Whois.Whois>();

            var domains = settings.Domains.Distinct();
            var slds = domains
                .Where(d => d.Contains("."))
                .Select(d =>
                {
                    var x = d.Split(".").Reverse().ToArray();
                    return string.Join(".", x[1], x[0]);
                })
                .Distinct();

            // TODO: exec parallelly
            foreach (string sld in slds)
            {
                try
                {
                    WhoisResponse whoisResult = null;
                    whoisResult = SiteStatus.Utils.Whois.Lookup(sld)
                        .GetAwaiter()
                        .GetResult();

                    if (whoisResult.ParsedResponse == null)
                    {
                        throw new Exception("Can not resolve host");
                    }

                    var json = JsonSerializer.Serialize(whoisResult.ParsedResponse);
                    var w = JsonSerializer.Deserialize<Domains.Whois.Whois>(json);
                    w.Status = "success";
                    w.CheckedAt = DateTimeOffset.Now.ToUnixTimeSeconds();
                    whoisInfos.Add(w);
                }
                catch (Exception ex)
                {
                    // TODO: logging
                    Console.WriteLine(ex.Message);
                    whoisInfos.Add(new Domains.Whois.Whois
                    {
                        DomainName = sld,
                        Status = "error",
                        CheckedAt = DateTimeOffset.Now.ToUnixTimeSeconds()
                    });
                }
            }

            // TODO: exec parallelly
            foreach (string domain in domains)
            {
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

            var whoisStorage = WhoisStorageFactory.CreateWhoisStorage(settings);
            var whoisRepository = new WhoisRepository(whoisStorage);
            var whoisService = new WhoisService(whoisRepository);

            var certificateStorage = CertificateStorageFactory.CreateCertificateStorage(settings);
            var certificateRepository = new CertificateRepository(certificateStorage);
            var certificateService = new CertificateService(certificateRepository);

            try
            {
                whoisService.Put(whoisInfos);
                certificateService.Put(certificates);
            } 
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
