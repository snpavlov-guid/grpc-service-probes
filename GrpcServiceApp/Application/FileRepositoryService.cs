using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcServiceApp.Application
{
    public class FileRepositoryService : IFileRepositoryService
    {
        protected readonly string _basePath;

        public FileRepositoryService(string basePath)
        {
            _basePath = basePath;

            if (!Directory.Exists(_basePath))
                Directory.CreateDirectory(_basePath);
        }

        public Stream CreateFile(string fileName)
        {
            var filePath = Path.Combine(_basePath, fileName);

            return File.Create(filePath);
        }

 
        public Stream OpenFile(string fileName)
        {
            var filePath = Path.Combine(_basePath, fileName);

            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Not found {filePath}");

            return File.OpenRead(filePath);
        }

        public GrpcAppService.GrpcServices.FileInfo[] ListFiles()
        {
            var baseDirInfo = new DirectoryInfo(_basePath);

            return baseDirInfo.GetFiles()
                .Select(p => new GrpcAppService.GrpcServices.FileInfo()
                {
                    FileName = p.Name,
                    FileSize = p.Length,

                }).ToArray();
    
        }

        public bool DeleteFile(string fileName)
        {
            var filePath = Path.Combine(_basePath, fileName);

            if (!File.Exists(filePath)) return false;

            File.Delete(filePath);

            return true;

        }
    }
}
