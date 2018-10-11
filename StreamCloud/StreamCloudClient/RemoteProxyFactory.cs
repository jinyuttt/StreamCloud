//using CommonClass;
//using Microsoft.CSharp;
//using System;
//using System.CodeDom.Compiler;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Reflection;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;

///**
//* 命名空间: Client 
//* 类 名： RemoteProxyFactory
//* 版本 ：v1.0
//* Copyright (c) year 
//*/

//namespace Client
//{
//    /// <summary>
//    /// 功能描述    ：RemoteProxyFactory  
//    /// 创 建 者    ：jinyu
//    /// 创建日期    ：2018/10/10 1:14:09 
//    /// 最后修改者  ：jinyu
//    /// 最后修改日期：2018/10/10 1:14:09 
//    /// </summary>
//    public class RemoteProxyFactory
//    {
//        private static Dictionary<string, object> dic_Objet = new Dictionary<string, object>();
//        private static List<string> listSource = new List<string>();
//        private static List<string> listReference = new List<string>();
//        public static volatile int waitTime = 10 * 60 * 1000;//10分钟
//        public static volatile int lastNum =0;//10分钟
//        public static volatile bool isRun = false;//

//        /// <summary>
//        /// 创建类型
//        /// </summary>
//        /// <typeparam name="T"></typeparam>
//        /// <param name="className"></param>
//        /// <param name="referencedAssembly"></param>
//        /// <returns></returns>
//        public static T CreateClassProxy<T>(string className = null, params  string[] referencedAssembly)
//        {
//            Type DynamicCls = typeof(T);
//            if (string.IsNullOrEmpty(className))
//            {
//                className = DynamicCls.Name + "_DynamicCls";
//            }
//            object instance = null;
//            if(dic_Objet.TryGetValue(className,out instance))
//            {
//                return (T)instance;
//            }
//            else if(dic_Objet.Count==0)
//            {
//                //加载一次
//                LoadDll();
//                if (dic_Objet.TryGetValue(className, out instance))
//                {
//                    return (T)instance;
//                }
//            }
            
//            // 1.CSharpCodePrivoder
//            CSharpCodeProvider objCSharpCodePrivoder = new CSharpCodeProvider();

//            // 2.ICodeComplier
//            CSharpCodeProvider objICodeCompiler = new CSharpCodeProvider();

//            // 3.CompilerParameters
//            CompilerParameters objCompilerParameters = new CompilerParameters();
//            objCompilerParameters.ReferencedAssemblies.Add("System.dll");
//            objCompilerParameters.ReferencedAssemblies.Add("CommonClass.dll");
//            objCompilerParameters.ReferencedAssemblies.Add("NETStandard.dll");
//            listReference.Add("System.dll");
//            listReference.Add("CommonClass.dll");
//            listReference.Add("NETStandard.dll");
//            try
//            {
//                //默认引用接口的类库
//                objCompilerParameters.ReferencedAssemblies.Add(DynamicCls.Namespace + ".dll");
//            }
//            catch(Exception ex)
//            {
//                Console.WriteLine(ex.Message);
//            }
//            foreach (string dll in referencedAssembly )
//            {
//                listReference.Add(dll);
//                objCompilerParameters.ReferencedAssemblies.Add(dll);
//            }
//            objCompilerParameters.GenerateExecutable = false;
//            objCompilerParameters.GenerateInMemory = true;
//            //objCompilerParameters.OutputAssembly = "DynamicInterfaceCls.dll";         //输出路径
//            objCompilerParameters.IncludeDebugInformation = false; //是否产生pdb调试文件      默认是false
//            // 4.CompilerResults
//            string source = GenerateCode(DynamicCls, className);
//            CompilerResults cr = objICodeCompiler.CompileAssemblyFromSource(objCompilerParameters, source);

//            if (cr.Errors.HasErrors)
//            {
//                Console.WriteLine("编译错误：");
//                foreach (CompilerError err in cr.Errors)
//                {
//                    Console.WriteLine(err.ErrorText);
//                }
//            }
//            else
//            {
//                listSource.Add(source);
//                Start();//有成功；
//                // 通过反射，调用HelloWorld的实例
//                Assembly objAssembly = cr.CompiledAssembly;
//                object objHelloWorld = objAssembly.CreateInstance("DynamicCodeGenerate." + className);
//                return (T)objHelloWorld;
//            }
//            return default(T);

//        }

//        /// <summary>
//        /// 处理源码
//        /// </summary>
//        /// <param name="type"></param>
//        /// <param name="name"></param>
//        /// <returns></returns>
//        public static string GenerateCode(Type type, string name)
//        {
//            if (!type.IsInterface)
//            {
//                return null;
//            }
//            StringBuilder sb = new StringBuilder();
//            sb.Append(" using System;");
//            sb.Append(Environment.NewLine);
//            sb.Append(" using CommonClass; ");
//            sb.Append(Environment.NewLine);
//            sb.Append(" using System.Collections.Generic; ");
//            sb.Append(Environment.NewLine);
//            sb.Append("namespace DynamicCodeGenerate");
//            sb.Append(Environment.NewLine);
//            sb.Append("{");
//            sb.Append(Environment.NewLine);
//            sb.AppendFormat(" public class {0}:{1}", name, type.FullName);

