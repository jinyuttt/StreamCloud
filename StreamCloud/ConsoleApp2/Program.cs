using NetHighSocket;
using System;
using System.Net;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            TCPSocketChannel channel = new TCPSocketChannel();
            EndPoint endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 7777);
            channel.Connect(endPoint);
            while (true)
            {
                channel.SendData(System.Text.UTF8Encoding.UTF8.GetBytes("ssss"));
                Console.Read();
            }
            //client.SendData(System.Text.UTF8Encoding.UTF8.GetBytes("ssss"));
        }
    }
}
