using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Text;

namespace SocketUdpServer.WcfServer
{
     [ServiceContract(Name = "wcfSendServer", Namespace = "Listen.Fly")]
  public  interface IwcfSendServer
    {
          [OperationContract]
        /// <summary>
        /// udp  发送数据
        /// </summary>
        /// <param name="content"></param>
        /// <param name="remoteEndPoint"></param>
         bool UdpSendData(byte[] content, EndPoint remoteEndPoint);
          [OperationContract]
        /// <summary>
        ///  udp  发送数据
        /// </summary>
        /// <param name="content"></param>
        /// <param name="ip"></param>
        /// <param name="port"></param>
         bool UdpSendData2(byte[] content, string ip, int port);
          [OperationContract]
        /// <summary>
        ///  udp  发送数据
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="remoteEndPoint"></param>
         bool UdpSendData3(string datas, EndPoint remoteEndPoint);
          [OperationContract]
        /// <summary>
        ///  udp  发送数据
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="ip"></param>
        /// <param name="port"></param>
         bool UdpSendData4(string datas, string ip, int port);
    }
}
