using Grpc.Net.Client;
using GrpcAppService.GrpcServices.V1;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrpcConsoleClient.Application
{
    public class GreetingService : IGreetingService
    {
        readonly string _serviceUrl;

        public GreetingService(IConfiguration configuration)
        {
            _serviceUrl = configuration.GetValue<string>("GrpcServices:GreetingService:ServiceUrl");
        }

        public async Task<string> GetGreetings(string name, string title)
        {
            using var channel = GrpcChannel.ForAddress(_serviceUrl);

            var client = new GreeterV1.GreeterV1Client(channel);

            var reply = await client.SayHelloAsync(new HelloRequest()
            {
                Name = name??"",
                Title = title??"",
            });

            return reply?.Message;
        }
    }
}
