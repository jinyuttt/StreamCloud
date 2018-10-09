using DotNetty.Codecs;
using DotNetty.Handlers.Logging;
using DotNetty.Handlers.Timeout;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetSocket.Netty
{
   public class NettyUDPClient
    {
        IChannel channel = null;
        ManualResetEvent resetEvent;
        RecviceNetData recviceData = null;
        BlockingCollection<NetChannel> netSockets = null;

        private async Task RunClientAsync()
        {

            resetEvent = new ManualResetEvent(false);
            netSockets = new BlockingCollection<NetChannel>();
            var group = new MultithreadEventLoopGroup();
           
            try
            {
               
                 var bootstrap = new Bootstrap();
                bootstrap
                    .Group(group)
                    .Channel<SocketDatagramChannel>()
                    .Option(ChannelOption.SoBroadcast, true)
                    .Option(ChannelOption.SoRcvbuf,NettyConfig.BufSize)
                    .Option(ChannelOption.SoSndbuf,NettyConfig.BufSize)
                    .Handler(new ActionChannelInitializer<ISocketChannel>(channel =>
                    {
                        IChannelPipeline pipeline = channel.Pipeline;
                       
                        pipeline.AddLast(new LoggingHandler());

                        pipeline.AddLast("timeout", new IdleStateHandler(0, 0, NettyConfig.HeartTime / 1000));
                        //消息前处理

                        pipeline.AddLast("framing-enc", new LengthFieldPrepender(2));
                        pipeline.AddLast("framing-dec", new LengthFieldBasedFrameDecoder(ushort.MaxValue, 0, 2, 0, 2));
                        pipeline.AddLast("decoder", new MsgPackDecode());
                        pipeline.AddLast("ecoder", new MsgPackEcode());
                        pipeline.AddLast("echo", new EchoUDPClientHandler(this));
                    }));
              //  bootstrap.
              //  channel = await bootstrap.ConnectAsync(new IPEndPoint(IPAddress.Parse(NettyConfig.Host), NettyConfig.Port));
                resetEvent.WaitOne();

                await channel.CloseAsync();
            }
            finally
            {
                await group.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1));
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

        public async Task Connect()
        {
            await RunClientAsync();
        }
        public void SendData(byte[] data)
        {
            channel.WriteAndFlushAsync(data);
        }
        public void SendData(string ip,int port,byte[]data)
        {
            channel.WriteAndFlushAsync(data);
        }
        public void CallData(RecviceNetData recviceData)
        {
            this.recviceData = recviceData;
        }
        public NetChannel GetNetSocket()
        {
            return netSockets.Take();
        }
        public void Close()
        {
            this.resetEvent.Reset();
            netSockets.Dispose();
        }
    }
}
