using System;
using System.Collections.Generic;
using System.Text;

namespace NetSocket.Netty
{
    public class SocketNettyUDPClient : ISocketClient
    {
        NettyUDPClient nettyUDPClient = new NettyUDPClient();
        public void CallData(RecviceNetData recviceNetData)
        {
            nettyUDPClient.CallData(recviceNetData);
        }

        public void Close()
        {
            nettyUDPClient.Close();
        }

        public bool Connect()
        {
            nettyUDPClient.Connect();
            return true;
        }

        public bool Connect(string ip, int port)
        {
            return true;
        }

        public NetChannel GetNetSocket()
        {
           return nettyUDPClient.GetNetSocket();
        }

        public int SendData(byte[] data)
        {
            nettyUDPClient.SendData(data);
            return 0;
        }

        public int SendDataUDP(string ip, int port, byte[] data)
        {
            return 0;
        }
    }
}
