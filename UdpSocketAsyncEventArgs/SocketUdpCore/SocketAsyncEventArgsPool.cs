using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace SocketUdpCore
{
    public class SocketAsyncEventArgsPool
    {


        private Stack<SocketAsyncEventArgs> m_pool;
        private static readonly object thislock = new object();


        /// <summary>
        /// 根据参数初始化对象池，并且设立池的大小
        /// </summary>
        /// <param name="capacity">对象池的最大容量</param>
        public  SocketAsyncEventArgsPool(int capacity)
        {
            m_pool = new Stack<SocketAsyncEventArgs>(capacity);
        }


        /// <summary>
        /// 将SocketAsyncEventArgs对象放回池中
        /// </summary>
        /// <param name="item"></param>
        public void Push(SocketAsyncEventArgs item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("Items不能为 Null");

            }
            lock (thislock)
            {
                m_pool.Push(item);
            }

        }


        /// <summary>
        /// 从池中取出一个SocketAsyncEventArgs对象
        /// </summary>
        /// <param name="item"></param>
        public SocketAsyncEventArgs Pop()
        {
            lock (thislock)
            {
                return m_pool.Pop();
            }
        }

        /// <summary>
        /// 池中SocketAsyncEventArgs的个数
        /// </summary>
        public int Count
        {
            get { return m_pool.Count; }
        }
    }
}
