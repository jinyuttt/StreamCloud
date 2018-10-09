using System;
using System.Collections.Generic;
using System.Text;

namespace StreamCloud.Srv
{

    /// <summary>
    /// 服务配置
    /// </summary>
   public static class SrvConfig
    {

        /// <summary>
        /// 服务IP
        /// </summary>
        public static string SrvHost { get; set; }

        /// <summary>
        /// 服务端口
        /// </summary>
        public static int SrvPort { get; set; }

        /// <summary>
        /// 服务目录
        /// </summary>
        public static  string SrvDir { get; set; }

        /// <summary>
        /// 传输组件名称
        /// </summary>
        public static string NetComponent { get; set; }

        /// <summary>
        /// 传输组件目录
        /// </summary>
        public static string NetDir { get; set; }
    }
}
