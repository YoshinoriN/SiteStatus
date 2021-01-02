using System;
using System.Collections.Generic;
using System.Net.Security;
using System.Text.Encodings.Web;
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
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
            return JsonSerializer.Serialize(certificates, options);
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

        /// <summary>
        /// Get server certificate
        /// </summary>
        /// <param name="domain"></param>
        /// <returns></returns>
        public Certificate GetServerCertificate(string domain)
        {
            try
            {
                var result = SiteStatus.Utils.HttpClient.GetServerCertificate(new Uri("https://" + domain))
                    .GetAwaiter()
                    .GetResult();

                return new Certificate
                {
                    Domain = domain,
                    Status = result != null ? (result.SslErrors == SslPolicyErrors.None ? "success" : "error") : "error",
                    Issuer = result != null ? result.Certificate.Issuer : "N/A",
                    // NOTE: timezone
                    ValidFrom = result != null ? ((DateTimeOffset)result.Certificate.NotBefore).ToUnixTimeSeconds() : null,
                    ValidTo = result != null ? ((DateTimeOffset)result.Certificate.NotAfter).ToUnixTimeSeconds() : null,
                    CheckedAt = DateTimeOffset.Now.ToUnixTimeSeconds()
                };
            }
            catch (Exception ex)
            {
                // TODO: logging
                Console.WriteLine(ex.Message);
                return new Certificate
                {
                    Domain = domain,
                    Status = "error",
                    Issuer = "N/A",
                    ValidFrom = null,
                    ValidTo = null,
                    CheckedAt = DateTimeOffset.Now.ToUnixTimeSeconds()
                };
            }
        }
    }
}
