using Google.Protobuf;
using Grpc.Net.Client;
using GrpcAppService.GrpcServices;
using GrpcServiceClient.Common;
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
    [Cmdlet(VerbsCommon.Add, "GSTransferFile")]
    [OutputType(typeof(GrpcAppService.GrpcServices.FileInfo))]
    [CmdletBinding()]
    public class GSTransferUploadCmdlet : GSCmdletBase
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
        public string FilePath { get; set; }


        protected override void ProcessRecord()
        {
            base.ProcessRecord();

            var reply = UploadFileAsync(FilePath, _cancellationTokenSource.Token).GetAwaiter().GetResult();

            WriteObject(reply);

        }

        #region Implementation

        protected async Task<GrpcAppService.GrpcServices.FileInfo> UploadFileAsync(string filePath, CancellationToken cancellationToken)
        {
            if (!File.Exists(FilePath)) throw new FileNotFoundException(filePath);

            var fileName = Path.GetFileName(filePath);

            // open the file to read it into chunks  
            using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);

            int bufferChunkSize = Helpers.GeChunkSize(fs.Length, MinFileSize, MaxFileSize);

            byte[] chunkBuffer = new byte[bufferChunkSize];


            // Create client
            using var channel = GrpcChannel.ForAddress(ServiceUrl);

            var client = new FileTransfer.FileTransferClient(channel);


            using var streamingCall = client.UploadFile(null, null, cancellationToken);

            // calculate the number of files that will be created  
            var totalFileChunks = fs.Length > bufferChunkSize ?
                (int)Math.Ceiling((float)fs.Length / bufferChunkSize) : 1;

            int fileChunkCount = 0;

            while (fs.Position < fs.Length)
            {
                if (cancellationToken.IsCancellationRequested) break;

                var offset = (int)fs.Position;
                var remainingBytes = fs.Length - fs.Position;
                var readBytes = remainingBytes < bufferChunkSize ? remainingBytes : bufferChunkSize;

                var chunkBytes = await fs.ReadAsync(chunkBuffer.AsMemory(0, (int)readBytes), cancellationToken);

                var chunk = new FileChunk()
                {
                    FileName = fileName,
                    ChunkData = ByteString.CopyFrom(chunkBuffer, 0, chunkBytes),
                };

                await streamingCall.RequestStream.WriteAsync(chunk);

                // file written, loop for next chunk  
                fileChunkCount++;
            }

            // Complete trasnsmission
            await streamingCall.RequestStream.CompleteAsync();

            return (await streamingCall.ResponseAsync).FileInfo;

        }

        #endregion

    }
}
