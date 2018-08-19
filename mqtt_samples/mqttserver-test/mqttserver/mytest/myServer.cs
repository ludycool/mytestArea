using NettyServer;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mytest
{
    public class myServer : AppServer
    {
        #region 重写的方法
        protected override void NewDataReceived(session mysession, byte[] data)
        {
            mysession.writeAndFlush(data);
        }

        protected override void NewDataReceived(session mysession, string data)
        {
          mysession.writeAndFlush(data, Encoding.UTF8);
        }

        protected override void NewSessionConnected(session mysession)
        {
           // byte[] data = System.Text.Encoding.Unicode.GetBytes();
            mysession.writeAndFlush("欢迎 6666",Encoding.Unicode);
        }

        protected override void SessionClosed(session mysession)
        {
            RemoveSession(mysession);//从池中移除
        }
        #endregion
        
        #region 连接维护


        /// <summary>
        /// 连接池，新连接加入，断连接，移除
        /// </summary>
        internal static ConcurrentDictionary<string, session> SessionCache = new ConcurrentDictionary<string, session>();

        /// <summary>
        /// 移除在线列表
        /// </summary>
        /// <param name="token"></param>
        internal void RemoveSession(session token)
        {

            session tem1 = null;
            if (!string.IsNullOrEmpty(token.id))
            {
                if (SessionCache.Keys.Contains(token.id))
                {
                    // var tem = SessionCache[token.id];
                    token.isLogin = false;
                    SessionCache.TryRemove(token.id, out tem1);

                }
            }

        }
        /// <summary>
        /// 加入在线列表，如果已经存在，就更新
        /// </summary>
        /// <param name="token"></param>
        internal void PushSession(session token)
        {
            if (!string.IsNullOrEmpty(token.id))
            {
                if (!SessionCache.Keys.Contains(token.id))
                {
                    SessionCache.TryAdd(token.id, token);

                }
                else
                {
                    if (SessionCache[token.id] != token)
                    {
                        SessionCache[token.id].id = "";
                        SessionCache[token.id].isLogin = false;
                        SessionCache[token.id].close();
                    }
                    SessionCache[token.id] = token;
                }
            }
        }
        /// <summary>
        /// 是否在线 是否有连接
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        internal bool IsOnline(string Id)
        {

            return SessionCache.Keys.Contains(Id);
        }
        /// <summary>
        /// 获取连接
        /// </summary>
        /// <param name="DeviceId"></param>
        /// <returns></returns>
        internal session GetSessionId(String Id)
        {
            session ret = null;

            if (SessionCache.ContainsKey(Id))
            {
                ret = SessionCache[Id];
            }
            return ret;
        }

        /// <summary>
        /// 获取连接个数
        /// </summary>
        /// <returns></returns>
        internal int GetSessionCacheCount()
        {

            return SessionCache.Count();
        }

        #endregion
    }
}
