using BitBayTraderApp.Server.Interfaces;
using BitBayTraderApp.Server.Models.Hubs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BitBayTraderApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublicRESTController : ControllerBase
    {
        private readonly IHubContext<TickerStatusHub> tickerStatusHubContext;
        private readonly IPublicRESTService publicRestService;

        public PublicRESTController(IPublicRESTService publicRestService , IHubContext<TickerStatusHub> tickerStatusHubContext)
        {
            this.publicRestService = publicRestService;
            this.tickerStatusHubContext = tickerStatusHubContext;
        }

        [HttpGet("ticker/{marketCode}")]
        public async Task <IActionResult> GetTicker (string marketCode)
        {
            while (true)
            {
                var ticker = await publicRestService.GetTicker(marketCode);
                await tickerStatusHubContext.Clients.All.SendAsync("ReceiveStatus", marketCode, ticker);
            }
            return Ok();
        }
    }
}
