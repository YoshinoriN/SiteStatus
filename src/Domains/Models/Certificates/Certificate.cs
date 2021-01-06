using System.Text.Json.Serialization;

namespace SiteStatus.Domains.Certificates
{
    public class Certificate
    {
        [JsonPropertyName("domain")]
        public string Domain { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("issuer")]
        public string Issuer { get; set; }

        [JsonPropertyName("validFrom")]
        public long? ValidFrom { get; set; }

        [JsonPropertyName("validTo")]
        public long? ValidTo { get; set; }

        [JsonPropertyName("checkedAt")]
        public long CheckedAt { get; set; }
    }
}
