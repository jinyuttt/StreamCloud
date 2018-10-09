using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp2
{
   public class TaskTest
    {
        public  int  Sum()
        {
            try
            {
                Console.WriteLine("测试执行线程："+Thread.CurrentThread.ManagedThreadId);
                Thread.Sleep(3000);
                Console.WriteLine("执行中");
                Thread.Sleep(2000);
                Console.WriteLine("再次执行中");
            }
            catch(ThreadAbortException ex)
            {
                Console.WriteLine(ex.Message+","+Thread.CurrentThread.Name);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message + "," + Thread.CurrentThread.Name);
            }
            return 8;
        }

        public  long SumLong()
        {
            Thread.Sleep(3000);
            return 2;
        }
        public TestResult TestClass()
        {
            return new TestResult() { MY = "sss" };
        }
    }
}
