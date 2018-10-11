using System;
using System.Collections.Generic;
using System.Text;

/**
* 命名空间: StreamCloud 
* 类 名： SrvParam
* 版本 ：v1.0
* Copyright (c) year 
*/

namespace CommonClass
{
    /// <summary>
    /// 功能描述    ：SrvParam  
    /// 创 建 者    ：jinyu
    /// 创建日期    ：2018/10/2 23:24:07 
    /// 最后修改者  ：jinyu
    /// 最后修改日期：2018/10/2 23:24:07 
    /// </summary>
   public class SrvParam
    {
        public string ParamName { get; set; }
        /// <summary>
        /// 参数类型名称
        /// </summary>
        public string ParamType { get; set; }

        /// <summary>
        /// 暂时无用
        /// </summary>
        public string ParamValue { get; set; }

        /// <summary>
        /// 参数值
        /// </summary>
        public  object ParamObj { get; set; }
    }
}
