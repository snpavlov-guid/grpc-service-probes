using GrpcServiceApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcServiceApp.Application
{
    public interface IProtosService
    {
        (string, string) GetProto(string version, string name);
        ProtoInfo[] GetProtoList();
    }
}
