using GrpcAppService.GrpcServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GrpcConsoleClient.Application
{
    public interface IFileTransferService
    {
        Task<IList<FileInfo>> ListFilesAsync(string authToken, CancellationToken cancellationToken);
    }
}
