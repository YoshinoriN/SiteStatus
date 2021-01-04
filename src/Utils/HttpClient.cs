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
            // HttpClient Instance should be singleton.
            // But, server certificate infos have to store somewhere.
            // It can be accessed only in the callback func.
            _httpClient = new System.Net.Http.HttpClient(handler);
            // TODO: from settings
            _httpClient.Timeout = TimeSpan.FromSeconds(60);
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
