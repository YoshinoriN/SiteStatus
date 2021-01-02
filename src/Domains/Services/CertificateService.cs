using System.Collections.Generic;
using System.Text.Json;
using SiteStatus.Domains.Certificates;

namespace SiteStatus.Domains.Services
{
    public class CertificateService
    {
        private readonly ICertificatesRepository _certificateRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="certificatesRepository">ICertificateRepository</param>
        public CertificateService(ICertificatesRepository certificatesRepository)
        {
            this._certificateRepository = certificatesRepository;
        }

        /// <summary>
        /// Serialize Certificates instance to JSON
        /// </summary>
        /// <param name="certificates">List of Certificate instance</param>
        /// <returns>JSON string</returns>
        public string Serialize(List<Domains.Certificates.Certificate> certificates)
        {
            return JsonSerializer.Serialize(certificates);
        }

        /// <summary>
        /// Store certificates
        /// </summary>
        /// <param name="certificates">List of Certificate instance</param>
        public void Put(List<Domains.Certificates.Certificate> certificates)
        {
            var json = this.Serialize(certificates);
            this._certificateRepository.Put(json);
        }
    }
}
