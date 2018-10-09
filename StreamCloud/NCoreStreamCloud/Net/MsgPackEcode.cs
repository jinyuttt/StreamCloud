using MessagePack;
using NetHighSocket;
using System;
using System.Collections.Generic;

namespace NStStreamCloud.Net
{
    public class MsgPackEcode : MessageToByteEncoder<Object>
    {
        protected override void Encode(SocketArgEvent argEvent, object message, object output)
        {
           byte[] data= MessagePackSerializer.Serialize<object>(message);
            var list = output as List<byte[]>;
            list.Add(data);

        }
    }
}
