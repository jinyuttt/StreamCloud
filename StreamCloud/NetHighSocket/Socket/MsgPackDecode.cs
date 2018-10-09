using System.Collections.Generic;

namespace NetHighSocket
{
    public class MessageDecode : MessageToMessageDecoder<byte[]>
    {
        protected internal override void Decode(SocketArgEvent context, byte[] message, List<object> output)
        {
            output.Add(message);
        }
    }
}
