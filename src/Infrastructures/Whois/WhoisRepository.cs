using SiteStatus.Domains.Whois;
using SiteStatus.Infrastructures.Storage;

namespace SiteStatus.Infrastructures.Whois
{
    public class WhoisRepository: IWhoisRepository
    {
        private readonly IWhoisStorage _whoisStorage;

        public WhoisRepository(IWhoisStorage whoisStorage)
        {
            this._whoisStorage = whoisStorage;
        }

        public void Put(string json)
        {
            this._whoisStorage.Put(json);
        }
    }
}
