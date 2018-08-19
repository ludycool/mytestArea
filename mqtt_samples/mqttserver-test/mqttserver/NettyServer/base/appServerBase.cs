using DotNetty.Transport.Channels;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace NettyServer
{
    public class appServerBase
    {


        public ServerConfig config
        {
            set;
            get;
        }


        System.Timers.Timer tik;


        protected ConcurrentDictionary<string, session> map_session = new ConcurrentDictionary<string, session>();

        #region 方法

        //新的连接
        protected void NewSessionConnected(IChannelHandlerContext ctx)
        {
            String id = ctx.Channel.Id.AsLongText();
            session session_item = new session(id, ctx.Channel, config.Mode, ctx.Channel.RemoteAddress);
            session_item.activeTime = DateTime.Now;//更新时间
            if (map_session.ContainsKey(id))
            {
                map_session[id] = session_item;
            }
            else
            {
                map_session.TryAdd(id, session_item);
            }
            if (OnNewSessionConnected != null)
            {
                OnNewSessionConnected(session_item);
            }
        }

        //断开连接
        protected void SessionClosed(IChannelHandlerContext ctx)
        {
            String id = ctx.Channel.Id.AsLongText();
            if (map_session.ContainsKey(id))
            {
                session mysession = null;
                map_session.TryGetValue(id, out mysession);
                runremovechanal(id, mysession);
            }

            // 
        }

        //新消息
        protected virtual void NewDataReceived(IChannelHandlerContext ctx, byte[] data)
        {
            String id = ctx.Channel.Id.AsLongText();
            if (map_session.ContainsKey(id))
            {
                session mysession = null;
                map_session.TryGetValue(id, out mysession);
                if (mysession == null)
                    return;
                mysession.activeTime = DateTime.Now;//更新时间
                if (OnNewDataReceived != null)
                {
                    OnNewDataReceived(mysession, data);
                }
            }

        }


        #endregion

        #region 属性

        //新的连接
        public session_listener.OnNewSessionConnected OnNewSessionConnected
        {
            set;
            get;
        }
        //断开连接
        public session_listener.OnSessionClosed OnSessionClosed
        {
            set;
            get;
        }

        //接收新消息 
        public session_listener.OnNewDataReceived OnNewDataReceived
        {
            set;
            get;
        }

        //新消息
        public session_listener.OnNewStringReceived OnNewStringReceived
        {
            set;
            get;
        }

        #endregion


        #region 超时处理

        //定时运行 处理session超时 ，udp没有断开连接，连接的概念 UDP映射保持时间15-20s左右，映射就被重置了。
        public void StartSchedulerJob()
        {
            tik = new System.Timers.Timer((config.ClearIdleSessionInterval + 3) * 1000); //设置时间间隔秒 Timer 参数是毫秒
            tik.Elapsed += new System.Timers.ElapsedEventHandler(run);
            tik.AutoReset = true; //每到指定时间Elapsed事件是触发一次（false），还是一直触发（true）
            tik.Start();
        }


        public void stopSchedulerJob()
        {
            if (tik != null)
            {
                tik.Stop();
            }
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
                        double t = (now - sessionItem.activeTime).TotalSeconds;
                        if (t > config.ClearIdleSessionInterval)//udp 超过20秒当做断开连接
                        {
                            #region

                            if (sessionItem.channel != null && sessionItem.channel.Open)
                            {

                                if (config.Mode == SocketMode.Udp)
                                {
                                    runremovechanal(key, sessionItem);
                                }
                                else
                                {
                                    sessionItem.channel.CloseAsync();
                                    Console.WriteLine("定时run： CloseAsync");
                                }
                            }
                            else
                            {
                                runremovechanal(key, sessionItem);
                            }
                            #endregion
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log4jHelper.logger.error("udp 定时执行ClearIdleSession 移除超时session 出错", ex);
            }
        }


        void runremovechanal(string key, session sessionItem)
        {
            if (sessionItem == null)
                return;

            Console.WriteLine("定时run： TryRemove");
            if (OnSessionClosed != null)
            {
                OnSessionClosed(sessionItem);//断开连接
            }
            session out1;
            map_session.TryRemove(key, out out1);
        }
        #endregion
    }
}
