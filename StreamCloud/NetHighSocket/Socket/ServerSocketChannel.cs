using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

/**
* 命名空间: NetHighSocket 
* 类 名： ServerSocket
* 版本 ：v1.0
* Copyright (c) year 
*/

namespace NetHighSocket
{
    /// <summary>
    /// 功能描述    ：ServerSocket  启动
    /// 创 建 者    ：jinyu
    /// 创建日期    ：2018/10/5 14:27:15 
    /// 最后修改者  ：jinyu
    /// 最后修改日期：2018/10/5 14:27:15 
    /// </summary>
   public class ServerSocketChannel
    {
       private TCPSocketChannel channel = null;
       private  ServerSocketTask socketTask = null;
       private Dictionary<HanderType, IChannelHandler> handers = new Dictionary<HanderType, IChannelHandler>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="socketNum"></param>
        /// <param name="workThread"></param>
        private void Init(int socketNum,int workThread)
        {
            if(channel==null)
            {
                channel = new TCPSocketChannel();
            }
            if (socketTask == null)
            {
                socketTask = new ServerSocketTask();
                socketTask.InitSocket(workThread, socketNum);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="socketNum"></param>
        /// <param name="workThread"></param>
        public ServerSocketChannel(int socketNum=100,int workThread=0)
        {
           if(workThread==0)
            {
                workThread = Environment.ProcessorCount * 2;
            }
            Init(socketNum, workThread);
        }

        /// <summary>
        /// 绑定服务端地址并且监听
        /// </summary>
        /// <param name="port"></param>
        /// <param name="host"></param>
        /// <returns></returns>
       public async Task<bool> BindAsync(int port, string host=null)
        {
            EndPoint endPoint = null;
            if (!string.IsNullOrEmpty(host))
            {
                endPoint = new IPEndPoint(IPAddress.Parse(host), port);
            }
            else
            {
                endPoint = new IPEndPoint(IPAddress.Any, port);
            }
            bool result = await channel.BindAsync(endPoint);
            if (result)
            {
                socketTask.TCPSocketChannel = channel;
                socketTask.HanderTypes = handers;
                socketTask.Start();
            }
            return result;

        }
        public ServerSocketChannel Hander(IChannelHandler handler)
        {
            if(handler!=null)
            {

                if (handler.GetType().IsImplementedRawGeneric(typeof(MessageToByteEncoder<>)))
                {
                    handers[HanderType.Encoder] = handler;
                }
                else if (handler.GetType().IsImplementedRawGeneric(typeof(MessageToMessageDecoder<>)))
                {
                    handers[HanderType.Decoder] = handler;
                }
                else if (typeof(ChannelHandlerAdapter).IsAssignableFrom(handler.GetType()))
                {
                    handers[HanderType.ReadWrite] = handler;
                }
            }
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sockOption"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public ServerSocketChannel Option(SockOption sockOption,int size)
        {
            channel.SetOption(sockOption, size);
            return this;
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public void Close()
        {
            socketTask.Close();
            channel.Close();
        }
      
    }
}
