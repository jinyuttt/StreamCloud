using HTTPServerLib;
using StreamCloud.Srv;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

/**
* 命名空间: NStStreamCloud 
* 类 名： ServiceHttpProcessor
* 版本 ：v1.0
* Copyright (c) year 
*/

namespace NStStreamCloud
{
    /// <summary>
    /// 功能描述    ：ServiceHttpProcessor  处理HTTP请求
    /// 创 建 者    ：jinyu
    /// 创建日期    ：2018/10/8 12:30:19 
    /// 最后修改者  ：jinyu
    /// 最后修改日期：2018/10/8 12:30:19 
    /// </summary>
  public  class ServiceHttpProcessor: RequestProcesser
    {
        public static bool UnitTest = true;
       public ServiceHttpProcessor()
        {
            ServerRoot = "";
        }

        #region 服务扩展请求
        public override HttpRequest ProcessRequestReturn(byte[] request)
        {
            return base.ProcessRequestReturn(request);
        }
        public override void ProcessResult(string result, HttpRequest request, Stream respStream)
        {
            base.ProcessResult(result, request, respStream);
        }

        /// <summary>
        /// JSON格式返回
        /// </summary>
        /// <param name="result"></param>
        /// <param name="response"></param>
        private void ProcessStreamJSON(string result, HttpResponse response)
        {
            if(response.Headers==null)
            {
                response.Headers = new Dictionary<string, string>();
            }
            if (result == null)
            {
                if (File.Exists("QueryDefault/json.txt"))
                {
                    using (StreamReader rd = new StreamReader("QueryDefault/json.txt",Encoding.UTF8))
                    {
                        result = rd.ReadToEnd();
                    }
                }
            }
            //
            response.FromJSON(result);
            response.Content_Encoding = "utf-8";
            response.Headers["Server"] = "StreamCloudServer";
            response.Send();
        }
        public override void OnGet(string result, HttpResponse response)
        {
            if (UnitTest)
            {
                //发送响应
                if (result == null)
                {
                    //静态网页
                    response.FromFile("QueryDefault/index.html");
                    response.Content_Type = "text/html; charset=UTF-8";
                }
                else
                {
                    //网页返回
                    response.SetContent(result);
                    response.Content_Encoding = "utf-8";
                    response.StatusCode = "200";
                    response.Content_Type = "text/html; charset=UTF-8";
                    response.Headers["Server"] = "StreamCloudServer";
                }
                //发送HTTP响应
                response.Send();
            }
           else
            {
                ProcessStreamJSON(result, response);
            }
           

        }
        public override void OnPost(string result, HttpResponse response)
        {
            ProcessStreamJSON(result, response);
        }

        #endregion

        #region 原网页请求

        /// <summary>
        /// 从缓存中获取数据
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public override Stream OnResponse(Stream stream,int size)
        {
           return HTTPStream.GetInstance().GetStreamCompare(size).Buffer;
        }

        public override void OnPost(HttpRequest request, HttpResponse response)
        {
            //获取客户端传递的参数
            string data = request.Params == null ? "" : string.Join(";", request.Params.Select(x => x.Key + "=" + x.Value).ToArray());

            //设置返回信息
            string content = string.Format("这是通过Post方式返回的数据:{0}", data);

            //构造响应报文
            response.SetContent(content);
            response.Content_Encoding = "utf-8";
            response.StatusCode = "200";
            response.Content_Type = "text/html; charset=UTF-8";
            response.Headers["Server"] = "ExampleServer";

            //发送响应
            response.Send();
        }

        public override void OnGet(HttpRequest request, HttpResponse response)
        {
            //当文件不存在时应返回404状态码
            string requestURL = request.URL;
            requestURL = requestURL.Replace("/", @"\").Replace("\\..", "").TrimStart('\\');
            string requestFile = Path.Combine(ServerRoot, requestURL.Split('?')[0]);
     
            //判断地址中是否存在扩展名
            string extension = Path.GetExtension(requestFile);

            //根据有无扩展名按照两种不同链接进行处
            if (extension != "")
            {
                //从文件中返回HTTP响应
                response = response.FromFile(requestFile);
            }
            else
            {
                //目录存在且不存在index页面时时列举目录
                if (Directory.Exists(requestFile) && !File.Exists(requestFile + "\\index.html"))
                {
                    requestFile = Path.Combine(ServerRoot, requestFile);
                    var content = ListDirectory(requestFile, requestURL);
                    response = response.SetContent(content, Encoding.UTF8);
                    response.Content_Type = "text/html; charset=UTF-8";
                }
                else
                {
                    //加载静态HTML页面
                    requestFile = Path.Combine(requestFile, "index.html");
                    response = response.FromFile(requestFile);
                    response.Content_Type = "text/html; charset=UTF-8";
                }
            }
            if ("404" == response.StatusCode)
            {
                response.FromFile("html/index.html");
            }
            //发送HTTP响应
            response.Send();
        }

        public override void OnDefault(HttpRequest request, HttpResponse response)
        {

        }

        private string ConvertPath(string[] urls)
        {
            string html = string.Empty;
            int length = ServerRoot.Length;
            foreach (var url in urls)
            {
                var s = url.StartsWith("..") ? url : url.Substring(length).TrimEnd('\\');
                html += String.Format("<li><a href=\"{0}\">{0}</a></li>", s);
            }

            return html;
        }

        private string ListDirectory(string requestDirectory, string requestURL)
        {
            //列举子目录
            var folders = requestURL.Length > 1 ? new string[] { "../" } : new string[] { };
            folders = folders.Concat(Directory.GetDirectories(requestDirectory)).ToArray();
            var foldersList = ConvertPath(folders);

            //列举文件
            var files = Directory.GetFiles(requestDirectory);
            var filesList = ConvertPath(files);

            //构造HTML
            StringBuilder builder = new StringBuilder();
            builder.Append(string.Format("<html><head><title>{0}</title></head>", requestDirectory));
            builder.Append(string.Format("<body><h1>{0}</h1><br/><ul>{1}{2}</ul></body></html>",
                 requestURL, filesList, foldersList));

            return builder.ToString();
        }
        #endregion
    }
}
