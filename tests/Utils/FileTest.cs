using Xunit;
using System.IO;

namespace SiteStatus.Tests
{
    public class FileTest
    {
        [Fact]
        public void FileReadableTest()
        {
            var content = SiteStatus.utils.File.ReadFile(Path.Join(Directory.GetCurrentDirectory(), "settings.example.json"));
            Assert.StartsWith("{", content);
        } 

        [Fact]
        public void ThrowFileNotFoundExceptionTest()
        {
            Assert.Throws<FileNotFoundException>(() =>
            {
                SiteStatus.utils.File.ReadFile(Path.Join(Directory.GetCurrentDirectory(), "notfound.example.settings.json"));
            });
        }
    }
}
