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
    public static class HttpClient
    {
        private static readonly System.Net.Http.HttpClient _httpClient;

        // TODO: fix
        private static ServerCertificate _tmpResponseResult = null;

        static HttpClient()
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (
                    HttpRequestMessage requestMessage,
                    X509Certificate2 certificate,
                    X509Chain chain,
                    SslPolicyErrors sslErrors) =>
                {
                    // TODO: fix
                    _tmpResponseResult = new ServerCertificate(new X509Certificate2(certificate), sslErrors);
                    return sslErrors == SslPolicyErrors.None;
                }
            };
            // TODO: set timeout
            _httpClient = new System.Net.Http.HttpClient(handler);
        }

        public static async Task<ServerCertificate> GetServerCertificate(Uri uri)
        {
            // TODO: fix
            _tmpResponseResult = null;
            await _httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Head, uri));
            return _tmpResponseResult;
        }

    }
}
