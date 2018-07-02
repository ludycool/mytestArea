using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase.Config;
using System.Configuration;
using SuperSocket.Common;
using Newtonsoft.Json.Linq;

namespace SuperSocket.SocketEngine.Configuration
{
    /// <summary>
    /// Listener configuration
    /// </summary>
    public class Listener : IListenerConfig
    {
        JObject obj;
        public Listener(JObject _obj)
        {
            obj = _obj;
        }

        /// <summary>
        /// Gets the ip of listener
        /// </summary>
        public string Ip
        {
            get { return obj["ip"].ToString(); }
        }

        /// <summary>
        /// Gets the port of listener
        /// </summary>
        public int Port
        {
            get
            {

                return int.Parse(obj["backlog"].ToString());

            }
        }

        /// <summary>
        /// Gets the backlog.
        /// </summary>
        public int Backlog
        {
            get
            {
                if (obj != null && obj["backlog"] != null)
                {
                    return int.Parse(obj["backlog"].ToString());
                }
                else
                {
                    return 100;
                }
            }
        }

        /// <summary>
        /// Gets the security option, None/Default/Tls/Ssl/...
        /// </summary>
        public string Security
        {
            get
            {
                if (obj != null && obj["security"] != null)
                {
                    return obj["security"].ToString();
                }
                else
                {
                    return "";
                }
            }
        }
    }
}
