using System;
using System.Collections.Generic;
using System.Text;

namespace NetSocket
{
  public  interface ISocketClient
    {
        bool Connect();
        bool Connect(string ip, int port);
        int SendData(byte[] data);
        int SendDataUDP(string ip, int port, byte[] data);

        NetChannel GetNetSocket();

        void CallData(RecviceNetData recviceNetData);

        void Close();

    }
}
