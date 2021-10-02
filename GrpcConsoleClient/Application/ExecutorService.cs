using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GrpcConsoleClient.Application
{
    public class ExecutorService : IExecutorService
    {
        readonly IAuthenticationService _authService;
        readonly IFileTransferService _transferService;

        public ExecutorService(IAuthenticationService authService, IFileTransferService transferService)
        {
            _authService = authService;
            _transferService = transferService;
        }

        public async Task ExecuteExample01Async(CancellationToken cancellationToken)
        {
            Console.WriteLine("Enter service credentionals");

            Console.Write("Enter username: ");
            var userName = Console.ReadLine();

            Console.Write("Enter password: ");
            var password = Console.ReadLine();

            var authToken = await _authService.AuthenticateAsync(userName, password, cancellationToken);

            var files = await _transferService.ListFilesAsync(authToken, cancellationToken);

            Console.WriteLine("List of server files:");
            Console.WriteLine($"{"Name".PadRight(42)} {"Size".PadRight(12)}");
            foreach (var file in files)
            {
                Console.WriteLine($"{file.FileName.PadRight(42)} {file.FileSize.ToString().PadRight(12)}");
            }

        }
    }
}
