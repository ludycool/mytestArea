using DotNetty.Buffers;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using NettyServer;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NettyServer
{
    public class UdpSocketServer : appServerBase, IAppServer
    {
        public UdpSocketServer(session_listener.OnNewSessionConnected _OnNewSessionConnected,
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
        static System.Timers.Timer t;
        public Task CloseServer()
        {
            if (boundChannel != null)
            {
                boundChannel.CloseAsync();
            }
            if (t != null)
            {
                t.Stop();
            }
            return Task.CompletedTask;
        }
        public async Task<bool> startServer(ServerConfig _config)
        {

            config = _config;
            Mode = SocketMode.Udp;
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
                .ChildHandler(new ActionChannelInitializer<IDatagramChannel>(channel =>
                { //工作线程连接器 是设置了一个管道，服务端主线程所有接收到的信息都会通过这个管道一层层往下传输
                  //同时所有出栈的消息 也要这个管道的所有处理器进行一步步处理
                    IChannelPipeline pipeline = channel.Pipeline;
                    // pipeline.AddLast("decoder", new MqttDecoder(true, 256 * 1024));
                    // pipeline.AddLast("encoder", new MqttEncoder());
                    pipeline.AddLast("echo", new udpHandler(NewDataReceived));
                }));


                try
                {
                    // bootstrap绑定到指定端口的行为 就是服务端启动服务，同样的Serverbootstrap可以bind到多个端口
                    boundChannel = await b.BindAsync(config.Port);

                    schedulerJob();//定时任务
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
            }
            finally
            {
                //释放工作组线程
                await Task.WhenAll(
                    bossGroup.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1)),
                    workerGroup.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1)));
            }
            return isStarSucees;
            // return Task.CompletedTask;
        }



        //新消息
        protected override void NewDataReceived(IChannelHandlerContext ctx, byte[] msg)
        {
            String id = ctx.Channel.Id.AsLongText();
            session session_item = map_session[id];
            if (session_item == null)
            {
                session_item = new session(id, ctx.Channel, SocketMode.Udp);
                session_item.activeTime = DateTime.Now;//更新时间
                map_session.TryAdd(id, session_item);
                if (OnNewSessionConnected != null)
                {
                    OnNewSessionConnected(session_item);
                }
            }
            else
            {
                session_item.activeTime = DateTime.Now;//更新时间
                                                       // InetSocketAddress old=session_item.getRecipient();
                                                       // InetSocketAddress newp= msg.sender();
                if (session_item.channel.RemoteAddress != ctx.Channel.RemoteAddress)
                {
                    if (OnNewSessionConnected != null)
                    {
                        OnNewSessionConnected(session_item);
                    }
                    Console.WriteLine("RemoteAddress 有问题");
                }


            }
            if (OnNewDataReceived != null)
            {
                OnNewDataReceived(session_item, msg);
            }
            // IByteBuffer directBuf = msg.Content;//获取二进制

        }
        //定时运行 处理session超时 ，udp没有断开连接，连接的概念 UDP映射保持时间15-20s左右，映射就被重置了。
        public void schedulerJob()
        {
            t = new System.Timers.Timer(config.ClearIdleSessionInterval); //设置时间间隔为5秒
            t.Elapsed += new System.Timers.ElapsedEventHandler(run);
            t.AutoReset = true; //每到指定时间Elapsed事件是触发一次（false），还是一直触发（true）
            t.Start();
        }

        public void run(object source, System.Timers.ElapsedEventArgs e)
        {

            try
            {
                // Log4jHelper.Debug("udp 定时执行ClearIdleSession 移除超时session");
                //不使用泛型
                KeyValuePair<string, session>[] setkp = map_session.ToArray();
                DateTime now = DateTime.Now;
                if (setkp != null && setkp.Length > 0)
                {
                    foreach (var entry in setkp)
                    {
                        //其他代码

                        string key = entry.Key;
                        session sessionItem = entry.Value;
                        if ((now - sessionItem.activeTime).TotalSeconds > 20)//udp 超过20秒当做断开连接
                        {
                            session out1;
                            map_session.TryRemove(key, out out1);
                            if (OnSessionClosed != null)
                            {
                                OnSessionClosed(sessionItem);//断开连接
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log4jHelper.logger.error("udp 定时执行ClearIdleSession 移除超时session 出错", ex);
            }

        }
    }


}