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
        readonly IGreetingService _greetingService;

        public ExecutorService(IAuthenticationService authService, IFileTransferService transferService, IGreetingService greetingService)
        {
            _authService = authService;
            _transferService = transferService;
            _greetingService = greetingService;
        }

        public async Task ExecuteExample01Async(CancellationToken cancellationToken)
        {
            Console.WriteLine("Enter service credentionals (any non-empty strings are allowed):");

            Console.Write("Enter username: ");
            var userName = Console.ReadLine();

            Console.Write("Enter password: ");
            var password = Console.ReadLine();

            var authToken = await _authService.AuthenticateAsync(userName, password, cancellationToken);

            var greetings = await _greetingService.GetGreetings(userName, null);
            Console.WriteLine(greetings);

            Console.WriteLine();

            var files = await _transferService.ListFilesAsync(authToken, cancellationToken);

            Console.WriteLine("List of server files:");
            Console.WriteLine($"{"Name",-42} {"Size",-12}");
            foreach (var file in files)
            {
                Console.WriteLine($"{file.FileName,-42} {file.FileSize,-12}");
            }

        }
    }
}
