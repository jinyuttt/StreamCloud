using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using System;
using System.Net;

namespace NetSocket.Netty
{
    internal class EchoUDPServerHandler : ChannelHandlerAdapter
    {
        private string localIP = "";
        private int localPort = 0;
        private bool isInit = true;
        byte[] heart = null;
        private NettyUDPServer nettyUDPServer;

        public EchoUDPServerHandler(NettyUDPServer nettyUDPServer)
        {
            this.nettyUDPServer = nettyUDPServer;
        }
        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            var buffer = message as IByteBuffer;
            if (buffer != null)
            {
                // Console.WriteLine("Received from client: " + buffer.ToString(Encoding.UTF8));
                if (isInit)
                {
                    IPEndPoint iPEndPoint = context.Channel.LocalAddress as IPEndPoint;
                    if (iPEndPoint != null)
                    {
                        this.localIP = iPEndPoint.Address.ToString();
                        this.localPort = iPEndPoint.Port;
                    }
                    isInit = false;
                }
                if (heart == null)
                {
                    heart = NettyConfig.NettyEncod.GetBytes(NettyConfig.HeartMessage);
                }
                if (buffer.Array.Length == heart.Length)
                {
                    if (NettyConfig.NettyEncod.GetString(buffer.Array) == NettyConfig.HeartMessage)
                    {
                        return;
                    }
                }
                IPEndPoint endPoint = context.Channel.LocalAddress as IPEndPoint;
                NetChannel netSocket = new NetChannel() { recData = buffer.Array, localIP = localIP, localPort = localPort, remoteIP = endPoint.Address.ToString(), remotePort = endPoint.Port, channel = context.Channel };
                nettyUDPServer.AddData(netSocket);

            }
           
        }

        public override void ChannelReadComplete(IChannelHandlerContext context) => context.Flush();

        //捕获 异常，并输出到控制台后断开链接，提示：客户端意外断开链接，也会触发
        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            Console.WriteLine("Exception: " + exception);
            context.CloseAsync();
        }

    }
}