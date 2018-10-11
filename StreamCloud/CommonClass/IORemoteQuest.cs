using SrvNetSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

/**
* 命名空间: CommonClass 
* 类 名： IORemoteQuest
* 版本 ：v1.0
* Copyright (c) year 
*/

namespace CommonClass
{
    /// <summary>
    /// 功能描述    ：IORemoteQuest  服务执行请求
    /// 创 建 者    ：jinyu
    /// 创建日期    ：2018/10/11 19:16:39 
    /// 最后修改者  ：jinyu
    /// 最后修改日期：2018/10/11 19:16:39 
    /// </summary>
    public class IORemoteQuest
    {

  
        private static SocketPool<byte[]> pool = new SocketPool<byte[]>();
        private static SocketPool<QuestResult> poolResult = new SocketPool<QuestResult>();

        /// <summary>
        /// 发送请求，带序列化
        /// </summary>
        /// <typeparam name="TSerializer">序列化对象</typeparam>
        /// <param name="srvRquest"></param>
        /// <param name="timeOut">超时</param>
        /// <returns></returns>
        public static QuestResult  RemoteQuest<TSerializer>(SrvRquest srvRquest,int timeOut=100000) where TSerializer : CommonSerializer
        {
            byte[] req = SerializerFactory<TSerializer>.Serializer<SrvRquest>(srvRquest);
            var cts = new CancellationTokenSource(timeOut);
            CancellationTokenRegistration registration = cts.Token.Register(() => { Console.WriteLine("超时"); });
            Task<byte[]> result = Task.Factory.StartNew<byte[]>(() =>
            {
                var client = pool.GetSocket();
                client.SendData(req);
                return client.GetNetSocket().recData;
            });
            byte[] rec = result.Result;
            result.Dispose();
            registration.Dispose();
            return SerializerFactory<TSerializer>.Deserialize<QuestResult>(rec);
        }

        /// <summary>
        /// 发送请求；序列化放在传输层
        /// </summary>
        /// <param name="srvRquest">类型</param>
        /// <param name="timeOut">超时</param>
        /// <returns></returns>
        public static QuestResult RemoteQuestNET(SrvRquest srvRquest, int timeOut = 100000)
        {
          
            var cts = new CancellationTokenSource(timeOut);
            CancellationTokenRegistration registration = cts.Token.Register(() => { Console.WriteLine("超时"); });
            Task<QuestResult> result = Task.Factory.StartNew<QuestResult>(() =>
            {
                var client = poolResult.GetSocket();
                    client.SendData(srvRquest);
                return client.GetNetSocket().recData;
            });
            QuestResult rec = result.Result;
            result.Dispose();
            registration.Dispose();
            return rec;
        }
    }
}
