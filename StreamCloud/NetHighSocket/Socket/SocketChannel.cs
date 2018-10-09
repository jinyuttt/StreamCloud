using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

/**
* 命名空间: NetHighSocket 
* 类 名： SocketChannel
* 版本 ：v1.0
* Copyright (c) year 
*/

namespace NetHighSocket
{
    /// <summary>
    /// 功能描述    ：SocketChannel  处理客户端管道，
    /// 创 建 者    ：jinyu
    /// 创建日期    ：2018/10/9 2:04:30 
    /// 最后修改者  ：jinyu
    /// 最后修改日期：2018/10/9 2:04:30 
    /// 该类与服务端有点不同，直接使用底层IO线程，客户端不控制线程数
    /// </summary>
   public class SocketChannel
    {
        ISocketChannel channel = null;
        private Dictionary<HanderType, IChannelHandler> handers = new Dictionary<HanderType, IChannelHandler>();
        private volatile bool isRunRecvice = false;
        private SocketTask socketTask = null;
        private int workNum = 1;//工作线程数量
        private bool isRecEvent = true;//是否采用事件模型

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="workThread">接收线程数据量</param>
        /// <param name="isEvent">是否采用事件模型接收</param>
        public SocketChannel(int workThread=1,bool isEvent=true)
        {
            workNum = workThread;
            isRecEvent = isEvent;
        }
        /// <summary>
        /// 开启接收
        /// </summary>
        private void StartRecvice()
        {
            if(isRunRecvice)
            {
                return;
            }
            isRunRecvice = true;
            if (socketTask == null)
            {
                socketTask = new SocketTask();
                socketTask.WorkNum = workNum;
            }
            if (isRecEvent)
            {
                while(socketTask.WorkNum>-1)
                {
                    socketTask.Start();
                }
            }
            else
            {
                while (socketTask.WorkNum > -1)
                {
                    socketTask.WorkThread();
                }
               
            }
        }
        /// <summary>
        /// 绑定服务端地址并且监听
        /// </summary>
        /// <param name="port"></param>
        /// <param name="host"></param>
        /// <returns></returns>
        public async Task<bool> BindAsync(int port, string host = null)
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
            if(result)
            {
                StartRecvice();//绑定成功就介绍
            }
            return result;

        }

        /// <summary>
        /// 连接
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public async Task<bool> ConnectAsync(string host,int port)
        {
            EndPoint endPoint = new IPEndPoint(IPAddress.Parse(host), port);
            return await ConnectAsync(endPoint);
        }

        /// <summary>
        /// 连接
        /// </summary>
        /// <param name="endPoint"></param>
        /// <returns></returns>
        public async Task<bool> ConnectAsync(EndPoint endPoint)
        {
            bool r= await channel.ConnectAsync(endPoint);
            if(r)
            {
                StartRecvice();
            }
            return r;
        }

        /// <summary>
        /// 设置操作
        /// </summary>
        /// <param name="handler"></param>
        /// <returns></returns>
        public SocketChannel Hander(IChannelHandler handler)
        {
            if (handler != null)
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
        /// 设置参数
        /// </summary>
        /// <param name="sockOption"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public SocketChannel Option(SockOption sockOption, int size)
        {
            channel.SetOption(sockOption, size);
            return this;
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public void Close()
        {
            channel.Close();
        }

        /// <summary>
        /// 创建通信
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public ISocketChannel InitChannel<T>() where T:ISocketChannel
        {
            return channel = Activator.CreateInstance<T>();
        }
    }
}
