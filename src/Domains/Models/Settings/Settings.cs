using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SiteStatus.Domains.Settings
{
    public class SecondLevelDomain
    {
        [JsonPropertyName("destination")]
        public string Destination { get; set; }
    }

    public class Certifications
    {
        [JsonPropertyName("destination")]
        public string Destination { get; set; }
    }
    public class OutPut
    {
        [JsonPropertyName("secondLevelDomain")]
        public SecondLevelDomain SecondLevelDomain { get; set; }

        [JsonPropertyName("certifications")]
        public Certifications Certifications { get; set; }
    }

    public class S3
    {
        [JsonPropertyName("bucketName")]
        public string BucketName { get; set; }

        [JsonPropertyName("isPublicRead")]
        public string IsPublicRead { get; set; }
    }

    public class Settings
    {
        [JsonPropertyName("domains")]
        public List<string> Domains { get; set; } = new List<string>();

        [JsonPropertyName("storageType")]
        public string StorageType { get; set; }

        [JsonPropertyName("output")]
        public OutPut OutPut { get; set; }

        [JsonPropertyName("s3")]
        public S3 S3 { get; set; }
    }
}
