using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BitBayTraderApp.Server.Services
{
    public class DownloadDataHostedService : BackgroundService
    {
        public IServiceProvider services { get; }

        public DownloadDataHostedService(IServiceProvider services)
        {
            this.services = services;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var scope = services.CreateScope())
            {
                var downloadDataService =
                    scope.ServiceProvider
                        .GetRequiredService<DownloadDataService>();

                await downloadDataService.GetTicker(stoppingToken);
            }
        }
    }
}
