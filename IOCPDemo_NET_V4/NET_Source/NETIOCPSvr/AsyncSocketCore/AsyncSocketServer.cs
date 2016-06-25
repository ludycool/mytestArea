using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace AsyncSocketServer
{
    /// <summary>
    /// 异步Socket服务器(IOCP)
    /// </summary>
    public class AsyncSocketServer
    {
        private Socket listenSocket;
        
        /// <summary>
        /// 最大支持连接个数
        /// </summary>
        private int m_numConnections; 
        /// <summary>
        /// 每个连接接收缓存大小
        /// </summary>
        private int m_receiveBufferSize; 
        /// <summary>
        /// 信号量 限制访问接收连接的线程数，用来控制最大并发数
        /// </summary>
        private Semaphore m_maxNumberAcceptedClients; 
        /// <summary>
        /// Socket最大超时时间，单位为MS
        /// </summary>
        private int m_socketTimeOutMS; 
        /// <summary>
        ///  Socket最大超时时间，单位为MS
        /// </summary>
        public int SocketTimeOutMS { get { return m_socketTimeOutMS; } set { m_socketTimeOutMS = value; } }
               
        /// <summary>
        /// 对象池
        /// </summary>
        private AsyncSocketUserTokenPool m_asyncSocketUserTokenPool;
        /// <summary>
        /// 在线列表
        /// </summary>
        private AsyncSocketUserTokenList m_asyncSocketUserTokenList;
        /// <summary>
        /// 在线列表
        /// </summary>
        public AsyncSocketUserTokenList AsyncSocketUserTokenList { get { return m_asyncSocketUserTokenList; } }

        private LogOutputSocketProtocolMgr m_logOutputSocketProtocolMgr;
        public LogOutputSocketProtocolMgr LogOutputSocketProtocolMgr { get { return m_logOutputSocketProtocolMgr; } }

        private UploadSocketProtocolMgr m_uploadSocketProtocolMgr;
        public UploadSocketProtocolMgr UploadSocketProtocolMgr { get { return m_uploadSocketProtocolMgr; } }

        private DownloadSocketProtocolMgr m_downloadSocketProtocolMgr;
        public DownloadSocketProtocolMgr DownloadSocketProtocolMgr { get { return m_downloadSocketProtocolMgr; } }
        /// <summary>
        /// 守护线程
        /// </summary>
        private DaemonThread m_daemonThread;
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="numConnections"></param>
        public AsyncSocketServer(int numConnections)
        {
            m_numConnections = numConnections;
            m_receiveBufferSize = ProtocolConst.ReceiveBufferSize;

            m_asyncSocketUserTokenPool = new AsyncSocketUserTokenPool(numConnections);
            m_asyncSocketUserTokenList = new AsyncSocketUserTokenList();
            m_maxNumberAcceptedClients = new Semaphore(numConnections, numConnections);

            m_logOutputSocketProtocolMgr = new LogOutputSocketProtocolMgr();
            m_uploadSocketProtocolMgr = new UploadSocketProtocolMgr();
            m_downloadSocketProtocolMgr = new DownloadSocketProtocolMgr();
        }
        /// <summary>
        ///  初始化函数
        /// </summary>
        public void Init()
        {
            AsyncSocketUserToken userToken;
            for (int i = 0; i < m_numConnections; i++) //按照连接数建立读写对象
            {
                userToken = new AsyncSocketUserToken(m_receiveBufferSize);
                userToken.ReceiveEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(IO_Completed);
                userToken.SendEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(IO_Completed);
                m_asyncSocketUserTokenPool.Push(userToken);
            }
        }
        /// <summary>
        /// 启动
        /// </summary>
        /// <param name="localEndPoint"></param>
        public void Start(IPEndPoint localEndPoint)
        {
            listenSocket = new Socket(localEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);


            if (localEndPoint.AddressFamily == AddressFamily.InterNetworkV6)
            {
                // 配置监听socket为 dual-mode (IPv4 & IPv6) 
                // 27 is equivalent to IPV6_V6ONLY socket option in the winsock snippet below,
                listenSocket.SetSocketOption(SocketOptionLevel.IPv6, (SocketOptionName)27, false);
                listenSocket.Bind(new IPEndPoint(IPAddress.IPv6Any, localEndPoint.Port));
            }
            else
            {
                listenSocket.Bind(localEndPoint);
            }
            listenSocket.Listen(m_numConnections);
            Program.Logger.InfoFormat("Start listen socket {0} success", localEndPoint.ToString());
            //for (int i = 0; i < 64; i++) //不能循环投递多次AcceptAsync，会造成只接收8000连接后不接收连接了
            StartAccept(null);
            m_daemonThread = new DaemonThread(this);
        }
        /// <summary>
        /// 从客户端开始接受一个连接操作
        /// </summary>
        /// <param name="acceptEventArgs"></param>
        public void StartAccept(SocketAsyncEventArgs acceptEventArgs)
        {
            if (acceptEventArgs == null)
            {
                acceptEventArgs = new SocketAsyncEventArgs();
                acceptEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(AcceptEventArg_Completed);
            }
            else
            {
                acceptEventArgs.AcceptSocket = null; //释放上次绑定的Socket，等待下一个Socket连接
            }

            m_maxNumberAcceptedClients.WaitOne(); //获取信号量
            bool willRaiseEvent = listenSocket.AcceptAsync(acceptEventArgs);
            if (!willRaiseEvent)
            {
                ProcessAccept(acceptEventArgs);
                //如果I/O挂起等待异步则触发AcceptAsyn_Asyn_Completed事件
                //此时I/O操作同步完成，不会触发Asyn_Completed事件，所以指定BeginAccept()方法
            }
        }
        /// <summary>
        /// accept 操作完成时回调函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="acceptEventArgs"></param>
        void AcceptEventArg_Completed(object sender, SocketAsyncEventArgs acceptEventArgs)
        {
            try
            {
                ProcessAccept(acceptEventArgs);
            }
            catch (Exception E)
            {
                Program.Logger.ErrorFormat("Accept client {0} error, message: {1}", acceptEventArgs.AcceptSocket, E.Message);
                Program.Logger.Error(E.StackTrace);  
            }            
        }
        /// <summary>
        /// 监听Socket接受处理
        /// </summary>
        /// <param name="acceptEventArgs"></param>
        private void ProcessAccept(SocketAsyncEventArgs acceptEventArgs)
        {
            Program.Logger.InfoFormat("Client connection accepted. Local Address: {0}, Remote Address: {1}",
                acceptEventArgs.AcceptSocket.LocalEndPoint, acceptEventArgs.AcceptSocket.RemoteEndPoint);

            AsyncSocketUserToken userToken = m_asyncSocketUserTokenPool.Pop();
            m_asyncSocketUserTokenList.Add(userToken); //添加到正在连接列表
            userToken.ConnectSocket = acceptEventArgs.AcceptSocket;
            userToken.ConnectDateTime = DateTime.Now;

            try
            {
                bool willRaiseEvent = userToken.ConnectSocket.ReceiveAsync(userToken.ReceiveEventArgs); //投递接收请求
                if (!willRaiseEvent)
                {
                    lock (userToken)
                    {
                        ProcessReceive(userToken.ReceiveEventArgs);
                    }
                }                    
            }
            catch (Exception E)
            {
                Program.Logger.ErrorFormat("Accept client {0} error, message: {1}", userToken.ConnectSocket, E.Message);
                Program.Logger.Error(E.StackTrace);                
            }            

            StartAccept(acceptEventArgs); //把当前异步事件释放，等待下次连接
        }
        /// <summary>
        /// 当Socket上的发送或接收请求被完成时，调用此函数
        /// </summary>
        /// <param name="sender">激发事件的对象</param>
        /// <param name="asyncEventArgs">与发送或接收完成操作相关联的SocketAsyncEventArg对象</param>
        void IO_Completed(object sender, SocketAsyncEventArgs asyncEventArgs)
        {
            AsyncSocketUserToken userToken = asyncEventArgs.UserToken as AsyncSocketUserToken;
            userToken.ActiveDateTime = DateTime.Now;
            try
            {                
                lock (userToken)
                {
                    if (asyncEventArgs.LastOperation == SocketAsyncOperation.Receive)
                        ProcessReceive(asyncEventArgs);
                    else if (asyncEventArgs.LastOperation == SocketAsyncOperation.Send)
                        ProcessSend(asyncEventArgs);
                    else
                        throw new ArgumentException("The last operation completed on the socket was not a receive or send");
                }   
            }
            catch (Exception E)
            {
                Program.Logger.ErrorFormat("IO_Completed {0} error, message: {1}", userToken.ConnectSocket, E.Message);
                Program.Logger.Error(E.StackTrace);
            }                     
        }

        /// <summary>
        /// 接收完成时处理函数
        /// </summary>
        /// <param name="receiveEventArgs"></param>
        private void ProcessReceive(SocketAsyncEventArgs receiveEventArgs)
        {
            AsyncSocketUserToken userToken = receiveEventArgs.UserToken as AsyncSocketUserToken;
            if (userToken.ConnectSocket == null)
                return;
            userToken.ActiveDateTime = DateTime.Now;
            if (userToken.ReceiveEventArgs.BytesTransferred > 0 && userToken.ReceiveEventArgs.SocketError == SocketError.Success)
            {
                int offset = userToken.ReceiveEventArgs.Offset;
                int count = userToken.ReceiveEventArgs.BytesTransferred;
                if ((userToken.AsyncSocketInvokeElement == null) & (userToken.ConnectSocket != null)) //存在Socket对象，并且没有绑定协议对象，则进行协议对象绑定
                {
                    BuildingSocketInvokeElement(userToken);
                    offset = offset + 1;
                    count = count - 1;
                }
                if (userToken.AsyncSocketInvokeElement == null) //如果没有解析对象，提示非法连接并关闭连接
                {
                    Program.Logger.WarnFormat("Illegal client connection. Local Address: {0}, Remote Address: {1}", userToken.ConnectSocket.LocalEndPoint, 
                        userToken.ConnectSocket.RemoteEndPoint);
                    CloseClientSocket(userToken);
                }
                else
                {
                    if (count > 0) //处理接收数据
                    {
                        if (!userToken.AsyncSocketInvokeElement.ProcessReceive(userToken.ReceiveEventArgs.Buffer, offset, count))
                        { //如果处理数据返回失败，则断开连接
                            CloseClientSocket(userToken);
                        }
                        else //否则投递下次介绍数据请求
                        {
                            bool willRaiseEvent = userToken.ConnectSocket.ReceiveAsync(userToken.ReceiveEventArgs); //投递接收请求
                            if (!willRaiseEvent)
                                ProcessReceive(userToken.ReceiveEventArgs);
                        }
                    }
                    else
                    {
                        bool willRaiseEvent = userToken.ConnectSocket.ReceiveAsync(userToken.ReceiveEventArgs); //投递接收请求
                        if (!willRaiseEvent)
                            ProcessReceive(userToken.ReceiveEventArgs);//同步接收时处理接收完成事件
                    }
                }
            }
            else
            {
                CloseClientSocket(userToken);
            }
        }
        /// <summary>
        /// 协议分类
        /// </summary>
        /// <param name="userToken"></param>
        private void BuildingSocketInvokeElement(AsyncSocketUserToken userToken)
        {
            byte flag = userToken.ReceiveEventArgs.Buffer[userToken.ReceiveEventArgs.Offset];
            if (flag == (byte)ProtocolFlag.Upload)
                userToken.AsyncSocketInvokeElement = new UploadSocketProtocol(this, userToken);
            else if (flag == (byte)ProtocolFlag.Download)
                userToken.AsyncSocketInvokeElement = new DownloadSocketProtocol(this, userToken);
            else if (flag == (byte)ProtocolFlag.RemoteStream)
                userToken.AsyncSocketInvokeElement = new RemoteStreamSocketProtocol(this, userToken);
            else if (flag == (byte)ProtocolFlag.Throughput)
                userToken.AsyncSocketInvokeElement = new ThroughputSocketProtocol(this, userToken);
            else if (flag == (byte)ProtocolFlag.Control)
                userToken.AsyncSocketInvokeElement = new ControlSocketProtocol(this, userToken);
            else if (flag == (byte)ProtocolFlag.LogOutput)
                userToken.AsyncSocketInvokeElement = new LogOutputSocketProtocol(this, userToken);
            if (userToken.AsyncSocketInvokeElement != null)
            {
                Program.Logger.InfoFormat("Building socket invoke element {0}.Local Address: {1}, Remote Address: {2}",
                    userToken.AsyncSocketInvokeElement, userToken.ConnectSocket.LocalEndPoint, userToken.ConnectSocket.RemoteEndPoint);
            } 
        }

        /// <summary>
        /// 发送完成时处理函数
        /// </summary>
        /// <param name="sendEventArgs">与发送完成操作相关联的SocketAsyncEventArg对象</param>
        /// <returns></returns>
        private bool ProcessSend(SocketAsyncEventArgs sendEventArgs)
        {
            AsyncSocketUserToken userToken = sendEventArgs.UserToken as AsyncSocketUserToken;
            if (userToken.AsyncSocketInvokeElement == null)
                return false;
            userToken.ActiveDateTime = DateTime.Now;
            if (sendEventArgs.SocketError == SocketError.Success)
                return userToken.AsyncSocketInvokeElement.SendCompleted(); //调用子类回调函数
            else
            {
                CloseClientSocket(userToken);
                return false;
            }
        }
        /// <summary>
        /// 同步的使用socket发送数据
        /// </summary>
        /// <param name="connectSocket"></param>
        /// <param name="sendEventArgs"></param>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public bool SendAsyncEvent(Socket connectSocket, SocketAsyncEventArgs sendEventArgs, byte[] buffer, int offset, int count)
        {
            if (connectSocket == null)
                return false;
            sendEventArgs.SetBuffer(buffer, offset, count);
            bool willRaiseEvent = connectSocket.SendAsync(sendEventArgs);
            if (!willRaiseEvent)
            {
                return ProcessSend(sendEventArgs);
            }
            else
                return true;
        }

       
        /// <summary>
        /// 关闭socket连接
        /// </summary>
        /// <param name="userToken"></param>
        public void CloseClientSocket(AsyncSocketUserToken userToken)
        {
            if (userToken.ConnectSocket == null)
                return;
            string socketInfo = string.Format("Local Address: {0} Remote Address: {1}", userToken.ConnectSocket.LocalEndPoint,
                userToken.ConnectSocket.RemoteEndPoint);
            Program.Logger.InfoFormat("Client connection disconnected. {0}", socketInfo);
            try
            {
                userToken.ConnectSocket.Shutdown(SocketShutdown.Both);
            }
            catch (Exception E) 
            {
                Program.Logger.ErrorFormat("CloseClientSocket Disconnect client {0} error, message: {1}", socketInfo, E.Message);
            }
            userToken.ConnectSocket.Close();
            userToken.ConnectSocket = null; //释放引用，并清理缓存，包括释放协议对象等资源

            m_maxNumberAcceptedClients.Release();
            m_asyncSocketUserTokenPool.Push(userToken);
            m_asyncSocketUserTokenList.Remove(userToken);
        }

  
    }
}
