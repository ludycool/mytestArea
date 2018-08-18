using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace NettyServer
{
    public class TcpSocketServer : appServerBase, IAppServer
    {

        public TcpSocketServer(session_listener.OnNewSessionConnected _OnNewSessionConnected,
     session_listener.OnSessionClosed _OnSessionClosed,
      session_listener.OnNewDataReceived _OnNewDataReceived
      )
        {
            OnNewSessionConnected = _OnNewSessionConnected;
            OnSessionClosed = _OnSessionClosed;
            OnNewDataReceived = _OnNewDataReceived;
        }




        ServerConfig config;
        IChannel boundChannel;

        public Task CloseServer()
        {
            if (boundChannel != null)
            {
                boundChannel.CloseAsync();
            }
            return Task.CompletedTask;
        }
        public async Task<bool> startServer(ServerConfig _config)
        {

            config = _config;
            Mode =SocketMode.Tcp;
            bool isStarSucees = true;

            // 设置输出日志到Console
            //ExampleHelper.SetConsoleLogger();

            // 主工作线程组，设置为1个线程
            MultithreadEventLoopGroup bossGroup = new MultithreadEventLoopGroup(1);
            // 工作线程组，默认为内核数*2的线程数
            MultithreadEventLoopGroup workerGroup = new MultithreadEventLoopGroup();

            try
            {
                ServerBootstrap b = new ServerBootstrap();
                b.Group(bossGroup, workerGroup)// 设置主和工作线程组
                  .Channel<TcpServerSocketChannel>()// 设置通道模式为TcpSocket
                .ChildHandler(new ActionChannelInitializer<ISocketChannel>(channel =>
                { //工作线程连接器 是设置了一个管道，服务端主线程所有接收到的信息都会通过这个管道一层层往下传输
                  //同时所有出栈的消息 也要这个管道的所有处理器进行一步步处理
                    IChannelPipeline pipeline = channel.Pipeline;
                    // pipeline.AddLast("decoder", new MqttDecoder(true, 256 * 1024));
                    // pipeline.AddLast("encoder", new MqttEncoder());
                   pipeline.AddLast("echo", new tcpHandler(NewSessionConnected, SessionClosed, NewDataReceived));
                  
                    //pipeline.AddLast( new tcpHandler(NewSessionConnected, SessionClosed, NewDataReceived));
                }));


                try
                {
                    // bootstrap绑定到指定端口的行为 就是服务端启动服务，同样的Serverbootstrap可以bind到多个端口
                    boundChannel = await b.BindAsync(config.Port);

                    //关闭服务
                    // await boundChannel.CloseAsync();
                }
                catch (Exception ex)
                {
                    //Console.WriteLine(ex.StackTrace);
                    // Log4jHelper.logger.error("tcp Server启动失败", e);
                    isStarSucees = false;
                }

            }
            catch (Exception ex)
            {
                // Log4jHelper.logger.error("tcp Server启动失败", e);
                // Console.WriteLine(ex.StackTrace);
                isStarSucees = false;

                try
                {
                    //释放工作组线程
                    await Task.WhenAll(
                        bossGroup.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1)),
                        workerGroup.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1)));
                }
                catch (Exception ex_1)
                {

                }
            }
            return isStarSucees;
            // return Task.CompletedTask;
        }
    }


}