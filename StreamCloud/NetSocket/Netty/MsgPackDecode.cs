using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using MessagePack;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetSocket.Netty
{
    public class MsgPackDecode : MessageToMessageDecoder<IByteBuffer>
    {
        protected override void Decode(IChannelHandlerContext context, IByteBuffer message, List<object> output)
        {
            byte[] array;
            int length = message.ReadableBytes;
            array = new byte[length];
            message.GetBytes(message.ReaderIndex, array, 0, length);
            output.Add(MessagePackSerializer.Deserialize<Object>(array));
        }
    }
}
