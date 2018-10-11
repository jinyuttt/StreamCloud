using Client;
using System;

namespace ConsoleApp3
{
    class Program
    {
        static void Main(string[] args)
        {
          ITest obj=  RemoteProxyFactory.CreateClassProxy<ITest>();
        }
    }
}
