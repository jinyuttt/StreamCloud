using NStStreamCloud;
using StreamCloud;
using StreamCloud.Srv;
using System;
using System.Collections.Generic;
using System.IO;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
          
            SrvConfig.SrvHost = "127.0.0.1";
            SrvConfig.SrvPort = 7777;
            SrvConfig.NetComponent = "stream";
            SrvConfig.NetDir = Directory.GetCurrentDirectory();
            SrvRquest srvRquest = new SrvRquest();
            srvRquest.SrvMethod = "sss";
            srvRquest.SrvName = "test";
            srvRquest.SrvParam = new  List<SrvParam>();
            ServiceHttpProcessor.UnitTest = false;
             StreamCloudServer cloudServer = new StreamCloudServer();
            cloudServer.Start();
            Console.Read();
            
        }
    }
}
