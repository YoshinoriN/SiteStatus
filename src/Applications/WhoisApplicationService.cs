using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using SiteStatus.Domains.Services;
using Whois.Models;

namespace SiteStatus.Applications
{
    public class WhoisApplicationService
    {
        private readonly WhoisService _whoisService;
        private readonly ParallelOptions parallelOptions = new ParallelOptions();

        public WhoisApplicationService(WhoisService whoisService, int maxDegreeOfParallelism = 4)
        {
            this._whoisService = whoisService;
            this.parallelOptions.MaxDegreeOfParallelism = maxDegreeOfParallelism;
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
                whoisResult = this._whoisService.Lookup(sld)
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

        /// <summary>
        /// Whois lookup parallelly
        /// </summary>
        /// <param name="sld">second level domains</param>
        /// <returns></returns>
        public List<SiteStatus.Domains.Whois.Whois> LookupParallelly(IEnumerable<string> slds)
        {
            ConcurrentQueue<SiteStatus.Domains.Whois.Whois> whoisInfos = new ConcurrentQueue<SiteStatus.Domains.Whois.Whois>();
            Parallel.ForEach(slds, parallelOptions, sld =>
            {
                whoisInfos.Enqueue(this.Lookup(sld));
            });
            return whoisInfos.ToList();
        }

        /// <summary>
        /// Store whois
        /// </summary>
        /// <param name="whois">List of Whois instances</param>
        public void Put(List<Domains.Whois.Whois> whois)
        {
            this._whoisService.Put(whois);
        }
    }
}
