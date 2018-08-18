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

            // ���������־��Console
            //ExampleHelper.SetConsoleLogger();


            // �����߳��飬Ĭ��Ϊ�ں���*2���߳���
            MultithreadEventLoopGroup workerGroup = new MultithreadEventLoopGroup();
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
                          //pipeline.AddLast("echo", new udpHandler(NewDataReceived));
                          pipeline.AddLast(new udpHandler(NewDataReceived));

                      }));

                try
                {
                    // bootstrap�󶨵�ָ���˿ڵ���Ϊ ���Ƿ������������ͬ����Serverbootstrap����bind������˿�
                    boundChannel = await bootstrap.BindAsync(config.Port);

                    schedulerJob();//��ʱ����
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
        protected  void NewDataReceived(IChannelHandlerContext ctx, DatagramPacket msg)
        {
            String id = ctx.Channel.Id.AsLongText();
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
                    Console.WriteLine("RemoteAddress ������");
                }


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
        //��ʱ���� ����session��ʱ ��udpû�жϿ����ӣ����ӵĸ��� UDPӳ�䱣��ʱ��15-20s���ң�ӳ��ͱ������ˡ�
        public void schedulerJob()
        {
            t = new System.Timers.Timer(config.ClearIdleSessionInterval*1000); //����ʱ����Ϊ5��
            t.Elapsed += new System.Timers.ElapsedEventHandler(run);
            t.AutoReset = true; //ÿ��ָ��ʱ��Elapsed�¼��Ǵ���һ�Σ�false��������һֱ������true��
            t.Start();
        }

        public void run(object source, System.Timers.ElapsedEventArgs e)
        {

            try
            {
                Console.WriteLine("��ʱrun��" + DateTime.Now.ToString("mm:ss"));
                // Log4jHelper.Debug("udp ��ʱִ��ClearIdleSession �Ƴ���ʱsession");
                //��ʹ�÷���
                KeyValuePair<string, session>[] setkp = map_session.ToArray();
                DateTime now = DateTime.Now;
                if (setkp != null && setkp.Length > 0)
                {
                    foreach (var entry in setkp)
                    {
                        //��������

                        string key = entry.Key;
                        session sessionItem = entry.Value;
                        double t= (now - sessionItem.activeTime).TotalSeconds;
                        if (t > 20)//udp ����20�뵱���Ͽ�����
                        {
                            session out1;
                            map_session.TryRemove(key, out out1);
                            if (OnSessionClosed != null)
                            {
                                OnSessionClosed(sessionItem);//�Ͽ�����
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log4jHelper.logger.error("udp ��ʱִ��ClearIdleSession �Ƴ���ʱsession ����", ex);
            }

        }
    }


}