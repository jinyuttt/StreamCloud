using System;
using System.IO;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

/**
* 命名空间: HttpServerLib 
* 类 名： RequestProcesser
* 版本 ：v1.0
* Copyright (c) year 
*/

namespace HTTPServerLib
{
    /// <summary>
    /// 功能描述    ：RequestProcesser  处理Http请求与HttpServer基本一致
    /// 创 建 者    ：jinyu
    /// 创建日期    ：2018/10/7 14:10:14 
    /// 最后修改者  ：jinyu
    /// 最后修改日期：2018/10/7 14:10:14 
    /// </summary>
    public class RequestProcesser
    {
       

        /// <summary>
        /// 服务器目录
        /// </summary>
        public string ServerRoot { get;  set; }

        /// <summary>
        /// 服务器协议
        /// </summary>
        public Protocols Protocol { get;  set; }

        /// <summary>
        /// 日志接口
        /// </summary>
        public ILogger Logger { get; set; }
        /// <summary>
        /// SSL证书
        /// </summary>
        private X509Certificate serverCertificate = null;
        #region 公开方法
        public void SetSSL(string certificate)
        {
             SetSSL(X509Certificate.CreateFromCertFile(certificate));
        }


        public void SetSSL(X509Certificate certifiate)
        {
            this.serverCertificate = certifiate;
          
        }
        /// <summary>
        /// 获取服务器目录
        /// </summary>
        public string GetRoot()
        {
            return this.ServerRoot;
        }
        #endregion
      
        
        #region 改造服务端类，处理外部数据

        /// <summary>
        /// 解析请求
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public virtual HttpRequest ProcessRequestReturn(byte[] request)
        {
            MemoryStream memoryStream = new MemoryStream(request);
            memoryStream.Position = 0;
            Stream result = memoryStream;
           
            if (serverCertificate != null) result = ProcessSSL(memoryStream);
            if (result == null) return null;
            //构造HTTP请求
            HttpRequest req = new HttpRequest(result);
            req.Logger = Logger;
            //释放
            memoryStream.Close();
            memoryStream.Dispose();
            memoryStream = null;
            return req;
        }

        /// <summary>
        /// 返回传递的结果
        /// </summary>
        /// <param name="result">结果字符串</param>
        /// <param name="request">请求</param>
        /// <param name="respStream">返回流</param>
        public virtual void ProcessResult(string result, HttpRequest request, Stream respStream)
        {
            //构造HTTP响应
            HttpResponse response = new HttpResponse(respStream);
            response.Logger = Logger;
            switch (request.Method)
            {
                case "GET":
                    OnGet(result, response);
                    break;
                case "POST":
                    OnPost(result, response);
                    break;
                default:
                    OnDefault(request, response);
                    break;
            }
        }

        public virtual void OnGet(string result, HttpResponse response)
        {

        }
        public virtual void OnPost(string result, HttpResponse response)
        {

        }
        public virtual void OnDefault(string result, HttpResponse response)
        {

        }

        #endregion
      
        
        #region 内部方法
       

        /// <summary>
        /// 处理客户端请求
        /// </summary>
        /// <param name="handler">客户端Socket</param>
        public Stream ProcessRequest(Stream clientStream)
        {
            //处理SSL
            if (serverCertificate != null) clientStream = ProcessSSL(clientStream);
            if (clientStream == null) return null;

            //构造HTTP请求
            HttpRequest request = new HttpRequest(clientStream);
            request.Logger = Logger;

            //构造HTTP响应
            Stream respStream= this.OnResponse(clientStream);
            HttpResponse response = new HttpResponse(respStream);
            response.Logger = Logger;

            //处理请求类型
            

            return respStream;
        }


        /// <summary>
        /// 处理ssl加密请求
        /// </summary>
        /// <param name="clientStream"></param>
        /// <returns></returns>
        private Stream ProcessSSL(Stream clientStream)
        {
            try
            {
                SslStream sslStream = new SslStream(clientStream);
                sslStream.AuthenticateAsServer(serverCertificate, false, SslProtocols.Tls, true);
                sslStream.ReadTimeout = 10000;
                sslStream.WriteTimeout = 10000;
                return sslStream;
            }
            catch (Exception e)
            {
                Log(e.Message);
                clientStream.Close();
            }

            return null;
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="message">日志消息</param>
        protected void Log(object message)
        {
            if (Logger != null) Logger.Log(message);
        }

        #endregion

        #region 虚方法

        /// <summary>
        /// 响应Get请求
        /// </summary>
        /// <param name="request">请求报文</param>
        public virtual void OnGet(HttpRequest request, HttpResponse response)
        {

        }

        /// <summary>
        /// 响应Post请求
        /// </summary>
        /// <param name="request"></param>
        public virtual void OnPost(HttpRequest request, HttpResponse response)
        {

        }

        /// <summary>
        /// 响应默认请求
        /// </summary>

        public virtual void OnDefault(HttpRequest request, HttpResponse response)
        {

        }

        /// <summary>
        /// 回发流
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public virtual Stream OnResponse(Stream stream,int size=0)
        {
            return stream;
        }
       

        #endregion
    }
}
