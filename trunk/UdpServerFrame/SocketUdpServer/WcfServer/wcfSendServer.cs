using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace SocketUdpServer.WcfServer
{
    public class wcfSendServer : IwcfSendServer
    {
        /// <summary>
        /// udp  发送数据
        /// </summary>
        /// <param name="content"></param>
        /// <param name="remoteEndPoint"></param>
        public bool UdpSendData(byte[] content, EndPoint remoteEndPoint)
        {

            return ContextCenter.UdpSendData(content, remoteEndPoint);

        }
        /// <summary>
        ///  udp  发送数据
        /// </summary>
        /// <param name="content"></param>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        public bool UdpSendData2(byte[] content, string ip, int port)
        {
            IPAddress broadcast = IPAddress.Parse(ip);
            IPEndPoint ep = new IPEndPoint(broadcast, port);
            return ContextCenter.UdpSendData(content, ep);

        }
        /// <summary>
        ///  udp  发送数据
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="remoteEndPoint"></param>
        public bool UdpSendData3(string datas, EndPoint remoteEndPoint)
        {
            byte[] sendData = Encoding.UTF8.GetBytes(datas);
            return ContextCenter.UdpSendData(sendData, remoteEndPoint);

        }
        /// <summary>
        ///  udp  发送数据
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        public bool UdpSendData4(string datas, string ip, int port)
        {
            byte[] sendData = Encoding.UTF8.GetBytes(datas);
            IPAddress broadcast = IPAddress.Parse(ip);
            IPEndPoint ep = new IPEndPoint(broadcast, port);
            return ContextCenter.UdpSendData(sendData, ep);

        }

    }
}
