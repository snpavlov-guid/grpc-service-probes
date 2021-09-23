using Grpc.Core;
using Grpc.Net.Client;
using GrpcAppService.GrpcServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GrpcServiceClient.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, "GSTransferFile")]
    [OutputType(typeof(string))]
    [CmdletBinding()]
    public class GSTransferDownloadCmdlet : GSCmdletBase
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
        public string FileName { get; set; }

        [Parameter(
            Mandatory = true,
            Position = 2,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public string OutputDir { get; set; }

        protected override void ProcessRecord()
        {
            base.ProcessRecord();

            var outputFilePath = DownloadFileAsync(FileName, OutputDir, _cancellationTokenSource.Token).GetAwaiter().GetResult();

            WriteObject(outputFilePath);

        }

        #region Implementation

        protected async Task<string> DownloadFileAsync(string fileName, string outputDir, CancellationToken cancellationToken)
        {
            var filePath = Path.Combine(outputDir, fileName);

            // Create client
            using var channel = GrpcChannel.ForAddress(ServiceUrl);

            var client = new FileTransfer.FileTransferClient(channel);

            var request = new DownloadFileRequest() { FileName = FileName};

            using var streamingCall = client.DownloadFile(request, null, null, cancellationToken);

            using var fs = File.Create(filePath);

            await foreach (var chunkData in streamingCall.ResponseStream.ReadAllAsync(cancellationToken))
            {
                if (cancellationToken.IsCancellationRequested)
                    throw new TaskCanceledException();

                await fs.WriteAsync(chunkData.ChunkData.ToByteArray(), cancellationToken);
            }

            fs.Close();

            return filePath;
        }

        #endregion

    }
}
