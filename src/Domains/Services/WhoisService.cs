using System;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Text.Json;
using SiteStatus.Domains.Whois;
using Whois.Models;

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

        /// <summary>
        /// Whois lookup
        /// </summary>
        /// <param name="sld">second level domain</param>
        /// <returns></returns>
        public Domains.Whois.Whois Lookup(string sld)
        {
            try
            {
                WhoisResponse whoisResult = null;
                whoisResult = SiteStatus.Utils.Whois.Lookup(sld)
                    .GetAwaiter()
                    .GetResult();

                if (whoisResult.ParsedResponse == null)
                {
                    throw new Exception("Can not resolve host");
                }

                var options = new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                    PropertyNameCaseInsensitive = true
                };
                var json = JsonSerializer.Serialize(whoisResult.ParsedResponse, options);
                var w = JsonSerializer.Deserialize<Domains.Whois.Whois>(json, options);
                w.Status = "success";
                w.CheckedAt = DateTimeOffset.Now.ToUnixTimeSeconds();
                return w;
            }
            catch (Exception ex)
            {
                // TODO: logging
                Console.WriteLine(ex.Message);
                return new Domains.Whois.Whois
                {
                    DomainName = sld,
                    Status = "error",
                    CheckedAt = DateTimeOffset.Now.ToUnixTimeSeconds()
                };
            }
        }
    }
}
