﻿using Grpc.Net.Client;
using GrpcAppService.GrpcServices;
using GrpcServiceClient.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace GrpcServiceClient.Cmdlets
{
    [Cmdlet(VerbsCommon.Remove, "GSTransferFile")]
    [OutputType(typeof(DeleteFileReply))]
    [CmdletBinding()]
    public class GSTransferRemoveCmdlet : GSCmdletBase
    {
        [Parameter(
         Mandatory = true,
         Position = 0,
         ValueFromPipeline = true,
         ValueFromPipelineByPropertyName = true)]
        public string ServiceUrl { get; set; }

        [Parameter(
             Mandatory = true,
             Position = 1,
             ValueFromPipeline = true,
             ValueFromPipelineByPropertyName = true)]
        public string FileName { get; set; }

        [Parameter(
          Position = 2,
          ValueFromPipeline = true,
          ValueFromPipelineByPropertyName = true)]
        public string Token { get; set; }

        protected override void ProcessRecord()
        {
            base.ProcessRecord();

            using var channel = GrpcHelpers.CreateAuthenticatedChannel(ServiceUrl, Token);

            var client = new FileTransfer.FileTransferClient(channel);

            var reply = client.DeleteFile(new DeleteFileRequest() { FileName = FileName },
                null, null, _cancellationTokenSource.Token);

            WriteObject(reply);
        }

    }
}
