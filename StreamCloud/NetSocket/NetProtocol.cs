using System;
using System.Collections.Generic;
using System.Text;

namespace NetSocket
{
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
