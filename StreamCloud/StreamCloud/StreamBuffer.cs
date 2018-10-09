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
    /// 创建日期    ：2018/10/8 3:09:10 
    /// 最后修改者  ：jinyu
    /// 最后修改日期：2018/10/8 3:09:10 
    /// </summary>
   public class StreamBuffer:CacheBuffer<MemoryStream>
    {

        public override void Reset(int postion = 0, int size = 0, int capacity = -1)
        {
            base.Reset(postion, size, capacity);
            this.Buffer.Capacity = capacity;
            this.Buffer.Seek(postion, SeekOrigin.Begin);
        }
        public override void ResetPostion(int postion = 0)
        {
            base.ResetPostion(postion);
            this.Buffer.Seek(postion, SeekOrigin.Begin);
        }
        public override void ResetCapacity(int capacity = -1)
        {
            base.ResetCapacity(capacity);
            this.Buffer.Capacity = capacity;
        }
        public override void ResetSize(int size = 0)
        {
            base.ResetSize(size);
            this.Buffer.SetLength(size);
        }
    }
}
