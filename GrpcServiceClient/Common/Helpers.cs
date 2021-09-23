using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrpcServiceClient.Common
{
    public static class Helpers
    {
        public static int Clamp(int value, int min, int max) 
        {
            return (value < min) ? min : (value > max) ? max : value;
        }

        public static int GeChunkSize(long length, int min, int max)
        {
            var estChunks = Math.Floor(Math.Log10(length));

            var chunkSize = (int)Math.Ceiling(length / estChunks);

            return Clamp(chunkSize, min, max);
        }

 
    }
}
