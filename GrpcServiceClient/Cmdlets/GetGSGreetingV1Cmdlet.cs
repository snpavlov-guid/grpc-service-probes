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
    [Cmdlet(VerbsCommon.Get, "GSGreetingV1")]
    [OutputType(typeof(string))]
    [CmdletBinding()]
    public class GetGSGreetingV1Cmdlet : GSCmdletBase
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
        public string Name { get; set; }

        [Parameter(
            Position = 2,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public string Title { get; set; }

        protected override void ProcessRecord()
        {
            base.ProcessRecord();

            using var channel = GrpcChannel.ForAddress(ServiceUrl);

            var client = new GreeterV1.GreeterV1Client(channel);

            var reply = client.SayHello(new HelloRequest { Name = Name??"", Title = Title??"" });

            WriteObject(reply?.Message);

        }
    }
}
