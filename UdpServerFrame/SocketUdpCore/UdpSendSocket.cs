using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic ;
using System.Threading;

namespace SocketUdpCore
{
   public  class UdpSendSocket
    {

       private SocketAsyncEventArgsPool socketArgsPool;

       private BufferManager bfManager;

       private Socket socket;

       private SocketAsyncEventArgs socketArgs;

       private int numClient;
       /// <summary>
       /// 发送完成之后处理
       /// </summary>
       public event EventHandler<SocketAsyncEventArgs> DataSent;

       public event EventHandler<SocketAsyncEventArgs> DataSending;

       private static readonly object asyncLock = new object();
       /// <summary>
       /// 最大客户端数
       /// </summary>
       /// <param name="numClient"></param>
       public UdpSendSocket(int numClient)
       {
           socket  = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
           this.numClient = numClient;
           int bufferSize = 1024;
           bfManager = new BufferManager(numClient * bufferSize * 2, bufferSize);
           socketArgsPool = new SocketAsyncEventArgsPool(numClient);

       }
       /// <summary>
       /// 初始化
       /// </summary>
       public void Init()
       {
           //初始化数据池
           bfManager.InitBuffer();
           //生成一定数量的对象池
           for (int i = 0; i < numClient; i++)
           {
               socketArgs = new SocketAsyncEventArgs();
               socketArgs .Completed +=new EventHandler<SocketAsyncEventArgs>(socketArgs_Completed);
              //设置SocketAsyncEventArgs的Buffer
               bfManager.SetBuffer(socketArgs);
               socketArgsPool.Push(socketArgs);
           }
       }



       


   
       /// <summary>
       /// 发送数据
       /// </summary>
       /// <param name="content">字节流</param>
       /// <param name="remoteEndPoint">远程ip和端口</param>
       public void Send(byte[] content, EndPoint remoteEndPoint)
       {
           socketArgs = socketArgsPool.Pop();
           socketArgs.RemoteEndPoint = remoteEndPoint;
           //设置发送的内容
           //bfManager.SetBufferValue(socketArgs, content); 这里有问题
           socketArgs.SetBuffer(content, 0, content.Length);
           if (socketArgs.RemoteEndPoint != null)
           {
               if (!socket.SendToAsync(socketArgs))
               {
                   ProcessSent(socketArgs);
               }
           }
       }

       private void socketArgs_Completed(object sender, SocketAsyncEventArgs e)
       {
           switch (e.LastOperation)
           {
               case SocketAsyncOperation.SendTo:
                   this.ProcessSent(e);
                   break;
               default:
                   throw new ArgumentException("The last operation completed on the socket was not a send");
           }
       }
       /// <summary>
       /// 发送完成之后 
       /// </summary>
       /// <param name="e"></param>
       private void ProcessSent(SocketAsyncEventArgs e)
       {
           
           if (e.BytesTransferred > 0 && e.SocketError == SocketError.Success)
           {
               if (DataSent != null)
               {
                   //用于统计发送了多少数据
                   DataSent(socket, e);
               }
           }
           //发送完成后将SocketAsyncEventArgs对象放回对象池中
           socketArgsPool.Push(e);
         
       }

    }
}
