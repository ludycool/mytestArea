using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;

namespace NettyServer
{

    public class udpHandler : SimpleChannelInboundHandler<byte[]>
    {

        //��������Ϣ tcp,webtcp ��
        channel_listener.channelRead channelRead
        {
            set;
            get;
        }

        public udpHandler(channel_listener.channelRead _channelRead)
        {
            channelRead = _channelRead;
        }

        protected override void ChannelRead0(IChannelHandlerContext ctx, byte[] msg)
        {
            channelRead(ctx, msg);//����
        }
    }
}