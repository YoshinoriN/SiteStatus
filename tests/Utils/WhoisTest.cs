using System.Text.Json;
using Xunit;

namespace SiteStatus.Tests.Utils
{
    public class WhoisTest
    {
        [Fact]
        public void WhoisLookupSuccessTest()
        {
            var whoisResult = SiteStatus.Utils.Whois.Lookup("example.com")
                            .GetAwaiter()
                            .GetResult();
            var prsedResponse = JsonSerializer.Serialize(whoisResult.ParsedResponse);
            Assert.StartsWith("{", prsedResponse);
            Assert.EndsWith("}", prsedResponse);
            Assert.Contains("DomainName", prsedResponse);

        }
    }
}
