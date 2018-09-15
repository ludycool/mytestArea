using DotNetty.Buffers;
using DotNetty.Codecs.Http.WebSockets;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NettyServer
{

    //自定义session
    public class session
    {


        public session(string _channelid, IChannel _channel, SocketMode _socketType, EndPoint _recipient)
        {
            this.channelId = _channelid;
            this.channel = _channel;
            this.socketType = _socketType;
            this.recipient = _recipient;
        }
        #region 基本属性
        //自动生成的 channelId
        public string channelId { set; get; }
        //自定义id
        public string id { set; get; }
        //channel
        public IChannel channel { set; get; }
        //0 tcp, 1 udp
        public SocketMode socketType { set; get; }
        //客户端地址 ip+ 端口
        public EndPoint recipient { set; get; }
        //   最后活跃时间 从1970-0-0开始走过的毫秒数
        public DateTime activeTime;

        /// <summary>
        /// 远程ip 字条串
        /// </summary>
        public string RemoteIp
        {
            get
            {
                IPEndPoint ipp = (IPEndPoint)recipient;
                string real_ip = ipp.Address.ToString();
                int lastIndex = real_ip.LastIndexOf(":");
                if (lastIndex > 0)
                {
                    real_ip = real_ip.Substring(lastIndex + 1);
                }
                return real_ip;
            }
        }
        /// <summary>
        /// 远程ip 端口
        /// </summary>
        public IPEndPoint RemoteEndPoint
        {
            get
            {

                return (IPEndPoint)recipient;
            }
        }
        #endregion

        #region 自定义属性
        public bool isLogin
        {
            set;
            get;
        }
        #endregion

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public Task writeAndFlush(byte[] data)
        {
            if (socketType == SocketMode.Udp)//udp
            {
                return channel.WriteAndFlushAsync(new DatagramPacket(Unpooled.CopiedBuffer(data), recipient));
            }
            else if (socketType == SocketMode.Tcp)
            {//tcp
                return channel.WriteAndFlushAsync(Unpooled.CopiedBuffer(data));
            }
            else
            {//websocket

                return channel.WriteAndFlushAsync(new BinaryWebSocketFrame(Unpooled.CopiedBuffer(data)));
                //return channel.write(new TextWebSocketFrame(stringHelper.byteArrayToStr(data)));
            }
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="str"></param>
        /// <param name="encodeing">编码，Websocket 不用传</param>
        /// <returns></returns>
        public Task writeAndFlush(string str, Encoding encodeing)
        {
            if (socketType == SocketMode.Udp)//udp
            {
                byte[] data = encodeing.GetBytes(str);
                return channel.WriteAndFlushAsync(new DatagramPacket(Unpooled.CopiedBuffer(data), recipient));
            }
            else if (socketType == SocketMode.Tcp)
            {//tcp
                byte[] data = encodeing.GetBytes(str);
                return channel.WriteAndFlushAsync(Unpooled.CopiedBuffer(data));
            }
            else
            {//websocket
                return channel.WriteAndFlushAsync(new TextWebSocketFrame(str));
            }
        }

        //关闭连接
        public void close()
        {
            this.channel.CloseAsync();
        }
    }
}