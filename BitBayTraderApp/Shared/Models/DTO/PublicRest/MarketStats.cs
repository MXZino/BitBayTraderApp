using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BitBayTraderApp.Shared.Models.DTO.PublicRest
{
    public class MarketStats
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("stats")]
        public Stats Stats { get; set; }
    }
    public class Stats
    {
        [JsonPropertyName("m")]
        public string MarketCode { get; set; }

        [JsonPropertyName("h")]
        public string HighestRate { get; set; }

        [JsonPropertyName("l")]
        public string LowestRate { get; set; }

        [JsonPropertyName("v")]
        public string Last24HVolumen { get; set; }

        [JsonPropertyName("r24h")]
        public string Last24HAverageRate { get; set; }
    }
}
