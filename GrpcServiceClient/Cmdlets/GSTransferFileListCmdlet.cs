using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using Grpc.Net.Client;
using GrpcAppService.GrpcServices;
using Google.Protobuf.WellKnownTypes;
using GrpcServiceClient.Common;

namespace GrpcServiceClient.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, "GSTransferFileList")]
    [OutputType(typeof(IList<FileInfo>))]
    [CmdletBinding()]
    public class GSTransferFileListCmdlet : GSCmdletBase
    {
        [Parameter(
            Mandatory = true,
            Position = 0,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public string ServiceUrl { get; set; }

        [Parameter(
          Position = 1,
          ValueFromPipeline = true,
          ValueFromPipelineByPropertyName = true)]
        public string Token { get; set; }

        protected override void ProcessRecord()
        {
            base.ProcessRecord();

            using var channel = GrpcHelpers.CreateAuthenticatedChannel(ServiceUrl, Token);

            var client = new FileTransfer.FileTransferClient(channel);

            var reply = client.List(new Empty(), null, null, _cancellationTokenSource.Token);

            WriteObject(reply?.Files);

        }

    }
}
