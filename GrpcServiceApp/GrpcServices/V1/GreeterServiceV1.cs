using Grpc.Core;
using GrpcAppService.GrpcServices.V1;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcServiceApp.GrpcServices.V1
{
    public class GreeterServiceV1 : GreeterV1.GreeterV1Base
    {
        private readonly ILogger<GreeterServiceV1> _logger;
        public GreeterServiceV1(ILogger<GreeterServiceV1> logger)
        {
            _logger = logger;
        }

        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            var person = string.Join(" ", new[] { request.Title, request.Name }.Where(p => !string.IsNullOrEmpty(p)));

            return Task.FromResult(new HelloReply
            {
                Message = $"Hello my friend {person}"
            });

        }

        public override Task<HelloReply> SayBonjour(HelloRequest request, ServerCallContext context)
        {
            var person = string.Join(" ", new[] { request.Title, request.Name }.Where(p => !string.IsNullOrEmpty(p)));

            return Task.FromResult(new HelloReply
            {
                Message = $"Bonjour mon cher {person}"
            });
        }

        public override Task<EchoReply> Echo(EchoRequest request, ServerCallContext context)
        {
            return Task.FromResult(new EchoReply
            {
                Message = !request.Reverse??false ? request.Message : new string(request.Message.Reverse().ToArray())
            });
        }
    }
}
