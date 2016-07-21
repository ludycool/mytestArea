using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSocket.Common;
using SuperSocket.SocketBase;
using SuperSocket.SocketEngine;
using SuperSocket.SocketBase.Protocol;
using SuperSocket.SocketBase.Config;
using SuperSocket.SocketBase.Logging;
using System.Net;  


namespace consolTest
{
    class Program
    {
        static void Main(string[] args)
        {

            //var appServer = new AppServer();

            ////服务器端口
            //int port = 2000;

            ////设置服务监听端口
            //if (!appServer.Setup(port))
            //{
            //    Console.WriteLine("端口设置失败!");
            //    Console.ReadKey();
            //    return;
            //}

            ////新连接事件
            //appServer.NewSessionConnected += new SessionHandler<AppSession>(NewSessionConnected);

            ////收到消息事件
            //appServer.NewRequestReceived += new RequestHandler<AppSession, StringRequestInfo>(NewRequestReceived);

            ////连接断开事件
            //appServer.SessionClosed += new SessionHandler<AppSession, CloseReason>(SessionClosed);

            ////启动服务
            //if (!appServer.Start())
            //{
            //    Console.WriteLine("启动服务失败!");
            //    Console.ReadKey();
            //    return;
            //}

            //Console.WriteLine("启动服务成功，输入exit退出!");

            //while (true)
            //{
            //    var str = Console.ReadLi"exit"))
            //    {ne();
            //    if (str.ToLower().Equals(
            //        break;
            //    }
            //}

            //Console.WriteLine();

            ////停止服务
            //appServer.Stop();

            //Console.WriteLine("服务已停止，按任意键退出!");
     




            var appServer = new MyServer();
              IServerConfig m_Config;
             
              m_Config = new ServerConfig
              {
                  Port = 2000, //服务器端口
                  Ip = "Any",
                  MaxConnectionNumber = 20000,
                  Mode = SocketMode.Tcp,//tcp udp
                  Name = "GPSServer"
              };

            //设置服务监听端口
            if (!appServer.Setup(new RootConfig(), m_Config))
            {
                Console.WriteLine("端口设置失败!");
                Console.ReadKey();
                return;
            }

            //启动服务
            if (!appServer.Start())
            {
                Console.WriteLine("启动服务失败!");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("启动服务成功，输入exit退出!");

            while (true)
            {
                var str = Console.ReadLine();
                if (str.ToLower().Equals("exit"))
                {
                    break;
                }
            }

            Console.WriteLine();

            //停止服务
            appServer.Stop();

            Console.WriteLine("服务已停止，按任意键退出!");


            Console.ReadKey();
        }


        static void NewSessionConnected(AppSession session)
        {
            //向对应客户端发送数据
            session.Send("Hello User!");
        }

        static void NewRequestReceived(AppSession session, StringRequestInfo requestInfo)
        {
            /**
             * requestInfo为客户端发送的指令，默认为命令行协议
             * 例：
             * 发送 ping 127.0.0.1 -n 5
             * requestInfo.Key: "ping"
             * requestInfo.Body: "127.0.0.1 -n 5"
             * requestInfo.Parameters: ["127.0.0.1","-n","5"]
             **/
            switch (requestInfo.Key.ToUpper())
            {
                case ("HELLO"):
                    session.Send("Hello World!");
                    break;

                default:
                    session.Send("未知的指令。");
                    break;
            }
        }

        static void SessionClosed(AppSession session, CloseReason reason)
        {

        }
    }
}
