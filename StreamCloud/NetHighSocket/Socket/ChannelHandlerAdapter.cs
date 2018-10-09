using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

/**
* 命名空间: NetHighSocket 
* 类 名： ChannelHandlerAdapter
* 版本 ：v1.0
* Copyright (c) year 
*/

namespace NetHighSocket
{
    /// <summary>
    /// 功能描述    ：ChannelHandlerAdapter  
    /// 创 建 者    ：jinyu
    /// 创建日期    ：2018/10/5 3:23:09 
    /// 最后修改者  ：jinyu
    /// 最后修改日期：2018/10/5 3:23:09 
    /// </summary>
   public class ChannelHandlerAdapter : IChannelHandler
    {
        public virtual async Task BindAsync(SocketArgEvent argEvent, EndPoint localAddress)
        {
           await  argEvent.chanel.BindAsync(localAddress);
        }

        public virtual void ChannelActive(SocketArgEvent argEvent)
        {
            argEvent.chanel.FireChannelActive();
        }

        public virtual void ChannelActive(SocketAsyncEventArgs e)
        {
           
        }

        public virtual void ChannelInactive(SocketArgEvent argEvent)
        {
            argEvent.chanel.FireChannelInActive();
        }

        public virtual void ChannelRead(SocketArgEvent argEvent, object message)
        {
            argEvent.chanel.FireChannelRead(message);
        }

        public virtual void ChannelReadComplete(SocketArgEvent argEvent)
        {
            argEvent.chanel.FireChannelReadComplete();
        }

        public virtual void ChannelWritabilityChanged(SocketArgEvent argEvent)
        {
            argEvent.chanel.FireChannelWritabilityChanged();
        }

        public virtual async Task CloseAsync(SocketArgEvent argEvent)
        {
           await  argEvent.chanel.CloseAsync();
        }

        public virtual async Task ConnectAsync(SocketArgEvent argEvent, EndPoint remoteAddress, EndPoint localAddress)
        {
            await argEvent.chanel.ConnectAsync(remoteAddress, localAddress);
        }

        public virtual async Task DeregisterAsync(SocketArgEvent argEvent)
        {
             
        }

        public virtual async Task DisconnectAsync(SocketArgEvent argEvent)
        {
             argEvent.chanel.Close();
        }

        public virtual void ExceptionCaught(SocketArgEvent argEvent, Exception exception)
        {
            argEvent.chanel.FireExceptionCaught(exception);
        }

        public virtual void Flush(SocketArgEvent argEvent)
        {
           
        }

        public virtual void HandlerAdded(SocketArgEvent argEvent)
        {
           
        }

        public virtual void HandlerRemoved(SocketArgEvent argEvent)
        {
           
        }

        public virtual void Read(SocketArgEvent argEvent)
        {
            argEvent.chanel.FireChannelReadComplete();
        }

        public virtual void UserEventTriggered(SocketArgEvent argEvent, object evt)
        {
           
        }

        public virtual async Task WriteAsync(SocketArgEvent argEvent, object message)
        {
           await  argEvent.chanel.WriteAndFlushAsync(message);
        }
    }
}
