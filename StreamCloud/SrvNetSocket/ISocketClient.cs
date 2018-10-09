using System;
using System.Collections.Generic;
using System.Text;

namespace SrvNetSocket
{
  public  interface ISocketClient<TData>
    {
        bool Connect();
        bool Connect(string ip, int port);
        int SendData(byte[] data);

        int SendData(object data);

        int SendDataUDP(string ip, int port, byte[] data);

        int SendDataUDP(string ip, int port, object data);

        NetChannel<TData> GetNetSocket();

        void CallData(RecviceNetData<TData> recviceNetData);

        void Close();

    }
}
