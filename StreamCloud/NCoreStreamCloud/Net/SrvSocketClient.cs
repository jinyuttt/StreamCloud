using SrvNetSocket;
using System;
using System.Collections.Generic;
using System.Text;

namespace NStStreamCloud.Net
{
    [NetProtocol("stream_client")]
    public class SrvSocketClient<T> : ISocketClient<T>
    {
        HighSocketClient<T> client = new HighSocketClient<T>();
        public void CallData(RecviceNetData<T> recviceNetData)
        {
            client.CallData(recviceNetData);
        }

        public void Close()
        {
            client.Close();
        }

        public bool Connect()
        {
            var r= client.RunClientAsync(NettyConfig.Host, NettyConfig.Port);
            return r.Result;
          
        }

        public bool Connect(string ip, int port)
        {
            var r=  client.RunClientAsync(ip, port);
            return r.Result;
        }

        public NetChannel<T> GetNetSocket()
        {
            return client.GetNetSocket();
        }

       

        public int SendData(byte[] data)
        {
            client.SendData(data);
            return 0;
        }

        public int SendData(object data)
        {
            client.SendData(data);
            return 0;
        }

        public int SendDataUDP(string ip, int port, byte[] data)
        {
           return SendData(data);
        }

        public int SendDataUDP(string ip, int port, object data)
        {
            return SendData(data);
        }
    }
}
