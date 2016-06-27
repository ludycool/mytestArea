using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace SocketUdpServer
{
    /// <summary>
    /// 使用运行的服务中的上下文 提供引用方法
    /// </summary>
    internal class ContextCenter
    { 
        /// <summary>
        /// 进程运行前进行赋值(引用地址)
        /// </summary>
        internal static UdpSend SendServer { get; set; }
        internal static void Init(UdpSend udpCient)
      {
          SendServer = udpCient;
      }

     /// <summary>
        /// udp  发送数据
     /// </summary>
     /// <param name="content"></param>
     /// <param name="remoteEndPoint"></param>
        internal static bool UdpSendData(byte[] content, EndPoint remoteEndPoint)
        {

           return SendServer.SendData(content, remoteEndPoint);

        }
    }
}
