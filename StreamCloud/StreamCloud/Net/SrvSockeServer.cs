
using SrvNetSocket;

namespace NStStreamCloud.Net
{
    [NetProtocol("stream_server")]
    public class SrvSockeServer<T> : ISocketServer<T>
    {
        HighSocketServer<T> server = new HighSocketServer<T>();
        public void CallData(RecviceNetData<T> recviceNetData)
        {
            server.CallData(recviceNetData);
        }

        public void Close()
        {
            server.Close();
        }

        public NetChannel<T> GetNetSocket()
        {
           return server.GetNetSocket();
        }

        public int SendData(NetChannel<T> channel, byte[] data)
        {
            //
            server.SendData(channel, data);
            return 0;
        }

        public int SendData(NetChannel<T> channel, object data)
        {
            server.SendData(channel, data);
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

        public  bool StartRun()
        {
            var r= server.RunServerAsync(NettyConfig.Host, NettyConfig.Port);
            return r.Result;
        }

      
    }
}
