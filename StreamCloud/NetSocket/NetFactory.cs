using System;
using System.Collections.Generic;
using System.Text;

namespace NetSocket
{
   public static class NetFactory
    {
        static TransferDLL transferDLL = new TransferDLL();
        public static ISocketClient CreateSocketClient(string name)
        {
            Type clientCls= transferDLL.GetClass(name + "_client");
            if (clientCls == null)
            {
                return null;
            }
            ISocketClient client = Activator.CreateInstance(clientCls) as ISocketClient;
            return client;
        }
        public static ISocketServer CreateSocketServer(string name)
        {
            Type clientCls = transferDLL.GetClass(name + "_server");
            if (clientCls == null)
            {
                return null;
            }
            ISocketServer server = Activator.CreateInstance(clientCls) as ISocketServer;
            return server;
        }

    }
}
