namespace NettyServer
{

    public abstract class AppServer
    {



        IAppServer sever;
        //�µ�����
        protected abstract void NewSessionConnected(session mysession);

        //�Ͽ�����
        protected abstract void SessionClosed(session mysession);

        //����Ϣ
        protected abstract void NewDataReceived(session mysession, byte[] data);
        //����Ϣ
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