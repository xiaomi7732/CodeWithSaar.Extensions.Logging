using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace NetCore31Console
{
    internal class WorkerService : BackgroundService
    {
        private readonly ILogger _logger;

        public WorkerService(ILogger<WorkerService> logger)
        {
            _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Logging info!");
                await Task.Delay(TimeSpan.FromSeconds(5));
            }
        }
    }
}