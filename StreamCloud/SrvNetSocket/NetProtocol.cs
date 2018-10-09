using System;

namespace SrvNetSocket
{

   /// <summary>
   /// 传输协议类型
   /// 管理名称
   /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
   public class NetProtocol:Attribute
    {
        private string protocolName;
        public NetProtocol(string name)
        {
            this.protocolName = name;
        }
        public string NetProtocolName
        {
            get { return protocolName; }
        }
    }
}
