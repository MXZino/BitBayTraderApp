using BitBayTraderApp.Server.Interfaces;
using BitBayTraderApp.Server.Models.Hubs;
using BitBayTraderApp.Server.Services;
using BitBayTraderApp.Shared.Models.DTO.PublicRest;
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
    public class PublicAPIController : ControllerBase
    {
        private readonly IHubContext<PublicAPIHub> publicAPIHubContext;
        private readonly IPublicAPIService publicAPIService;
        private readonly AlgorithmTesting testing;

        public PublicAPIController(IPublicAPIService publicAPIService, IHubContext<PublicAPIHub> publicAPIHubContext, AlgorithmTesting testing)
        {
            this.publicAPIService = publicAPIService;
            this.publicAPIHubContext = publicAPIHubContext;
            this.testing = testing;
        }

        [HttpGet("ticker/{marketCode}")]
        public async Task<IActionResult> GetTicker(string marketCode) => Ok(await publicAPIService.GetItems<CurrentStatus>(marketCode));

        [HttpGet("stats24h/{marketCode}")]
        public async Task<IActionResult> GetLast24hStats(string marketCode) => Ok(await publicAPIService.GetItems<MarketStats>(marketCode));

        [HttpGet("orderbook/{marketCode}")]
        public async Task<IActionResult> GetOrderBook(string marketCode) => Ok(await publicAPIService.GetItems<Orderbook>(marketCode));

        [HttpGet("test")]
        public async Task<IActionResult> Test()
        {
            await testing.Start();
            return Ok();
        }
    }
}
