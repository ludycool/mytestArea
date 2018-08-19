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

            // ���������־��Console
            //ExampleHelper.SetConsoleLogger();

            // �������߳��飬����Ϊ1���߳�
            MultithreadEventLoopGroup bossGroup = new MultithreadEventLoopGroup(1);
            // �����߳��飬Ĭ��Ϊ�ں���*2���߳���
            MultithreadEventLoopGroup workerGroup = new MultithreadEventLoopGroup();

            try
            {
                ServerBootstrap b = new ServerBootstrap();
                b.Group(bossGroup, workerGroup)// �������͹����߳���
                  .Channel<TcpServerSocketChannel>()// ����ͨ��ģʽΪTcpSocket
                .ChildHandler(new ActionChannelInitializer<ISocketChannel>(channel =>
                { //�����߳������� ��������һ���ܵ�����������߳����н��յ�����Ϣ����ͨ������ܵ�һ������´���
                  //ͬʱ���г�ջ����Ϣ ҲҪ����ܵ������д���������һ��������
                    IChannelPipeline pipeline = channel.Pipeline;
                    // pipeline.AddLast("decoder", new MqttDecoder(true, 256 * 1024));
                    // pipeline.AddLast("encoder", new MqttEncoder());
                    pipeline.AddLast("echo", new tcpHandler(NewSessionConnected, SessionClosed, NewDataReceived));
                    //pipeline.AddLast( new tcpHandler(NewSessionConnected, SessionClosed, NewDataReceived));

                    #region  ��ʱ����
                    //pipeline.AddLast(new ReadTimeoutHandler(config.ClearIdleSessionInterval)); 
                    //  ��ʱ����  ����1 ָ��ʱ��chanalû�յ����ݣ�����2 ָ��ʱ��û�·�����chanal ����3 chanal��ָ��ʱ����û�յ���û�·���������
                    // pipeline.AddLast(new IdleStateHandler(0, 0, config.ClearIdleSessionInterval));//�Ƿ���յ����жϲ�׼ȷ�ܻᳬʱ ��bug
                    //pipeline.AddLast(new ChannelInboundHandlerAdapter());
                    #endregion

                }));


                try
                {
                    // bootstrap�󶨵�ָ���˿ڵ���Ϊ ���Ƿ������������ͬ����Serverbootstrap����bind������˿�
                    boundChannel = await b.BindAsync(config.Port);
                    StartSchedulerJob();//��ʱ����
                    //�رշ���
                    // await boundChannel.CloseAsync();
                }
                catch (Exception ex)
                {
                    //Console.WriteLine(ex.StackTrace);
                    // Log4jHelper.logger.error("tcp Server����ʧ��", e);
                    isStarSucees = false;
                }

            }
            catch (Exception ex)
            {
                // Log4jHelper.logger.error("tcp Server����ʧ��", e);
                // Console.WriteLine(ex.StackTrace);
                isStarSucees = false;

                try
                {
                    //�ͷŹ������߳�
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
                    Console.WriteLine("all time out��" + DateTime.Now.ToString("mm:ss") + "id:" + ctx.Channel.Id);
                }
                else if (evv.State == IdleState.WriterIdle)
                {
                    Console.WriteLine("writer time out��" + DateTime.Now.ToString("mm:ss") + "id:" + ctx.Channel.Id);
                }
                else if (evv.State == IdleState.ReaderIdle)
                {
                    Console.WriteLine("read time out��" + DateTime.Now.ToString("mm:ss") + "id:" + ctx.Channel.Id);

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