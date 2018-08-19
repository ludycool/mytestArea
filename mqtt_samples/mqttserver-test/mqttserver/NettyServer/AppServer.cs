namespace NettyServer
{

    public abstract class AppServer
    {

        public AppServer(ServerConfig _config)
        {
            config = _config;
        }
        ServerConfig config;
        IAppServer sever;
        //新的连接
        protected abstract void NewSessionConnected(session mysession);

        //断开连接
        protected abstract void SessionClosed(session mysession);

        //新消息
        protected abstract void NewDataReceived(session mysession, byte[] data);
        //新消息
        protected abstract void NewDataReceived(session mysession, string data);

        IAppServer getSever()
        {
            if (config.Mode == SocketMode.Udp)
            {
                sever = new UdpSocketServer(config, NewSessionConnected, SessionClosed, NewDataReceived);

            }
            else if (config.Mode == SocketMode.Tcp)
            {
                sever = new TcpSocketServer(config, NewSessionConnected, SessionClosed, NewDataReceived);
            }
            else
            {
                sever = new WebTcpSocketServer(config, NewSessionConnected, SessionClosed, NewDataReceived, NewDataReceived);

            }
            return sever;
        }

        public bool startServer()
        {
            if (sever == null)
            {
                sever = getSever();

            }
            return sever.startServer().Result;
        }
        public void StopServer()
        {
            sever.CloseServer();
        }

    }
}