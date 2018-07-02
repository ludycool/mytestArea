import java.net.InetAddress;

import io.netty.buffer.ByteBuf;
import io.netty.buffer.Unpooled;
import io.netty.channel.ChannelHandlerContext;
import io.netty.channel.SimpleChannelInboundHandler;
import io.netty.channel.socket.DatagramPacket;
import io.netty.util.CharsetUtil;
import io.netty.util.ReferenceCountUtil;

public class UdpHandler  extends SimpleChannelInboundHandler<DatagramPacket> {
	
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

@Override
protected void messageReceived(ChannelHandlerContext ctx, DatagramPacket msg) throws Exception {
	// TODO Auto-generated method stub
	 // ��ΪNetty��UDP�����˷�װ�����Խ��յ�����DatagramPacket����
    String req = msg.content().toString(CharsetUtil.UTF_8);
    System.out.println(req);

        ctx.writeAndFlush(new DatagramPacket(Unpooled.copiedBuffer(
                "�����"+req, CharsetUtil.UTF_8), msg.sender()));
    
}

}
