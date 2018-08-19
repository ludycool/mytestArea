using DotNetty.Handlers.Timeout;
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

        public TcpSocketServer(ServerConfig _config, 
            session_listener.OnNewSessionConnected _OnNewSessionConnected,
     session_listener.OnSessionClosed _OnSessionClosed,
      session_listener.OnNewDataReceived _OnNewDataReceived
      )
        {
            config = _config;
            OnNewSessionConnected = _OnNewSessionConnected;
            OnSessionClosed = _OnSessionClosed;
            OnNewDataReceived = _OnNewDataReceived;
        }




  
        IChannel boundChannel;

        public Task CloseServer()
        {
            if (boundChannel != null)
            {
                boundChannel.CloseAsync();
            }
            stopSchedulerJob();
            return Task.CompletedTask;
        }
        public async Task<bool> startServer()
        {
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

                    #region  超时设置
                    //pipeline.AddLast(new ReadTimeoutHandler(config.ClearIdleSessionInterval)); 
                    //  超时设置  参数1 指定时间chanal没收到数据，参数2 指定时间没下发数据chanal 参数3 chanal在指定时间内没收到或没下发，都触发
                    // pipeline.AddLast(new IdleStateHandler(0, 0, config.ClearIdleSessionInterval));//是否接收到，判断不准确总会超时 有bug
                    //pipeline.AddLast(new ChannelInboundHandlerAdapter());
                    #endregion

                }));


                try
                {
                    // bootstrap绑定到指定端口的行为 就是服务端启动服务，同样的Serverbootstrap可以bind到多个端口
                    boundChannel = await b.BindAsync(config.Port);
                    StartSchedulerJob();//定时任务
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
    class ChannelInboundHandlerAdapter : ChannelHandlerAdapter
    {

        public override void UserEventTriggered(IChannelHandlerContext ctx, object evt)
        {

            if (evt is IdleStateEvent)
            {
                IdleStateEvent evv = (IdleStateEvent)evt;
                if (evv.State == IdleState.AllIdle)
                {
                    Console.WriteLine("all time out：" + DateTime.Now.ToString("mm:ss") + "id:" + ctx.Channel.Id);
                }
                else if (evv.State == IdleState.WriterIdle)
                {
                    Console.WriteLine("writer time out：" + DateTime.Now.ToString("mm:ss") + "id:" + ctx.Channel.Id);
                }
                else if (evv.State == IdleState.ReaderIdle)
                {
                    Console.WriteLine("read time out：" + DateTime.Now.ToString("mm:ss") + "id:" + ctx.Channel.Id);

                    // ctx.CloseAsync();
                }
            }
            else
            {
                base.UserEventTriggered(ctx, evt);
            }
        }
    }

}