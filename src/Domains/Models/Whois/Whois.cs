using System;
using System.Text.Json.Serialization;

namespace SiteStatus.Domains.Whois
{
    public class Registrar
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public class Whois
    {
        [JsonPropertyName("domainName")]
        public string DomainName { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        private string? _registerd = null;
        [JsonPropertyName("registered")]
        public string? Registered
        {
            get
            {
                return this._registerd;
            }
            set
            {
                this._registerd = value != null ? DateTimeOffset.Parse(value).ToUnixTimeSeconds().ToString() : null;
            }
        }

        private string? _updated = null;
        [JsonPropertyName("updated")]
        public string? Updated
        {
            get
            {
                return this._updated;
            }
            set
            {
                this._updated = value != null ? DateTimeOffset.Parse(value).ToUnixTimeSeconds().ToString() : null;
            }
        }

        private string? _expiration = null;
        [JsonPropertyName("expiration")]
        public string? Expiration
        {
            get
            {
                return this._expiration;
            }
            set
            {
                this._expiration = value != null ? DateTimeOffset.Parse(value).ToUnixTimeSeconds().ToString() : null;
            }
        }

        [JsonPropertyName("registrar")]
        public Registrar Registrar { get; set; }

        [JsonPropertyName("nameServers")]
        public string[] NameServers { get; set; }

        [JsonPropertyName("checkedAt")]
        public long CheckedAt { get; set; }
    }


}
