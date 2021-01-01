using System;
using SiteStatus.Infrastructures.Storage;

namespace SiteStatus.Infrastructures.Certificates.Storage
{
    public static class CertificateStorageFactory
    {
        public static ICertificateStorage CreateCertificateStorage(Domains.Settings.Settings settings)
        {
            switch(settings.StorageType)
            {
                case "local":
                    return new Storage.Local(settings);
                case "s3":
                    return new Storage.S3(settings);
                default:
                    throw new ArgumentException();
            }
        }
    }
}
