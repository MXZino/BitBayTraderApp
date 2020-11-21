using BitBayTraderApp.Server.Interfaces;
using BitBayTraderApp.Server.Models;
using BitBayTraderApp.Server.Models.Hubs;
using BitBayTraderApp.Shared.Models.DTO.PublicRest;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace BitBayTraderApp.Server.Services
{
    public class PublicAPIService : IPublicAPIService
    {
        private readonly HttpClient httpClient;
        private readonly IHubContext<PublicAPIHub> publicAPIHubContext;
        private readonly IConfiguration configuration;
        public PublicAPIService(HttpClient httpClient, IHubContext<PublicAPIHub> publicAPIHubContext, IConfiguration configuration)
        {
            this.httpClient = httpClient;
            this.publicAPIHubContext = publicAPIHubContext;
            this.configuration = configuration;
        }

        public async Task<T> GetItems<T>(string marketCode)
        {
            string path, hubMessage;

            switch (true)
            {
                case var _ when typeof(T) == typeof(CurrentStatus):
                    path = configuration.GetSection("PublicApi")["Ticker"];
                    hubMessage = "ReceiveTickerStatus";
                    break;

                case var _ when typeof(T) == typeof(MarketStats):
                    path = configuration.GetSection("PublicApi")["MarketStats"];
                    hubMessage = "ReceiveLast24HStats";
                    break;

                case var _ when typeof(T) == typeof(Orderbook):
                    path = configuration.GetSection("PublicApi")["Orderbook"];
                    hubMessage = "ReceiveOrderbook";
                    break;

                default:
                    return default;
            }


            var response = await httpClient.GetAsync($"{path}{marketCode}");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                try
                {
                    var opts = SerializationOption.Add(new Serialization[] { Serialization.DoubleToString, Serialization.TimeStampToString });
                    opts.Converters.Add(new DoubleToStringConverter());
                    opts.Converters.Add(new TimeStampToStringConverter());

                    var item = JsonSerializer.Deserialize<T>(json, opts);

                    await publicAPIHubContext.Clients.All.SendAsync(hubMessage, marketCode, item);

                    return item;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            return default;
        }
    }
}
