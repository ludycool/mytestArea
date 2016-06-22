using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace WCFServiceHostInConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Uri uri = new Uri(ConfigurationManager.AppSettings["addr"]);
            using (ServiceHost host = new ServiceHost(typeof(StudentService), uri))
            {
                ServiceMetadataBehavior smb = host.Description.Behaviors.Find<ServiceMetadataBehavior>();
                if (smb == null)
                    host.Description.Behaviors.Add(new ServiceMetadataBehavior());

                host.AddServiceEndpoint(typeof(IMetadataExchange),MetadataExchangeBindings.CreateMexTcpBinding(),"mex");
                host.Open();

                Console.WriteLine("Service listen on endpoint {0}", uri.ToString());
                Console.WriteLine("press any key to teriminate...");
                Console.ReadKey();

                host.Abort();
                host.Close();
            }
        }
    }
}
