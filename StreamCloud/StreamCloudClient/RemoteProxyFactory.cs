//using CommonClass;
//using Microsoft.CSharp;
//using System;
//using System.CodeDom.Compiler;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using System.Reflection.Emit;
//using System.Text;
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
//   public class RemoteProxyFactory
//    {
       
//        public static  T   CreateClassProxy<T>(string className=null)
//        {
//            Type DynamicCls = typeof(T);
//            if (string.IsNullOrEmpty(className))
//            {
//                className = DynamicCls.Name + "_DynamicCls";
//            }
//            // 1.CSharpCodePrivoder
//            CSharpCodeProvider objCSharpCodePrivoder = new CSharpCodeProvider();

//            // 2.ICodeComplier
//            CSharpCodeProvider objICodeCompiler = new CSharpCodeProvider();

//            // 3.CompilerParameters
//            CompilerParameters objCompilerParameters = new CompilerParameters();
//            objCompilerParameters.ReferencedAssemblies.Add("System.dll");
//            objCompilerParameters.GenerateExecutable = false;
//            objCompilerParameters.GenerateInMemory = true;

//            // 4.CompilerResults
//            CompilerResults cr = objICodeCompiler.CompileAssemblyFromSource(objCompilerParameters, GenerateCode(DynamicCls, className));

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
//                // 通过反射，调用HelloWorld的实例
//                Assembly objAssembly = cr.CompiledAssembly;
//                object objHelloWorld = objAssembly.CreateInstance("DynamicCodeGenerate."+className);
//                return (T)objHelloWorld;
//            }
//            return default(T);
          
//        }
//        public static string GenerateCode(Type type,string name)
//        {
//            if(!type.IsInterface)
//            {
//                return null;
//            }
//            StringBuilder sb = new StringBuilder();
//            sb.Append("using System;");
//            sb.Append(Environment.NewLine);
//            sb.Append("namespace DynamicCodeGenerate");
//            sb.Append(Environment.NewLine);
//            sb.Append("{");
//            sb.Append(Environment.NewLine);
//            sb.AppendFormat(" public class {0}:{1}", name,type.Name);
           
//            sb.Append(Environment.NewLine);
//            sb.Append("    {");
//            sb.Append(Environment.NewLine);

//            MethodInfo[] methodInfos=  type.GetMethods();
//            StringBuilder paramBuilder = new StringBuilder();
//            foreach(MethodInfo method in methodInfos)
//            {
//                paramBuilder.Clear();
//                sb.AppendFormat("public {0} {1} (", method.ReflectedType, method.Name);
//                ParameterInfo[] parameters= method.GetParameters();
//                foreach(ParameterInfo parameter in parameters)
//                {
                     
//                    sb.AppendFormat("{0} {1},", parameter.ParameterType, parameter.Name);
//                    paramBuilder.Append(" SrvParam srvParam = new SrvParam(); ");
//                    paramBuilder.Append(Environment.NewLine);
//                    paramBuilder.AppendFormat(" srvParam.ParamName =\"{0}\";",parameter.Name);
//                    paramBuilder.Append(Environment.NewLine);
//                    paramBuilder.AppendFormat(" srvParam.ParamType ={0};", parameter.ParameterType);
//                    paramBuilder.Append(Environment.NewLine);
//                    paramBuilder.AppendFormat(" srvParam.ParamObj ={0};", parameter.Name);
//                }
//                sb.Remove(sb.Length - 1, 1);
//                sb.Append(") {");
//                //定义方法体
//                if(paramBuilder.Length>0)
//                {
//                    paramBuilder.Insert(0," SrvParam ");
//                }
//                sb.AppendLine(paramBuilder.ToString());
//                sb.AppendLine("return Call(srvParam);");
//                sb.AppendLine("}");

//            }
//            //sb.Append("        public string OutPut()");
//            //sb.Append(Environment.NewLine);
//            //sb.Append("        {");
//            //sb.Append(Environment.NewLine);
//            //sb.Append("             return \"Hello world!\";");
//            //sb.Append(Environment.NewLine);
//            //sb.Append("        }");
//            sb.Append(Environment.NewLine);
//            sb.Append("    }");
//            sb.Append(Environment.NewLine);
//            sb.Append("}");

//            string code = sb.ToString();
//            Console.WriteLine(code);
//            Console.WriteLine();

//            return code;
//        }
//    }
//}
