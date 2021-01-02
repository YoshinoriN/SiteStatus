using SiteStatus.Domains.Certificates;
using SiteStatus.Infrastructures.Storage;

namespace SiteStatus.Infrastructures.Certificates
{
    public class CertificateRepository: ICertificatesRepository
    {
        private readonly ICertificateStorage _certificateStorage;

        public CertificateRepository(ICertificateStorage certificateStorage)
        {
            this._certificateStorage = certificateStorage;
        }

        public void Put(string json)
        {
            this._certificateStorage.Put(json);
        }
    }
}
