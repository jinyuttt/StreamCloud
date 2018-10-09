using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Concurrent;
using System.Threading.Tasks;
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
    /// 创建日期    ：2018/10/5 12:35:36 
    /// 最后修改者  ：jinyu
    /// 最后修改日期：2018/10/5 12:35:36 
    /// 一组数据，不同协议头  是否分组（0不分组，1分组）
    /// 不分组，则2字节总长度，读取完成即可
    /// 分组，每包包含，总长度，组序号，包数+数据
    /// </summary>
   public class ServerSocketTask
    {

        private int workNum = 1;//工作线程数据量
        private object lock_obj = new object();
        private TCPSocketChannel socketChannel = null;
        private  SocketAsyncEventArgsPool pool = null;
        private Dictionary<HanderType, IChannelHandler> handers;
        private int socketNum;//同时连接的socket数据量，最小10
        private volatile bool isStop = false;
        private BlockingCollection<SocketArgEvent> channels = new BlockingCollection<SocketArgEvent>();
        private int m_numConnectedSockets;
        private volatile bool isRunListen = false;
        AutoResetEvent acceptEvent = new AutoResetEvent(false);

        /// <summary>
        /// 处理方法
        /// </summary>
        internal  Dictionary<HanderType, IChannelHandler> HanderTypes { set { handers = value; } }

        public  TCPSocketChannel TCPSocketChannel { set { socketChannel = value; } }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="workThread"></param>
        /// <param name="num"></param>
        public void InitSocket(int workThread,int num)
        {
            workNum = workThread;
            if(num<10)
            {
                num = 10;
            }
            socketNum = num;
            pool = new SocketAsyncEventArgsPool(num);
            for(int i=0;i<num;i++)
            {
                SocketAsyncEventArgs item = new SocketAsyncEventArgs();
                item.Completed += Item_Completed;
                pool.Push(item);
            }
            //
        }

        private void Item_Completed(object sender, SocketAsyncEventArgs e)
        {
            acceptEvent.Set();
            IChannelHandler handler = null;
            SocketArgEvent arg = new SocketArgEvent();
            IPEndPoint point = e.AcceptSocket.RemoteEndPoint as IPEndPoint;
            arg.remoteIP = point.Address.ToString();
            arg.remotePort = point.Port;
            point = e.AcceptSocket.LocalEndPoint as IPEndPoint;
            arg.localIP = point.Address.ToString();
            arg.localPort = point.Port;
            arg.chanel = socketChannel;
            TCPSocketChannel channel = new TCPSocketChannel();
            channel.HandlerTypes = this.handers;
            channel.Socket = e.AcceptSocket;
            channel.SocketArgEvent = arg;
            arg.chanel = channel;
            if (handers.TryGetValue(HanderType.ReadWrite, out handler))
            {
                handler.ChannelActive(arg);
            }
            lock (lock_obj)
            {
                channels.Add(arg);
            }
            Interlocked.Increment(ref m_numConnectedSockets);
            FreeArgEvent(e);
            WorkThread();
        }

        private void FreeArgEvent(SocketAsyncEventArgs e)
        {
            e.AcceptSocket = null;
            pool.Push(e);
        }

        /// <summary>
        /// 启动监听
        /// </summary>
        public void Start()
        {
            //在线程池中等待连接，这里使用线程池线程，充分利用已有线程
           if(isRunListen)
            {
                return;
            }
            isRunListen = true;
            Task.Factory.StartNew(() =>
            {
                try
                {
                    socketChannel.Socket.Listen(socketNum);
                }
                catch(Exception ex)
                {
                    //监听异常后，退出监听，重启启动
                    isRunListen = false;
                    return;
                }
                while (!isStop)
                {
                    SocketAsyncEventArgs e = pool.Pop();
                    if (e == null)
                    {
                        e = new SocketAsyncEventArgs();
                        e.Completed += Item_Completed;
                    }
                   //连接使用事件模型，使用完成就还回
                   //这里是线程池中通知
                    socketChannel.Socket.AcceptAsync(e);
                    acceptEvent.WaitOne();
                }
            });
        }

        /// <summary>
        /// 监测数据
        /// </summary>
        private  void DoWork()
        {
            while(!isStop)
            {
                if(socketChannel.IsClose)
                {
                    //说明监听被关闭；
                    this.Close();

                }
                SocketArgEvent item = null;
                if (!channels.TryTake(out item, 1000))
                {
                    continue;
                }

                TCPSocketChannel channel = item.chanel as TCPSocketChannel;
                if(channel!=null)
                {
                    IChannelHandler rwhandler = null;
                    IChannelHandler handler = null;
                    if(!handers.TryGetValue(HanderType.Decoder, out handler))
                    {
                        //如果没有解析类，就直接获取读取管道
                        handers.TryGetValue(HanderType.ReadWrite, out rwhandler);
                    }
                  
                    if (channel.Socket.Available>0)
                    {
                        //读取数据
                        while(channel.Socket.Connected&&channel.Socket.Available>0)
                        {
                            int num = channel.Socket.Available;
                            List<ByteBuffer> buffers= BufferManager.GetInstance().GetBufferSize(num);
                            for(int i=0;i<buffers.Count;i++)
                            {
                                int r=channel.Socket.Receive(buffers[i].buffer);
                                buffers[i].Reset(0,r);
                            }
                            if(channel.AddBuffer(buffers,num)>0)
                            {
                                if(handler!=null)
                                {
                                    foreach (byte[] result in channel.RecBuffer)
                                    {
                                        handler.ChannelRead(item, result);
                                    }
                                }
                                else if(rwhandler!=null)
                                {
                                    foreach (byte[] result in channel.RecBuffer)
                                    {
                                        rwhandler.ChannelRead(item, result);
                                    }
                                }
                            }
                        }
                        
                    }
                    if (handler != null)
                    {
                        //当前一次读取已经完成
                        handler.ChannelReadComplete(item);
                    }
                    if(channel.Socket.Connected)
                    {
                        //还是连接状态就放回等待监测数据
                        channels.Add(item);
                    }
                    else
                    {
                        if (handler != null)
                        {
                            //
                            handler.ChannelInactive(item);
                            handler.CloseAsync(item);
                        }
                        Interlocked.Decrement(ref m_numConnectedSockets);

                    }
                }
            }
        }

        /// <summary>
        ///启动监测线程
        /// </summary>
        private void WorkThread()
        {
            Interlocked.Decrement(ref workNum);
            if(workNum>0)
            {
                Thread thread = new Thread(DoWork);
                thread.IsBackground = true;
                thread.Start();
            }
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public void Close()
        {
            isStop = true;
            this.isRunListen = false;
            this.pool.Close();
            try
            {
                this.socketChannel.Close();
                this.socketChannel.Socket.Close();
                this.channels.Dispose();
            }
            catch
            {

            }
        }

    }
}
