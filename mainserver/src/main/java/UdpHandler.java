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
    * 这个方法会在发生异常时触发
    * 
    * @param ctx
    * @param cause
    */
   @Override
   public void exceptionCaught(ChannelHandlerContext ctx, Throwable cause) throws Exception {
       /**
        * exceptionCaught() 事件处理方法是当出现 Throwable 对象才会被调用，即当 Netty 由于 IO
        * 错误或者处理器在处理事件时抛出的异常时。在大部分情况下，捕获的异常应该被记录下来 并且把关联的 channel
        * 给关闭掉。然而这个方法的处理方式会在遇到不同异常的情况下有不 同的实现，比如你可能想在关闭连接之前发送一个错误码的响应消息。
        */
       // 出现异常就关闭
       cause.printStackTrace();
       ctx.close();
   }

@Override
protected void messageReceived(ChannelHandlerContext ctx, DatagramPacket msg) throws Exception {
	// TODO Auto-generated method stub
	 // 因为Netty对UDP进行了封装，所以接收到的是DatagramPacket对象。
    String req = msg.content().toString(CharsetUtil.UTF_8);
    System.out.println(req);

        ctx.writeAndFlush(new DatagramPacket(Unpooled.copiedBuffer(
                "结果："+req, CharsetUtil.UTF_8), msg.sender()));
    
}

}
