using BitBayTraderApp.Server.Interfaces;
using BitBayTraderApp.Server.Models.Hubs;
using BitBayTraderApp.Shared.Models.DTO.PublicRest;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BitBayTraderApp.Server.Services
{
    public class DownloadDataService
    {
        private readonly IConfiguration configuration;
        private readonly IPublicAPIService publicAPIService;
        private readonly List<string> marketCodes;

        public DownloadDataService(IConfiguration configuration, IPublicAPIService publicAPIService)
        {
            this.configuration = configuration;
            this.publicAPIService = publicAPIService;

            marketCodes = new List<string> { "BTC-PLN", "ETH-PLN" };
        }

        public async Task GetTicker(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                foreach (var market in marketCodes)
                {
                    var ticker = await publicAPIService.GetItems<CurrentStatus>(market);
                    var orderbook = await publicAPIService.GetItems<Orderbook>(market);
                }
                await Task.Delay(100, stoppingToken);
            }
        }
    }
}
