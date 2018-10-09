using DotNetty.Codecs;
using DotNetty.Handlers.Logging;
using DotNetty.Handlers.Timeout;
using DotNetty.Handlers.Tls;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Concurrent;
namespace  NetSocket.Netty
{
  
    public class NettyClient
    {

        IChannel channel = null;
        ManualResetEvent resetEvent;
        RecviceNetData recviceData = null;
        BlockingCollection<NetChannel> netSockets = null;
      
      private  async Task RunClientAsync()
        {

            resetEvent = new ManualResetEvent(false);
            netSockets = new BlockingCollection<NetChannel>();
            var group = new MultithreadEventLoopGroup();
            X509Certificate2 cert = null;
            string targetHost = null;
            if (NettyConfig.IsSsl)
            {
                cert = new X509Certificate2(Path.Combine(NettyConfig.CertificatePath, "dotnetty.com.pfx"), "password");
                targetHost = cert.GetNameInfo(X509NameType.DnsName, false);
            }
            try
            {
                var bootstrap = new Bootstrap();
                bootstrap
                    .Group(group)
                    .Channel<TcpSocketChannel>()
                    .Option(ChannelOption.TcpNodelay, true)
                    .Option(ChannelOption.ConnectTimeout, TimeSpan.FromSeconds(3))
                    .Handler(new ActionChannelInitializer<ISocketChannel>(channel =>
                    {
                        IChannelPipeline pipeline = channel.Pipeline;
                        if (cert != null)
                        {
                            pipeline.AddLast("tls", new TlsHandler(stream => new SslStream(stream, true, (sender, certificate, chain, errors) => true), new ClientTlsSettings(targetHost)));
                        }
                        pipeline.AddLast(new LoggingHandler());
                      
                        pipeline.AddLast("timeout", new IdleStateHandler(0, 0, NettyConfig.HeartTime/ 1000));
                        //消息前处理
                       
                        pipeline.AddLast("framing-enc", new LengthFieldPrepender(2));
                        pipeline.AddLast("framing-dec", new LengthFieldBasedFrameDecoder(ushort.MaxValue, 0, 2, 0, 2));
                        pipeline.AddLast("decoder", new MsgPackDecode());
                        pipeline.AddLast("ecoder", new MsgPackEcode());
                        pipeline.AddLast("echo", new EchoClientHandler(this));
                    }));

                channel = await bootstrap.ConnectAsync(new IPEndPoint(IPAddress.Parse(NettyConfig.Host), NettyConfig.Port));
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
            if(recviceData!=null)
            {
                recviceData(netSocket);
            }
            else
            {
                netSockets.Add(netSocket);
            }
        }

        public async Task  Connect()
        {
             await RunClientAsync();
        }
        public void SendData(byte[]data)
        {
            channel.WriteAndFlushAsync(data);
        }
        public void  CallData(RecviceNetData recviceData)
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
