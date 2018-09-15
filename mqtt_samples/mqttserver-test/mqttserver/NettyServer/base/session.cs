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

        /// <summary>
        /// Զ��ip ������
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
        /// Զ��ip �˿�
        /// </summary>
        public IPEndPoint RemoteEndPoint
        {
            get
            {

                return (IPEndPoint)recipient;
            }
        }
        #endregion

        #region �Զ�������
        public bool isLogin
        {
            set;
            get;
        }
        #endregion

        /// <summary>
        /// ������Ϣ
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
        /// ������Ϣ
        /// </summary>
        /// <param name="str"></param>
        /// <param name="encodeing">���룬Websocket ���ô�</param>
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

        //�ر�����
        public void close()
        {
            this.channel.CloseAsync();
        }
    }
}