﻿using Xunit;
using System.IO;
using SiteStatus.Domains.Services;
using SiteStatus.Infrastructures.Settings;

namespace SiteStatus.Tests.Domains.Services
{
    public class SettingsServiceTest
    {
        [Fact]
        public void GetSettingsTest()
        {
            var settingsRepo = new SettingsRepository();
            var settingsService = new SettingsService(settingsRepo);
            
            var settings = settingsService.Get(Path.Join(Directory.GetCurrentDirectory(), "settings.example.json"));
            Assert.Equal("local", settings.StorageType);
            Assert.Equal("example.com", settings.Domains[0]);
            Assert.Equal("sub.example.com", settings.Domains[1]);
            Assert.Equal("./certifications.json", settings.OutPut.Certifications.Destination);
            Assert.Equal("./whois.json", settings.OutPut.Whois.Destination);
        }
    }
}
