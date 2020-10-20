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
        private readonly IHubContext<PublicRESTHub> publicRESTHubContext;
        private readonly IPublicRESTService publicRestService;

        public PublicRESTController(IPublicRESTService publicRestService, IHubContext<PublicRESTHub> publicRESTHubContext)
        {
            this.publicRestService = publicRestService;
            this.publicRESTHubContext = publicRESTHubContext;
        }

        [HttpGet("ticker/{marketCode}")]
        public async Task<IActionResult> GetTicker(string marketCode)
        {
            var ticker = await publicRestService.GetTicker(marketCode);
            await publicRESTHubContext.Clients.All.SendAsync("ReceiveTickerStatus", marketCode, ticker);
            return Ok(ticker);
        }

        [HttpGet("stats24h/{marketCode}")]
        public async Task<IActionResult> GetLast24hStats(string marketCode)
        {
            var stats = await publicRestService.GetMarketStats(marketCode);
            await publicRESTHubContext.Clients.All.SendAsync("ReceiveLast24HStats", marketCode, stats);
            return Ok(stats);
        }
    }
}
