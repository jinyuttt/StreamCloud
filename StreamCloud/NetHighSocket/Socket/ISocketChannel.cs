using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

/**
* 命名空间: NetHighSocket 
* 类 名： ISocketChannel
* 版本 ：v1.0
* Copyright (c) year 
*/

namespace NetHighSocket
{
    /// <summary>
    /// 功能描述    ：ISocketChannel  对外通信接口
    /// 创 建 者    ：jinyu
    /// 创建日期    ：2018/10/4 16:42:03 
    /// 最后修改者  ：jinyu
    /// 最后修改日期：2018/10/4 16:42:03 
    /// </summary>
   public interface ISocketChannel
    {

        /// <summary>
        /// 分配ID,暂时无用，没有实现
        /// </summary>
         long ID { get; set; }

        /// <summary>
        /// 通信Socket
        /// </summary>
         Socket Socket { get; set; }

        /// <summary>
        /// 是否调用了关闭方法
        /// </summary>
         bool IsClose { get; set; }

       /// <summary>
       /// 绑定本地地址
       /// </summary>
       /// <param name="localAddress"></param>
       /// <returns></returns>
         Task<bool> BindAsync(EndPoint localAddress);

       /// <summary>
       /// 写入发送的数据对象
       /// </summary>
       /// <param name="message"></param>
        void WriteAndFlush(object message);

        /// <summary>
        ///写入对象
        /// </summary>
        /// <param name="message"></param>
        void Write(object message);

        /// <summary>
        /// 写入对象
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        Task WriteAndFlushAsync(object message);

        /// <summary>
        /// 写入对象
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        Task WriteAsync(object message);

        /// <summary>
        /// 同步直接发送到网络
        /// </summary>
        /// <param name="data"></param>
        void SendData(byte[] data,int offset=0,int len=-1);

       /// <summary>
       /// 异步直接发送网络
       /// </summary>
       /// <param name="data"></param>
        void SendDataAsync(byte[] data);

        /// <summary>
        /// 连接
        /// </summary>
        /// <param name="endPoint"></param>
        /// <returns></returns>
        bool Connect(EndPoint endPoint);

        /// <summary>
        /// 连接
        /// </summary>
        /// <param name="remotePoint"></param>
        /// <param name="localPoint"></param>
        /// <returns></returns>
        bool Connect(EndPoint remotePoint, EndPoint localPoint);

       /// <summary>
       /// 关闭通信
       /// </summary>
        void Close();
      
        /// <summary>
        /// 异步连接
        /// </summary>
        /// <param name="endPoint"></param>
        /// <returns></returns>
        Task<bool> ConnectAsync(EndPoint endPoint);

        /// <summary>
        /// 异步连接
        /// </summary>
        /// <param name="remotePoint"></param>
        /// <param name="localPoint"></param>
        /// <returns></returns>
       Task<bool> ConnectAsync(EndPoint remotePoint, EndPoint localPoint);

        /// <summary>
        /// 异步连接
        /// </summary>
        /// <returns></returns>
        Task CloseAsync();

       /// <summary>
       /// 连接触发
       /// </summary>
        void FireChannelActive();

        /// <summary>
        /// 关闭触发
        /// </summary>
        void FireChannelInActive();

        /// <summary>
        /// 直接传送给管道的反序列化后的数据
        /// </summary>
        /// <param name="message"></param>
        void FireChannelRead(object message);

        /// <summary>
        /// 直接传送给管道的序列化的数据
        /// </summary>
        /// <param name="message"></param>
        void FireChannelWrite(object message);

        /// <summary>
        /// 管道当前读取完成
        /// </summary>
        void FireChannelReadComplete();

        /// <summary>
        /// 暂时无用
        /// </summary>
        void FireChannelWritabilityChanged();

        /// <summary>
        /// 错误信息
        /// </summary>
        /// <param name="exception"></param>
        void FireExceptionCaught(Exception exception);

        /// <summary>
        /// 设置参数
        /// </summary>
        /// <param name="sockOption"></param>
        /// <param name="size"></param>
        void SetOption(SockOption sockOption, int size);
    }
}
