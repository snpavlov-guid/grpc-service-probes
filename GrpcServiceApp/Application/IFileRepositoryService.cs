using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcServiceApp.Application
{
    public interface IFileRepositoryService
    {
        Stream CreateFile(string fileName);
        Stream OpenFile(string fileName);

        bool DeleteFile(string fileName);

        GrpcAppService.GrpcServices.FileInfo[] ListFiles();
    }
}
