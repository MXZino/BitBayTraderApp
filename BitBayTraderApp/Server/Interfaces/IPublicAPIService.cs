using BitBayTraderApp.Shared.Models.DTO.PublicRest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BitBayTraderApp.Server.Interfaces
{
    public interface IPublicAPIService
    {
        Task<T> GetItems<T>(string marketCode);
    }
}
