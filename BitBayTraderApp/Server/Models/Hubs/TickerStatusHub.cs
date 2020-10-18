using BitBayTraderApp.Shared.Models.DTO.PublicRest;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BitBayTraderApp.Server.Models.Hubs
{
    public class TickerStatusHub : Hub
    {
        public async Task SendTickerStatus(string marketCode, CurrentStatus status)
        {
            await Clients.All.SendAsync("ReceiveStatus", marketCode, status);
        }
    }
}
