
using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using System;

namespace NettyServer
{
    public class tcpHandler : ChannelHandlerAdapter
    {

        // 连接， 管道从不活跃状态  转到  活跃状态 触发 udp没有
        channel_listener.channelActive channelActive
        {
            set;
            get;
        }
        // 断开连接，管道从活跃状态  转到  不活跃状态 触发 udp没有
        channel_listener.channelInactive channelInactive
        {
            set;
            get;
        }

        //接收新消息 tcp,webtcp 用
        channel_listener.channelRead channelRead
        {
            set;
            get;
        }

        public tcpHandler(channel_listener.channelActive _mychannelActive,
            channel_listener.channelInactive _mychannelInactive,
             channel_listener.channelRead _mychannelRead
            )
        {
            channelActive = _mychannelActive;
            channelInactive = _mychannelInactive;
            channelRead = _mychannelRead;
        }

        public override void ChannelRead(IChannelHandlerContext ctx, object msg)
        {
            try
            {
                IByteBuffer directBuf = (IByteBuffer)msg;
                if (directBuf.HasArray)
                {
                    int length = directBuf.ReadableBytes;//得到可读字节数
                    byte[] array = new byte[length];    //分配一个具有length大小的数组
                    directBuf.GetBytes(directBuf.ReaderIndex, array); //将缓冲区中的数据拷贝到这个数组中
                    channelRead(ctx, array);
                }
            }
            catch (Exception ex)
            {
                // Log4jHelper.logger.error("channelRead 处理出错", e);
            }
            /*
                       
                        channelRead(ctx, msg);
                        try
                        {
                            if (!DirectBuf.hasArray())
                            {//false表示为这是直接缓冲
                                int length = directBuf.readableBytes();//得到可读字节数
                                byte[] array = new byte[length];    //分配一个具有length大小的数组
                                directBuf.getBytes(directBuf.readerIndex(), array); //将缓冲区中的数据拷贝到这个数组中
                                session mysession = map_session.get(id);
                                OnNewDataReceived(mysession, array);
                            }
                        }
                        catch (Exception e)
                        {
                           // Log4jHelper.logger.error("channelRead 处理出错", e);
                        }
                        finally
                        {
                            directBuf.release();
                            //ReferenceCountUtil.release(msg);
                        }
                        // System.out.print(in.toString(CharsetUtil.UTF_8));
                        //ctx.writeAndFlush(msg);//right
                        //ctx.channel().writeAndFlush(msg);//也可以
                        */
        }

        // 管道从不活跃状态  转到  活跃状态 触发

        public override void ChannelActive(IChannelHandlerContext ctx)
        {

            // System.out.println("RamoteAddress : " + ctx.channel().remoteAddress() + " active !");
            // ctx.writeAndFlush( "Welcome to " + InetAddress.getLocalHost().getHostName() + " service!\n");
            channelActive(ctx);
            base.ChannelActive(ctx);
        }

        // 管道从活跃状态  转到  不活跃状态 触发
        public override void ChannelInactive(IChannelHandlerContext ctx)
        {

            //IChannel channel = ctx.Channel;
            //if (!channel.Open)
            //{


            //}
            channelInactive(ctx);
            base.ChannelInactive(ctx);
        }

        /*  @Override
          protected void messageReceived(ChannelHandlerContext ctx, String msg) throws Exception {
              // TODO Auto-generated method stub
              System.out.print(msg);
          }
      */
    }
}