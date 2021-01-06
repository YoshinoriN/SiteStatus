using System;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace SiteStatus.Utils
{
    public class ServerCertificate
    {
        public X509Certificate2 Certificate { get; private set; }

        public SslPolicyErrors SslErrors { get; private set; }

        public ServerCertificate(X509Certificate2 certificate, SslPolicyErrors sslErrors)
        {
            this.Certificate = certificate;
            this.SslErrors = sslErrors;
        }
    }

    // TODO: Should I move this to ServiceClass??
    public static class HttpClient
    {
        public static async Task<ServerCertificate> GetServerCertificate(Uri uri)
        {
            ServerCertificate _tmpResponseResult = null;

            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (
                    HttpRequestMessage requestMessage,
                    X509Certificate2 certificate,
                    X509Chain chain,
                    SslPolicyErrors sslErrors) =>
                {
                    _tmpResponseResult = new ServerCertificate(new X509Certificate2(certificate), sslErrors);
                    return sslErrors == SslPolicyErrors.None;
                }
            };

            // TODO: avoid using
            using (var httpClient = new System.Net.Http.HttpClient(handler))
            {
                httpClient.Timeout = TimeSpan.FromSeconds(60);
                _tmpResponseResult = null;
                await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Head, uri));
            }
            return _tmpResponseResult;
        }

    }
}
