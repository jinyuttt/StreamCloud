using NetHighSocket;
using SrvNetSocket;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

/**
* 命名空间: NStStreamCloud.Net 
* 类 名： HighSocketServer
* 版本 ：v1.0
* Copyright (c) year 
*/

namespace NStStreamCloud.Net
{
    /// <summary>
    /// 功能描述    ：HighSocketServer  
    /// 创 建 者    ：jinyu
    /// 创建日期    ：2018/10/8 0:13:28 
    /// 最后修改者  ：jinyu
    /// 最后修改日期：2018/10/8 0:13:28 
    /// </summary>
    public class HighSocketServer<TData>
    {
        ManualResetEvent resetEvent;
        RecviceNetData<TData> recviceData = null;
        BlockingCollection<NetChannel<TData>> netSockets = null;
        ServerSocketChannel server = null;

        /// <summary>
        /// 启动服务
        /// </summary>
        /// <returns></returns>
        public async Task<bool> RunServerAsync(string ip,int port)
        {
             server = new ServerSocketChannel();
             server.Hander(new DataChannelHandler<TData>(this))
            .Hander(new MsgPackDecode())
            .Hander(new MsgPackEcode());
             return await  server.BindAsync(port, ip);
        }

        /// <summary>
        /// 内部使用，回传数据
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
        /// 回调数据
        /// </summary>
        /// <param name="recviceData"></param>
        public void CallData(RecviceNetData<TData> recviceData)
        {
            this.recviceData = recviceData;
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        public NetChannel<TData> GetNetSocket()
        {
            return netSockets.Take();
        }


        /// <summary>
        /// 直接发送数据到网络
        /// </summary>
        /// <param name="netChannel"></param>
        /// <param name="data"></param>
        public void SendData(NetChannel<TData> netChannel, byte[] data)
        {
            ISocketChannel channel = netChannel.channel as ISocketChannel;
            if (channel != null)
            {
                channel.SendData(data);
            }

        }
        /// <summary>
        /// 发送数据处理
        /// </summary>
        /// <param name="netChannel"></param>
        /// <param name="data"></param>
        public void SendData(NetChannel<TData> netChannel, object data)
        {
            ISocketChannel channel = netChannel.channel as ISocketChannel;
            if (channel != null)
            {
                channel.WriteAndFlushAsync(data);
            }

        }
        /// <summary>
        /// 关闭
        /// </summary>
        public void Close()
        {
            this.resetEvent.Reset();
            server.Close();
        }
    }
}
