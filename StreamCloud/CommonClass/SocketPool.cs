using CacheBuffer;
using LoadBalance;
using SrvNetSocket;
using System;
using System.Collections.Generic;
using System.Text;

/**
* 命名空间: CommonClass 
* 类 名： SocketManager
* 版本 ：v1.0
* Copyright (c) year 
*/

namespace CommonClass
{
    /// <summary>
    /// 功能描述    ：SocketManager  
    /// 创 建 者    ：jinyu
    /// 创建日期    ：2018/10/11 20:58:38 
    /// 最后修改者  ：jinyu
    /// 最后修改日期：2018/10/11 20:58:38 
    /// </summary>
   internal class SocketPool<T> : BufferPool<ISocketClient<T>>
    {

        private HashRingNode hashSrv = new HashRingNode();
        private List<string> lstSrv = null;

        public string NetName { get; set; }
        /// <summary>
        /// 服务器地址
        /// 格式：host:port
        /// </summary>
        private  List<string>  ServerList
        {
            get { return lstSrv; }
            set { lstSrv = value; hashSrv.AddNode(lstSrv.ToArray()); }
        }
        public override BaseBuffer<ISocketClient<T>> Create()
        {
            SocketBuffer<T> buffer = new SocketBuffer<T>();
            ISocketClient<T> client = NetFactory.CreateSocketClient<T>(this.NetName);
            if(lstSrv != null)
            {
                string srv= hashSrv.GetCurrent();
                string[] srvAddrs = srv.Split(':');
                if(srvAddrs.Length==2)
                {
                    client.Connect(srvAddrs[0], int.Parse(srvAddrs[1]));
                }
            }
            buffer.Data = client;
            return buffer;
        }

    }
}
