using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;

namespace SocketUdpServer
{
    class Program
    {
        /// <summary>
        /// 当前通信中的客户端
        /// </summary>
        static Int32 numClient = 0;
        /// <summary>
        /// 当前收到的新客户端
        /// </summary>
        static Int32 client = 0;
        /// <summary>
        /// 即时收到消息数
        /// </summary>
        static Int32 receivedMessages = 0;
        /// <summary>
        /// 即时发送消息数
        /// </summary>
        static Int32 sentMessages = 0;
        /// <summary>
        /// 即时收到字节
        /// </summary>
        static Int32 receivedBytes = 0;
        /// <summary>
        /// 即时发送消息数
        /// </summary>
        static Int32 sentBytes = 0;

        static void Main(string[] args)
        {
            Console.WriteLine("输入监听端口号");
            int listenPort = int.Parse(Console.ReadLine());
            Console.WriteLine("输入通讯端口号");
            int port = int.Parse(Console.ReadLine());
            Console.WriteLine("最大允许的客户端数");
            int numClient = int.Parse(Console.ReadLine());


            UdpServer server = new UdpServer(listenPort, port, numClient);
            server.OnNewClient += new EventHandler<SocketAsyncEventArgs>(server_OnNewClientAccept);
            server.OnReceivedData += new EventHandler<SocketAsyncEventArgs>(server_OnDataReceived);
            server.OnSentData += new EventHandler<SocketAsyncEventArgs>(server_OnDataSending);

            server.Start();
           
            
            Timer timer = new Timer(new TimerCallback(DrawDisplay), null, 200, 1000);
            Console.ReadKey();


        }

        

       
        #region 事件处理
        static void server_OnDataReceived(object sender, EventArgs e)
        {
            receivedMessages = Interlocked.Increment(ref receivedMessages);
        

            receivedBytes = Interlocked.Add(ref receivedBytes, (e as SocketAsyncEventArgs).BytesTransferred);
        
        }

        static void server_OnNewClientAccept(object sender, EventArgs e)
        {
            numClient = Interlocked.Increment(ref numClient);
            client = Interlocked.Increment(ref client);
        }

        static void server_OnDataSending(object sender, EventArgs e)
        {
            sentMessages = Interlocked.Increment(ref sentMessages);
      

            sentBytes = Interlocked.Add(ref sentBytes, (e as SocketAsyncEventArgs).BytesTransferred);
         
        }
        #endregion


        static void DrawDisplay(Object obj)
        {
            Console.Clear();
            Console.WriteLine(String.Format("服务器 运行中...\n\n" +
                "新客户端数：{0}\n"+
                "当前客户端数： {1}\n\n" +
                "当前每秒收到消息： {2}\n" +
                "当前每秒发送消息：{3}\n\n"+
                "当前每秒收到字节：{4}\n"+
                "当前每秒发送字节：{5}\n\n"+

                "按任意键结束。。。。。。。。。。。",client, numClient , receivedMessages ,sentMessages,receivedBytes ,sentBytes));

            receivedBytes = 0;
            receivedMessages = 0;
            sentBytes = 0;
            sentMessages = 0;
            client = 0;
        }

    }
}
