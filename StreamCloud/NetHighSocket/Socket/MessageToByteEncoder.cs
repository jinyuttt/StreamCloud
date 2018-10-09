using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

/**
* 命名空间: NetHighSocket 
* 类 名： MessageToByteEncoder
* 版本 ：v1.0
* Copyright (c) year 
*/

namespace NetHighSocket
{
    /// <summary>
    /// 功能描述    ：MessageToByteEncoder  
    /// 创 建 者    ：jinyu
    /// 创建日期    ：2018/10/5 13:26:27 
    /// 最后修改者  ：jinyu
    /// 最后修改日期：2018/10/5 13:26:27 
    /// </summary>
   public abstract class MessageToByteEncoder<T> : ChannelHandlerAdapter
    {
        public override async Task WriteAsync(SocketArgEvent argEvent, object message)
        {
            // return null;
            if (message == null)
            {
                return;
            }
            if (message.GetType() == typeof(byte[]))
            {
                argEvent.chanel.FireChannelWrite(message);
            }
            else
            {
                List<byte> list = new List<byte>();
                var input = (T)message;
                this.Encode(argEvent, input, list);
                if(list.Count>0)
                {
                    argEvent.chanel.FireChannelWrite(list.ToArray());
                }
            }
        }
        protected abstract void Encode(SocketArgEvent argEvent, T message, object output);
    }
   
}
