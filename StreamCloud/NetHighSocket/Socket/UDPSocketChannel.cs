using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

/**
* 命名空间: NetHighSocket 
* 类 名： UDPSocketChannel
* 版本 ：v1.0
* Copyright (c) year 
*/

namespace NetHighSocket
{
    /// <summary>
    /// 功能描述    ：UDPSocketChannel  
    /// 创 建 者    ：jinyu
    /// 创建日期    ：2018/10/9 11:50:23 
    /// 最后修改者  ：jinyu
    /// 最后修改日期：2018/10/9 11:50:23 
    /// </summary>
    public class UDPSocketChannel : ISocketChannel
    {
        Socket socket = null;
        private int keepAlive = -1;
        /// <summary>
        /// 关联的数据信息
        /// </summary>
        public SocketArgEvent SocketArgEvent { get; set; }
        /// <summary>
        /// 当前包总长度
        /// </summary>
        public int recSumLen = 0;

        /// <summary>
        /// 是否分包
        /// </summary>
        public byte packType = 0;

        private int recLen = 0;
        private List<ByteBuffer> buffers = null;
        public List<byte[]> RecBuffer;
        private Dictionary<HanderType, IChannelHandler> handlerTypes;
        internal Dictionary<HanderType, IChannelHandler> HandlerTypes { set { handlerTypes = value; } }

        public  bool  IsClose{get;set;}
        /// <summary>
        /// 组包
        /// </summary>
        private void PackData()
        {
            if (RecBuffer == null)
            {
                RecBuffer = new List<byte[]>(2);
            }
            byte[] tmp = new byte[recSumLen];
            int index = 0;//buffer中的拷贝索引
            int offset = 0;//bufer中偏移量，取出头
            int len = 0;//拷贝的长度
            List<ByteBuffer> curBuffer = null;
            if (packType == 0)
            {
                index = 3;
                offset = 0;
            }
            else
            {
                //分包了
                index = 6;
                offset = 6;
            }
            for (int i = 0; i < buffers.Count; i++)
            {
                len = recSumLen > buffers[i].Size ? buffers[i].Size : recSumLen;
                Array.Copy(buffers[i].buffer, buffers[i].Position, tmp, index, len);
                index += buffers[i].Size + offset;
                recSumLen -= buffers[i].Size;
            }
            if (recSumLen < 0)
            {
                //说明最后一个buffer粘包
                //重置获取
                int bufferNum = buffers.Count;
                len = buffers[bufferNum - 1].Size - Math.Abs(recSumLen);//计算数据长度
                index = buffers[bufferNum - 1].Size - len;//计算数据位置
                buffers[bufferNum - 1].Size = len;
                buffers[bufferNum - 1].Position = index;
                curBuffer = new List<ByteBuffer>(1);
                curBuffer.Add(buffers[bufferNum - 1]);

            }
            RecBuffer.Add(tmp);
            BufferManager.GetInstance().FreeBuffers(buffers);
            buffers = null;
            if (curBuffer != null)
            {
                //放在最后处理，再次解析数据包，不改变接收顺序
                AddBuffer(curBuffer, curBuffer[0].Size);
            }
        }

        /// <summary>
        /// 接收的数据回传
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="sum"></param>
        /// <returns></returns>
        public int AddBuffer(List<ByteBuffer> bytes, int sum)
        {
            int recNum = 0;
            if (buffers == null)
            {
                int num = 0;
                recSumLen = BitConverter.ToInt16(bytes[0].buffer, bytes[0].Position);
                packType = bytes[0].buffer[bytes[0].Position + 2];
                num = recSumLen / BufferManager.GetInstance().BufferSize;
                buffers = new List<ByteBuffer>(num + 2);
                
            }
            for (int i = 0; i < bytes.Count; i++)
            {
                buffers.Add(bytes[i]);
                recLen += (short)bytes[i].Size;
                if (recSumLen < recLen)
                {
                    recNum++;
                    PackData();
                }

            }
            return recNum;
        }


        public long ID { get; set; }

        public Socket Socket { get { return socket; } set { socket = value; } }

