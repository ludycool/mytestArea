﻿using System;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace WCFServiceGeneratedByConfig
{
    class Program
    {
        static void Main(string[] args)
        {
            
            using (ServiceHost host = new ServiceHost(typeof(StudentService)))
            {
                
                ServiceMetadataBehavior smb = host.Description.Behaviors.Find<ServiceMetadataBehavior>();
                if (smb == null)
                    host.Description.Behaviors.Add(new ServiceMetadataBehavior());

                //暴露出元数据，以便能够让SvcUtil.exe自动生成配置文件
                host.AddServiceEndpoint(typeof(IMetadataExchange), MetadataExchangeBindings.CreateMexTcpBinding(), "mex");

                //开启服务
                host.Open();

                Console.WriteLine("Service listen begin to listen...");
                Console.WriteLine("press any key to teriminate...");
                Console.ReadKey();

                host.Abort();
                host.Close();
            }
        }
    }
}
