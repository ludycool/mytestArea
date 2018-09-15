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
                    //�ͷŹ������߳�
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

            // ���������־��Console
            //ExampleHelper.SetConsoleLogger();


            // �����߳��飬Ĭ��Ϊ�ں���*2���߳���
            workerGroup = new MultithreadEventLoopGroup();
            try
            {
                var bootstrap = new Bootstrap();
                bootstrap
                    .Group(workerGroup)
                    .Channel<SocketDatagramChannel>()
                      .Handler(new ActionChannelInitializer<IDatagramChannel>(channel =>
                      { //�����߳������� ��������һ���ܵ�����������߳����н��յ�����Ϣ����ͨ������ܵ�һ������´���
                        //ͬʱ���г�ջ����Ϣ ҲҪ����ܵ������д���������һ��������
                          IChannelPipeline pipeline = channel.Pipeline;
                          // pipeline.AddLast("decoder", new MqttDecoder(true, 256 * 1024));
                          // pipeline.AddLast("encoder", new MqttEncoder());
                          pipeline.AddLast("udpserver", new udpHandler(NewDataReceived));
                          //pipeline.AddLast(new udpHandler(NewDataReceived));

                      }));

                try
                {
                    // bootstrap�󶨵�ָ���˿ڵ���Ϊ ���Ƿ������������ͬ����Serverbootstrap����bind������˿�
                    boundChannel = await bootstrap.BindAsync(config.Port);

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



        //����Ϣ
        protected void NewDataReceived(IChannelHandlerContext ctx, DatagramPacket msg)
        {
            //String id = ctx.Channel.Id.AsLongText();//udpͬһ�����Կͻ��ˣ����ɵ�id��һ��
            IPEndPoint ipp = (IPEndPoint)msg.Sender;
            string addr = ipp.Address.ToString();
            String id = ctx.Channel.Id + msg.Sender.ToString();
            session session_item = null;
            map_session.TryGetValue(id, out session_item);
            if (session_item == null)
            {
                session_item = new session(id, ctx.Channel, SocketMode.Udp, msg.Sender);
                session_item.activeTime = DateTime.Now;//����ʱ��
                map_session.TryAdd(id, session_item);
                if (OnNewSessionConnected != null)
                {
                    OnNewSessionConnected(session_item);
                }
            }
            else
            {
                session_item.activeTime = DateTime.Now;//����ʱ��
                /*
                if (!session_item.recipient.ToString().Equals(msg.Sender.ToString()))
                {
                    session_item.recipient = msg.Sender;
                    if (OnNewSessionConnected != null)
                    {
                        OnNewSessionConnected(session_item);
                    }
                    // Console.WriteLine("RemoteAddress ������");
                }
                 */
            }
            if (OnNewDataReceived != null)
            {
                IByteBuffer directBuf = msg.Content;//��ȡ������
                if (directBuf.HasArray)
                {
                    int length = directBuf.ReadableBytes;//�õ��ɶ��ֽ���
                    byte[] array = new byte[length];    //����һ������length��С������
                    directBuf.GetBytes(directBuf.ReaderIndex, array); //���������е����ݿ��������������
                    OnNewDataReceived(session_item, array);
                }

            }
            // IByteBuffer directBuf = msg.Content;//��ȡ������

        }
    }


}