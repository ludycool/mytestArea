using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using System;

namespace NettyServer
{

    public class udpHandler : SimpleChannelInboundHandler<DatagramPacket>
    {

        //��������Ϣ tcp,webtcp ��
        channel_listener.channelRead0 channelRead
        {
            set;
            get;
        }

        public udpHandler(channel_listener.channelRead0 _channelRead)
        {
            channelRead = _channelRead;
        }


        protected override void ChannelRead0(IChannelHandlerContext ctx, DatagramPacket msg)
        {
            try
            {
              
                channelRead(ctx, msg);

            }
            catch (Exception ex)
            {
                // Log4jHelper.logger.error("channelRead �������", e);
            }
        }
    }
}