using System;
using System.IO;
using SiteStatus.Domains.Services;
using SiteStatus.Infrastructures.Settings;
using System.Collections.Generic;
using SiteStatus.Infrastructures.Certificates;
using SiteStatus.Infrastructures.Certificates.Storage;
using SiteStatus.Domains.Certificates;
using SiteStatus.Infrastructures.Whois.Storage;
using SiteStatus.Infrastructures.Whois;
using System.Linq;
using System.Threading.Tasks;
using SiteStatus.Applications;

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

            ParallelOptions parallelOptions = new ParallelOptions();
            parallelOptions.MaxDegreeOfParallelism = 4;  // TODO: option

            try
            {
                var whoisStorage = WhoisStorageFactory.CreateWhoisStorage(settings);
                var whoisRepository = new WhoisRepository(whoisStorage);
                var whoisService = new WhoisService(whoisRepository);
                var whoisApplicationService = new WhoisApplicationService(whoisService);

                var whoisInfos = whoisApplicationService.LookupParallelly(slds);
                whoisApplicationService.Put(whoisInfos);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            try
            {
                var certificateStorage = CertificateStorageFactory.CreateCertificateStorage(settings);
                var certificateRepository = new CertificateRepository(certificateStorage);
                var certificateService = new CertificateService(certificateRepository);

                List<Certificate> certificates = new List<Certificate>();
                Parallel.ForEach(domains, parallelOptions, domain =>
                {
                    certificates.Add(certificateService.GetServerCertificate(domain));
                });
                certificateService.Put(certificates);
            } 
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
