using Xunit;
using System.IO;
using System;
using System.Net.Security;
using System.Net.Http;

namespace SiteStatus.Tests
{
    public class HttpClientTest
    {
        [Fact]
        public void HttpsOkResponseTest()
        {
            var result = SiteStatus.Utils.HttpClient.GetServerCertificate(new Uri("https://example.com"))
                .GetAwaiter()
                .GetResult();
            Assert.Equal(SslPolicyErrors.None, result.SslErrors);
            Assert.StartsWith("CN=", result.Certificate.Issuer);
        } 

        [Fact]
        public void DomainDoesNotExistsTest()
        {
            Assert.Throws<HttpRequestException>(() =>
            {
                _ = SiteStatus.Utils.HttpClient.GetServerCertificate(new Uri("https://sub.example.com"))
                .GetAwaiter()
                .GetResult();
            });
        }
    }
}
