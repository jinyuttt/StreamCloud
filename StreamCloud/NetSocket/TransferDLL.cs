using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Concurrent;
using System.IO;
using System.Reflection;

namespace NetSocket
{
   public class TransferDLL
    {
        public ConcurrentDictionary<string, Type> dic_NetDLL = new ConcurrentDictionary<string, Type>();
        public List<string> loadDlls = new List<string>();
        public TransferDLL()
        {
            LoadDll();
        }
        /// <summary>
        /// 获取类
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Type GetClass(string name)
        {
            if(dic_NetDLL.TryGetValue(name, out Type value))
            {
                return value;
            }
            else
            {
                LoadDll();
                dic_NetDLL.TryGetValue(name, out  value);
                return value;

            }
        }
        private  void GetTypes(Assembly assembly)
        {
          
            Type[] types = assembly.GetTypes();
            foreach (Type type in types)
            {
                if (typeof(ISocketClient).IsAssignableFrom(type) || typeof(ISocketServer).IsAssignableFrom(type))
                {
                    NetProtocol netProtocol = type.GetCustomAttribute<NetProtocol>();
                    if (netProtocol != null)
                    {
                        dic_NetDLL[netProtocol.NetProtocolName] = type;
                    }
                }
            }
        }
        private void LoadDll()
        {
            //
           Assembly curassembly= this.GetType().Assembly;
            GetTypes(curassembly);
            string path = TransferConfig.TransferDLLDir;
            if(!Directory.Exists(path))
            {
                Console.WriteLine("没有配置的" + path);
                return;
            }
            string[] files = Directory.GetFiles(path);
            if(files==null||files.Length==0)
            {
                return;
            }
            foreach (string file in files)
            {
                if(loadDlls.Contains(file))
                {
                    continue;
                }
                loadDlls.Add(file);
                Assembly assembly = Assembly.LoadFrom(file);
                GetTypes(assembly);
            }
        }
    }
}
