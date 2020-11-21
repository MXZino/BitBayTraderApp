using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BitBayTraderApp.Shared.Models.DTO.PublicRest
{
    public class Candle
    {
        public DateTime TimeStamp { get; set; }
        [JsonPropertyName("o")]
        public double Open { get; set; }
        [JsonPropertyName("c")]
        public double Close { get; set; }
        [JsonPropertyName("h")]
        public double Highest { get; set; }
        [JsonPropertyName("l")]
        public double Lowest { get; set; }
        [JsonPropertyName("v")]
        public double Volume { get; set; }
    }

    public class Root
    {

        [JsonPropertyName("items")]
        public object Items { get; set; }
    }

}
