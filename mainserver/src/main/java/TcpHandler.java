

import java.net.InetAddress;

import io.netty.buffer.ByteBuf;
import io.netty.channel.ChannelHandlerContext;
import io.netty.channel.SimpleChannelInboundHandler;
import io.netty.util.CharsetUtil;
public class TcpHandler extends SimpleChannelInboundHandler<String> {

	 /**
     * �������Ǹ�����chanelRead()�¼��������� ÿ���ӿͻ����յ��µ�����ʱ�� ������������յ���Ϣʱ�����ã�
     * ��������У��յ�����Ϣ��������ByteBuf
     * 
     * @param ctx
     *            ͨ���������������Ϣ
     * @param msg
     *            ���յ���Ϣ
     */
    @Override
    public void channelRead(ChannelHandlerContext ctx, Object msg) {

        try {
            ByteBuf in = (ByteBuf) msg;
            // ��ӡ�ͻ������룬��������ĵ��ַ�
           System.out.print(in.toString(CharsetUtil.UTF_8));
           ctx.writeAndFlush(msg);
        } finally {
            /**
             * ByteBuf��һ�����ü�������������������ʾ�ص���release()�������ͷš�
             * ���ס��������ְ�����ͷ����д��ݵ������������ü�������
             */
            // �����յ�������
           // ReferenceCountUtil.release(msg);
        }

    }

    /***
     * ����������ڷ����쳣ʱ����
     * 
     * @param ctx
     * @param cause
     */
    @Override
    public void exceptionCaught(ChannelHandlerContext ctx, Throwable cause) throws Exception {
        /**
         * exceptionCaught() �¼��������ǵ����� Throwable ����Żᱻ���ã����� Netty ���� IO
         * ������ߴ������ڴ����¼�ʱ�׳����쳣ʱ���ڴ󲿷�����£�������쳣Ӧ�ñ���¼���� ���Ұѹ����� channel
         * ���رյ���Ȼ����������Ĵ���ʽ����������ͬ�쳣��������в� ͬ��ʵ�֣�������������ڹر�����֮ǰ����һ�����������Ӧ��Ϣ��
         */
        // �����쳣�͹ر�
        cause.printStackTrace();
        ctx.close();
    }



    
    /*
     * 
     * ���� channelActive ���� ��channel�����õ�ʱ�򴥷� (�ڽ������ӵ�ʱ��)
     * 
     * channelActive �� channelInActive �ں���������н����������Ȳ�����ϸ������
     * */
    @Override
    public void channelActive(ChannelHandlerContext ctx) throws Exception {
        
        System.out.println("RamoteAddress : " + ctx.channel().remoteAddress() + " active !");
        
        ctx.writeAndFlush( "Welcome to " + InetAddress.getLocalHost().getHostName() + " service!\n");
        
        super.channelActive(ctx);
    }

	@Override
	protected void messageReceived(ChannelHandlerContext ctx, String msg) throws Exception {
		// TODO Auto-generated method stub
		  System.out.print(msg);
	}

}
