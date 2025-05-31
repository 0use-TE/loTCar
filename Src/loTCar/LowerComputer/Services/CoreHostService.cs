using Microsoft.Extensions.Hosting;
using System;
using System.Threading;

namespace LowerComputer.Services
{
    class CoreHostService : BackgroundService
    {
        protected override void ExecuteAsync(CancellationToken stoppingToken)
        {
        }
    }
}
