using Client;
using NetHighSocket;
using System;
using System.Net;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
           ITest obj= RemoteProxyFactory.CreateClassProxy<ITest>("MYTEST");
        }
    }
}
