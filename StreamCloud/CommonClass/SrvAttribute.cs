using System;

namespace CommonClass
{
    [AttributeUsage(AttributeTargets.Class| AttributeTargets.Method|AttributeTargets.Interface, Inherited = true, AllowMultiple = false)]
    public class SrvAttribute:Attribute
    {
        private string srvName = "";
        public SrvAttribute(string name)
        {
            this.srvName = name;
        }
        public string  SrvName
        {
            get { return srvName; }
        }
    }
}