        bool ISocketChannel.IsClose { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        private void InitSocket()
        {
            if (socket == null)
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            }
        }

        public async Task<bool> BindAsync(EndPoint localAddress)
        {
            try
            {
                InitSocket();
                socket.Bind(localAddress);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public void Close()
        {
            if (socket != null)
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
                socket.Dispose();
                IsClose = true;
            }
        }

        public async Task CloseAsync()
        {
            this.Close();
        }

        public bool Connect(EndPoint endPoint)
        {
            InitSocket();
            socket.Connect(endPoint);
            return true;
        }

        public bool Connect(EndPoint remotePoint, EndPoint localPoint)
        {
            InitSocket();
            socket.Bind(localPoint);
            socket.Connect(remotePoint);
           
            return true;

        }

        public async Task<bool> ConnectAsync(EndPoint endPoint)
        {
           return this.Connect(endPoint);
        }

        public async Task<bool> ConnectAsync(EndPoint remotePoint, EndPoint localPoint)
        {
            return Connect(remotePoint, localPoint);
        }

        public void Write(object message)
        {
            IChannelHandler handler;
            if (handlerTypes.TryGetValue(HanderType.Encoder, out handler))
            {
                handler.WriteAsync(SocketArgEvent, message);
            }
        }

        public void WriteAndFlush(object message)
        {
            IChannelHandler handler;
            if (handlerTypes.TryGetValue(HanderType.Encoder, out handler))
            {
                handler.WriteAsync(SocketArgEvent, message);
            }

        }

        public async Task WriteAndFlushAsync(object message)
        {
            IChannelHandler handler;
            if (handlerTypes.TryGetValue(HanderType.Encoder, out handler))
            {
                await handler.WriteAsync(SocketArgEvent, message);
            }
        }

        public async Task WriteAsync(object message)
        {
            IChannelHandler handler;
            if (handlerTypes.TryGetValue(HanderType.Encoder, out handler))
            {
                await handler.WriteAsync(SocketArgEvent, message);
            }
        }

        public void SetOption(SockOption sockOption, int size)
        {
            switch (sockOption)
            {
                case SockOption.recBufSize:
                    socket.SendBufferSize = size;
                    break;
                case SockOption.sendBufSize:
                    socket.SendBufferSize = size;
                    break;
                case SockOption.ttl:
                    socket.Ttl = (short)size;
                    break;
                case SockOption.reuse:
                    socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                    break;
                case SockOption.receiveTimeout:
                    socket.ReceiveTimeout = size;
                    break;
                case SockOption.sendTimeout:
                    socket.SendTimeout = size;
                    break;
                case SockOption.keepAlive:
                    keepAlive = size;
                    break;

            }
        }

        public void FireChannelActive()
        {

        }

        public void FireChannelInActive()
        {

        }

        public void FireChannelRead(object message)
        {
            //解析数据对象传递的值;
            IChannelHandler handler;
            if (handlerTypes.TryGetValue(HanderType.ReadWrite, out handler))
            {
                handler.ChannelRead(SocketArgEvent, message);
            }
        }

        public void FireChannelReadComplete()
        {

        }

        public void FireChannelWritabilityChanged()
        {

        }

        public void FireExceptionCaught(Exception exception)
        {
            Console.WriteLine(exception.Message);
        }


        /// <summary>
        /// 异步发送数据
        /// </summary>
        /// <param name="data"></param>
        public void SendDataAsync(byte[] data)
        {
            SocketAsyncEventArgs e = new SocketAsyncEventArgs();
            e.SetBuffer(data, 0, data.Length);
            socket.SendAsync(e);
        }

        /// <summary>
        /// 同步发送数据
        /// </summary>
        /// <param name="data"></param>
        public void SendData(byte[] data, int offset = 0, int len = -1)
        {
            if (len == -1)
            {
                len = data.Length;
            }
            socket.Send(data, offset, len, SocketFlags.None);
        }

        /// <summary>
        /// 序列化后的数据
        /// </summary>
        /// <param name="message"></param>
        public void FireChannelWrite(object message)
        {
            if (message != null)
            {
                SendData((byte[])message);
            }
        }
    }
}
