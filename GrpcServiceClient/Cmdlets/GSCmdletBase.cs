using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GrpcServiceClient.Cmdlets
{
    public class GSCmdletBase : Cmdlet
    {
        public const int MinFileSize = 64 * 1024;
        public const int MaxFileSize = 8192 * 1024;

        protected CancellationTokenSource _cancellationTokenSource;

        protected override void BeginProcessing()
        {
            base.BeginProcessing();

            _cancellationTokenSource = new CancellationTokenSource();

        }

        protected override void ProcessRecord()
        {
            base.ProcessRecord();

        }

        protected override void EndProcessing()
        {
            base.EndProcessing();

            _cancellationTokenSource?.Cancel();
        }

    }
}
