using System;
using System.Text.Json.Serialization;

namespace SiteStatus.Domains.Whois
{
    public class Registrar
    {
        [JsonPropertyName("Name")]
        public string DomainName { get; set; }
    }

    // TODO: lowerCamelCase
    public class Whois
    {
        [JsonPropertyName("DomainName")]
        public string DomainName { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        private string? _registerd = null;
        [JsonPropertyName("Registered")]
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
        [JsonPropertyName("Updated")]
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
        [JsonPropertyName("Expiration")]
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

        [JsonPropertyName("Registrar")]
        public Registrar Registrar { get; set; }

        [JsonPropertyName("NameServers")]
        public string[] NameServers { get; set; }

        [JsonPropertyName("CheckedAt")]
        public long CheckedAt { get; set; }
    }


}
