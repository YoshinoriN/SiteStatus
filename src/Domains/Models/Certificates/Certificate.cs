using System.Text.Json.Serialization;

namespace SiteStatus.Domains.Certificates
{
    public class Certificate
    {
        [JsonPropertyName("domain")]
        public string Domain { get; set; }

        // TODO: setterでErrorの場合にN/Aにする
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
