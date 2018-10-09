using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

/**
* 命名空间: NetHighSocket 
* 类 名： SocketTask
* 版本 ：v1.0
* Copyright (c) year 
*/

namespace NetHighSocket
{
    /// <summary>
    /// 功能描述    ：SocketTask  
    /// 创 建 者    ：jinyu
    /// 创建日期    ：2018/10/9 16:29:29 
    /// 最后修改者  ：jinyu
    /// 最后修改日期：2018/10/9 16:29:29 
    /// </summary>
   public class SocketTask
    {
        private int workNum = 1;//工作线程数据量
        //只有这一个
        private ISocketChannel socketChannel = null;
        private Dictionary<HanderType, IChannelHandler> handers;
        private volatile bool isStop = false;
        private volatile bool isTCP = true;
        private string localIP = "";
        private int localPort = 0;
        private string remoteIP = "";
        private int remotePort = 0;

        /// <summary>
        /// TCP时是唯一的一个
        /// </summary>
        private SocketArgEvent socketArg = null;

        public int WorkNum { get { return workNum; } set { workNum = value; } }


        #region 手动控制接收，其实是差不多的
        /// <summary>
        /// 监测数据
        /// </summary>
        private void DoWork()
        {
            
            SocketArgEvent item =new SocketArgEvent();
            IPEndPoint iPEndPoint = socketChannel.Socket.LocalEndPoint as IPEndPoint;
            item.localIP = iPEndPoint.Address.ToString();
            item.localPort = iPEndPoint.Port;
            localIP = item.localIP;
            localPort = item.localPort;
            item.chanel = socketChannel;
            if (socketChannel.Socket.RemoteEndPoint!=null)
            {
                iPEndPoint = socketChannel.Socket.RemoteEndPoint as IPEndPoint;
                item.remoteIP = iPEndPoint.Address.ToString();
                item.remotePort = iPEndPoint.Port;
            }
            while (!isStop)
            {
                if (socketChannel!= null)
                {
                    if(socketChannel.IsClose)
                    {
                        //已经关闭socket,最后接收一次数据
                        Close();
                    }
                    IChannelHandler rwhandler = null;
                    IChannelHandler handler = null;
                    if (!handers.TryGetValue(HanderType.Decoder, out handler))
                    {
                        //如果没有解析类，就直接获取读取管道
                        handers.TryGetValue(HanderType.ReadWrite, out rwhandler);
                    }

                    if (socketChannel.Socket.Available > 0)
                    {
                        //读取数据
                        if (isTCP)
                        {
                            TCPSocketChannel channel = socketChannel as TCPSocketChannel;
                            while (channel.Socket.Connected && channel.Socket.Available > 0)
                            { 
                                int num = channel.Socket.Available;
                                List<ByteBuffer> buffers = BufferManager.GetInstance().GetBufferSize(num);
                                for (int i = 0; i < buffers.Count; i++)
                                {
                                    int r = channel.Socket.Receive(buffers[i].buffer);
                                    buffers[i].Reset(0, r);
                                }
                                if (channel.AddBuffer(buffers, num) > 0)
                                {
                                    if (handler != null)
                                    {
                                        foreach (byte[] result in channel.RecBuffer)
                                        {
                                            handler.ChannelRead(item, result);
                                        }
                                    }
                                    else if (rwhandler != null)
                                    {
                                        foreach (byte[] result in channel.RecBuffer)
                                        {
                                            rwhandler.ChannelRead(item, result);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        UDPSocketChannel channel =socketChannel as UDPSocketChannel;
                        EndPoint endPoint = null;
                        while (!channel.IsClose && channel.Socket.Available > 0)
                        {
                            int num = channel.Socket.Available;
                            List<ByteBuffer> buffers = BufferManager.GetInstance().GetBufferSize(num);
                            for (int i = 0; i < buffers.Count; i++)
                            {
                                int r = channel.Socket.ReceiveFrom(buffers[i].buffer,ref endPoint);
                                buffers[i].Reset(0, r);
                            }
                            if (channel.AddBuffer(buffers, num) > 0)
                            {
                                SocketArgEvent udpItem = new SocketArgEvent();
                                //
                                udpItem.localIP = localIP;
                                udpItem.localPort = localPort;
                                if (endPoint != null)
                                {
                                    iPEndPoint = endPoint as IPEndPoint;
                                    item.remoteIP = iPEndPoint.Address.ToString();
                                    item.remotePort = iPEndPoint.Port;
                                }
                                if (handler != null)
                                {
                                    foreach (byte[] result in channel.RecBuffer)
                                    {
                                        handler.ChannelRead(udpItem, result);
                                    }
                                }
                                else if (rwhandler != null)
                                {
                                    foreach (byte[] result in channel.RecBuffer)
                                    {
                                        rwhandler.ChannelRead(udpItem, result);
                                    }
                                }
                                handler.ChannelReadComplete(udpItem);
                            }
                        }

                    }
                    
                    if (isTCP)
                    {
                        if (handler != null)
                        {
                            //当前一次读取已经完成
                            handler.ChannelReadComplete(item);
                        }
                        if (item.chanel.Socket.Connected)
                        {
                            //还是连接状态就放回等待监测数据
                            Thread.Sleep(100);
                        }
                        else
                        {
                            if (handler != null)
                            {
                                //
                                handler.ChannelInactive(item);
                                handler.CloseAsync(item);
                            }
                        }
                    }
                    else
                    {
                        UDPSocketChannel channel = item.chanel as UDPSocketChannel;
                        if (!channel.IsClose)
                        {
                            //还是连接状态就放回等待监测数据
                            Thread.Sleep(100);
                        }
                        else
                        {
                            if (handler != null)
                            {
                                //
                                handler.ChannelInactive(item);
                                handler.CloseAsync(item);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        ///启动监测线程
        /// </summary>
        public void WorkThread()
        {
            Interlocked.Decrement(ref workNum);
            if (WorkNum > -1)
            {
                Thread thread = new Thread(DoWork);
                thread.IsBackground = true;
                thread.Start();
            }
        }
      
        #endregion
      
        
        /// <summary>
        /// 关闭
        /// </summary>
        public void Close()
        {
            isStop = true;
            try
            {
                this.socketChannel.Close();
                this.socketChannel.Socket.Close();
            }
            catch
            {

            }
 
            
        }
       
        
        #region 直接使用IO线程控制接收,使用事件模型

        public void Start()
        {
            Interlocked.Decrement(ref workNum);
            while (WorkNum > -1)
            {
               
                if (isTCP)
                {

                    //这里是客户端的Socket，并不需要缓存
                    SocketAsyncEventArgs item = new SocketAsyncEventArgs();
                    item.Completed += Item_Completed;
                    socketChannel.Socket.ReceiveAsync(item);
                    //
                    IPEndPoint iPEndPoint = socketChannel.Socket.RemoteEndPoint as IPEndPoint;
                    if (iPEndPoint != null)
                    {
                        remoteIP = iPEndPoint.Address.ToString();
                        remotePort = iPEndPoint.Port;
                    }
                    iPEndPoint = socketChannel.Socket.LocalEndPoint as IPEndPoint;
                    if (iPEndPoint != null)
                    {
                        localIP = iPEndPoint.Address.ToString();
                        localPort = iPEndPoint.Port;
                    }
                }
                else
                {
                    SocketAsyncEventArgs udpEvent = new SocketAsyncEventArgs();
                    udpEvent.Completed += UdpEvent_Completed;
                    socketChannel.Socket.ReceiveAsync(udpEvent);
                }
            }
        }

        private void UdpEvent_Completed(object sender, SocketAsyncEventArgs e)
        {
            IChannelHandler rwhandler = null;
            IChannelHandler handler = null;
            if (!handers.TryGetValue(HanderType.Decoder, out handler))
            {
                //如果没有解析类，就直接获取读取管道
                handers.TryGetValue(HanderType.ReadWrite, out rwhandler);
            }
            SocketArgEvent item = new SocketArgEvent();
            item.chanel = socketChannel;
            item.data = e.Buffer;
            item.localIP = localIP;
            item.localPort = localPort;
            item.remoteIP = remoteIP;
            item.remotePort = remotePort;
            if (handler != null)
            {
                handler.ChannelRead(socketArg, e.Buffer);
                handler.ChannelReadComplete(socketArg);
            }
            else if (rwhandler != null)
            {
                rwhandler.ChannelRead(socketArg, e.Buffer);
                rwhandler.ChannelReadComplete(socketArg);
            }
        }

        private void Item_Completed(object sender, SocketAsyncEventArgs e)
        {
            IChannelHandler rwhandler = null;
            IChannelHandler handler = null;
            if (!handers.TryGetValue(HanderType.Decoder, out handler))
            {
                //如果没有解析类，就直接获取读取管道
                handers.TryGetValue(HanderType.ReadWrite, out rwhandler);
            }
            if(socketArg==null)
            {
                socketArg = new SocketArgEvent();
                socketArg.chanel = socketChannel;
            }
            socketArg.data = e.Buffer;
            socketArg.localIP = localIP;
            socketArg.localPort = localPort;
            socketArg.remoteIP = remoteIP;
            socketArg.remotePort = remotePort;
            if(handler!=null)
            {
                handler.ChannelRead(socketArg, e.Buffer);
                handler.ChannelReadComplete(socketArg);
            }
            else if(rwhandler!=null)
            {
                rwhandler.ChannelRead(socketArg, e.Buffer);
                rwhandler.ChannelReadComplete(socketArg);
            }

        }

        #endregion
    }
}
