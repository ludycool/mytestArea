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
                }));


                try
                {
                    // bootstrap�󶨵�ָ���˿ڵ���Ϊ ���Ƿ������������ͬ����Serverbootstrap����bind������˿�
                    boundChannel = await b.BindAsync(config.Port);

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


}