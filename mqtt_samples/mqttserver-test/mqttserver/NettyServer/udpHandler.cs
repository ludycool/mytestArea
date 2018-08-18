using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using System;

namespace NettyServer
{

    public class udpHandler : SimpleChannelInboundHandler<DatagramPacket>
    {

        //接收新消息 tcp,webtcp 用
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
                // Log4jHelper.logger.error("channelRead 处理出错", e);
            }
        }
    }
}