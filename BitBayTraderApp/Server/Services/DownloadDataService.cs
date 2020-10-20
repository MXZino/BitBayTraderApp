using BitBayTraderApp.Server.Interfaces;
using BitBayTraderApp.Server.Models.Hubs;
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
        private readonly IHubContext<PublicRESTHub> publicRESTHubContext;
        private readonly IPublicRESTService publicRestService;
        private readonly List<string> marketCodes;

        public DownloadDataService(IConfiguration configuration, IPublicRESTService publicRestService, IHubContext<PublicRESTHub> publicRESTHubContext)
        {
            this.configuration = configuration;
            this.publicRestService = publicRestService;
            this.publicRESTHubContext = publicRESTHubContext;

            marketCodes = new List<string> { "BTC-PLN", "ETH-PLN" };
        }

        public async Task GetTicker(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                foreach (var market in marketCodes)
                {
                    var ticker = await publicRestService.GetTicker(market);
                    await publicRESTHubContext.Clients.All.SendAsync("ReceiveTickerStatus", market, ticker);
                }

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
