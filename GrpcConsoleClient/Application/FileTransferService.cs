using Grpc.Net.Client;
using GrpcAppService.GrpcServices;
using GrpcConsoleClient.Common;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GrpcConsoleClient.Application
{
    public class FileTransferService : IFileTransferService
    {
        readonly string _serviceUrl;

        public FileTransferService(IConfiguration configuration)
        {
            _serviceUrl = configuration.GetValue<string>("GrpcServices:FileTransferService:ServiceUrl");
        }

        public async Task<IList<FileInfo>> ListFilesAsync(string authToken, CancellationToken cancellationToken)
        {
            using var channel = GrpcHelpers.CreateAuthenticatedChannel(_serviceUrl, authToken);

            var client = new FileTransfer.FileTransferClient(channel);

            var reply = await client.ListAsync(new Google.Protobuf.WellKnownTypes.Empty(), 
                null, null, cancellationToken);

            return reply.Files;
        }
    }
}
