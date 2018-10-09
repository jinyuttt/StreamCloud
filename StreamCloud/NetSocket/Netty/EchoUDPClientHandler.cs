using DotNetty.Buffers;
using DotNetty.Handlers.Timeout;
using DotNetty.Transport.Channels;
using System;
using System.Net;
using System.Text;

namespace NetSocket.Netty
{
    internal class EchoUDPClientHandler :  ChannelHandlerAdapter
    {
        readonly IByteBuffer initialMessage;
        private NettyUDPClient nettyUDPClient;

        public EchoUDPClientHandler(NettyUDPClient nettyUDPClient)
        {
            this.nettyUDPClient = nettyUDPClient;
            this.initialMessage = Unpooled.Buffer(NettyConfig.Size);
            byte[] messageBytes = Encoding.UTF8.GetBytes("Hello world");
            this.initialMessage.WriteBytes(messageBytes);
        }
        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            var byteBuffer = message as IByteBuffer;
            string localIP = "";
            int localPort = 0;
            string remoteIP = "";
            int remotePort = 0;
            if (byteBuffer != null)
            {
              
                    IPEndPoint iPEndPoint = context.Channel.LocalAddress as IPEndPoint;
                    if (iPEndPoint != null)
                    {
                       localIP = iPEndPoint.Address.ToString();
                        localPort = iPEndPoint.Port;
                    }
                    iPEndPoint = context.Channel.RemoteAddress as IPEndPoint;
                    if (iPEndPoint != null)
                    {
                        remoteIP = iPEndPoint.Address.ToString();
                        remotePort = iPEndPoint.Port;
                    }
                  
                }
                NetChannel netSocket = new NetChannel() { recData = byteBuffer.Array, localIP = localIP, localPort = localPort, remoteIP = remoteIP, remotePort = remotePort };
                this.nettyUDPClient.AddData(netSocket);
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