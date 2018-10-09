using System;
using System.Collections.Generic;
using System.Text;

namespace NetSocket
{
    public delegate void RecviceNetData(NetChannel netSocket);
   public class NetChannel
    {
        public string localIP;
        public int localPort;
        public string remoteIP;
        public int remotePort;
        public byte[] recData;
        public Object channel;
    }
}
