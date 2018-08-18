
using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using System;

namespace NettyServer
{
    public class tcpHandler : ChannelHandlerAdapter
    {

        // ���ӣ� �ܵ��Ӳ���Ծ״̬  ת��  ��Ծ״̬ ���� udpû��
        channel_listener.channelActive channelActive
        {
            set;
            get;
        }
        // �Ͽ����ӣ��ܵ��ӻ�Ծ״̬  ת��  ����Ծ״̬ ���� udpû��
        channel_listener.channelInactive channelInactive
        {
            set;
            get;
        }

        //��������Ϣ tcp,webtcp ��
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
                    int length = directBuf.ReadableBytes;//�õ��ɶ��ֽ���
                    byte[] array = new byte[length];    //����һ������length��С������
                    directBuf.GetBytes(directBuf.ReaderIndex, array); //���������е����ݿ��������������
                    channelRead(ctx, array);
                }
            }
            catch (Exception ex)
            {
                // Log4jHelper.logger.error("channelRead �������", e);
            }
            /*
                       
                        channelRead(ctx, msg);
                        try
                        {
                            if (!DirectBuf.hasArray())
                            {//false��ʾΪ����ֱ�ӻ���
                                int length = directBuf.readableBytes();//�õ��ɶ��ֽ���
                                byte[] array = new byte[length];    //����һ������length��С������
                                directBuf.getBytes(directBuf.readerIndex(), array); //���������е����ݿ��������������
                                session mysession = map_session.get(id);
                                OnNewDataReceived(mysession, array);
                            }
                        }
                        catch (Exception e)
                        {
                           // Log4jHelper.logger.error("channelRead �������", e);
                        }
                        finally
                        {
                            directBuf.release();
                            //ReferenceCountUtil.release(msg);
                        }
                        // System.out.print(in.toString(CharsetUtil.UTF_8));
                        //ctx.writeAndFlush(msg);//right
                        //ctx.channel().writeAndFlush(msg);//Ҳ����
                        */
        }

        // �ܵ��Ӳ���Ծ״̬  ת��  ��Ծ״̬ ����

        public override void ChannelActive(IChannelHandlerContext ctx)
        {

            // System.out.println("RamoteAddress : " + ctx.channel().remoteAddress() + " active !");
            // ctx.writeAndFlush( "Welcome to " + InetAddress.getLocalHost().getHostName() + " service!\n");
            channelActive(ctx);
            base.ChannelActive(ctx);
        }

        // �ܵ��ӻ�Ծ״̬  ת��  ����Ծ״̬ ����
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