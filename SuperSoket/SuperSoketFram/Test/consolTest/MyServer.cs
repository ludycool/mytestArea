using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace consolTest
{
    /// <summary>
    /// 自定义服务器类MyServer，继承AppServer，并传入自定义连接类MySession
    /// </summary>
    public class MyServer : AppServer<MySession>
    {
        protected override bool Setup(IRootConfig rootConfig, IServerConfig config)
        {
            return base.Setup(rootConfig, config);
        }

        protected override void OnStarted()
        {
            //LogHelper.WriteLog("WeChat服务启动");
            base.OnStarted();

        }

        protected override void OnStopped()
        {
            //LogHelper.WriteLog("WeChat服务停止");
            base.OnStopped();
        }

        /// <summary>
        /// 新的连接
        /// </summary>
        /// <param name="session"></param>
        protected override void OnNewSessionConnected(MySession session)
        {
            //LogHelper.WriteLog("WeChat服务新加入的连接:" + session.LocalEndPoint.Address.ToString());
            base.OnNewSessionConnected(session);
        }
    }
}
