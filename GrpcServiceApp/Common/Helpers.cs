using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcServiceApp.Common
{
    public static class Helpers
    {
        #region Common

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

        #endregion


        #region Formatting

        public static string FormatFileSize(long byteCount, int decimals = 2)
        {
            string[] suffixes = { "B", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
            if (byteCount == 0) return "0 " + suffixes[0];

            var bytes = Math.Abs(byteCount);
            var pos = (int)Math.Log(bytes, 1024);
            var num = Math.Round(bytes / Math.Pow(1024, pos), decimals);
            return String.Format("{0} {1}", Math.Sign(byteCount) * num, suffixes[pos]);

        }

        #endregion

    }
}