//            sb.Append(Environment.NewLine);
//            sb.Append("    {");
//            sb.Append(Environment.NewLine);
//            string srvName = name;
//            SrvAttribute srvAttribute = type.GetCustomAttribute<SrvAttribute>();
//            if(srvAttribute!=null)
//            {
//                srvName = srvAttribute.SrvName;
//            }
//            MethodInfo[] methodInfos = type.GetMethods();
//            StringBuilder paramBuilder = new StringBuilder();
//            foreach (MethodInfo method in methodInfos)
//            {
//                paramBuilder.Clear();
//                string returnType = method.ReturnType.FullName;
//                if(returnType.Equals("System.Void"))
//                {
//                    returnType = " void ";
//                }
//                sb.AppendFormat("public {0} {1} (", returnType, method.Name);
//                ParameterInfo[] parameters = method.GetParameters();
//                foreach (ParameterInfo parameter in parameters)
//                { 
//                    sb.AppendFormat("{0} {1},", parameter.ParameterType, parameter.Name);
//                    paramBuilder.Append(" srvParam = new SrvParam(); ");
//                    paramBuilder.Append(Environment.NewLine);
//                    paramBuilder.AppendFormat(" srvParam.ParamName =\"{0}\";", parameter.Name);
//                    paramBuilder.Append(Environment.NewLine);
//                    paramBuilder.AppendFormat(" srvParam.ParamType =\"{0}\";", parameter.ParameterType);
//                    paramBuilder.Append(Environment.NewLine);
//                    paramBuilder.AppendFormat(" srvParam.ParamObj ={0};", parameter.Name);
//                    paramBuilder.Append(" srvRquest.SrvParam.Add(srvParam);");
//                }
//                if (paramBuilder.Length > 0)
//                {
//                    //有参数
//                    sb.Remove(sb.Length - 1, 1);
//                }
//                sb.Append(") {");
//                //定义方法体
//                if (paramBuilder.Length > 0)
//                {
//                    paramBuilder.Insert(0, " SrvParam ");
//                }
//                //处理方法体
               
//                string srvMethod = method.Name;
//                 srvAttribute= method.GetCustomAttribute<SrvAttribute>();
//                if(srvAttribute!=null)
//                {
//                    srvMethod = srvAttribute.SrvName;
//                }
//                sb.AppendLine();
//                sb.AppendLine("SrvRquest srvRquest = new SrvRquest();"); 
//                sb.AppendFormat("srvRquest.SrvName = \"{0}\";",srvName);
//                sb.AppendLine();
//                sb.AppendFormat("srvRquest.SrvMethod =\"{0}\";", srvMethod);
//                sb.AppendLine();
//                sb.AppendLine("srvRquest.SrvParam = new List<SrvParam>();") ;
//                sb.AppendLine(paramBuilder.ToString());
//                sb.Append("QuestResult result=");
//                sb.AppendLine("IORemoteQuest.RemoteQuestNET(srvRquest);");
//                if (method.ReturnType.Name != "Void")
//                {
//                    sb.AppendFormat(" return  ({0})result.Result;", returnType);
//                }
//                sb.AppendLine(" }");

//            }
//            sb.Append(Environment.NewLine);
//            sb.Append("    }");
//            sb.Append(Environment.NewLine);
//            sb.Append("}");

//            string code = sb.ToString();
//            Console.WriteLine(code);
//            Console.WriteLine();

//            return code;
//        }

//        /// <summary>
//        /// 加载dll
//        /// </summary>
//        public static void LoadDll()
//        {
//            if (!File.Exists("DynamicInterfaceCls.dll"))
//            {
//                return;
//            }
//            byte[] allStram = File.ReadAllBytes("DynamicInterfaceCls.dll");
//            Assembly assembly = Assembly.Load(allStram);
          
//            Type[] allcls = assembly.GetTypes();
//            foreach (Type type in allcls)
//            {
//                SrvAttribute srvAttribute = type.GetCustomAttribute<SrvAttribute>();
//                if (srvAttribute != null)
//                {
//                    dic_Objet[type.Name] = Activator.CreateInstance(type);
//                }
//            }
//        }

//        /// <summary>
//        /// 启动保持
//        /// </summary>

//        public static void Start()
//        {
//            if(isRun)
//            {
//                return;
//            }
//            isRun = true;
//             Task.Factory.StartNew(() => {
//                Thread.Sleep(waitTime);
//                 if(lastNum==listSource.Count)
//                 { return; }
//                SaveDLL();
//                 lastNum = listSource.Count;
//                 isRun = false;
//            }
//            );
//        }

//        /// <summary>
//        /// 保持
//        /// </summary>
//        private static void SaveDLL()
//        {
//            // 1.CSharpCodePrivoder
//            CSharpCodeProvider objCSharpCodePrivoder = new CSharpCodeProvider();

//            // 2.ICodeComplier
//            CSharpCodeProvider objICodeCompiler = new CSharpCodeProvider();

//            // 3.CompilerParameters
//            CompilerParameters objCompilerParameters = new CompilerParameters();
//           string[] lst= listReference.Distinct<string>().ToArray<string>();
//            foreach (string dll in lst)
//            {
//                objCompilerParameters.ReferencedAssemblies.Add(dll);
//            }
           
//            objCompilerParameters.GenerateExecutable = false;
//            objCompilerParameters.GenerateInMemory = true;
//            objCompilerParameters.OutputAssembly = "DynamicInterfaceCls.dll";         //输出路径
//            objCompilerParameters.IncludeDebugInformation = false; //是否产生pdb调试文件      默认是false
//            // 4.CompilerResults
            
//            CompilerResults cr = objICodeCompiler.CompileAssemblyFromSource(objCompilerParameters, listSource.ToArray<string>());

//            if (cr.Errors.HasErrors)
//            {
//                Console.WriteLine("编译错误：");
//                foreach (CompilerError err in cr.Errors)
//                {
//                    Console.WriteLine(err.ErrorText);
//                }
//            }
//        }
//    }
//}
