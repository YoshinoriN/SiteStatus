                                                                                                  using System;
using SiteStatus.Infrastructures.Storage;

namespace SiteStatus.Infrastructures.Certificates.Storage
{
    public class S3 : ICertificateStorage
    {
        public S3(Domains.Settings.Settings settings)
        {
            // TODO: set settings
        }

        public void Put(string path)
        {
            // TODO: impl
            throw new NotImplementedException();
        }
    }
}
