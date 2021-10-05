using GrpcConsoleClient.Application;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrpcConsoleClient.Configuration
{
    public static class ApplicationServiceConfig
    {
        public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services)
        {
            // Add examples' executor service
            services.AddSingleton<IExecutorService, ExecutorService>();

            // Add greeting service
            services.AddTransient<IGreetingService, GreetingService>();

            // Add authentication service
            services.AddTransient<IAuthenticationService, AuthenticationService>();

            // Add file transfer service
            services.AddTransient<IFileTransferService, FileTransferService>();

            return services;
        }
    }
}
