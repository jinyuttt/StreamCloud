using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

/**
* 命名空间: NetHighSocket 
* 类 名： HttpRequest
* 版本 ：v1.0
* Copyright (c) year 
*/

namespace NetHighSocket
{
    /// <summary>
    /// 功能描述    ：HttpRequest  
    /// 创 建 者    ：jinyu
    /// 创建日期    ：2018/10/7 14:54:59 
    /// 最后修改者  ：jinyu
    /// 最后修改日期：2018/10/7 14:54:59 
    /// </summary>
    public class HttpQuery
    {
        private static string json = "";
        public static bool IsHttp(byte[] buf, int index, int size)
        {
            if (size < 15)
            {
                return false;
            }
            else
            {
                string header = Encoding.UTF8.GetString(buf, index, 8);
                if (header.StartsWith("GET") || header.StartsWith("POST"))
                {
                    //继续判断
                    if (size > 1024)
                    {
                        size = 1024;
                    }
                    header = Encoding.UTF8.GetString(buf, index, size);
                    if (header.Contains("HTTP/1.1") && header.Contains("Connection: keep-alive"))
                    {
                        return true;
                    }
                }
                return false;
            }

        }
        public static byte[] HttpResponse(string content=null)
        {
            StringBuilder sbr = new StringBuilder();
            // sbr.AppendLine("HTTP/1.1 200 OK");
            // sbr.AppendLine("Content-Type: text/html");
            // sbr.AppendLine("Connection: keep-alive");
            // sbr.AppendLine("Content-Encoding: utf-8");
            // sbr.AppendLine(content);
            string headStr = @"HTTP/1.0 200 OK
           Content-Type: text/html
            Connection: keep-alive
            Content-Encoding: utf-8";
            sbr.AppendLine(headStr);
           if(string.IsNullOrEmpty(content))
            {
                if(string.IsNullOrEmpty(json))
                {
                    StreamReader reader = new StreamReader("json.txt");
                    json= reader.ReadToEnd();
                }
                sbr.AppendLine(json);
            }
            return Encoding.UTF8.GetBytes(sbr.ToString());
        }
       
    }
}
