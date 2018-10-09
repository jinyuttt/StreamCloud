using System;
using System.Collections.Generic;
using System.Text;

/**
* 命名空间: NetHighSocket 
* 类 名： BufferManager
* 版本 ：v1.0
* Copyright (c) year 
*/

namespace NetHighSocket
{
    /// <summary>
    /// 功能描述    ：BufferManager  
    /// 创 建 者    ：jinyu
    /// 创建日期    ：2018/10/6 2:53:26 
    /// 最后修改者  ：jinyu
    /// 最后修改日期：2018/10/6 2:53:26 
    /// </summary>
  public  class BufferManager
    {
        private static BufferManager instance = null;
        private static object lock_obj = new object();
        private static Stack<ByteBuffer> byteBuffers = null;
        private int bufferSize = 1024 * 100;
        public int BufferSize { get { return bufferSize; } set { bufferSize = value; } }
        public static BufferManager GetInstance()
        {
            if(instance == null)
            {
                lock(lock_obj)
                {
                    if(instance == null)
                    {
                        instance = new BufferManager();
                    }
                }
            }
            return instance;
        }
        private BufferManager()
        {
            byteBuffers = new Stack<ByteBuffer>(100);
            InitBuffer();
        }
        private void InitBuffer()
        {
            for (int i = 0; i < 100; i++)
            {
                byteBuffers.Push(Create());
            }
        }
        public ByteBuffer Create()
        {
            ByteBuffer byteBuffer = new ByteBuffer() { buffer = new byte[bufferSize] };
            return byteBuffer;
        }

        /// <summary>
        /// 获取指定个数缓存数组
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public List<ByteBuffer>GetBufferNum(int num=1)
        {
            List<ByteBuffer> list = new List<ByteBuffer>(num);
            lock (lock_obj)
            {
                ByteBuffer cur = null;
                for (int i = 0; i < num; i++)
                {

                    try
                    {
                         cur = byteBuffers.Pop();
                        if (cur == null)
                        {
                            cur = Create();
                        }
                    }
                    catch
                    {
                        cur = Create();
                    }

                    list.Add(cur);
                }
            }
            return list;
        }

        /// <summary>
        /// 获取大小容量的缓存数组
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public List<ByteBuffer> GetBufferSize(int size)
        {
            int num = size / bufferSize;
            num = size % bufferSize == 0 ? num : num + 1;
            return GetBufferNum(num);
        }

        /// <summary>
        /// 释放缓存
        /// </summary>
        /// <param name="buffer"></param>
        public void FreeBuffer(ByteBuffer buffer)
        {
            lock (lock_obj)
            {
                buffer.Reset();//重置
                byteBuffers.Push(buffer);
            }
        }

        /// <summary>
        /// 释放缓存
        /// </summary>
        /// <param name="buffers"></param>
        public void FreeBuffers(List<ByteBuffer> buffers)
        {
            lock (lock_obj)
            {
                for (int i = 0; i < buffers.Count; i++)
                {
                    buffers[i].Reset();
                    byteBuffers.Push(buffers[i]);
                }
            }
        }
    }
}
