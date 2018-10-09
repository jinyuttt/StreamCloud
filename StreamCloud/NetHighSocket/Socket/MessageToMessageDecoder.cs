using System;
using System.Collections.Generic;
using System.Text;

/**
* 命名空间: NetHighSocket 
* 类 名： MessageToMessageDecoder
* 版本 ：v1.0
* Copyright (c) year 
*/

namespace NetHighSocket
{
    /// <summary>
    /// 功能描述    ：MessageToMessageDecoder  
    /// 创 建 者    ：jinyu
    /// 创建日期    ：2018/10/5 13:24:30 
    /// 最后修改者  ：jinyu
    /// 最后修改日期：2018/10/5 13:24:30 
    /// </summary>
   public abstract class MessageToMessageDecoder<T>:ChannelHandlerAdapter
    {
        public override void ChannelRead(SocketArgEvent argEvent, object message)
        {
            if(message.GetType()==typeof(byte[]))
            {
                var intput = (T)message;
                List<object> output = new List<object>(10);
                this.Decode(argEvent, intput, output);
                int size = output.Count;
                for (int i = 0; i < size; i++)
                {
                    argEvent.chanel.FireChannelRead(output[i]);
                }
            }
        }
        protected internal abstract void Decode(SocketArgEvent context, T message, List<object> output);
    }
}
