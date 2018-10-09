using System;
using System.Collections.Generic;
using System.Text;

/**
* 命名空间: NetHighSocket 
* 类 名： ByteBuffer
* 版本 ：v1.0
* Copyright (c) year 
*/

namespace NetHighSocket
{
    /// <summary>
    /// 功能描述    ：ByteBuffer  
    /// 创 建 者    ：jinyu
    /// 创建日期    ：2018/10/6 3:01:26 
    /// 最后修改者  ：jinyu
    /// 最后修改日期：2018/10/6 3:01:26 
    /// </summary>
   public class ByteBuffer
    {

        /// <summary>
        /// 数据存储区
        /// </summary>
        public byte[] buffer;

        /// <summary>
        /// 使用时间
        /// </summary>
        public DateTime DateTime=DateTime.Now;

        /// <summary>
        /// 数据长度
        /// </summary>
        public int Size = 0;

        /// <summary>
        /// 数据使用位置
        /// </summary>
        public int Position = 0;

        /// <summary>
        /// 重置为空
        /// </summary>
        public void Reset(int position=0,int size=0)
        {
            Size = size;
            Position = position;
        }
    }
}
