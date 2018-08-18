namespace NettyServer
{

    public class ServerConfig
    {

        /// <summary>
        /// Default ReceiveBufferSize
        /// </summary>
        public static int DefaultReceiveBufferSize = 4096;

        /// <summary>
        /// Default MaxConnectionNumber
        /// </summary>
        public static int DefaultMaxConnectionNumber = 100;


        /// <summary>
        /// Default sending queue size
        /// </summary>
        public static int DefaultSendingQueueSize = 5;

        /// <summary>
        /// Default MaxRequestLength
        /// </summary>
        public static int DefaultMaxRequestLength = 1024;


        /// <summary>
        /// Default send timeout value, in milliseconds
        /// </summary>
        public static int DefaultSendTimeout = 5000;


        /// <summary>
        /// Default clear idle session interval
        /// </summary>
        public static int DefaultClearIdleSessionInterval = 120;

        /// <summary>
        /// Default idle session timeout
        /// </summary>
        public static int DefaultIdleSessionTimeOut = 300;


        /// <summary>
        /// The default send buffer size
        /// </summary>
        public static int DefaultSendBufferSize = 2048;


        /// <summary>
        /// The default session snapshot interval
        /// </summary>
        public static int DefaultSessionSnapshotInterval = 5;

        /// <summary>
        /// The default keep alive time
        /// </summary>
        public static int DefaultKeepAliveTime = 600; // 60 * 10 = 10 minutes


        /// <summary>
        /// The default keep alive interval
        /// </summary>
        public static int DefaultKeepAliveInterval = 60; // 60 seconds


        /// <summary>
        /// The default listen backlog
        /// </summary>
        public static int DefaultListenBacklog = 100;


        /// <summary>
        /// Initializes a new instance of the <see cref="ServerConfig"/> class.
        /// </summary>
        public ServerConfig()
        {
            Security = "None";
            MaxConnectionNumber = DefaultMaxConnectionNumber;
            Mode = SocketMode.Tcp;
            MaxRequestLength = DefaultMaxRequestLength;
            KeepAliveTime = DefaultKeepAliveTime;
            KeepAliveInterval = DefaultKeepAliveInterval;
            ListenBacklog = DefaultListenBacklog;
            ReceiveBufferSize = DefaultReceiveBufferSize;
            SendingQueueSize = DefaultSendingQueueSize;
            SendTimeOut = DefaultSendTimeout;
            ClearIdleSessionInterval = DefaultClearIdleSessionInterval;
            IdleSessionTimeOut = DefaultIdleSessionTimeOut;
            SendBufferSize = DefaultSendBufferSize;
            LogBasicSessionActivity = true;
            SessionSnapshotInterval = DefaultSessionSnapshotInterval;
        }

        /// <summary>
        /// Gets/sets the name of the server type of this appServerBase want to use.
        /// </summary>
        /// <value>
        /// The name of the server type.
        /// </value>
        public string ServerTypeName { set; get; }


        /// <summary>
        /// Gets/sets the type definition of the appserver.
        /// </summary>
        /// <value>
        /// The type of the server.
        /// </value>
        public string ServerType { set; get; }

        /// <summary>bool
        /// Gets/sets the Receive filter factory.
        /// </summary>
        public string ReceiveFilterFactory { set; get; }

        /// <summary>
        /// Gets/sets the ip.
        /// </summary>
        public string Ip { set; get; }

        /// <summary>
        /// Gets/sets the port.
        /// </summary>
        public int Port { set; get; }


        /// <summary>
        /// Gets/sets a value indicating whether this <see cref="IServerConfig"/> is disabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if disabled{set;get;} otherwise, <c>false</c>.
        /// </value>
        public bool Disabled { set; get; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { set; get; }

        /// <summary>
        /// Gets/sets the mode.
        /// </summary>
        public SocketMode Mode { set; get; }

        /// <summary>
        /// Gets/sets the send time out.
        /// </summary>
        public int SendTimeOut { set; get; }

        /// <summary>
        /// Gets the max connection number.
        /// </summary>
        public int MaxConnectionNumber { set; get; }

        /// <summary>
        /// Gets the size of the receive buffer.
        /// </summary>
        /// <value>
        /// The size of the receive buffer.
        /// </value>
        public int ReceiveBufferSize { set; get; }

        /// <summary>
        /// Gets the size of the send buffer.
        /// </summary>
        /// <value>
        /// The size of the send buffer.
        /// </value>
        public int SendBufferSize { set; get; }


        /// <summary>
        /// Gets a value indicating whether sending is in synchronous mode.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [sync send]{set;get;} otherwise, <c>false</c>.
        /// </value>
        public bool SyncSend { set; get; }

        /// <summary>
        /// Gets/sets a value indicating whether log command in log file.
        /// </summary>
        /// <value>
        ///   <c>true</c> if log command{set;get;} otherwise, <c>false</c>.
        /// </value>

        public bool LogCommand { set; get; }

        /// <summary>
        /// Gets/sets a value indicating whether clear idle session.
        /// </summary>
        /// <value>
        ///   <c>true</c> if clear idle session{set;get;} otherwise, <c>false</c>.
        /// </value>
        public bool ClearIdleSession { set; get; }

        /// <summary>
        /// Gets/sets the clear idle session interval, in seconds.
        /// </summary>
        /// <value>
        /// The clear idle session interval.
        /// </value>
        public int ClearIdleSessionInterval { set; get; }

        /// <summary>
        /// Gets/sets the idle session timeout time length, in seconds.
        /// </summary>
        /// <value>
        /// The idle session time out.
        /// </value>

        public int IdleSessionTimeOut { set; get; }

        /// <summary>
        /// Gets/sets X509Certificate configuration.
        /// </summary>
        /// <value>
        /// X509Certificate configuration.
        /// </value>

        /// <summary>
        /// Gets/sets the security protocol, X509 certificate.
        /// </summary>
        public string Security { set; get; }

        /// <summary>
        /// Gets/sets the length of the max request.
        /// </summary>
        /// <value>
        /// The length of the max request.
        /// </value>

        public int MaxRequestLength { set; get; }

        /// <summary>
        /// Gets/sets a value indicating whether [disable session snapshot].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [disable session snapshot]{set;get;} otherwise, <c>false</c>.
        /// </value>
        public bool DisableSessionSnapshot { set; get; }

        /// <summary>
        /// Gets/sets the interval to taking snapshot for all live sessions.
        /// </summary>
        public int SessionSnapshotInterval { set; get; }

        /// <summary>
        /// Gets/sets the connection filters used by this server instance.
        /// </summary>
        /// <value>
        /// The connection filter's name list, seperated by comma
        /// </value>
        public string ConnectionFilter { set; get; }

        /// <summary>
        /// Gets the command loader, multiple values should be separated by comma.
        /// </summary>
        public string CommandLoader { set; get; }

        /// <summary>
        /// Gets/sets the start keep alive time, in seconds
        /// </summary>
        public int KeepAliveTime { set; get; }

        /// <summary>
        /// Gets/sets the keep alive interval, in seconds.
        /// </summary>
        public int KeepAliveInterval { set; get; }

        /// <summary>
        /// Gets the backlog size of socket listening.
        /// </summary>
        public int ListenBacklog { set; get; }


        /// <summary>
        /// Gets/sets the log factory name.
        /// </summary>
        public string LogFactory { set; get; }

        /// <summary>
        /// Gets/sets the size of the sending queue.
        /// </summary>
        /// <value>
        /// The size of the sending queue.
        /// </value>
        public int SendingQueueSize { set; get; }

        /// <summary>
        /// Gets a value indicating whether [log basic session activity like connected and disconnected].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [log basic session activity]{set;get;} otherwise, <c>false</c>.
        /// </value>

        public bool LogBasicSessionActivity { set; get; }

        /// <summary>
        /// Gets/sets a value indicating whether [log all socket exception].
        /// </summary>
        /// <value>
        /// <c>true</c> if [log all socket exception]{set;get;} otherwise, <c>false</c>.
        /// </value>

        public bool LogAllSocketException
        {
            set;
            get;
        }

        /// <summary>
        /// Gets/sets the default text encoding.
        /// </summary>
        /// <value>
        /// The text encoding.
        /// </value>
        public string TextEncoding
        {
            set;
            get;
        }


    }
}