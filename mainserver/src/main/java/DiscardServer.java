
import io.netty.bootstrap.Bootstrap;
import io.netty.bootstrap.ServerBootstrap;
import io.netty.channel.ChannelFuture;
import io.netty.channel.ChannelInitializer;
import io.netty.channel.EventLoopGroup;
import io.netty.channel.nio.NioEventLoopGroup;
import io.netty.channel.socket.SocketChannel;
import io.netty.channel.socket.nio.NioDatagramChannel;
import io.netty.channel.socket.nio.NioServerSocketChannel;


/**
 * 丢弃任何进入的数据 启动服务端的DiscardServerHandler
 */
public class DiscardServer {
    private int port;

    public DiscardServer(int port) {
        super();
        this.port = port;
    }

    public static void main(String[] args) throws Exception {
    	   EventLoopGroup bossGroup = new NioEventLoopGroup();
           EventLoopGroup workerGroup = new NioEventLoopGroup();
           try {
               ServerBootstrap b = new ServerBootstrap();
               b.group(bossGroup, workerGroup);
               b.channel(NioServerSocketChannel.class);
              /* b.childHandler(new ChannelInitializer<SocketChannel>(){

				@Override
				protected void initChannel(SocketChannel ch) throws Exception {
					// TODO Auto-generated method stub
					ch.pipeline().addLast("handler", new TcpHandler());
				}
            	   
               });
               	*/
               b.childHandler(new TcpHandler());
               // 服务器绑定端口监听
               ChannelFuture f = b.bind(8095).sync();
               // 监听服务器关闭监听
               f.channel().closeFuture().sync();


           } finally {
               bossGroup.shutdownGracefully();
               workerGroup.shutdownGracefully();
           }
         
          /*
    	  final NioEventLoopGroup nioEventLoopGroup = new NioEventLoopGroup();

          Bootstrap bootstrap = new Bootstrap();
          bootstrap.channel(NioDatagramChannel.class);
          bootstrap.group(nioEventLoopGroup);
          bootstrap.handler(new UdpHandler());
          // 监听端口 udp
          bootstrap.bind(8095).sync();
         // ChannelFuture sync = bootstrap.bind(8090).sync();
          Runtime.getRuntime().addShutdownHook(new Thread(new Runnable() {
              public void run() {
                  nioEventLoopGroup.shutdownGracefully();
              }
          }));
    	*/
    }
}