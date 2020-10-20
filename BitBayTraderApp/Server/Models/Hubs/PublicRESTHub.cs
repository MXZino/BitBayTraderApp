using BitBayTraderApp.Shared.Models.DTO.PublicRest;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BitBayTraderApp.Server.Models.Hubs
{
    public class PublicRESTHub : Hub
    {
        public async Task SendTickerStatus(string marketCode, CurrentStatus status)
        {
            await Clients.All.SendAsync("ReceiveTickerStatus", marketCode, status);
        }
        public async Task SendLast24HStats(string marketCode, MarketStats stats)
        {
            await Clients.All.SendAsync("ReceiveLast24HStats", marketCode, stats);
        }
    }
}
