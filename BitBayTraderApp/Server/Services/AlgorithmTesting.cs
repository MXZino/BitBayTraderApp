using BitBayTraderApp.Server.Models;
using BitBayTraderApp.Server.Models.Hubs;
using BitBayTraderApp.Shared.Models.DTO.PublicRest;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text.Json;
using System.Threading.Tasks;

namespace BitBayTraderApp.Server.Services
{
    public class AlgorithmTesting
    {
        private List<Candle> candles;
        private readonly HttpClient httpClient;
        private readonly IHubContext<PublicAPIHub> publicAPIHubContext;
        private readonly IConfiguration configuration;
        public AlgorithmTesting(HttpClient httpClient, IHubContext<PublicAPIHub> publicAPIHubContext, IConfiguration configuration)
        {
            this.httpClient = httpClient;
            this.publicAPIHubContext = publicAPIHubContext;
            this.configuration = configuration;
            candles = new List<Candle>();
        }
        private async Task GetData(string marketCode, string interval, long from)
        {
            var response = await httpClient.GetAsync($"https://api.binance.com/api/v3/klines?symbol={marketCode}&interval={interval}&startTime={from}&limit=1000");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                try
                {
                    var opts = SerializationOption.Add(new Serialization[] { Serialization.DoubleToString, Serialization.TimeStampToString });
                    opts.Converters.Add(new DoubleToStringConverter());
                    opts.Converters.Add(new TimeStampToStringConverter());

                    var items = JsonConvert.DeserializeObject<object[]>(json);

                    foreach (var elem in items)
                    {
                        var array = JsonConvert.DeserializeObject<object[]>(elem.ToString());
                        var time = FromUnixTime(Convert.ToInt64(array[0].ToString()));

                        var matched = candles.FirstOrDefault(x => x.TimeStamp == time);
                        if (matched == null)
                        {
                            Candle candle = new Candle
                            {
                                TimeStamp = time,
                                Open = double.Parse(array[1].ToString(), System.Globalization.CultureInfo.InvariantCulture),
                                Highest = double.Parse(array[2].ToString(), System.Globalization.CultureInfo.InvariantCulture),
                                Lowest = double.Parse(array[3].ToString(), System.Globalization.CultureInfo.InvariantCulture),
                                Close = double.Parse(array[4].ToString(), System.Globalization.CultureInfo.InvariantCulture),
                                Volume = double.Parse(array[5].ToString(), System.Globalization.CultureInfo.InvariantCulture),
                            };

                            candles.Add(candle);
                        }
                        else
                        {

                        }

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        public async Task Start()
        {
            var startTime = 1585008000000;
            long frequency = 3600000;
            long next1000recordsTime = frequency * 1000;

            for (int i = 0; i < 7; i++)
            {
                await GetData("BTCUSDT", "1h", startTime + next1000recordsTime * i);
            }

            double amount = 3000;
            double currency = 0.2;
            double currentCurrency = 0;
            bool isLastTransactionBuy = false;
            candles = candles.OrderBy(x => x.TimeStamp).ToList();
            var lastTransactionAmount = 8000000.0;

            for (int i = 52; i < candles.Count; i++)
            {
                var shorterNumber = 5;
                var longerNumber = 20;

                var lastShorter = candles.Where(x => x.TimeStamp < candles[i].TimeStamp && x.TimeStamp >= candles[i].TimeStamp.AddHours(-shorterNumber)).ToList();
                var lastLonger = candles.Where(x => x.TimeStamp < candles[i].TimeStamp && x.TimeStamp >= candles[i].TimeStamp.AddHours(-longerNumber)).ToList();

                var shorterAverage = 0.0;
                var longerAverage = 0.0;

                

                foreach (var elem in lastShorter)
                {
                    shorterAverage += elem.Open;
                }

                foreach (var elem in lastLonger)
                {
                    longerAverage += elem.Open;
                }

                shorterAverage = shorterAverage / shorterNumber;
                longerAverage = longerAverage / longerNumber;

                if (!isLastTransactionBuy)
                {
                    var value = candles[i].Open * currency;
                    if (shorterAverage > longerAverage)
                    {          
                        currentCurrency = 0.2;
                        amount = amount - value;
                        lastTransactionAmount = value;

                        Console.WriteLine($"!KUPNO za {value}!");
                        Console.WriteLine($"{candles[i].TimeStamp}: Kwota na koncie - {amount}, Kwota w krypto - {currentCurrency * candles[i].Open}, Razem - {amount + currentCurrency * candles[i].Open}");
                        if (amount < 0)
                        {
                            Console.WriteLine($"{candles[i].TimeStamp} {amount} - brak funduszy");
                            break;
                        }
                        isLastTransactionBuy = true;
                        continue;
                    }
                    Console.WriteLine($"{candles[i].TimeStamp}: Kwota na koncie - {amount}, Kwota w krypto - {currentCurrency * candles[i].Open}, Razem - {amount + currentCurrency * candles[i].Open}");
                }
                else
                {
                    var value = candles[i].Open * currency;
                    if ((shorterAverage < longerAverage) && lastTransactionAmount < value)
                    {
                        amount = amount + value;
                        //lastTransactionAmount = value;

                        Console.WriteLine($"!SPRZEDAŻ za {value}!");
                        Console.WriteLine($"{candles[i].TimeStamp}: Kwota na koncie - {amount}");
                        isLastTransactionBuy = false;
                        currentCurrency = 0;
                        continue;
                    }
                    Console.WriteLine($"{candles[i].TimeStamp}: Kwota na koncie - {amount}, Kwota w krypto - {currentCurrency * candles[i].Open}, Razem - {amount + currentCurrency * candles[i].Open}");
                }
            }

        }

        private DateTime FromUnixTime(long unixTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddMilliseconds(unixTime);
        }
    }
}
