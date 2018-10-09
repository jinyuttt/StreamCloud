using System;
using System.Collections.Generic;

namespace NetHighSocket
{
    public class MessageEcode : MessageToByteEncoder<Object>
    {
        protected override void Encode(SocketArgEvent argEvent, object message, object output)
        {
            List<byte[]> list = output as List<byte[]>;
            list.Add((byte[])message);
        }
    }
}
