using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using Grpc.Net.Client;
using GrpcAppService.GrpcServices;
using Google.Protobuf.WellKnownTypes;

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

        protected override void ProcessRecord()
        {
            base.ProcessRecord();

            using var channel = GrpcChannel.ForAddress(ServiceUrl);

            var client = new FileTransfer.FileTransferClient(channel);

            var reply = client.List(new Empty());

            WriteObject(reply?.Files);

        }

    }
}
