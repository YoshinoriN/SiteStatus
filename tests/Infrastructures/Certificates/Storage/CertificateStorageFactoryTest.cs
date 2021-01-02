using System.IO;
using SiteStatus.Domains.Services;
using SiteStatus.Infrastructures.Settings;
using SiteStatus.Infrastructures.Certificates.Storage;
using Xunit;
using System;

namespace SiteStatus.Tests.Infrastructures.Certificates.Storage
{
    public class CertificateStorageFactoryTest
    {
        [Fact]
        public void LocalStorageInstanceCreateTest()
        {
            var settingsRepo = new SettingsRepository();
            var settingsService = new SettingsService(settingsRepo);
            var settings = settingsService.Get(Path.Join(Directory.GetCurrentDirectory(), "settings.example.json"));

            var instance = CertificateStorageFactory.CreateCertificateStorage(settings);
            Assert.IsType<SiteStatus.Infrastructures.Certificates.Storage.Local>(instance);
        }

        [Fact]
        public void S3StorageInstanceCreateTest()
        {
            var settingsRepo = new SettingsRepository();
            var settingsService = new SettingsService(settingsRepo);
            var settings = settingsService.Get(Path.Join(Directory.GetCurrentDirectory(), "settings.example.json"));
            settings.StorageType = "s3";

            var instance = CertificateStorageFactory.CreateCertificateStorage(settings);
            Assert.IsType<SiteStatus.Infrastructures.Certificates.Storage.S3>(instance);
        }

        [Fact]
        public void ThrowExceptionTest()
        {
            var settingsRepo = new SettingsRepository();
            var settingsService = new SettingsService(settingsRepo);
            var settings = settingsService.Get(Path.Join(Directory.GetCurrentDirectory(), "settings.example.json"));
            settings.StorageType = "dummy";

            Assert.Throws<ArgumentException>(() =>
            {
                CertificateStorageFactory.CreateCertificateStorage(settings);
            });
        }
    }
}
