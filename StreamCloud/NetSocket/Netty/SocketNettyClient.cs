using System;
using System.Collections.Generic;
using System.Text;

namespace NetSocket.Netty
{
    [NetProtocol("netty_client")]
    public class SocketNettyClient : ISocketClient
    {
        NettyClient nettyClient = new NettyClient();
        public void CallData(RecviceNetData recviceNetData)
        {
            nettyClient.CallData(recviceNetData);
        }

        public void Close()
        {
            nettyClient.Close();
        }

        public bool Connect()
        {
            nettyClient.Connect();
            return true;
        }

        public bool Connect(string ip, int port)
        {
            NettyConfig.Host = ip;
            NettyConfig.Port = port;
            return Connect();
        }

        public NetChannel GetNetSocket()
        {
            return nettyClient.GetNetSocket();
        }

        public int SendData(byte[] data)
        {
            nettyClient.SendData(data);
            return 0;
        }

        public int SendDataUDP(string ip, int port, byte[] data)
        {
           return SendData(data);
        }
    }
}
