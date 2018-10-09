using System;
using System.Collections.Generic;
using System.Text;

/**
* 命名空间: NetSocket 
* 类 名： HtttpRequest
* 版本 ：v1.0
* Copyright (c) year 
*/

namespace NetSocket
{
    /// <summary>
    /// 功能描述    ：HtttpRequest  
    /// 创 建 者    ：jinyu
    /// 创建日期    ：2018/10/3 4:16:43 
    /// 最后修改者  ：jinyu
    /// 最后修改日期：2018/10/3 4:16:43 
    /// </summary>
   public class HtttpRequest
    {
        public string Method { get; private set; }
        public string URL { get; private set; }
        public Dictionary<string, string> Params { get; private set; }

        /*
GET /?num1=23&num2=12 HTTP/1.1
Accept: text/html, application/xhtml+xml, image/jxr, 
Accept-Language: zh-Hans-CN,zh-Hans;q=0.5
User-Agent: Mozilla/5.0 (Windows NT 10.0) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/46.0.2486.0 Safari/537.36 Edge/13.10586
Accept-Encoding: gzip, deflate
Host: localhost:4040
Connection: Keep-Alive
Cookie: _ga=GA1.1.1181222800.1463541781
*/
        /*
         * 
         * POST / HTTP/1.1
Accept: text/html, application/xhtml+xml, image/jxr, 
Accept-Language: zh-Hans-CN,zh-Hans;q=0.5
User-Agent: Mozilla/5.0 (Windows NT 10.0) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/46.0.2486.0 Safari/537.36 Edge/13.10586
Accept-Encoding: gzip, deflate
Host: localhost:4040
Connection: Keep-Alive
Cookie: _ga=GA1.1.1181222800.1463541781
         */
        //string statusLine = "HTTP/1.1 200 OK\r\n";

        protected Dictionary<string, string> GetRequestParams(string content)
        {
            //防御编程
            if (string.IsNullOrEmpty(content))
                return null;

            //按照&对字符进行分割
            string[] reval = content.Split('&');
            if (reval.Length <= 0)
                return null;

            //将结果添加至字典
            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach (string val in reval)
            {
                string[] kv = val.Split('=');
                if (kv.Length <= 1)
                    dict.Add(kv[0], "");
                dict.Add(kv[0], kv[1]);
            }

            //返回字典
            return dict;
        }
        public void ProcessHandler(string content)
        {
            //if (this.Method == "GET" && this.URL.Contains('?'))
            //    this.Params = GetRequestParams(lines[0].Split(' ')[1].Split('?')[1]);
            //if (this.Method == "POST")
            //    this.Params = GetRequestParams(lines[lines.Length - 1]);
        }

        private Dictionary<string, string> GetRequestParams(object p)
        {
            throw new NotImplementedException();
        }
    }
}
