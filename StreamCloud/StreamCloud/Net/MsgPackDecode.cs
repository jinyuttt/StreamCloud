
using MessagePack;
using NetHighSocket;
using System;
using System.Collections.Generic;
using System.Text;

namespace NStStreamCloud.Net
{
    public class MsgPackDecode : MessageToMessageDecoder<byte[]>
    {
        protected override void Decode(SocketArgEvent context, byte[] message, List<object> output)
        {
            if(HttpQuery.IsHttp(message, 0, message.Length))
            {
                //是http请求
                 output.Add(message);
            }
            else
            {
               object obj= MessagePackSerializer.Deserialize<object>(message);
                output.Add(obj);
            }
        }

        
    }
}
