using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace consolTest.Command
{
    /// <summary>
    /// 自定义命令类HELLO，继承CommandBase，并传入自定义连接类MySession
    /// </summary>
    public class HELLO : CommandBase<MySession, BinaryRequestInfo>
    {
        /// <summary>
        /// 自定义执行命令方法，注意传入的变量session类型为MySession
        /// </summary>
        /// <param name="session"></param>
        /// <param name="requestInfo"></param>
        public override void ExecuteCommand(MySession session, BinaryRequestInfo requestInfo)
        {

            #region  设置远程ip 进行转发
            //session.RemoteEndPoint.Address = IPAddress.Parse("192.168.11.205");
            //session.RemoteEndPoint.Port = 60000;
            #endregion
            session.Send("Hello World!");
        }
    }
}
