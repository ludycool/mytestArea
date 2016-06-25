using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace AsyncSocketServer
{
    /// <summary>
    /// 基础协议
    /// </summary>
    public class BaseSocketProtocol : AsyncSocketInvokeElement
    {
        protected string m_userName;
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get { return m_userName; } }
        protected bool m_logined;
        /// <summary>
        /// 是否登录
        /// </summary>
        public bool Logined { get { return m_logined; } }
        protected string m_socketFlag;
        /// <summary>
        /// 标签
        /// </summary>
        public string SocketFlag { get { return m_socketFlag; } }

        public BaseSocketProtocol(AsyncSocketServer asyncSocketServer, AsyncSocketUserToken asyncSocketUserToken)
            : base(asyncSocketServer, asyncSocketUserToken)
        {
            m_userName = "";
            m_logined = false;
            m_socketFlag = "";
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        public bool DoLogin()
        {
            string userName = "";
            string password = "";
            if (m_incomingDataParser.GetValue(ProtocolKey.UserName, ref userName) & m_incomingDataParser.GetValue(ProtocolKey.Password, ref password))
            {
                if (password.Equals(BasicFunc.MD5String("admin"), StringComparison.CurrentCultureIgnoreCase))
                {
                    m_outgoingDataAssembler.AddSuccess();
                    m_userName = userName;
                    m_logined = true;
                    Program.Logger.InfoFormat("{0} login success", userName);
                }
                else
                {
                    m_outgoingDataAssembler.AddFailure(ProtocolCode.UserOrPasswordError, "");
                    Program.Logger.ErrorFormat("{0} login failure,password error", userName);
                }
            }
            else
                m_outgoingDataAssembler.AddFailure(ProtocolCode.ParameterError, "");
            return DoSendResult();
        }

        /// <summary>
        /// 连接
        /// </summary>
        /// <returns></returns>
        public bool DoActive()
        {
            m_outgoingDataAssembler.AddSuccess();
            return DoSendResult();
        }
    }
}
