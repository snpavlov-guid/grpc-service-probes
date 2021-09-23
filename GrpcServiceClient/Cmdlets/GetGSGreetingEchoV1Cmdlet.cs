using Grpc.Net.Client;
using GrpcAppService.GrpcServices.V1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace GrpcServiceClient.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, "GSGreetingEchoV1")]
    [OutputType(typeof(string))]
    [CmdletBinding()]
    public class GetGSGreetingEchoV1Cmdlet : GSCmdletBase
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
        public SwitchParameter Reverse{ get; set; }

        protected override void ProcessRecord()
        {
            base.ProcessRecord();

            using var channel = GrpcChannel.ForAddress(ServiceUrl);

            var client = new Greeter.GreeterClient(channel);

            var reply = client.Echo(new EchoRequest { Message = Message, Reverse = Reverse.IsPresent });

            WriteObject(reply?.Message);

        }

    }
}
