using Client;
using ClsLibTest;
using NetHighSocket;
using System;
using System.Net;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
           ITest obj= RemoteProxyFactory.CreateClassProxy<ITest>("MYTEST", "ClsLibTest.dll");
            ITestCls test= RemoteProxyFactory.CreateClassProxy<ITestCls>("MYTESTCls", "ClsLibTest.dll");
         
            Console.Read();
        }
    }
}
