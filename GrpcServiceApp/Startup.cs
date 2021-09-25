using GrpcAppService.Configuration;
using GrpcAppService.GrpcServices;
using GrpcServiceApp.Common;
using GrpcServiceApp.Configuration;
using GrpcServiceApp.GrpcServices.V1;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcServiceApp
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // Configure JwtBearer authentication
            services.AddJwtBearerAuthentication(Configuration);

            // add application services
            services.AddApplicationServices(Configuration);

            services.AddGrpc(options => {
                // TODO: configure grpc options
                options.EnableDetailedErrors = true;
                options.MaxReceiveMessageSize = GrpcDefaults.MaxReceiveMessageSize;
                options.MaxSendMessageSize = GrpcDefaults.MaxSendMessageSize;
            });

            // add mvc
            services.AddControllersWithViews(options =>
            { 

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<GreeterService>();

                endpoints.MapGrpcService<GreeterServiceV1>();

                endpoints.MapGrpcService<FileTransferService>();

                endpoints.MapControllerRoute(
                     name: "default",
                     pattern: "{controller=Home}/{action=Index}/{id?}");

            });
        }
    }
}
