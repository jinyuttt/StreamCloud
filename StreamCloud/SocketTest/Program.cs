using NetHighSocket;
using System;
using System.Threading.Tasks;

namespace SocketTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //ServerSocketChannel针对TCP服务端
            ServerSocketChannel server = new ServerSocketChannel();
             server.Hander(new MessageDecode())//反序列化
            .Hander(new MessageEcode())//序列化
            .Hander(new SimpleChannelRead())//读取
            .Option(SockOption.recBufSize, 100);//设置参数
             Task<bool> r=  server.BindAsync(7777, "127.0.0.1");

            //SocketChannel针对TCP客户端或者UDP
            SocketChannel client = new SocketChannel();
            ISocketChannel channel = client.InitChannel<TCPSocketChannel>();
            client.Hander(new MessageDecode())//反序列化
            .Hander(new MessageEcode())//序列化
            .Hander(new SimpleChannelRead())//读取
            .Option(SockOption.recBufSize, 100);//设置参数
              r =client.ConnectAsync ( "127.0.0.1",7777);

            //UDP 服务端接收
            SocketChannel udpServer = new SocketChannel(20);
            ISocketChannel udpChannel = udpServer.InitChannel<UDPSocketChannel>();//必须先创建
            udpServer.Hander(new MessageDecode())//反序列化
            .Hander(new MessageEcode())//序列化
            .Hander(new SimpleChannelRead())//读取
            .Option(SockOption.recBufSize, 100);//设置参数
             r = udpServer.BindAsync( 7777, "127.0.0.1");

            //udp客户端
            SocketChannel udpclient = new SocketChannel();
            ISocketChannel udpchannel = udpclient.InitChannel<TCPSocketChannel>();
            udpclient.Hander(new MessageDecode())//反序列化
            .Hander(new MessageEcode())//序列化
            .Hander(new SimpleChannelRead())//读取
            .Option(SockOption.recBufSize, 100);//设置参数
             r = udpclient.ConnectAsync("127.0.0.1", 7777);
        }
    }
}
