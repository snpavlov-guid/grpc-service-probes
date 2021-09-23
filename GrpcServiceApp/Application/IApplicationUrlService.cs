using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcServiceApp.Application
{
    public interface IApplicationUrlService
    {
        string ApplicationUrl { get; }

        string GetProtobuftUrl(string name, string version);
    }
}
