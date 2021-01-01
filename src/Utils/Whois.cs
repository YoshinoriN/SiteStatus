using System.Threading.Tasks;
using Whois;
using Whois.Models;

namespace SiteStatus.Utils
{
    public static class Whois
    {
        public static async Task<WhoisResponse> Lookup(string domain)
        {
            var whois = new WhoisLookup();
            return await whois.LookupAsync(domain);
        }
    }
}
