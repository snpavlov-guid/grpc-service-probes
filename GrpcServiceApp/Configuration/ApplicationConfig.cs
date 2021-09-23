using GrpcServiceApp.Application;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcAppService.Configuration
{
    public static class ApplicationConfig
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            // add http context accessor service
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            services.AddScoped<IUrlHelper>(factory =>
            {
                return new UrlHelper(factory.GetService<IActionContextAccessor>().ActionContext);
            });


            // Add application urls information service
            services.AddScoped<IApplicationUrlService, ApplicationUrlService>();

            // add Protobuf files service
            services.AddScoped<IProtosService, ProtosService>();

            // add repository service
            services.AddSingleton<IFileRepositoryService, FileRepositoryService>(serviceProvider => {
                var baseDir = configuration.GetValue<string>("FileRepository:BaseDir");

                if (string.IsNullOrEmpty(baseDir))
                {   const string folder = @"app-data\repo";
                    var env = serviceProvider.GetService<IWebHostEnvironment>();
                    baseDir = Path.Combine(env.WebRootPath, folder);
                }

                return new FileRepositoryService(baseDir);
            });

            return services;

        }
    }
}
