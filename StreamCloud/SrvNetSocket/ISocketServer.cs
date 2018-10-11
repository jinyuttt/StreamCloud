using System;
using System.Collections.Generic;
using System.Text;

namespace SrvNetSocket
{

    /// <summary>
    /// 通信服务端
    /// </summary>
    /// <typeparam name="TData">传输，接收的数据类型，一般是byte[]</typeparam>
    public interface ISocketServer<TData>
    {

        /// <summary>
        /// 服务端启动
        /// </summary>
        /// <param name="port">绑定端口</param>
        /// <returns></returns>
        bool StartRun(int port);

        /// <summary>
        /// 服务端启动
        /// </summary>
        /// <param name="ip">绑定IP</param>
        /// <param name="port">绑定端口</param>
        /// <returns></returns>
        bool StartRun(string ip,int port);

        /// <summary>
        /// 服务启动，由继承的类内部实现ip，端口
        /// 一般是直接使用内部配置类
        /// </summary>
        /// <returns></returns>
        bool StartRun();

        /// <summary>
        /// 关闭
        /// </summary>
        void Close();

        /// <summary>
        /// 直接发送网络数据
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        int SendData(NetChannel<TData> channel,byte[] data);

        int SendData(NetChannel<TData> channel, object data);

        /// <summary>
        /// 获取接收的数据（阻塞模型）
        /// 可以参考我的使用
        /// </summary>
        /// <returns></returns>
        NetChannel<TData> GetNetSocket();

        /// <summary>
        /// 注册委托，获取接收的数据
        /// </summary>
        /// <param name="recviceNetData">委托</param>
        void CallData(RecviceNetData<TData> recviceNetData);

    }
}
