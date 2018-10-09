using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

/**
* 命名空间: NetHighSocket 
* 类 名： IChannelHandler
* 版本 ：v1.0
* Copyright (c) year 
*/

namespace NetHighSocket
{
    /// <summary>
    /// 功能描述    ：IChannelHandler  
    /// 创 建 者    ：jinyu
    /// 创建日期    ：2018/10/5 1:08:18 
    /// 最后修改者  ：jinyu
    /// 最后修改日期：2018/10/5 1:08:18 
    /// </summary>
   public interface IChannelHandler
    {

        void ChannelActive(SocketArgEvent argEvent);

        void ChannelInactive(SocketArgEvent argEvent);

        /// <summary>
        /// 传给管道
        /// </summary>
        /// <param name="argEvent"></param>
        /// <param name="message"></param>
        void ChannelRead(SocketArgEvent argEvent, object message);

        void ChannelReadComplete(SocketArgEvent argEvent);

    
        void ChannelWritabilityChanged(SocketArgEvent argEvent);

        void HandlerAdded(SocketArgEvent argEvent);

        void HandlerRemoved(SocketArgEvent argEvent);

        Task WriteAsync(SocketArgEvent argEvent, object message);

        void Flush(SocketArgEvent argEvent);

      
        Task BindAsync(SocketArgEvent argEvent, EndPoint localAddress);
    

     
        Task ConnectAsync(SocketArgEvent argEvent, EndPoint remoteAddress, EndPoint localAddress);

      
        Task DisconnectAsync(SocketArgEvent argEvent);

        Task CloseAsync(SocketArgEvent argEvent);

        void ExceptionCaught(SocketArgEvent argEvent, Exception exception);

        Task DeregisterAsync(SocketArgEvent argEvent);

        void Read(SocketArgEvent argEvent);

        void UserEventTriggered(SocketArgEvent argEvent, object evt);
    }
}
