using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace StreamCloud.Srv
{

    /// <summary>
    /// 服务组件信息
    /// </summary>
   public class SrvPath
    {
        public string SrvName { get; set; }
        public string ClsName { get; set; }

        public Type SrvType { get; set; }

        public string DLLPath { get; set; }

         public object SrvInstance { get; set; }

         public Assembly SrvAssembly { get; set; }

        public Dictionary<string,string> SrvMethod { get; set; }

        public Dictionary<string,MethodInfo> SrvMethodInfo { get; set; }


    }
}
