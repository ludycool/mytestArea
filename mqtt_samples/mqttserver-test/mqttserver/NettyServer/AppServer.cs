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
        //�µ�����
        protected abstract void NewSessionConnected(session mysession);

        //�Ͽ�����
        protected abstract void SessionClosed(session mysession);

        //����Ϣ
        protected abstract void NewDataReceived(session mysession, byte[] data);
        //����Ϣ
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