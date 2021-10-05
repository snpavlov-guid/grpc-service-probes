﻿using Grpc.Net.Client;
using GrpcAppService.GrpcServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace GrpcServiceClient.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, "GSAuthToken")]
    [OutputType(typeof(string))]
    [CmdletBinding()]
    public class GSAuthenticationCmdlet : GSCmdletBase
    {
        [Parameter(
            Mandatory = true,
            Position = 0,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public string ServiceUrl { get; set; }

        [Parameter(
             Mandatory = true,
             Position = 1,
             ValueFromPipeline = true,
             ValueFromPipelineByPropertyName = true)]
        public string Username { get; set; }

        [Parameter(
             Mandatory = true,
             Position = 2,
             ValueFromPipeline = true,
             ValueFromPipelineByPropertyName = true)]
        public string Password { get; set; }

        protected override void ProcessRecord()
        {
            base.ProcessRecord();

            using var channel = GrpcChannel.ForAddress(ServiceUrl);

            var client = new Authentication.AuthenticationClient(channel);

            var request = new AuthenticateRequest()
            {
                Username = Username,
                Password = Password
            };

            var reply = client.Authenticate(request, null, null, _cancellationTokenSource.Token);

            WriteObject(reply?.Token);

        }

    }
}
