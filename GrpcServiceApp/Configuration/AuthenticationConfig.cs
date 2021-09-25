using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using GrpcServiceApp.Models;
using Microsoft.IdentityModel.Tokens;
using GrpcServiceApp.Common;

namespace GrpcServiceApp.Configuration
{
    /// <summary>
    /// Configure JwtBearer authentication
    /// </summary>
    public static class JwtBearerAuthenticationConfig
    {
        public static IServiceCollection AddJwtBearerAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            //var schema = JwtBearerDefaults.AuthenticationScheme;

            // Add JwtBearer users identity
            services.AddAuthentication()
                .AddJwtBearer(options => ConfigureJwtBearer(configuration, options));

            return services;
        }

        public static void ConfigureJwtBearer(IConfiguration configuration, JwtBearerOptions options)
        {
            var jwtSettings = configuration.GetSection(Defaults.JwtBearerEntry).Get<JwtBearerSettings>();

            options.RequireHttpsMetadata = false;
            options.SaveToken = true;

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtSettings.JwtIssuer,

                ValidateAudience = true,
                ValidAudience = jwtSettings.JwtAudience,

                ValidateLifetime = true,

                IssuerSigningKey = Helpers.GetSecurityKeyFromBase64String(jwtSettings.JwtKey),
                ValidateIssuerSigningKey = true,

                ClockSkew = TimeSpan.FromSeconds(15),
            };

            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var accessToken = context.Request.Query["access_token"];

                    return Task.CompletedTask;
                },

            };

        }


    }

}
