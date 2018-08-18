namespace NettyServer
{

    public abstract class AppServer
    {



        IAppServer sever;
        //新的连接
        protected abstract void NewSessionConnected(session mysession);

        //断开连接
        protected abstract void SessionClosed(session mysession);

        //新消息
        protected abstract void NewDataReceived(session mysession, byte[] data);
        //新消息
        protected abstract void NewDataReceived(session mysession, string data);

        IAppServer getSever(ServerConfig config)
        {
            if (config.Mode == SocketMode.Udp)
            {
                sever =new UdpSocketServer(NewSessionConnected, SessionClosed, NewDataReceived);

            }
            else if (config.Mode == SocketMode.Tcp)
            {
                sever = new TcpSocketServer(NewSessionConnected, SessionClosed, NewDataReceived);
            }
            else
            {
                sever = new WebTcpSocketServer(NewSessionConnected, SessionClosed, NewDataReceived, NewDataReceived);

            }
            return sever;
        }

        public bool startServer(ServerConfig _config)
        {
            if (sever == null)
            {
                sever = getSever(_config);

            }
            return sever.startServer(_config).Result;
        }

    }
}