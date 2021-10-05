using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using GrpcAppService.GrpcServices;
using GrpcServiceClient.Common;

namespace GrpcServiceClient.Cmdlets
{
    [Cmdlet(VerbsDiagnostic.Ping, "GSTransfer")]
    [OutputType(typeof(string))]
    [CmdletBinding()]
    public class GSTransferPingCmdlet : GSCmdletBase
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
        public string Message { get; set; }

        [Parameter(
             Position = 2,
             ValueFromPipeline = true,
             ValueFromPipelineByPropertyName = true)]
        public string Token { get; set; }

        protected override void ProcessRecord()
        {
            base.ProcessRecord();

            using var channel = GrpcHelpers.CreateAuthenticatedChannel(ServiceUrl, Token);

            var client = new FileTransfer.FileTransferClient(channel);

            var reply = client.Ping(new PingRequest { Message = Message });

            WriteObject(reply?.Message);

        }

    }
}
