using NetSocket.Netty;
using NetSocket;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCloud.Netty
{
    [NetProtocol("netty_server")]
    public class SocketNettyServer : ISocketServer
    {
        NettyServer nettyServer = new NettyServer();
        public void CallData(RecviceNetData recviceNetData)
        {
            nettyServer.CallData(recviceNetData);
        }

        public void Close()
        {
            nettyServer.Close();
        }

        public NetChannel GetNetSocket()
        {
           return nettyServer.GetNetSocket();
        }

        public int SendData(NetChannel channel, byte[] data)
        {
            //
            nettyServer.SendData(channel, data);
            return 0;
        }

        public bool StartRun(int port)
        {
            NettyConfig.Port = port;
            return StartRun();
        }

        public bool StartRun(string ip, int port)
        {
            NettyConfig.Host = ip;
            NettyConfig.Port = port;
            return StartRun();
        }

        public bool StartRun()
        {
            nettyServer.RunServerAsync();
            return true;
        }

        NetSocket.NetChannel ISocketServer.GetNetSocket()
        {
           return nettyServer.GetNetSocket();
        }
    }
}
