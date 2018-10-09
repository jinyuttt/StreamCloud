using NettySocket;
using StreamCloud.transfer;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCloud.Netty
{
    public class SocketServer : ISocketServer
    {
        NettyServer nettyServer = new NettyServer();
        public void CallData(RecviceNetData recviceNetData)
        {
            nettyServer.CallData(recviceNetData);
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public NetSocket GetNetSocket()
        {
            throw new NotImplementedException();
        }

        public int SendData(byte[] data)
        {
            throw new NotImplementedException();
        }

        public bool StartRun(int port)
        {
            throw new NotImplementedException();
        }

        public bool StartRun(string ip, int port)
        {
            throw new NotImplementedException();
        }

        public bool StartRun()
        {
            throw new NotImplementedException();
        }
    }
}
