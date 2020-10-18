using BitBayTraderApp.Server.Interfaces;
using BitBayTraderApp.Server.Models;
using BitBayTraderApp.Shared.Models.DTO.PublicRest;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace BitBayTraderApp.Server.Services
{
    public class PublicRESTService : IPublicRESTService
    {
        private readonly HttpClient httpClient;
        public PublicRESTService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<CurrentStatus> GetTicker(string marketCode)
        {
            var response = await httpClient.GetAsync($"https://api.bitbay.net/rest/trading/ticker/{marketCode}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                try
                {
                    var opts = SerializationOption.Add(new Serialization[] { Serialization.DoubleToString, Serialization.TimeStampToString });
                    opts.Converters.Add(new DoubleToStringConverter());
                    opts.Converters.Add(new TimeStampToStringConverter());
                    return JsonSerializer.Deserialize<CurrentStatus>(json, opts);
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
