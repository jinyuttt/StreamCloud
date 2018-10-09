using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

/**
* 命名空间: NStStreamCloud 
* 类 名： StreamBuffer
* 版本 ：v1.0
* Copyright (c) year 
*/

namespace NStStreamCloud
{
    /// <summary>
    /// 功能描述    ：StreamBuffer  
    /// 创 建 者    ：jinyu
    /// 创建日期    ：2018/10/8 12:55:17 
    /// 最后修改者  ：jinyu
    /// 最后修改日期：2018/10/8 12:55:17 
    /// </summary>
   public class CacheBuffer<T>
    {
        
        public int ID { get; set; }
        public DateTime DateTime { get; set; }
        public T Buffer { get; set; }
        public int Capacity { get; internal set; }

        public int Size { get; set; }

        public int Position { get; set; }

        public virtual void Reset(int postion=0,int size=0,int capacity=-1)
        {
            Position = postion;
            Size = size;
            Capacity = capacity;
        }
        public virtual void ResetPostion(int postion = 0)
        {
            Position = postion;
        }
        public virtual void ResetSize(int size = 0)
        {
            Size = size;
        }
        public virtual void ResetCapacity(int capacity = -1)
        {
            Capacity = capacity;
        }
    }
}
