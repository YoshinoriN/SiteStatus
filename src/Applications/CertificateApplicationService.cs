using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiteStatus.Domains.Certificates;
using SiteStatus.Domains.Services;

namespace SiteStatus.Applications
{
    public class CertificateApplicationService
    {
        private readonly CertificateService _certificateService;
        private readonly ParallelOptions parallelOptions = new ParallelOptions();

        public CertificateApplicationService(CertificateService certificateService, int maxDegreeOfParallelism = 4)
        {
            this._certificateService = certificateService;
            this.parallelOptions.MaxDegreeOfParallelism = maxDegreeOfParallelism;
        }

        /// <summary>
        /// Get server certificate parallelly
        /// </summary>
        /// <param name="domain"></param>
        /// <returns></returns>
        public List<Certificate> GetServerCertificateParallelly(IEnumerable<string> domains)
        {
            List<Certificate> certificates = new List<Certificate>();
            Parallel.ForEach(domains, parallelOptions, domain =>
            {
                certificates.Add(this._certificateService.GetServerCertificate(domain));
            });
            return certificates;
        }

        /// <summary>
        /// Store certificates
        /// </summary>
        /// <param name="certificates">List of Certificate instance</param>
        public void Put(List<Domains.Certificates.Certificate> certificates)
        {
            this._certificateService.Put(certificates);
        }
    }
}
