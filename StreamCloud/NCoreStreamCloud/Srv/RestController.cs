using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;
using System.Reflection;
using CommonClass;

namespace StreamCloud.Srv
{
   public class RestController
    {
        private ReaderWriterLock readerWriter = new ReaderWriterLock();
        private List<string> loadDLL = new List<string>();
        private Dictionary<string, SrvPath> dicSrv = new Dictionary<string, SrvPath>();
        private int lockTime = 1000;
       
        /// <summary>
        /// 加载服务组件
        /// </summary>
        public void LoadSrv()
        {
            if(string.IsNullOrEmpty(SrvConfig.SrvDir))
            {
                SrvConfig.SrvDir = "SrvDLL";
            }
            if(!Directory.Exists(SrvConfig.SrvDir))
            {
                Directory.CreateDirectory(SrvConfig.SrvDir);
            }
            string[] srvDLLs = Directory.GetFiles(SrvConfig.SrvDir);
            foreach(string file in srvDLLs)
            {
                if(loadDLL.Contains(file))
                {
                    continue;
                }
                else
                {
                    loadDLL.Add(file);
                    Assembly assembly=  Assembly.LoadFile(file);
                    Type[] allTypes= assembly.GetTypes();
                    foreach(Type type in allTypes)
                    {
                       if(type.IsClass)
                        {

                            SrvAttribute srvAttribute = type.GetCustomAttribute<SrvAttribute>();
                            if(srvAttribute!=null)
                            {
                                //
                                SrvPath srvPath= new SrvPath();
                                srvPath.SrvName = srvAttribute.SrvName;
                                srvPath.ClsName = type.FullName;
                                srvPath.DLLPath = file;
                                srvPath.SrvAssembly = assembly;
                                srvPath.SrvType = type;
                                srvPath.SrvMethod = new Dictionary<string, string>();
                                srvPath.SrvMethodInfo = new Dictionary<string, MethodInfo>();
                                dicSrv[srvPath.SrvName] = srvPath;
                                MethodInfo[] methodInfos = type.GetMethods();
                                foreach(MethodInfo info in methodInfos)
                                {
                                    SrvAttribute srvMethod= info.GetCustomAttribute<SrvAttribute>();
                                    if(srvMethod!=null)
                                    {
                                        srvPath.SrvMethod[srvMethod.SrvName] = info.Name;
                                        srvPath.SrvMethodInfo[srvMethod.SrvName] = info;
                                    }
                                }
                            }
                        }
                    }

                }
            }
        }
       
       /// <summary>
       /// 调服务
       /// </summary>
       /// <param name="srvRquest"></param>
       /// <returns></returns>

        public  object SrvInvoke(SrvRquest srvRquest)
        {
            SrvPath srvPath = null;
            if(dicSrv.TryGetValue(srvRquest.SrvName,out srvPath))
            {
                return FastMethodInvoke(srvRquest);
            }
            else
            {
                LoadSrv();
                //
                return FastMethodInvoke(srvRquest);
            }
        }

        private object FastMethodInvoke(SrvRquest srvRquest)
        {
            SrvPath srvPath = null;
            if (dicSrv.TryGetValue(srvRquest.SrvName, out srvPath))
            {
                MethodInfo info = null;
                if (srvPath.SrvMethodInfo.TryGetValue(srvRquest.SrvMethod, out info))
                {
                    if (srvPath.SrvInstance == null)
                    {
                        srvPath.SrvInstance = Activator.CreateInstance(srvPath.SrvType);
                    }
                    //
                    List<object> list = new List<object>();
                    foreach (SrvParam param in srvRquest.SrvParam)
                    {
                        list.Add(param.ParamObj);
                    }
                    FastInvoke.FastInvokeHandler fastInvoker = FastInvoke.GetMethodInvoker(info);
                    object result = fastInvoker.Invoke(srvPath, list.ToArray());
                    return result;
                }
                return null;
            }
            return null;
        }

        /// <summary>
        /// 内部执行
        /// </summary>
        /// <param name="srvRquest"></param>
        /// <returns></returns>
        private object MethodInvoke(SrvRquest srvRquest)
        {
            SrvPath srvPath = null;
            if (dicSrv.TryGetValue(srvRquest.SrvName, out srvPath))
            {
                MethodInfo info = null;
                if (srvPath.SrvMethodInfo.TryGetValue(srvRquest.SrvMethod, out info))
                {
                    if (srvPath.SrvInstance == null)
                    {
                        srvPath.SrvInstance = Activator.CreateInstance(srvPath.SrvType);
                    }
                    //
                    List<object> list = new List<object>();
                    foreach (SrvParam param in srvRquest.SrvParam)
                    {
                        list.Add(param.ParamObj);
                    }
                    object result = info.Invoke(srvPath, list.ToArray());
                    return result;
                }
                return null;
            }
            return null;
        }

    }
}
