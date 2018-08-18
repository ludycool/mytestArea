using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;

namespace NettyServer
{

    public class udpHandler : SimpleChannelInboundHandler<byte[]>
    {

        //接收新消息 tcp,webtcp 用
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
            channelRead(ctx, msg);//调用
        }
    }
}