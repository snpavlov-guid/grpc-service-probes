using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcServiceApp.Application
{
    public interface IAuthenticationService
    {
        string Authenticaticate(string username, string password);
    }
}
