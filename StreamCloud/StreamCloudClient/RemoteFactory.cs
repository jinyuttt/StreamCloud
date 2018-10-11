//using System;
//using System.IO;
//using System.Reflection;
//using System.Reflection.Emit;

///**
//* 命名空间: StreamCloudClient 
//* 类 名： RemoteFactory
//* 版本 ：v1.0
//* Copyright (c) year 
//*/

//namespace StreamCloudClient
//{
//    /// <summary>
//    /// 功能描述    ：RemoteFactory  
//    /// 创 建 者    ：jinyu
//    /// 创建日期    ：2018/10/9 23:50:42 
//    /// 最后修改者  ：jinyu
//    /// 最后修改日期：2018/10/9 23:50:42 
//    /// </summary>
//    public  class RemoteFactory
//    {
//        private static string path;
//        private static string apiRemoteAsmName;
//        private static object SyntaxFactory;

//        public static object CreateType()
//        {
//            var apiRemoteProxyDllFile = Path.Combine(path,
//  apiRemoteAsmName + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".dll");
//            object codeBuilder = null;
//            var tree = SyntaxFactory.ParseSyntaxTree(codeBuilder.ToString());
//            var compilation = CSharpCompilation.Create(apiRemoteAsmName)
//             .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
//             .AddReferences(references)
//             .AddSyntaxTrees(tree);
//            //执行编译
//            EmitResult compilationResult = compilation.Emit(apiRemoteProxyDllFile);
//            if (compilationResult.Success)
//            {
//                // Load the assembly
//                apiRemoteAsm = Assembly.LoadFrom(apiRemoteProxyDllFile);
//            }
//            else
//            {
//                foreach (Diagnostic codeIssue in compilationResult.Diagnostics)
//                {
//                    string issue = $"ID: {codeIssue.Id}, Message: {codeIssue.GetMessage()}," +
//                    $" Location: { codeIssue.Location.GetLineSpan()}, " +
//                    $"Severity: { codeIssue.Severity}";
//                    AppRuntimes.Instance.Loger.Error("自动编译代码出现异常," + issue);
//                }
//            }

//    }

//}
//}
