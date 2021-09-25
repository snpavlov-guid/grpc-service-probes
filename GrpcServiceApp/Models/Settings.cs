using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcServiceApp.Models
{
    public class ProtoInfo
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Version { get;  set; }
        public string Description { get; set; }
    }

    public class JwtBearerSettings
    {
        public string JwtKey { get; set; }
        public string JwtIssuer { get; set; }
        public string JwtAudience { get; set; }
        public int JwtExpireMinutes { get; set; }
    }
}
