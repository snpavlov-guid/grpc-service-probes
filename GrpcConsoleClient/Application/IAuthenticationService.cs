using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GrpcConsoleClient.Application
{
    public interface IAuthenticationService
    {
        Task<string> AuthenticateAsync(string username, string password, CancellationToken cancellationToken);
    }
}
