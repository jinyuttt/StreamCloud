using System;
using System.Collections.Generic;
using System.Text;

/**
* 命名空间: NetHighSocket 
* 类 名： SockOption
* 版本 ：v1.0
* Copyright (c) year 
*/

namespace NetHighSocket
{
    /// <summary>
    /// 功能描述    ：SockOption  
    /// 创 建 者    ：jinyu
    /// 创建日期    ：2018/10/5 12:51:38 
    /// 最后修改者  ：jinyu
    /// 最后修改日期：2018/10/5 12:51:38 
    /// </summary>
    public enum SockOption
    {
        /// <summary>
        /// socket接收大小，默认64K
        /// </summary>
        recBufSize,

        /// <summary>
        /// socket发送大小，默认64K
        /// </summary>
        sendBufSize,

        /// <summary>
        /// 数据接收缓存大小
        /// </summary>
        recDataSize,

        /// <summary>
        /// 异步发送缓存大小
        /// </summary>
        sendDataSize,

        /// <summary>
        /// ttl大小
        /// </summary>
        ttl,

        /// <summary>
        /// 地址重用
        /// </summary>
        reuse,

        /// <summary>
        /// 连接超时
        /// </summary>
        conTimeOut,

        /// <summary>
        /// 发送超时
        /// </summary>
        sendTimeout,

        /// <summary>
        /// 接收超时
        /// </summary>
        receiveTimeout,

        /// <summary>
        /// 同时接收客户端连接数
        /// </summary>
        initAcepptNum,
        keepAlive,


    }
}
