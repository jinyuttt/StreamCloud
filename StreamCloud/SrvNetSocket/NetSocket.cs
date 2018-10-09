using System;
using System.Collections.Generic;
using System.Text;

namespace SrvNetSocket
{
    public delegate void RecviceNetData<T>(NetChannel<T> netSocket);

   /// <summary>
   /// 传输信息通用包装类
   /// </summary>
   public class NetChannel<T>
    {
        public string localIP;
        public int localPort;
        public string remoteIP;
        public int remotePort;

        /// <summary>
        /// 接收的数据
        /// </summary>
        public T recData;

        /// <summary>
        /// 传输的网络接口
        /// </summary>
        public Object channel;
    }
}
