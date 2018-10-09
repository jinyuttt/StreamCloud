using DotNetty.Buffers;
using DotNetty.Handlers.Timeout;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace  NetSocket.Netty
{
    internal class EchoClientHandler : ChannelHandlerAdapter
    {
        readonly IByteBuffer initialMessage;
      
        private NettyClient nettyClient = null;
        private string localIP = "";
        private int localPort = 0;
        private string remoteIP = "";
        private int remotePort = 0;
        bool isInit =true;
        public EchoClientHandler(NettyClient client)
        {
            this.nettyClient = client;
            this.initialMessage = Unpooled.Buffer(NettyConfig.Size);
            byte[] messageBytes = Encoding.UTF8.GetBytes("Hello world");
            this.initialMessage.WriteBytes(messageBytes);
        }

        //重写基类方法，当链接上服务器后，马上发送Hello World消息到服务端
        public override void ChannelActive(IChannelHandlerContext context) => context.WriteAndFlushAsync(this.initialMessage);

        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            var byteBuffer = message as IByteBuffer;
            if (byteBuffer != null)
            {
                if(isInit)
                {
                    IPEndPoint iPEndPoint = context.Channel.LocalAddress as IPEndPoint;
                    if (iPEndPoint != null)
                    {
                        this.localIP = iPEndPoint.Address.ToString();
                        this.localPort = iPEndPoint.Port;
                    }
                    iPEndPoint = context.Channel.RemoteAddress as IPEndPoint;
                    if (iPEndPoint != null)
                    {
                        this.remoteIP = iPEndPoint.Address.ToString();
                        this.remotePort = iPEndPoint.Port;
                    }
                    isInit = false;
                }
                NetChannel netSocket = new NetChannel() { recData = byteBuffer.Array, localIP = localIP, localPort = localPort, remoteIP = remoteIP, remotePort = remotePort };
                this.nettyClient.AddData(netSocket);
            }
           
        }

        public override void ChannelReadComplete(IChannelHandlerContext context) => context.Flush();

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            Console.WriteLine("Exception: " + exception);
            context.CloseAsync();
        }

        public override void UserEventTriggered(IChannelHandlerContext context, object evt)
        {
            if (evt is IdleStateEvent)
            {
                var eventState = evt as IdleStateEvent;
                if (eventState != null)
                {
                    byte[] heart = NettyConfig.NettyEncod.GetBytes(NettyConfig.HeartMessage);
                    context.WriteAndFlushAsync(heart);
                    //this.bootstrap.SendHeartbeatAsync(context, eventState);
                   
                }
            }
        }
    }
}