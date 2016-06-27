using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;

namespace SocketUdpServer.WcfServer
{
    /// <summary>
    /// 创建wcf服务
    /// </summary>
   public class WcfHost
    {

       public  WcfHost()
       { 
     
         
       
     
       
       }
       /// <summary>
       /// 开启服务
       /// </summary>
       public void start()
       {
           //using (ServiceHost host = new ServiceHost(typeof(wcfSendServer)))
           //{
           //    host.AddServiceEndpoint(
           //        typeof(IwcfSendServer),
           //        new WSHttpBinding(),
           //        "http://127.0.0.1:8111/wcfSendServer");
           //    if (host.Description.Behaviors.Find<ServiceMetadataBehavior>() == null)
           //    {
           //        ServiceMetadataBehavior behavior = new ServiceMetadataBehavior();
           //        behavior.HttpGetEnabled = true;
           //        behavior.HttpGetUrl = new Uri("http://127.0.0.1:8111/wcfSendServer/metadata");
           //        host.Description.Behaviors.Add(behavior);
           //    }
           //    host.Opened += delegate
           //    {
           //        Console.Write("CalculatorService已经启动，按任意键终止服务");
           //    };
           //    host.Open();
           //    Console.Read();
           //}

           ServiceHost host = new ServiceHost(typeof(wcfSendServer));//已经在app.config配置
          
               host.Opened += delegate
               {
                   //Console.Write("CalculatorService已经启动，按任意键终止服务");
               };
               host.Open();
           
       
       }
    }
}
