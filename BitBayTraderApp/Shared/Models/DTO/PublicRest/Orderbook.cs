using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BitBayTraderApp.Shared.Models.DTO.PublicRest
{
    public class Orderbook
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("sell")]
        public List<Order> Sell { get; set; }

        [JsonPropertyName("buy")]
        public List<Order> Buy { get; set; }

        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }

        [JsonPropertyName("seqNo")]
        public string SeqNo { get; set; }
    }

    public class Order
    {
        [JsonPropertyName("ra")]
        public double Rate { get; set; }

        [JsonPropertyName("ca")]
        public double CurrentAmount { get; set; }

        [JsonPropertyName("sa")]
        public double StartingAmount { get; set; }

        [JsonPropertyName("pa")]
        public double PreviousAmount { get; set; }

        [JsonPropertyName("co")]
        public int Offers { get; set; }
    }
}
