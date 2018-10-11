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
   internal class SocketPool<T>
    {
        private Stack<ISocketClient<T>> buffer=new Stack<ISocketClient<T>>(100);
        private object lock_obj = new object();

        /// <summary>
        /// 服务器地址
        /// 格式：host:port
        /// </summary>
        private  List<string>  ServerList
        {
            get;set;
        }

        /// <summary>
        /// 初始化缓存对象
        /// </summary>
        /// <param name="initNum"></param>
        public void InitPool(int initNum=10)
        {
            if(initNum>0)
            {
                for(int i=0;i<initNum;i++)
                {
                    buffer.Push(Create());
                }
            }
        }


        /// <summary>
        /// 创建通信对象
        /// </summary>
        /// <returns></returns>
        private ISocketClient<T> Create()
        {
            return null;
        }

        /// <summary>
        /// 获取通信对象
        /// </summary>
        /// <returns></returns>
        public ISocketClient<T> GetSocket()
        {
            lock (lock_obj)
            {
                try
                {
                    return buffer.Pop();
                }
                catch
                {
                    return Create();
                }
            }
        }

       /// <summary>
       /// 通信对象释放
       /// </summary>
       /// <param name="client"></param>
        public void Free(ISocketClient<T> client)
        {
            lock(lock_obj)
            {
                buffer.Push(client);
            }
        }
    }
}
