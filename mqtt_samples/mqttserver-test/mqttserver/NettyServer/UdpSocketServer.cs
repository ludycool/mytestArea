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


            // 工作线程组，默认为内核数*2的线程数
            MultithreadEventLoopGroup workerGroup = new MultithreadEventLoopGroup();
            try
            {
                var bootstrap = new Bootstrap();
                bootstrap
                    .Group(workerGroup)
                    .Channel<SocketDatagramChannel>()
                      .Handler(new ActionChannelInitializer<IDatagramChannel>(channel =>
                      { //工作线程连接器 是设置了一个管道，服务端主线程所有接收到的信息都会通过这个管道一层层往下传输
                        //同时所有出栈的消息 也要这个管道的所有处理器进行一步步处理
                          IChannelPipeline pipeline = channel.Pipeline;
                          // pipeline.AddLast("decoder", new MqttDecoder(true, 256 * 1024));
                          // pipeline.AddLast("encoder", new MqttEncoder());
                          //pipeline.AddLast("echo", new udpHandler(NewDataReceived));
                          pipeline.AddLast(new udpHandler(NewDataReceived));

                      }));

                try
                {
                    // bootstrap绑定到指定端口的行为 就是服务端启动服务，同样的Serverbootstrap可以bind到多个端口
                    boundChannel = await bootstrap.BindAsync(config.Port);

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
                try
                {
                    //释放工作组线程
                    await Task.WhenAll(
                       // bossGroup.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1)),
                        workerGroup.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1)));
                }
                catch (Exception ex_1)
                {

                }
            }
            return isStarSucees;
            // return Task.CompletedTask;
        }



        //新消息
        protected  void NewDataReceived(IChannelHandlerContext ctx, DatagramPacket msg)
        {
            String id = ctx.Channel.Id.AsLongText();
            session session_item = null;
            map_session.TryGetValue(id, out session_item);
            if (session_item == null)
            {
                session_item = new session(id, ctx.Channel, SocketMode.Udp, msg.Sender);
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
                //string oldipp = session_item.recipient.ToString();
                if (!session_item.recipient.ToString().Equals(msg.Sender.ToString()))
                {
                    session_item.recipient = msg.Sender;
                    if (OnNewSessionConnected != null)
                    {
                        OnNewSessionConnected(session_item);
                    }
                    Console.WriteLine("RemoteAddress 有问题");
                }


            }
            if (OnNewDataReceived != null)
            { 
                IByteBuffer directBuf = msg.Content;//获取二进制
                if (directBuf.HasArray)
                {
                    int length = directBuf.ReadableBytes;//得到可读字节数
                    byte[] array = new byte[length];    //分配一个具有length大小的数组
                    directBuf.GetBytes(directBuf.ReaderIndex, array); //将缓冲区中的数据拷贝到这个数组中
                    OnNewDataReceived(session_item, array);
                }
               
            }
            // IByteBuffer directBuf = msg.Content;//获取二进制

        }
        //定时运行 处理session超时 ，udp没有断开连接，连接的概念 UDP映射保持时间15-20s左右，映射就被重置了。
        public void schedulerJob()
        {
            t = new System.Timers.Timer(config.ClearIdleSessionInterval*1000); //设置时间间隔为5秒
            t.Elapsed += new System.Timers.ElapsedEventHandler(run);
            t.AutoReset = true; //每到指定时间Elapsed事件是触发一次（false），还是一直触发（true）
            t.Start();
        }

        public void run(object source, System.Timers.ElapsedEventArgs e)
        {

            try
            {
                Console.WriteLine("定时run：" + DateTime.Now.ToString("mm:ss"));
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
                        double t= (now - sessionItem.activeTime).TotalSeconds;
                        if (t > 20)//udp 超过20秒当做断开连接
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