using Client.ServiceReference1;
using mywcfService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            //using (wcfSendServerClient proxy = new Client.ServiceReference1.wcfSendServerClient())
            //{

            //    Console.Read();
            //    while (true)
            //    {
            //        string data = Console.ReadLine();
            //        string ip = "127.0.0.1";
            //        int pot = 9999;

            //     bool dd=   proxy.UdpSendData4(data, ip, pot);

            //     Console.Write(dd);
            //    }
            //}
            using (ChannelFactory<ICalculator> channelFactory = new ChannelFactory<ICalculator>("CalculatorService2"))
            {
                ICalculator proxy = channelFactory.CreateChannel();
                Console.WriteLine("x+y={2} when x={0} and y={1}", 1, 2, proxy.Add(1, 2));
                Console.WriteLine("x-y={2} when x={0} and y={1}", 10, 2, proxy.Subtract(10, 2));
                Console.WriteLine("x*y={2} when x={0} and y={1}", 10, 2, proxy.Multiply(10, 2));
                Console.WriteLine("x/y={2} when x={0} and y={1}", 10, 2, proxy.Divide(10, 2));
                Console.ReadKey();
            }
        }
    }
}
