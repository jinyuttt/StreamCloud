using CommonClass;
using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

/**
* 命名空间: Client 
* 类 名： RemoteProxyFactory
* 版本 ：v1.0
* Copyright (c) year 
*/

namespace Client
{
    /// <summary>
    /// 功能描述    ：RemoteProxyFactory  
    /// 创 建 者    ：jinyu
    /// 创建日期    ：2018/10/10 1:14:09 
    /// 最后修改者  ：jinyu
    /// 最后修改日期：2018/10/10 1:14:09 
    /// </summary>
   public class RemoteProxyFactory
    {
        public static  T  CreateObjectProxy<T>() 
        {
           Type type=  typeof(T);
            if(type.IsInterface)
            {
                Console.WriteLine(true);
            }
           var methodInfos= type.GetMethods();
            string typeName = type.Name + "_Cls";
            AssemblyName assemblyName = new AssemblyName("dynamic_assemblyName");
            AssemblyBuilder assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndSave);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("DynamicModule", "dynamicmodule.dll");
            TypeBuilder typeBuilder = moduleBuilder.DefineType(typeName, TypeAttributes.Public);
            //*添加所实现的接口
            typeBuilder.AddInterfaceImplementation(typeof(T));

            //实现方法
            foreach (var method in methodInfos)
            {
                //var  parameterInfos= method.GetParameters();
                // Type[] paramTypes = null;
                //if (parameterInfos.Length > 0)
                // {
                //    paramTypes = new Type[parameterInfos.Length];
                //  for (int i = 0; i < parameterInfos.Length; i++)
                // {
                //    paramTypes[i] = parameterInfos[i].ParameterType;
                // }
                // }
              var paramTypes = method.GetParameters().Select(p => p.ParameterType).ToArray();
                MethodBuilder mbIM = typeBuilder.DefineMethod(method.Name,
                MethodAttributes.Public,
               method.ReturnType,
               paramTypes);

                ILGenerator il = mbIM.GetILGenerator();
                il.Emit(OpCodes.Newobj, typeof(SrvRquest).GetConstructor(Type.EmptyTypes));
                //il.Emit(OpCodes.)
                il.Emit(OpCodes.Ldarga);
                il.Emit(OpCodes.Ldstr, "The SayHello implementation of IPerson");
                il.Emit(OpCodes.Call, typeof(Console).GetMethod("WriteLine",
                    new Type[] { typeof(string) }));
                il.Emit(OpCodes.Ret);
                typeBuilder.DefineMethodOverride(mbIM, typeof(T).GetMethod("SayHello"));
            }
            
            Type personType = typeBuilder.CreateType();
            assemblyBuilder.Save("Person.dll");
            object obj = Activator.CreateInstance(personType);
            MethodInfo methodInfo = personType.GetMethod("SayHello");

            methodInfo.Invoke(obj, null);
            return (T)Activator.CreateInstance(personType);
           
        }

        public static  T   CreateClassProxy<T>(string className=null)
        {
            Type DynamicCls = typeof(T);
            if (string.IsNullOrEmpty(className))
            {
                className = DynamicCls.Name + "_DynamicCls";
            }
            // 1.CSharpCodePrivoder
            CSharpCodeProvider objCSharpCodePrivoder = new CSharpCodeProvider();

            // 2.ICodeComplier
            CSharpCodeProvider objICodeCompiler = new CSharpCodeProvider();

            // 3.CompilerParameters
            CompilerParameters objCompilerParameters = new CompilerParameters();
            objCompilerParameters.ReferencedAssemblies.Add("System.dll");
            objCompilerParameters.GenerateExecutable = false;
            objCompilerParameters.GenerateInMemory = true;

            // 4.CompilerResults
            CompilerResults cr = objICodeCompiler.CompileAssemblyFromSource(objCompilerParameters, GenerateCode(DynamicCls, className));

            if (cr.Errors.HasErrors)
            {
                Console.WriteLine("编译错误：");
                foreach (CompilerError err in cr.Errors)
                {
                    Console.WriteLine(err.ErrorText);
                }
            }
            else
            {
                // 通过反射，调用HelloWorld的实例
                Assembly objAssembly = cr.CompiledAssembly;
                object objHelloWorld = objAssembly.CreateInstance("DynamicCodeGenerate."+className);
                return (T)objHelloWorld;
            }
            return default(T);
          
        }
        public static string GenerateCode(Type type,string name)
        {
            if(!type.IsInterface)
            {
                return null;
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("using System;");
            sb.Append(Environment.NewLine);
            sb.Append("namespace DynamicCodeGenerate");
            sb.Append(Environment.NewLine);
            sb.Append("{");
            sb.Append(Environment.NewLine);
            sb.AppendFormat(" public class {0}:{1}", name,type.Name);
           
            sb.Append(Environment.NewLine);
            sb.Append("    {");
            sb.Append(Environment.NewLine);

            MethodInfo[] methodInfos=  type.GetMethods();
            StringBuilder paramBuilder = new StringBuilder();
            foreach(MethodInfo method in methodInfos)
            {
                paramBuilder.Clear();
                sb.AppendFormat("public {0} {1} (", method.ReflectedType, method.Name);
                ParameterInfo[] parameters= method.GetParameters();
                foreach(ParameterInfo parameter in parameters)
                {
                     
                    sb.AppendFormat("{0} {1},", parameter.ParameterType, parameter.Name);
                    paramBuilder.Append(" SrvParam srvParam = new SrvParam(); ");
                    paramBuilder.Append(Environment.NewLine);
                    paramBuilder.AppendFormat(" srvParam.ParamName =\"{0}\";",parameter.Name);
                    paramBuilder.Append(Environment.NewLine);
                    paramBuilder.AppendFormat(" srvParam.ParamType ={0};", parameter.ParameterType);
                    paramBuilder.Append(Environment.NewLine);
                    paramBuilder.AppendFormat(" srvParam.ParamObj ={0};", parameter.Name);
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append(") {");
                //定义方法体
                if(paramBuilder.Length>0)
                {
                    paramBuilder.Insert(0," SrvParam ");
                }
                sb.AppendLine(paramBuilder.ToString());
                sb.AppendLine("return Call(srvParam);");
                sb.AppendLine("}");

            }
            //sb.Append("        public string OutPut()");
            //sb.Append(Environment.NewLine);
            //sb.Append("        {");
            //sb.Append(Environment.NewLine);
            //sb.Append("             return \"Hello world!\";");
            //sb.Append(Environment.NewLine);
            //sb.Append("        }");
            sb.Append(Environment.NewLine);
            sb.Append("    }");
            sb.Append(Environment.NewLine);
            sb.Append("}");

            string code = sb.ToString();
            Console.WriteLine(code);
            Console.WriteLine();

            return code;
        }
    }
}
