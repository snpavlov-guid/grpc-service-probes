using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.IO;
using GrpcConsoleClient.Configuration;
using GrpcConsoleClient.Application;
using GrpcConsoleClient.HostServices;
using System.Threading.Tasks;

namespace GrpcConsoleClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var builder = new HostBuilder();

            builder.ConfigureLogging((context, b) =>
            {
                b.AddConsole();
            })
            .ConfigureHostConfiguration(config =>
            {
                config.SetBasePath(Directory.GetCurrentDirectory());
            })
            .ConfigureAppConfiguration((context, config) =>
            {
                config.AddJsonFile("appsettings.json", true, true);
                config.AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", true, true);
            });

            // Configure application services
            builder.ConfigureServices(services =>
            {
                // Add application services
                services.ConfigureApplicationServices();

                // Add app host service
                services.AddHostedService<AppHostService>();
            });

            // Build host
            using var host = builder.Build();

            // Run host
            await host.RunAsync();

            Console.WriteLine("Done");

        }
    }
}
