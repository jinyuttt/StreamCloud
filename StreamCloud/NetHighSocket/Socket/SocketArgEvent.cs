using System;
using System.Collections.Generic;
using System.Text;

/**
* 命名空间: NetHighSocket 
* 类 名： SocketArgEvent
* 版本 ：v1.0
* Copyright (c) year 
*/

namespace NetHighSocket
{
    /// <summary>
    /// 功能描述    ：SocketArgEvent  
    /// 创 建 者    ：jinyu
    /// 创建日期    ：2018/10/4 16:39:40 
    /// 最后修改者  ：jinyu
    /// 最后修改日期：2018/10/4 16:39:40 
    /// </summary>
  public  class SocketArgEvent
    {
        public string localIP;
        public int localPort;
        public string remoteIP;
        public int remotePort;
        public byte[] data;
        /// <summary>
        /// TCP时的通信
        /// </summary>
        public ISocketChannel chanel;

    }
}
