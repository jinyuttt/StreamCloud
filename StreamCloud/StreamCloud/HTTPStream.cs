using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

/**
* 命名空间: NStStreamCloud 
* 类 名： HTTPStream
* 版本 ：v1.0
* Copyright (c) year 
*/

namespace NStStreamCloud
{
    /// <summary>
    /// 功能描述    ：HTTPStream  
    /// 创 建 者    ：jinyu
    /// 创建日期    ：2018/10/8 2:31:43 
    /// 最后修改者  ：jinyu
    /// 最后修改日期：2018/10/8 2:31:43 
    /// </summary>
   public class HTTPStream
    {
        private static  HTTPStream instance = null;
        private static  object lock_obj = new object();
        private  Stack<StreamBuffer> streams = null;
        private  Stack<StreamBuffer> largeStream = null;
        private int bufferSize = 1024 * 1024;//1M
        private Dictionary<MemoryStream, StreamBuffer> dic_Use = null;
        private int id = 0;
        /// <summary>
        /// 大缓存流5倍
        /// </summary>
        private int MaxBufSize { get; set; }

        /// <summary>
        /// 默认大小
        /// </summary>
        public int BufferSize { get { return bufferSize; } set { bufferSize = value;MaxBufSize = 3 * bufferSize; } }

        public static HTTPStream GetInstance()
        {
            if (instance == null)
            {
                lock (lock_obj)
                {
                    if (instance == null)
                    {
                        instance = new HTTPStream();
                    }
                }
            }
            return instance;
        }

        /// <summary>
        /// 构造
        /// </summary>
        private HTTPStream()
        {
            streams = new Stack<StreamBuffer>(100);
            largeStream = new Stack<StreamBuffer>(100);
            MaxBufSize = 3 * bufferSize;
            dic_Use = new Dictionary<MemoryStream, StreamBuffer>();
            InitBuffer();

        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void InitBuffer()
        {
            for (int i = 0; i < 10; i++)
            {
                streams.Push(Create());
            }
        }

        /// <summary>
        /// 创建流缓存
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public StreamBuffer Create(int size = 0)
        {
            if (size < 1)
            {
                size = bufferSize;
            }
            StreamBuffer byteBuffer = new StreamBuffer() { DateTime = DateTime.Now, Capacity=size, Buffer = new MemoryStream(size), ID = Interlocked.Increment(ref id) };
            return byteBuffer;
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <returns></returns>
        public StreamBuffer GetStream()
        {
            lock (lock_obj)
            {
                StreamBuffer stream= streams.Pop();
                dic_Use[stream.Buffer] = stream;
                return stream;
            }
        }

        /// <summary>
        /// 获取大内存流
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public StreamBuffer GetLargeStream(int size=0)
        {
            StreamBuffer stream = null;
            lock (largeStream)
            {
                try
                {
                    stream = largeStream.Pop();
                }
                catch
                {
                    stream = Create(MaxBufSize);
                }
            }
            if(size>stream.Capacity)
            {
                stream.ResetCapacity(size);
            }
            dic_Use[stream.Buffer] = stream;
            return stream;
        }

        /// <summary>
        /// 获取不同的缓存流
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public StreamBuffer GetStreamCompare(int size)
        {
            if (size < bufferSize)
            {
                return GetStream();
            }
            else
            {
                return GetLargeStream();
            }
        }

        public StreamBuffer GetStreamBuffer(Stream stream)
        {
            StreamBuffer buffer = null;
             dic_Use.TryGetValue((MemoryStream)stream, out buffer);
            return buffer;
        }

        /// <summary>
        /// 回收内存
        /// </summary>
        /// <param name="memory"></param>
        public void FreeStream(Stream memory)
        {
            lock (lock_obj)
            {
                StreamBuffer buffer = null;
                MemoryStream stream = (MemoryStream)memory;
                if (dic_Use.TryGetValue(stream, out buffer))
                {
                    FreeBuffer(buffer);
                    dic_Use.Remove(stream);
                }
            }
        }
        /// <summary>
        /// 释放缓存
        /// </summary>
        /// <param name="buffer"></param>
        public void FreeBuffer(StreamBuffer buffer)
        {
            lock (lock_obj)
            {
                buffer.ResetPostion();
                buffer.ResetSize();
                //小于大值的都放在默认里面
                if (buffer.Capacity < MaxBufSize)
                {
                    streams.Push(buffer);
                }
                else
                {
                    largeStream.Push(buffer);
                }
            }
        }

        /// <summary>
        /// 释放缓存
        /// </summary>
        /// <param name="buffers"></param>
        public void FreeBuffers(List<StreamBuffer> buffers)
        {
           
                for (int i = 0; i < buffers.Count; i++)
                {
                    FreeBuffer(buffers[i]);
                }
            
        }
    }
}
