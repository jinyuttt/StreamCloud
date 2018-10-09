using System;
using System.Collections.Generic;
using System.Text;

namespace NetSocket
{
  public  interface ISocketServer
    {
        bool StartRun(int port);
        bool StartRun(string ip,int port);

        bool StartRun();

        void Close();

        int SendData(NetChannel channel,byte[] data);

        NetChannel GetNetSocket();

        void CallData(RecviceNetData recviceNetData);

    }
}
