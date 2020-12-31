using System.IO;

namespace SiteStatus.utils
{
    public static class File
    {
        public static string ReadFile(string path)
        {
            using (var streamReader = new StreamReader(path))
            {
                return streamReader.ReadToEnd();
            };
        }
    }
}
