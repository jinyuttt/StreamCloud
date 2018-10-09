using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace StreamCloud.Srv
{
    class ProxyObject : MarshalByRefObject {
        Assembly assembly = null;
        public void LoadAssembly()
        {
            assembly = Assembly.LoadFile(@"TestDLL.dll");
        }
        public bool Invoke(string fullClassName, string methodName, params Object[] args)
        {
            if (assembly == null) return false;
            Type tp = assembly.GetType(fullClassName);
            if (tp == null) return false;
            MethodInfo method = tp.GetMethod(methodName);
            if (method == null) return false;
            Object obj = Activator.CreateInstance(tp);
            method.Invoke(obj, args);
            return true;
        }
    } 

}
