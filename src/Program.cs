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

            var domains = settings.Domains.Distinct();
            var slds = domains
                .Where(d => d.Contains("."))
                .Select(d =>
                {
                    var x = d.Split(".").Reverse().ToArray();
                    return string.Join(".", x[1], x[0]);
                })
                .Distinct();

            var whoisStorage = WhoisStorageFactory.CreateWhoisStorage(settings);
            var whoisRepository = new WhoisRepository(whoisStorage);
            var whoisService = new WhoisService(whoisRepository);

            var certificateStorage = CertificateStorageFactory.CreateCertificateStorage(settings);
            var certificateRepository = new CertificateRepository(certificateStorage);
            var certificateService = new CertificateService(certificateRepository);

            List<Certificate> certificates = new List<Certificate>();
            List<SiteStatus.Domains.Whois.Whois> whoisInfos = new List<SiteStatus.Domains.Whois.Whois>();

            // TODO: exec parallelly
            foreach (string sld in slds)
            {
                whoisInfos.Add(whoisService.Lookup(sld));
            }

            // TODO: exec parallelly
            foreach (string domain in domains)
            {
                certificates.Add(certificateService.GetServerCertificate(domain));
            }

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
