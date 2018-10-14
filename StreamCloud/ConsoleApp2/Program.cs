using CacheBuffer;
using System;
using System.Threading;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            // ITest obj= RemoteProxyFactory.CreateClassProxy<ITest>("MYTEST", "ClsLibTest.dll");
            // ITestCls test= RemoteProxyFactory.CreateClassProxy<ITestCls>("MYTESTCls", "ClsLibTest.dll");
              LRUCache<int, int> cache = new LRUCache<int, int>();
              cache.RemoveEntitiesEvent += Cache_RemoveEntitiesEvent;
              cache.CacheTime =3;//启动缓存时间
             Random random = new Random();
             DateTime start= DateTime.Now;
            for(int i=0;i<10000000;i++)
            {
                cache.Set(i,random.Next());
               
            }
           
            Console.WriteLine("时间：" + (DateTime.Now - start).TotalSeconds);
            Console.Read();
        }

        private static void Cache_RemoveEntitiesEvent(string cacheName, RemoveEntity<int, int> entity)
        {
           // Console.WriteLine("移除：" + entity.Key);
        }
    }
}
