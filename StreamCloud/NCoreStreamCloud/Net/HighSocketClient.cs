using NetHighSocket;
using SrvNetSocket;
using System.Collections.Concurrent;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

/**
* 命名空间: NStStreamCloud.Net 
* 类 名： Class2
* 版本 ：v1.0
* Copyright (c) year 
*/

namespace NStStreamCloud.Net
{
    /// <summary>
    /// 功能描述    ：Class2  
    /// 创 建 者    ：jinyu
    /// 创建日期    ：2018/10/8 0:52:29 
    /// 最后修改者  ：jinyu
    /// 最后修改日期：2018/10/8 0:52:29 
    /// </summary>
    public class HighSocketClient<TData>
    {
       // IChannel channel = null;
        ManualResetEvent resetEvent;
        RecviceNetData<TData> recviceData = null;
        BlockingCollection<NetChannel<TData>> netSockets = null;
        TCPSocketChannel tCPSocket = null;
        public async Task<bool> RunClientAsync(string ip,int port)
        {

            resetEvent = new ManualResetEvent(false);
            netSockets = new BlockingCollection<NetChannel<TData>>();
            tCPSocket = new TCPSocketChannel();

             EndPoint endPoint = new IPEndPoint(IPAddress.Parse(ip),port);
           return  await tCPSocket.ConnectAsync(endPoint);


        }

        /// <summary>
        /// 回传接收的数据
        /// </summary>
        /// <param name="netSocket"></param>
        public void AddData(NetChannel<TData> netSocket)
        {
            if (recviceData != null)
            {
                recviceData(netSocket);
            }
            else
            {
                netSockets.Add(netSocket);
            }
        }

       
        /// <summary>
        /// 直接发送数据到网络
        /// </summary>
        /// <param name="data"></param>
        public void SendData(byte[] data)
        {
            tCPSocket.SendData(data);
        }

        /// <summary>
        /// 提交数据处理后发送
        /// </summary>
        /// <param name="data"></param>
        public void SendData(object  data)
        {
            tCPSocket.WriteAndFlushAsync(data);
        }
        public void CallData(RecviceNetData<TData> recviceData)
        {
            this.recviceData = recviceData;
        }
        public NetChannel<TData> GetNetSocket()
        {
            return netSockets.Take();
        }
        public void Close()
        {
            this.resetEvent.Reset();
            tCPSocket.Close();
            netSockets.Dispose();
        }
    }
}
