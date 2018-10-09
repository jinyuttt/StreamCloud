using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using MessagePack;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetSocket.Netty
{
    public class MsgPackEcode : MessageToByteEncoder<Object>
    {
        protected override void Encode(IChannelHandlerContext context, object message, IByteBuffer output)
        {
            byte[] temp = MessagePackSerializer.Serialize(message);
            output.WriteBytes(temp);
        }
    }
}
