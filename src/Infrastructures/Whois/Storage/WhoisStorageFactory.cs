using System;
using SiteStatus.Infrastructures.Storage;

namespace SiteStatus.Infrastructures.Whois.Storage
{
    public static class WhoisStorageFactory
    {
        public static IWhoisStorage CreateWhoisStorage(Domains.Settings.Settings settings)
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
