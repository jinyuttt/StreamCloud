using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

/**
* 命名空间: NetHighSocket 
* 类 名： SocketAsyncEventArgsPool
* 版本 ：v1.0
* Copyright (c) year 
*/

namespace NetHighSocket
{
    /// <summary>
    /// 功能描述    ：SocketAsyncEventArgsPool  
    /// 创 建 者    ：jinyu
    /// 创建日期    ：2018/10/6 0:50:39 
    /// 最后修改者  ：jinyu
    /// 最后修改日期：2018/10/6 0:50:39 
    /// </summary>
    class SocketAsyncEventArgsPool
    {
        private Stack<SocketAsyncEventArgs> m_pool;

        public SocketAsyncEventArgsPool(int capacity)
        {
            m_pool = new Stack<SocketAsyncEventArgs>(capacity);
        }

        public void Push(SocketAsyncEventArgs item)
        {
            if (item == null) { throw new ArgumentException("Items added to a SocketAsyncEventArgsPool cannot be null"); }
            lock (m_pool)
            {
                m_pool.Push(item);
            }
        }

        public SocketAsyncEventArgs Pop()
        {
            lock (m_pool)
            {
                return m_pool.Pop();
            }
        }

        public int Count
        {
            get { return m_pool.Count; }
        } 

        public void Close()
        {
            m_pool.Clear();
            m_pool = null;
        }
    }
}
