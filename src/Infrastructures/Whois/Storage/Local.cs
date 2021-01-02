using System.IO;
using SiteStatus.Infrastructures.Storage;

namespace SiteStatus.Infrastructures.Whois.Storage
{
    public class Local : IWhoisStorage
    {
        public Domains.Settings.Settings _settings { get; private set; }

        public Local(Domains.Settings.Settings settings)
        {
            this._settings = settings;
        }
            
        public void Put(string json)
        {
            var filePath = Path.Join(Directory.GetCurrentDirectory(), this._settings.OutPut.Whois.Destination);
            using (StreamWriter streamWriter = new StreamWriter(filePath))
            {
                streamWriter.Write(json);
            }
        }
    }
}
