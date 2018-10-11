using System;
using System.Collections.Generic;
using System.Text;

/**
* 命名空间: StreamCloud 
* 类 名： SrvRquest
* 版本 ：v1.0
* Copyright (c) year 
*/

namespace CommonClass
{
    /// <summary>
    /// 功能描述    ：SrvRquest  请求参数
    /// 创 建 者    ：jinyu
    /// 创建日期    ：2018/10/2 23:21:54 
    /// 最后修改者  ：jinyu
    /// 最后修改日期：2018/10/2 23:21:54 
    /// </summary>
   public class SrvRquest
    {
        public string SrvName { get; set; }
        public string SrvMethod { get; set; }

        public List<SrvParam> SrvParam { get; set; }
    }
}
