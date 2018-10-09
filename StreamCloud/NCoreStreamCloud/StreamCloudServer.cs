using SrvNetSocket;
using StreamCloud.Srv;
using System;
using System.Collections.Generic;
using System.Text;

/**
* 命名空间: NStStreamCloud 
* 类 名： StreamCloudServer
* 版本 ：v1.0
* Copyright (c) year 
*/

namespace NStStreamCloud
{
    /// <summary>
    /// 功能描述    ：StreamCloudServer  
    /// 创 建 者    ：jinyu
    /// 创建日期    ：2018/10/8 11:19:48 
    /// 最后修改者  ：jinyu
    /// 最后修改日期：2018/10/8 11:19:48 
    /// </summary>
   public class StreamCloudServer
    {
        public void Start()
        {
            TransferConfig.TransferDLLDir = SrvConfig.NetDir;
            ISocketServer<object> server= NetFactory.CreateSocketServer<object>(SrvConfig.NetComponent);
           if(server.StartRun(SrvConfig.SrvHost, SrvConfig.SrvPort))
            {
                //网络启动成功，这里注册委托，不采用获取模式
                //由于我的实现，如果是另外一种方式，会阻塞等待，而且是一个线程
                server.CallData(RecviceNet);
            }
        }

        /// <summary>
        /// 接收网络数据
        /// </summary>
        /// <param name="netSocket"></param>
        private void RecviceNet(NetChannel<object> netSocket)
        {
            SeviceProcessor.AnalysisRequest(netSocket);
        }
    }
}
