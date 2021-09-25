using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcServiceApp.Application;
using GrpcServiceApp.Common;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GrpcAppService.GrpcServices;

namespace GrpcServiceApp.GrpcServices
{
    public class AuthenticationService : Authentication.AuthenticationBase
    {
        private readonly IAuthenticationService _authService;
        private readonly ILogger<AuthenticationService> _logger;

        public AuthenticationService(IAuthenticationService authService, ILogger<AuthenticationService> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        public override Task<AuthenticateReply> Authenticate(AuthenticateRequest request, ServerCallContext context)
        {
            var token = _authService.Authenticaticate(request.Username, request.Password);

            return Task.FromResult(new AuthenticateReply
            {
               Token = token,
               Timestamp = Timestamp.FromDateTimeOffset(DateTimeOffset.Now),
            });
        }
    }
}
