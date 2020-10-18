using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BitBayTraderApp.Shared.Models.DTO.PublicRest
{
    public class CurrentStatus
    {
        public string Status { get; set; }
        public Ticker Ticker { get; set; }
    }

    public class First
    {
        public string Currency { get; set; }
        public double MinOffer { get; set; }
        public int Scale { get; set; }
    }

    public class Second
    {
        public string Currency { get; set; }
        public double MinOffer { get; set; }
        public int Scale { get; set; }
    }

    public class Market
    {
        public string Code { get; set; }
        public First First { get; set; }
        public Second Second { get; set; }
    }

    public class Ticker
    {
        public Market Market { get; set; }
        public DateTime Time { get; set; }
        public double HighestBid { get; set; }
        public double LowestAsk { get; set; }
        public double Rate { get; set; }
        public double PreviousRate { get; set; }
    }
}
