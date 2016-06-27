using mywcfService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace Hosting
{
    class Program
    {
        static void Main(string[] args)
        {
            //using (ServiceHost host = new ServiceHost(typeof(CalculatorService)))
            //{
            //    host.AddServiceEndpoint(
            //        typeof(ICalculator),
            //        new WSHttpBinding(),
            //        "http://127.0.0.1:1111/CalculatorService");
            //    if (host.Description.Behaviors.Find<ServiceMetadataBehavior>() == null)
            //    {
            //        ServiceMetadataBehavior behavior = new ServiceMetadataBehavior();
            //        behavior.HttpGetEnabled = true;
            //        behavior.HttpGetUrl = new Uri("http://127.0.0.1:1111/CalculatorService/metadata");
            //        host.Description.Behaviors.Add(behavior);
            //    }
            //    host.Opened += delegate
            //    {
            //        Console.Write("CalculatorService已经启动，按任意键终止服务");
            //    };
            //    host.Open();
            //    Console.Read();
            //}
            using (ServiceHost host = new ServiceHost(typeof(CalculatorService)))//已经在app.config配置
            {
                host.Opened += delegate
                {
                    Console.Write("CalculatorService已经启动，按任意键终止服务");
                };
                host.Open();
                Console.Read();
               
            }
            
        }
    }
}
