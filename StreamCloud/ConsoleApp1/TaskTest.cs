using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1
{
   public class TaskTest
    {
        public  int  Sum()
        {
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
            Thread.Sleep(10000);
            Console.WriteLine("执行中");
            Thread.Sleep(2000);
            Console.WriteLine("再次执行中");
            return 8;
        }

        public int Say()
        {
            return 7;
        }
        public long SayInit(int s,int i,long k,string kk)
        {
            Console.WriteLine(kk);
            return s + i + k;
        }
        public Response GetResponse(Request request)
        {
            Console.WriteLine(request.name);
            return new Response() { Ansore = "jinyu" };
        }

        public void  SayHello()
        {
            Console.WriteLine("heool");
        }
    }
}
