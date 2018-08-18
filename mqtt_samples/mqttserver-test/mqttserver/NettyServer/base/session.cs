using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using System;
using System.Net;
using System.Threading.Tasks;

namespace NettyServer
{

    //�Զ���session
    public class session
    {


        public session(string _channelid, IChannel _channel, SocketMode _socketType, EndPoint _recipient)
        {
            this.channelId = _channelid;
            this.channel = _channel;
            this.socketType = _socketType;
            this.recipient = _recipient;
        }
        #region ��������
        //�Զ����ɵ� channelId
        public string channelId { set; get; }
        //�Զ���id
        public string id { set; get; }
        //channel
        public IChannel channel { set; get; }
        //0 tcp, 1 udp
        public SocketMode socketType { set; get; }
        //�ͻ��˵�ַ ip+ �˿�
        public EndPoint recipient { set; get; }
        //   ����Ծʱ�� ��1970-0-0��ʼ�߹��ĺ�����
        public DateTime activeTime;
        #endregion

        #region �Զ�������
        public bool isLogin
        {
            set;
            get;
        }
        #endregion
        //������Ϣ
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

        //�ر�����
        public void close()
        {
            this.channel.CloseAsync();
        }
    }
}