using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Text.Json;
using SiteStatus.Domains.Whois;

namespace SiteStatus.Domains.Services
{
    public class WhoisService
    {
        private readonly IWhoisRepository _whoisRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="whoisRepository">IWhoisRepository</param>
        public WhoisService(IWhoisRepository whoisRepository)
        {
            this._whoisRepository = whoisRepository;
        }

        /// <summary>
        /// Serialize whois instancess to JSON
        /// </summary>
        /// <param name="whois">List of Whois instances</param>
        /// <returns>JSON string</returns>
        public string Serialize(List<Domains.Whois.Whois> whois)
        {
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
            return JsonSerializer.Serialize(whois, options);
        }

        /// <summary>
        /// Store whois
        /// </summary>
        /// <param name="whois">List of Whois instances</param>
        public void Put(List<Domains.Whois.Whois> whois)
        {
            var json = this.Serialize(whois);
            this._whoisRepository.Put(json);
        }
    }
}
