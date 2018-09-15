using DotNetty.Buffers;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using NettyServer;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace NettyServer
{
    public class UdpSocketServer : appServerBase, IAppServer
    {
        public UdpSocketServer(ServerConfig _config,
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
        MultithreadEventLoopGroup workerGroup;
        public Task CloseServer()
        {
            try
            {
                if (boundChannel != null)
                {
                    boundChannel.CloseAsync();
                }
                stopSchedulerJob();
                if (workerGroup != null)
                {
                    //释放工作组线程
                    Task.WhenAll(
                       //bossGroup.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1)),
                       workerGroup.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1)));
                }

            }
            catch (Exception ex)
            {

            }
            return Task.CompletedTask;
        }
        public async Task<bool> startServer()
        {


            bool isStarSucees = true;

            // 设置输出日志到Console
            //ExampleHelper.SetConsoleLogger();


            // 工作线程组，默认为内核数*2的线程数
            workerGroup = new MultithreadEventLoopGroup();
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
                          pipeline.AddLast("udpserver", new udpHandler(NewDataReceived));
                          //pipeline.AddLast(new udpHandler(NewDataReceived));

                      }));

                try
                {
                    // bootstrap绑定到指定端口的行为 就是服务端启动服务，同样的Serverbootstrap可以bind到多个端口
                    boundChannel = await bootstrap.BindAsync(config.Port);

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
        protected void NewDataReceived(IChannelHandlerContext ctx, DatagramPacket msg)
        {
            //String id = ctx.Channel.Id.AsLongText();//udp同一个电脑客户端，生成的id都一样
            IPEndPoint ipp = (IPEndPoint)msg.Sender;
            string addr = ipp.Address.ToString();
            String id = ctx.Channel.Id + msg.Sender.ToString();
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
                /*
                if (!session_item.recipient.ToString().Equals(msg.Sender.ToString()))
                {
                    session_item.recipient = msg.Sender;
                    if (OnNewSessionConnected != null)
                    {
                        OnNewSessionConnected(session_item);
                    }
                    // Console.WriteLine("RemoteAddress 有问题");
                }
                 */
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
    }


}