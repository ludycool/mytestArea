using DotNetty.Transport.Channels;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace NettyServer
{
    public class appServerBase
    {

     protected   SocketMode Mode {
            set;
            get;
        }
        protected static ConcurrentDictionary<string, session> map_session = new ConcurrentDictionary<string, session>();

        #region 方法

        //新的连接
        protected void NewSessionConnected(IChannelHandlerContext ctx)
        {
            String id = ctx.Channel.Id.AsLongText();
            session session_item = new session(id, ctx.Channel, Mode, ctx.Channel.RemoteAddress);
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
                session mysession = map_session[id];
                if (OnSessionClosed != null)
                {
                    OnSessionClosed(mysession);
                }
                session outitem;
                map_session.TryRemove(id, out outitem);
            }

            // 
        }

        //新消息
        protected virtual void NewDataReceived(IChannelHandlerContext ctx, byte[] data)
        {
            String id = ctx.Channel.Id.AsLongText();
            if (map_session.ContainsKey(id))
            {
                session mysession = map_session[id];

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
    }
}
