using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;

namespace NettyServer
{

    public class channel_listener
    {

        // ���ӣ� �ܵ��Ӳ���Ծ״̬  ת��  ��Ծ״̬ ���� udpû��
     public   delegate void channelActive(IChannelHandlerContext ctx);
        // �Ͽ����ӣ��ܵ��ӻ�Ծ״̬  ת��  ����Ծ״̬ ���� udpû��
        public delegate void channelInactive(IChannelHandlerContext ctx);

        //��������Ϣudp ��
        public delegate void channelRead0(IChannelHandlerContext ctx, DatagramPacket msg);
        //��������Ϣ tcp,webtcp ��
        public delegate void channelRead(IChannelHandlerContext ctx, byte[] msg);

        //��������Ϣ web tcp��
        public delegate void channelRead2(IChannelHandlerContext ctx, string msg);
    }
}