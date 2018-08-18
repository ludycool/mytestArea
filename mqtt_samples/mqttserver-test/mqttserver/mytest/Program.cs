using NettyServer;
using System;

namespace mytest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            ServerConfig config = new ServerConfig();
            config.Mode = SocketMode.Tcp;
            config.ClearIdleSessionInterval = 120;
            config.Port = 15678;
            myServer server = new myServer();
            bool issee = server.startServer(config);

            Console.WriteLine("启动结果：" + issee);
            Console.ReadLine();
        }
    }
}
