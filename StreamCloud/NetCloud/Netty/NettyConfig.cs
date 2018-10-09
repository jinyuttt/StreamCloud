using System;
using System.Collections.Generic;
using System.Text;

namespace  NettySocket
{
  public  class NettyConfig
    {
        public static bool IsSsl { get; set; } = false;
        public static string CertificatePath { get; set; }

        public static string Host { get; set; } = "127.0.0.1";

        public static int Port { get; set; } = 6666;
        public static int Size { get; set; } = 65535;

        public static Encoding NettyEncod { get; set; } = Encoding.UTF8;

        public static int HeartTime { get; set; } = 1000;

        public static string HeartMessage { get; set; } = "heartmsg";
    }
}
