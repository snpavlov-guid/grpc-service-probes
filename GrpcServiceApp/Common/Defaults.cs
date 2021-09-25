using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcServiceApp.Common
{
    public static class Defaults
    {
        public const string JwtBearerEntry = "AppJwtBearer";
    }

    public static class GrpcDefaults
    {
        public const int MaxReceiveMessageSize = 10240 * 1024;
        public const int MaxSendMessageSize = 10240 * 1024;

    }
}
