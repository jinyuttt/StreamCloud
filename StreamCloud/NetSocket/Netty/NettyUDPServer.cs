using DotNetty.Codecs;
using DotNetty.Handlers.Logging;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetSocket.Netty
{
  public  class NettyUDPServer
    {
        ManualResetEvent resetEvent;
        RecviceNetData recviceData = null;
        BlockingCollection<NetChannel> netSockets = null;

        IChannel boundChannel = null;
        public async Task RunServerAsync()
        {
            //设置输出日志到Console
            // ExampleHelper.SetConsoleLogger();
            resetEvent = new ManualResetEvent(false);
            resetEvent = new ManualResetEvent(false);
            resetEvent = new ManualResetEvent(false);
            netSockets = new BlockingCollection<NetChannel>();
            // 主工作线程组，设置为1个线程
            var bossGroup = new MultithreadEventLoopGroup(1);
            // 工作线程组，默认为内核数*2的线程数
            var workerGroup = new MultithreadEventLoopGroup();
           
            try
            {

                //声明一个服务端Bootstrap，每个Netty服务端程序，都由ServerBootstrap控制，
                //通过链式的方式组装需要的参数

                var bootstrap = new Bootstrap();
                bootstrap
                    .Group(workerGroup) // 设置主和工作线程组
                    .Channel<SocketDatagramChannel>() // 设置通道模式为TcpSocket
                    .Option(ChannelOption.SoBroadcast, true)
                    .Option(ChannelOption.SoRcvbuf, NettyConfig.BufSize)
                    .Option(ChannelOption.SoSndbuf, NettyConfig.BufSize)
                    .Handler(new LoggingHandler("SRV-LSTN")) //在主线程组上设置一个打印日志的处理器
                    .Handler(new ActionChannelInitializer<ISocketChannel>(channel =>
                    { //工作线程连接器 是设置了一个管道，服务端主线程所有接收到的信息都会通过这个管道一层层往下传输
                      //同时所有出栈的消息 也要这个管道的所有处理器进行一步步处理
                        IChannelPipeline pipeline = channel.Pipeline;
                        
                        //日志拦截器
                        pipeline.AddLast(new LoggingHandler("SRV-CONN"));
                        //出栈消息，通过这个handler 在消息顶部加上消息的长度
                        pipeline.AddLast("framing-enc", new LengthFieldPrepender(2));
                        //入栈消息通过该Handler,解析消息的包长信息，并将正确的消息体发送给下一个处理Handler，该类比较常用，后面单独说明
                        pipeline.AddLast("framing-dec", new LengthFieldBasedFrameDecoder(ushort.MaxValue, 0, 2, 0, 2));
                        pipeline.AddLast("decoder", new MsgPackDecode());
                        pipeline.AddLast("ecoder", new MsgPackEcode());
                        //业务handler ，这里是实际处理Echo业务的Handler
                        pipeline.AddLast("echo", new EchoUDPServerHandler(this));

                    }));

                // bootstrap绑定到指定端口的行为 就是服务端启动服务，同样的Serverbootstrap可以bind到多个端口
                boundChannel = await bootstrap.BindAsync(NettyConfig.Port);
                resetEvent.WaitOne();
                //关闭服务
                await boundChannel.CloseAsync();
            }
            finally
            {
                //释放工作组线程
                await Task.WhenAll(
                    bossGroup.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1)),
                    workerGroup.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1)));
            }
        }

        public void AddData(NetChannel netSocket)
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
        public void CallData(RecviceNetData recviceData)
        {
            this.recviceData = recviceData;
        }
        public NetChannel GetNetSocket()
        {
            return netSockets.Take();
        }

        public void SendData(NetChannel netChannel, byte[] data)
        {
            IChannel channel = netChannel.channel as IChannel;
            if (channel != null)
            {
                channel.WriteAndFlushAsync(data);
            }

        }
        public void Close()
        {
            this.resetEvent.Reset();

        }
    }
}
