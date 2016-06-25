using System.Collections.Generic;
using System.Net.Sockets;
using System;
namespace SocketUdpCore
{
  public   class BufferManager
    {
        int numBytes;
        byte[] buffer;
        Stack<int> freeIndexPool;
        int currentIndex;
        int bufferSize;
        Random random;

      /// <summary>
      /// 缓存管理，可以创建不那么零散的缓存块
      /// </summary>
      /// <param name="totalBytes">缓存的总大小</param>
      /// <param name="bufferSize">每个缓存的大小</param>
        public BufferManager(int totalBytes, int bufferSize)
        {
            numBytes = totalBytes;
            currentIndex = 0;
            this.bufferSize = bufferSize;
            freeIndexPool = new Stack<int>();
            random = new Random();

        }

      /// <summary>
      /// 初始化缓存
      /// </summary>
        public void InitBuffer()
        {
            buffer = new byte[numBytes];
            //随机填写消息
            random.NextBytes(buffer);
        }

      /// <summary>
      /// 设置缓存
      /// </summary>
      /// <param name="args">增强型异步缓存上下文类</param>
      /// <returns></returns>
        public bool SetBuffer(SocketAsyncEventArgs args)
        {
            if (freeIndexPool.Count > 0)
            {
                args.SetBuffer(buffer, freeIndexPool.Pop(), bufferSize);
            }
            else
            {
                if ((numBytes - bufferSize) < currentIndex)
                {
                    return false;
                }
                args.UserToken = currentIndex;
                args.SetBuffer(buffer, currentIndex, bufferSize);
                currentIndex += bufferSize;

            }
            return true;
        }

        public void  SetBufferValue(SocketAsyncEventArgs args, byte[] value)
        { 
            int offsize=(int)args.UserToken ;
            for (int i = offsize; i < bufferSize+offsize ; i++)
            {
                if (i >= value.Length)
                {
                    break;
                }
                buffer[i] = value[i - offsize];
            }
        }


     
         /// <summary>
        /// 释放缓存
        /// </summary>
        /// <param name="args">增强型异步缓存上下文类</param>
        public void FreeBuffer(SocketAsyncEventArgs args)
        {
            freeIndexPool.Push(args.Offset);

            args.SetBuffer(null, 0, 0);
        }
    }
}
