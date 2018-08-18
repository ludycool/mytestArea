using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;

namespace NettyServer
{

    public class channel_listener
    {

        // 连接， 管道从不活跃状态  转到  活跃状态 触发 udp没有
     public   delegate void channelActive(IChannelHandlerContext ctx);
        // 断开连接，管道从活跃状态  转到  不活跃状态 触发 udp没有
        public delegate void channelInactive(IChannelHandlerContext ctx);

        //接收新消息udp 用
        public delegate void channelRead0(IChannelHandlerContext ctx, DatagramPacket msg);
        //接收新消息 tcp,webtcp 用
        public delegate void channelRead(IChannelHandlerContext ctx, byte[] msg);

        //接收新消息 web tcp用
        public delegate void channelRead2(IChannelHandlerContext ctx, string msg);
    }
}