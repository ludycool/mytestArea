using System;
using System.Collections.Generic;
using System.Text;

namespace NettyServer
{
    public   class session_listener
    {

        //新的连接
        public delegate void OnNewSessionConnected(session mysession);

        //断开连接
        public delegate void OnSessionClosed(session mysession);

        //新消息
        public delegate void OnNewDataReceived(session mysession, byte[] data);

        //新消息
        public delegate void OnNewStringReceived(session mysession, string data);
    }
}
