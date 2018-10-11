using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

/**
* 命名空间: CommonClass 
* 类 名： QuestResult
* 版本 ：v1.0
* Copyright (c) year 
*/

namespace CommonClass
{
    /// <summary>
    /// 功能描述    ：QuestResult  
    /// 创 建 者    ：jinyu
    /// 创建日期    ：2018/10/11 19:21:27 
    /// 最后修改者  ：jinyu
    /// 最后修改日期：2018/10/11 19:21:27 
    /// </summary>
  public  class QuestResult
    {

        /// <summary>
        /// 返回值
        /// </summary>
        public object Result { get; set; }

       /// <summary>
       /// 结果编码
       /// </summary>
        public ErrorCode Error { get; set; }

       /// <summary>
       /// 结果编码描述
       /// </summary>
        public string ErrorMsg { get; set; }

        /// <summary>
        /// 结果附近信息
        /// 主要是异常信息或者其它描述
        /// 例如：结果被截取
        /// </summary>
        public string ReslutMsg { get; set; }


    }
    public enum ErrorCode
    {

        /// <summary>
        /// 成功
        /// </summary>
        /// 
        [Description("执行成功")]
        Sucess,

        /// <summary>
        ///执行超时
        /// </summary>
        /// 
        [Description("执行超时")]
        TimeOut,

        /// <summary>
        /// 执行异常
        /// </summary>
        /// 
        [Description("执行异常")]
        Exception,

        /// <summary>
        /// 结果被截取
        /// </summary>
        /// 
        [Description("结果被截取")]
        Truncate,

    }
}
