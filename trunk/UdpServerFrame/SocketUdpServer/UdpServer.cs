using System;
using System.Net.Sockets;
using System.Net;
using SocketUdpCore;
using System.Threading;
using System.Collections.Generic;
using System.Text;

namespace SocketUdpServer
{
    public class UdpServer
    {

        /// <summary>
        /// 数据通讯的端口
        /// </summary>
        private int CommunicationPort;
        /// <summary>
        /// 最大的客户端数
        /// </summary>
        private int numClient;

        /// <summary>

        /// <summary>
        /// 负责接收旧客户端的数据
        /// </summary>
        private UdpReceiveSocket communicationRec;
        /// <summary>
        /// 负责向客户端发送数据
        /// </summary>
        private UdpSendSocket communicationSend;
        private bool isStartSend;

        /// <summary>
        /// 接收数据完后，处理事件
        /// </summary>
        public event EventHandler<SocketAsyncEventArgs> OnReceivedData;
        /// <summary>
        /// 发送数据完之后 处理事件
        /// </summary>
        public event EventHandler<SocketAsyncEventArgs> OnSentData;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataComPort">数据通讯端口</param>
        /// <param name="maxNumClient">最大客户端数</param>
        public UdpServer( int dataComPort, int maxNumClient)
        {
         
            CommunicationPort = dataComPort;
            numClient = maxNumClient;
            isStartSend = false;

            communicationRec = new UdpReceiveSocket(CommunicationPort);
            //接收后事件
            communicationRec.OnDataReceived += new EventHandler<SocketAsyncEventArgs>(communicationRec_OnDataReceived);

            communicationSend = new UdpSendSocket(numClient);
            communicationSend.DataSent += new EventHandler<SocketAsyncEventArgs>(communicationSend_DataSent);
            communicationSend.Init();
        }


        public void Start()
        {
            //接收数据
            communicationRec.StartReceive();

      
        }

        #region 事件
        /// <summary>
        /// 接收后，的业务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void communicationRec_OnDataReceived(object sender, SocketAsyncEventArgs e)
        {
            #region  接收之后 处理数据
            int offset = e.Offset;
            int count = e.BytesTransferred;
            //不要进行耗时操作
            byte[] receivedBuff = e.Buffer;
            string tmpStr = Encoding.UTF8.GetString(receivedBuff, offset, count);
            byte[] sendData = Encoding.UTF8.GetBytes("收到了kkkoo");
            #endregion

            if (OnReceivedData != null)
            {
                OnReceivedData(sender, e);
            }
            //处理完回发
            communicationSend.Send(sendData,e.RemoteEndPoint);
        }
    

        /// <summary>
        /// 发送完成之后
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void communicationSend_DataSent(object sender, SocketAsyncEventArgs e)
        {
          
            if (OnSentData != null)
            {
                OnSentData(sender, e);
            }
         
        }

        #endregion
    }
}
