using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using System;
using System.Net;
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
        #endregion

        #region 自定义属性
        public bool isLogin
        {
            set;
            get;
        }
        #endregion
        //发送消息
        public Task writeAndFlush(byte[] data)
        {
            if (socketType == SocketMode.Udp)//udp
            {
                return channel.WriteAndFlushAsync(new DatagramPacket(Unpooled.CopiedBuffer(data), recipient));
            }
            else //if (socketType == SocketMode.Tcp)
            {//tcp
                return channel.WriteAndFlushAsync(Unpooled.CopiedBuffer(data));
            }
            // else
            // {//websocket

            // return channel.WriteAndFlushAsync(new BinaryWebSocketFrame(Unpooled.CopiedBuffer(data)));
            // return   channel.write(new TextWebSocketFrame(stringHelper.byteArrayToStr(data)));
            // }
        }

        //public Task writeAndFlush(string data)
        //{
        //    if (socketType == SocketMode.Udp)//udp
        //    {
        //        return channel.WriteAndFlushAsync(new DatagramPacket(Unpooled.CopiedBuffer(stringHelper.strToByteArray(data)), recipient));
        //    }
        //    else // if (socketType == SocketMode.Tcp)
        //    {//tcp
        //        return channel.WriteAndFlushAsync(Unpooled.CopiedBuffer(stringHelper.strToByteArray(data)));
        //    }
        //   /* else
        //    {//websocket
        //        return channel.WriteAsync(new textWebSocketFrame(data));
        //    }
        //    */
        //}

        //关闭连接
        public void close()
        {
            this.channel.CloseAsync();
        }
    }
}