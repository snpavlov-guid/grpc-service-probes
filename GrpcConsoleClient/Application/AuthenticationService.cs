using Grpc.Net.Client;
using GrpcAppService.GrpcServices;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GrpcConsoleClient.Application
{
    public class AuthenticationService : IAuthenticationService
    {
        readonly string _serviceUrl;

        public AuthenticationService(IConfiguration configuration)
        {
            _serviceUrl = configuration.GetValue<string>("GrpcServices:AuthenticationService:ServiceUrl");
        }

        public async Task<string> AuthenticateAsync(string username, string password, CancellationToken cancellationToken)
        {
            using var channel = GrpcChannel.ForAddress(_serviceUrl);

            var client = new Authentication.AuthenticationClient(channel);

            var request = new AuthenticateRequest()
            {   Username = username,
                Password = password
            };

            var reply = await client.AuthenticateAsync(request, null, null, cancellationToken);

            return reply.Token;
        }
    }
}
