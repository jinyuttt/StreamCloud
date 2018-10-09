using NetHighSocket;
using NStStreamCloud.Net;
using SrvNetSocket;
using System;
using System.Collections.Generic;
using System.Text;

/**
* 命名空间: NStStreamCloud 
* 类 名： DataChannelHandler
* 版本 ：v1.0
* Copyright (c) year 
*/

namespace NStStreamCloud
{
    /// <summary>
    /// 功能描述    ：DataChannelHandler  
    /// 创 建 者    ：jinyu
    /// 创建日期    ：2018/10/7 15:46:44 
    /// 最后修改者  ：jinyu
    /// 最后修改日期：2018/10/7 15:46:44 
    /// </summary>
   public class DataChannelHandler<T>: ChannelHandlerAdapter
    {
        HighSocketServer<T> highSocket = null;
        public DataChannelHandler(HighSocketServer<T> server)
        {
            this.highSocket = server;
        }
        public override void ChannelRead(SocketArgEvent argEvent, object message)
        {
            //
           if(highSocket!=null)
            {
                NetChannel<T> netChannel = new NetChannel<T>();
                netChannel.channel = argEvent.chanel;
                netChannel.localIP = argEvent.localIP;
                netChannel.localPort = argEvent.localPort;
                netChannel.remoteIP = argEvent.remoteIP;
                netChannel.remotePort = argEvent.remotePort;
                netChannel.recData =(T) message;
                highSocket.AddData(netChannel);
            }
        }
    }
}
