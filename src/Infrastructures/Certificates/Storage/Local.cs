using System.IO;
using System.Text.Json;
using SiteStatus.Infrastructures.Storage;

namespace SiteStatus.Infrastructures.Certificates.Storage
{
    public class Local : ICertificateStorage
    {
        public Domains.Settings.Settings _settings { get; private set; }

        public Local(Domains.Settings.Settings settings)
        {
            this._settings = settings;
        }
            
        public void Put(string json)
        {
            var filePath = Path.Join(Directory.GetCurrentDirectory(), this._settings.OutPut.Certifications.Destination);
            using (StreamWriter streamWriter = new StreamWriter(filePath))
            {
                streamWriter.Write(JsonSerializer.Serialize(json));
            }
        }
    }
}
