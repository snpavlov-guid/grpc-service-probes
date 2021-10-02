using GrpcConsoleClient.Application;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace GrpcConsoleClient.HostServices
{
    public class AppHostService : IHostedService
    {
        readonly IExecutorService _executorService; 

        public AppHostService(IServiceProvider serviceProvider)
        {
            _executorService = serviceProvider.GetService<IExecutorService>();
        }


        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _executorService.ExecuteExample01Async(cancellationToken);

        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
