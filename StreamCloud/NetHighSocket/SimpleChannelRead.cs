using System;
using System.Collections.Generic;
using System.Text;

/**
* 命名空间: NetHighSocket 
* 类 名： SimpleChannelRead
* 版本 ：v1.0
* Copyright (c) year 
*/

namespace NetHighSocket
{
    /// <summary>
    /// 功能描述    ：SimpleChannelRead  
    /// 创 建 者    ：jinyu
    /// 创建日期    ：2018/10/9 20:40:49 
    /// 最后修改者  ：jinyu
    /// 最后修改日期：2018/10/9 20:40:49 
    /// </summary>
   public class SimpleChannelRead:ChannelHandlerAdapter
    {
        public override void ChannelRead(SocketArgEvent argEvent, object message)
        {
            //base.ChannelRead(argEvent, message);
            //接收的数据
            byte[] data = (byte[])message;
        }
    }
}
