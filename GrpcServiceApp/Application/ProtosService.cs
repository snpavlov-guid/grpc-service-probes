using GrpcServiceApp.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcServiceApp.Application
{
    public class ProtosService : IProtosService
    {
        protected const string _protobufFolder = "Protobuf";

        protected readonly string _protobufPath;

        protected readonly IApplicationUrlService _urlService;

        protected readonly ProtoInfo[] _protobufs;

        public ProtosService(IConfiguration configuration, IWebHostEnvironment webEnvironment, IApplicationUrlService urlService)
        {
            var stubFolder = configuration.GetValue<string>("Settings:ProtobufPath");

            _protobufPath = !string.IsNullOrEmpty(stubFolder) ?
                Path.Combine(webEnvironment.ContentRootPath, stubFolder, _protobufFolder) :
                Path.Combine(webEnvironment.WebRootPath, _protobufFolder);

            _protobufs = configuration.GetSection("Settings:ProtobufList").Get<ProtoInfo[]>();

            _urlService = urlService;
        }

        public (string, string) GetProto(string version, string name)
        {
            var file = $"{name}.proto";

            var path = string.IsNullOrEmpty(version) ?
                Path.Combine(_protobufPath, file) :
                Path.Combine(_protobufPath, version, file);

            if (!File.Exists(path)) return (file, null);

            return (file, File.ReadAllText(path));
        }

        public ProtoInfo[] GetProtoList()
        {
            if (!Directory.Exists(_protobufPath)) return null;

            var protoDirs = new List<string>() { _protobufPath };

            protoDirs.AddRange(Directory.GetDirectories(_protobufPath, "v*"));


            var protoFiles = protoDirs.SelectMany(folder =>
            {
                return Directory.GetFiles(folder, "*.proto").Select(p => {
                    var name = Path.GetFileNameWithoutExtension(p);
                    var folder = new DirectoryInfo(Path.GetDirectoryName(p)).Name;
                    var version = folder.StartsWith("v", StringComparison.OrdinalIgnoreCase) ? folder.ToLower() : null;
                    return new
                    {
                        Name = name,
                        Url = _urlService.GetProtobuftUrl(name, version),
                        Version = version,
                    };
                });
            });

            var files = protoFiles.ToArray();



            return (from f in protoFiles
                    join s in _protobufs on new { n = f.Name, v = f.Version??"" } equals new { n = s.Name, v = s.Version??"" }
                    select new ProtoInfo() 
                    { Name = s.Name,
                      Version = s.Version,
                      Url = f.Url,
                      Description = s.Description,
                    })
                    .ToArray();
        }


    }
}
