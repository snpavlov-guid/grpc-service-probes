using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcServiceApp.Application;
using GrpcServiceApp.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace GrpcAppService.GrpcServices
{
    [Authorize]
    public class FileTransferService : FileTransfer.FileTransferBase
    {
        private readonly IFileRepositoryService _repoService;
        private readonly ILogger<FileTransferService> _logger;

        public const int MinFileSize = 64 * 1024;
        public const int MaxFileSize = 8192 * 1024;

        public FileTransferService(IFileRepositoryService repoService, ILogger<FileTransferService> logger)
        {
            _repoService = repoService;
            _logger = logger;
        }

        #region FileTransfer service implementation

        [AllowAnonymous]
        public override Task<PingReply> Ping(PingRequest request, ServerCallContext context)
        {
            var time = DateTimeOffset.Now;

            return Task.FromResult(new PingReply
            {
                Message = $"Message: {request.Message}, time:{time:G}" ,
                Timestamp = Timestamp.FromDateTimeOffset(time),
            });
        }


        public override Task<ListReply> List(Empty request, ServerCallContext context)
        {
            var reply = new ListReply();

            reply.Files.Add(_repoService.ListFiles());

            return Task.FromResult(reply);
        }

        public override Task<DeleteFileReply> DeleteFile(DeleteFileRequest request, ServerCallContext context)
        {
            var deleted = _repoService.DeleteFile(request.FileName);

            return Task.FromResult(new DeleteFileReply()
            {
                Status = deleted ? FileOperationStatus.Ok : FileOperationStatus.Failed
            });
        }

        public override async Task<FileUploadReply> UploadFile(IAsyncStreamReader<FileChunk> requestStream, ServerCallContext context)
        {
            var status = FileOperationStatus.None;
            var fileName = "";
            var length = 0L;
            Stream fs = null;

            var chunks = requestStream.ReadAllAsync(context.CancellationToken);

            await foreach (var chunk in chunks)
            {
                if (context.CancellationToken.IsCancellationRequested) {
                    status = FileOperationStatus.Canceled;
                    break;
                }

                if (fs == null)
                {
                    fileName = chunk.FileName;
                    fs = _repoService.CreateFile(chunk.FileName);
                }
                    

                //  Write chunk data into a file
                await fs.WriteAsync(chunk.ChunkData.ToByteArray(), context.CancellationToken);

                length += chunk.ChunkData.Length;

            }

            // close file
            fs.Close();

            status = FileOperationStatus.Ok;

            return new FileUploadReply()
            {
                Status = status,
                FileInfo = new FileInfo()
                {   FileName = fileName,
                    FileSize = length
                }
            };

        }

        public override async Task DownloadFile(DownloadFileRequest request, IServerStreamWriter<FileChunk> responseStream, ServerCallContext context)
        {
            using var fs = _repoService.OpenFile(request.FileName);

            var downloadSize = fs.Length;

            int bufferChunkSize = Helpers.GeChunkSize(downloadSize, MinFileSize, MaxFileSize);

            byte[] chunkBuffer = new byte[bufferChunkSize];

            var totalFileChunks = fs.Length > bufferChunkSize ?
                (int)Math.Ceiling((float)fs.Length / bufferChunkSize) : 1;

            int fileChunkCount = 0;

            while (fs.Position < fs.Length)
            {
                if (context.CancellationToken.IsCancellationRequested)
                {
                    context.CancellationToken.ThrowIfCancellationRequested();
                }

                var offset = (int)fs.Position;
                var remainingBytes = fs.Length - fs.Position;
                var readBytes = remainingBytes < bufferChunkSize ? remainingBytes : bufferChunkSize;

                var chunkBytes = await fs.ReadAsync(chunkBuffer.AsMemory(0, (int)readBytes), context.CancellationToken);

                var chunk = new FileChunk()
                {
                    FileName = request.FileName,
                    ChunkData = ByteString.CopyFrom(chunkBuffer, 0, chunkBytes),
                };

                await responseStream.WriteAsync(chunk);

                fileChunkCount++;

            }

        }

        #endregion

 
    }
}
