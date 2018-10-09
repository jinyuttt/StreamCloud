using HTTPServerLib;
using NetHighSocket;
using SrvNetSocket;
using StreamCloud;
using StreamCloud.Srv;
using System.Collections.Generic;

/**
* 命名空间: NStStreamCloud 
* 类 名： SeviceProcessor
* 版本 ：v1.0
* Copyright (c) year 
*/

namespace NStStreamCloud
{
    /// <summary>
    /// 功能描述    ：SeviceProcessor  
    /// 创 建 者    ：jinyu
    /// 创建日期    ：2018/10/8 11:55:05 
    /// 最后修改者  ：jinyu
    /// 最后修改日期：2018/10/8 11:55:05 
    /// </summary>
    public class SeviceProcessor
    {
        private static ServiceHttpProcessor serviceHttp = null;
        private static RestController controller = null;

        /// <summary>
        /// 解析数据并且传回
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="channel"></param>
        public static void AnalysisRequest<T>(NetChannel<T> channel)
        {
            //
            if (channel.recData == null)
            {
                return;
            }
            else
            {
                if (typeof(byte[]) == channel.recData.GetType())
                {
                    object data = channel.recData;
                    byte[] req = (byte[])data;
                    //说明是HTTP
                    if (serviceHttp == null)
                    {
                        serviceHttp = new ServiceHttpProcessor();
                    }
                    HttpRequest request = serviceHttp.ProcessRequestReturn(req);
                    SrvRquest srvReq = null;
                    object result = null;
                    string json = null;
                    if (request != null)
                    {
                        srvReq = CreateSrvReq(request);
                        result = InvokeService(srvReq);
                    }
                    else
                    {
                        json = "解析HTTP错误，无法获取请求的参数或者服务名称";
                    }
                    if (result != null)
                    {
                        json = StreamSerializer.JSONObjectToString<object>(result);
                    }
                    int streamLen = 1000;
                    if(!string.IsNullOrEmpty(json))
                    {
                        streamLen = json.Length * 2;
                    }
                    StreamBuffer stream = HTTPStream.GetInstance().GetStreamCompare(streamLen);
                    serviceHttp.ProcessResult(json, request, stream.Buffer);
                    stream.ResetSize((int)stream.Buffer.Length);
                    stream.ResetPostion();//重置位置；
                    int len = stream.Size;//数据长度
                    byte[] tmp = null;
                    ByteBuffer buffer = null;
                    int position = 0;
                    if (len < BufferManager.GetInstance().BufferSize)
                    {
                        //取出缓存Buffer
                        buffer = BufferManager.GetInstance().GetBufferNum(1)[0];
                        tmp = buffer.buffer;
                        position = buffer.Position;
                    }
                    else
                    {
                        tmp = new byte[len];
                    }
                    ISocketChannel socket = channel.channel as ISocketChannel;
                    if (socket != null)
                    {
                        stream.Buffer.Read(tmp, position, len);
                        socket.SendData(tmp, position, len);
                        socket.Close();
                    }
                    if (buffer != null)
                    {
                        //释放取出的缓存
                        BufferManager.GetInstance().FreeBuffer(buffer);
                    }
                    HTTPStream.GetInstance().FreeBuffer(stream);
                }
                else
                {
                    object data = channel.recData;
                    SrvRquest req = (SrvRquest)data;
                    object result = InvokeService(req);
                    if (result != null)
                    {
                        ISocketClient<T> socket = channel.channel as ISocketClient<T>;
                        if (socket != null)
                        {
                            socket.SendData(result);
                        }
                        socket.Close();
                    }
                }

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private static SrvRquest CreateSrvReq(HttpRequest request)
        {
            //
            SrvRquest srvRquest = new SrvRquest();
            if (!string.IsNullOrEmpty(request.URL))
            {
                string[] query = request.URL.Split('/');
                if (query.Length == 2)
                {
                    srvRquest.SrvName = query[0];
                    srvRquest.SrvMethod = query[1];
                    //参数
                    srvRquest.SrvParam = new List<SrvParam>();
                    if (request.Params != null)
                    {
                        foreach (KeyValuePair<string, string> kv in request.Params)
                        {
                            srvRquest.SrvParam.Add(new SrvParam() { ParamValue = kv.Value, ParamName = kv.Key, ParamObj = kv.Value });

                        }
                    }

                }
            }
            return srvRquest;
        }
        /// <summary>
        /// 调用数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private static object InvokeService(SrvRquest request)
        {
            if(controller==null)
            {
                controller = new RestController();
                controller.LoadSrv();
            }
           return controller.SrvInvoke(request);
        }
    }
}
