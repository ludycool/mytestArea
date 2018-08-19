using DotNetty.Buffers;
using DotNetty.Codecs.Http;
using DotNetty.Codecs.Http.WebSockets;
using DotNetty.Common;
using DotNetty.Handlers.Timeout;
using DotNetty.Handlers.Tls;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using DotNetty.Transport.Libuv;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using WebSockets.Server;

namespace NettyServer
{
    public class WebTcpSocketServer : appServerBase, IAppServer
    {
        public WebTcpSocketServer(ServerConfig _config,
            session_listener.OnNewSessionConnected _OnNewSessionConnected,
session_listener.OnSessionClosed _OnSessionClosed,
session_listener.OnNewDataReceived _OnNewDataReceived,
session_listener.OnNewStringReceived _OnNewStringReceived
)
        {
            config = _config;
            OnNewSessionConnected = _OnNewSessionConnected;
            OnSessionClosed = _OnSessionClosed;
            OnNewDataReceived = _OnNewDataReceived;
            OnNewStringReceived = _OnNewStringReceived;
            IsSsl = config.isTls;

        }
        static bool UseLibuv = false;
        static bool IsSsl = false;
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
            bool isSuccess = true;
            ResourceLeakDetector.Level = ResourceLeakDetector.DetectionLevel.Disabled;

            //Console.WriteLine(
            //        $"\n{RuntimeInformation.OSArchitecture} {RuntimeInformation.OSDescription}"
            //        + $"\n{RuntimeInformation.ProcessArchitecture} {RuntimeInformation.FrameworkDescription}"
            //        + $"\nProcessor Count : {Environment.ProcessorCount}\n");

            bool useLibuv = UseLibuv;
            //Console.WriteLine("Transport type : " + (useLibuv ? "Libuv" : "Socket"));

            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                GCSettings.LatencyMode = GCLatencyMode.SustainedLowLatency;
            }

            Console.WriteLine($"Server garbage collection : {(GCSettings.IsServerGC ? "Enabled" : "Disabled")}");
            Console.WriteLine($"Current latency mode for garbage collection: {GCSettings.LatencyMode}");
            Console.WriteLine("\n");

            IEventLoopGroup bossGroup;
            IEventLoopGroup workGroup;
            if (useLibuv)
            {
                var dispatcher = new DispatcherEventLoopGroup();
                bossGroup = dispatcher;
                workGroup = new WorkerEventLoopGroup(dispatcher);
            }
            else
            {
                bossGroup = new MultithreadEventLoopGroup(1);
                workGroup = new MultithreadEventLoopGroup();
            }
            try
            {
                X509Certificate2 tlsCertificate = null;
                if (IsSsl)
                {
                    tlsCertificate = new X509Certificate2(Path.Combine(ServerHelper.ProcessDirectory, config.certificate_path), config.certificate_pwd);
                }

                var bootstrap = new ServerBootstrap();
                bootstrap.Group(bossGroup, workGroup);

                if (useLibuv)
                {
                    bootstrap.Channel<TcpServerChannel>();
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)
                        || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                    {
                        bootstrap
                            .Option(ChannelOption.SoReuseport, true)
                            .ChildOption(ChannelOption.SoReuseaddr, true);
                    }
                }
                else
                {
                    bootstrap.Channel<TcpServerSocketChannel>();
                }

                bootstrap
                    .Option(ChannelOption.SoBacklog, 8192)
                    .ChildHandler(new ActionChannelInitializer<IChannel>(channel =>
                    {
                        IChannelPipeline pipeline = channel.Pipeline;
                        if (tlsCertificate != null)
                        {
                            pipeline.AddLast(TlsHandler.Server(tlsCertificate));
                        }
                        pipeline.AddLast(new HttpServerCodec());
                        pipeline.AddLast(new HttpObjectAggregator(65536));
                        pipeline.AddLast(new WebSocketServerHandler(NewSessionConnected, SessionClosed, NewDataReceived));

                        // ��60sû���յ���Ϣ ��ʱ����
                        pipeline.AddLast(new IdleStateHandler(0, 0, config.ClearIdleSessionInterval));
                    }));

                int port = config.Port;
                boundChannel = await bootstrap.BindAsync(port);
                StartSchedulerJob();//��ʱ����
                Console.WriteLine("Open your web browser and navigate to "
                    + $"{(IsSsl ? "https" : "http")}"
                    + $"://127.0.0.1:{port}/");
                Console.WriteLine("Listening on "
                    + $"{(IsSsl ? "wss" : "ws")}"
                    + $"://127.0.0.1:{port}/websocket");
            }
            catch (Exception ex)
            {
                isSuccess = false;
                try
                {
                    //�ͷŹ������߳�
                    await Task.WhenAll(
                        bossGroup.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1)),
                        workGroup.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1)));
                }
                catch (Exception ex_1)
                {

                }
            }
            return isSuccess;
        }


        //����Ϣ
        protected void NewDataReceived(IChannelHandlerContext ctx, WebSocketFrame frame)
        {
            String id = ctx.Channel.Id.AsLongText();
            session mysession = null;
            map_session.TryGetValue(id, out mysession);
            if (mysession == null)
                return;
            mysession.activeTime = DateTime.Now;//����ʱ��


            if (frame is TextWebSocketFrame)//\�ı���Ϣ\
            {
                TextWebSocketFrame fr = frame as TextWebSocketFrame;

                string data = fr.Text();
                // Echo the frame
                // ctx.WriteAsync(frame.Retain());
                #region �������

                if (OnNewStringReceived != null)
                {
                    OnNewStringReceived(mysession, data);
                }

                #endregion
                return;
            }

            if (frame is BinaryWebSocketFrame)//֧�ֶ�������Ϣ
            {
                BinaryWebSocketFrame fr = frame as BinaryWebSocketFrame;

                IByteBuffer directBuf = fr.Content;//��ȡ������
                if (directBuf.HasArray)
                {
                    int length = directBuf.ReadableBytes;//�õ��ɶ��ֽ���
                    byte[] array = new byte[length];    //����һ������length��С������
                    directBuf.GetBytes(directBuf.ReaderIndex, array); //���������е����ݿ��������������

                    #region �������

                    if (OnNewDataReceived != null)
                    {
                        OnNewDataReceived(mysession, array);
                    }
                    #endregion
                }

                // Echo the frame
                //  ctx.WriteAsync(frame.Retain());
            }
        }

    }
}