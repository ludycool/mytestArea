using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SocketUdpClient
{
    class Program
    {
        static string serverIp;
        static string name = string.Empty;
        static int port1,port2;
        static UdpClient client;
        static void Main(string[] args)
        {
            Console.WriteLine("输入服务器IP地址");
            serverIp = Console.ReadLine();
            Console.WriteLine("输入接入端口");
            port1 = int.Parse(Console.ReadLine());
            Console.WriteLine("输入通信端口");
            port2 = int.Parse(Console.ReadLine());

            while (running()) ;

            Console.ReadKey();
        }


        static bool running()
        {
            int txtNum;
            Console.WriteLine("输入最大连接数");
            txtNum = int.Parse(Console.ReadLine());
            client = new UdpClient(port1, port2, serverIp, txtNum);

            for (int i = 0; i < txtNum; i++)
            {
                client.Start();
              
                Thread.Sleep(30);
            }
            return txtNum > 0;
        }
    }
}
