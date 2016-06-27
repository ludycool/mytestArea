using System;
using System.Net.Sockets;
using System.Net;
using SocketUdpCore;
using System.Threading;
using System.Collections.Generic;
using System.Text;

namespace SocketUdpServer
{
    /// <summary>
    /// 发送数据类
    /// </summary>
    public class UdpSend
    {
        /// <summary>
        /// 最大并发数
        /// </summary>
        private int numClient;

        /// <summary>

        /// <summary>
        /// 负责向客户端发送数据
        /// </summary>
        private UdpSendSocket communicationSend;

        /// <summary>
        /// 发送数据完之后 处理事件
        /// </summary>
        public event EventHandler<SocketAsyncEventArgs> OnSentData;


        /// <summary>
        /// udp 发送
        /// </summary>
        /// <param name="maxNumClient">最大客户端数</param>
        public UdpSend( int maxNumClient)
        {
         

            numClient = maxNumClient;
            communicationSend = new UdpSendSocket(numClient);
            communicationSend.DataSent += new EventHandler<SocketAsyncEventArgs>(communicationSend_DataSent);
            communicationSend.Init();
        }


        #region 事件

        public bool SendData(byte[] content, EndPoint remoteEndPoint)
        {
            //#region  test
            //int offset = e.Offset;
            //int count = e.BytesTransferred;
            ////不要进行耗时操作
            //byte[] receivedBuff = e.Buffer;
            //string tmpStr = Encoding.UTF8.GetString(receivedBuff, offset, count);
            //byte[] sendData = Encoding.UTF8.GetBytes("收到了kkkoo");
            //#endregion

            //向客户端发送数据
            try
            {
                communicationSend.Send(content, remoteEndPoint);
                return true;
            }
            catch {
                return false;
            
            }
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
