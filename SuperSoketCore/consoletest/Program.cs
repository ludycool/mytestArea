using Core.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Config;
using System;
using System.Collections.Generic;

namespace consoletest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            #region 硬编码 启动

            var appServer = new MyServer();
            IServerConfig m_Config;

            m_Config = new ServerConfig
            {
                Port = 2000, //服务器端口
                Ip = "Any",
                MaxConnectionNumber = 20000,
                Mode = SocketMode.Udp,//tcp udp
                Name = "GPSServer2"
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

            #endregion
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
            //  appServer.Stop();
            //Stop the appServer

            Console.WriteLine("服务已停止，按任意键退出!");


 
            Console.ReadLine();
        }
    }

 
}
