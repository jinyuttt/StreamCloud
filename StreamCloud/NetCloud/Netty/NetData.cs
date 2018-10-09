using DotNetty.Transport.Channels;
using System;
using System.Collections.Generic;
using System.Text;

namespace  NettySocket
{
   public class NetSocket
    {
        public string LocalHost;
        public int LocalPort;
        public string RemoteHost;
        public int RemotePort;
        public byte[] data = null;
        public IChannel channel = null;
    }
}
