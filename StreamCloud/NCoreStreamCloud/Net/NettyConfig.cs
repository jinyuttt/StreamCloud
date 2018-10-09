using System.Text;

namespace NStStreamCloud.Net
{
    /// <summary>
    /// 传输组件自有配置
    /// </summary>
    internal class NettyConfig
    {
        public static bool IsSsl { get; set; } = false;
        public static string CertificatePath { get; set; }

        public static string Host { get; set; } = "192.168.3.105";

        public static int Port { get; set; } = 7777;
        public static int Size { get; set; } = 65535;

        public static Encoding NettyEncod { get; set; } = Encoding.UTF8;

        public static int HeartTime { get; set; } = 1000;

        public static string HeartMessage { get; set; } = "heartmsg";

        public static int BufSize { get; set; } = 131072;
    }
}