using CacheBuffer;
using SrvNetSocket;
using System;
using System.Collections.Generic;
using System.Text;

/**
* 命名空间: CommonClass 
* 类 名： SocketBuffer
* 版本 ：v1.0
* Copyright (c) year 
*/

namespace CommonClass
{
    /// <summary>
    /// 功能描述    ：SocketBuffer   
    /// 创 建 者    ：jinyu
    /// 创建日期    ：2018/10/12 17:46:18 
    /// 最后修改者  ：jinyu
    /// 最后修改日期：2018/10/12 17:46:18 
    /// </summary>
   public class SocketBuffer<T> : BaseBuffer<ISocketClient<T>>
    {
        public SocketBuffer()
        {
            this.DateTime = DateTime.Now;
        }
        public override void Dispose()
        {
            this.Data.Close();
        }
    }
}
