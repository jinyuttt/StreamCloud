using System;

namespace SrvNetSocket
{

   /// <summary>
   /// 创建传输组件的对象
   /// </summary>
   public static class NetFactory
    {
        static TransferDLL transferDLL = new TransferDLL();

        /// <summary>
        /// 创建传输组件的客户端
        /// 定义传输组件的名称格式：名称_client,名称_server
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static ISocketClient<T> CreateSocketClient<T>(string name)
        {
            Type clientCls= transferDLL.GetClass(name + "_client");
            if (clientCls == null)
            {
                return null;
            }
            Type cls = clientCls.MakeGenericType(typeof(T));
            ISocketClient<T> client = Activator.CreateInstance(cls) as ISocketClient<T>;
            return client;
        }

        /// <summary>
        /// 创建传输组件的服务端
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static ISocketServer<T> CreateSocketServer<T>(string name)
        {
            Type srvCls = transferDLL.GetClass(name + "_server");
            if (srvCls == null)
            {
                return null;
            }
            Type cls= srvCls.MakeGenericType(typeof(T));
            ISocketServer<T> server = Activator.CreateInstance(cls) as ISocketServer<T>;
            return server;
        }

    }
}
