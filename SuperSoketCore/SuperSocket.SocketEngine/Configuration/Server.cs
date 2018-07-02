using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Security.Authentication;
using System.Text;
using System.Xml;
using System.Linq;
using SuperSocket.Common;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Config;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace SuperSocket.SocketEngine.Configuration
{
    /// <summary>
    /// Server configuration
    /// </summary>
    public partial class Server : IServerConfig
    {
        JToken obj;
        public Server(JToken _obj)
        {
            obj = _obj;
        }
        /// <summary>
        /// Gets the name of the server type obj appServer want to use.
        /// </summary>
        /// <value>
        /// The name of the server type.
        /// </value>
        public string ServerTypeName
        {
            get { return obj["serverTypeName"].ToString(); }
        }

        /// <summary>
        /// Gets the type definition of the appserver.
        /// </summary>
        /// <value>
        /// The type of the server.
        /// </value>
        public string ServerType
        {
            get { return obj["serverType"].ToString(); }
        }

        /// <summary>
        /// Gets the Receive filter factory.
        /// </summary>
        public string ReceiveFilterFactory
        {
            get { return obj["receiveFilterFactory"].ToString(); }
        }

        /// <summary>
        /// Gets the ip.
        /// </summary>
        public string Ip
        {
            get { return obj["ip"].ToString(); }
        }

        /// <summary>
        /// Gets the port.
        /// </summary>
        public int Port
        {
            get { return int.Parse(obj["port"].ToString()); }
        }
        /// <summary>
        /// Gets the port.
        /// </summary>
        public string Name
        {
            get { return obj["name"].ToString(); }
        }
        /// <summary>
        /// Gets the mode.
        /// </summary>
        public SocketMode Mode
        {
            get
            {
                if (obj != null && obj["mode"] != null)
                {
                    if (obj != null && obj["mode"].ToString().ToLower().Equals("udp"))
                    {
                        return SocketMode.Udp;
                    }
                    else
                    {
                        return SocketMode.Tcp;
                    }
                }
                else
                {
                    return SocketMode.Tcp;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether obj <see cref="IServerConfig"/> is disabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if disabled; otherwise, <c>false</c>.
        /// </value>
        public bool Disabled
        {
            get
            {
                if (obj != null && obj["disabled"] != null)
                {
                    return bool.Parse(obj["disabled"].ToString());
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Gets the send time out.
        /// </summary>
        public int SendTimeOut
        {

            get
            {
                if (obj != null && obj["sendTimeOut"] != null)
                {
                    return int.Parse(obj["sendTimeOut"].ToString());
                }
                else
                {
                    return ServerConfig.DefaultSendTimeout;
                }
            }
        }

        /// <summary>
        /// Gets the max connection number.
        /// </summary>
        public int MaxConnectionNumber
        {
            get
            {
                if (obj != null && obj["maxConnectionNumber"] != null)
                {
                    return int.Parse(obj["maxConnectionNumber"].ToString());
                }
                else
                {
                    return ServerConfig.DefaultMaxConnectionNumber;
                }
            }
        }

        /// <summary>
        /// Gets the size of the receive buffer.
        /// </summary>
        /// <value>
        /// The size of the receive buffer.
        /// </value>
        public int ReceiveBufferSize
        {
            get
            {
                if (obj != null && obj["receiveBufferSize"] != null)
                {
                    return int.Parse(obj["receiveBufferSize"].ToString());
                }
                else
                {
                    return ServerConfig.DefaultReceiveBufferSize;
                }
            }
        }

        /// <summary>
        /// Gets the size of the send buffer.
        /// </summary>
        /// <value>
        /// The size of the send buffer.
        /// </value>
        public int SendBufferSize
        {
            get
            {
                if (obj != null && obj["sendBufferSize"] != null)
                {
                    return int.Parse(obj["sendBufferSize"].ToString());
                }
                else
                {
                    return ServerConfig.DefaultSendBufferSize;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether sending is in synchronous mode.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [sync send]; otherwise, <c>false</c>.
        /// </value>
        public bool SyncSend
        {
            get
            {
                if (obj != null && obj["syncSend"] != null)
                {
                    return bool.Parse(obj["syncSend"].ToString());
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether log command in log file.
        /// </summary>
        /// <value><c>true</c> if log command; otherwise, <c>false</c>.</value>
        public bool LogCommand
        {
            get
            {
                if (obj != null && obj["logCommand"] != null)
                {
                    return bool.Parse(obj["logCommand"].ToString());
                }
                else
                {
                    return false;
                }
            }
        }


        /// <summary>
        /// Gets a value indicating whether [log basic session activity like connected and disconnected].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [log basic session activity]; otherwise, <c>false</c>.
        /// </value>
        public bool LogBasicSessionActivity
        {

            get
            {
                if (obj != null && obj["logBasicSessionActivity"] != null)
                {
                    return bool.Parse(obj["logBasicSessionActivity"].ToString());
                }
                else
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether [log all socket exception].
        /// </summary>
        /// <value>
        /// <c>true</c> if [log all socket exception]; otherwise, <c>false</c>.
        /// </value>
        public bool LogAllSocketException
        {

            get
            {
                if (obj != null && obj["logAllSocketException"] != null)
                {
                    return bool.Parse(obj["logAllSocketException"].ToString());
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether clear idle session.
        /// </summary>
        /// <value><c>true</c> if clear idle session; otherwise, <c>false</c>.</value>
        public bool ClearIdleSession
        {
            get
            {
                if (obj != null && obj["clearIdleSession"] != null)
                {
                    return bool.Parse(obj["clearIdleSession"].ToString());
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Gets the clear idle session interval, in seconds.
        /// </summary>
        /// <value>The clear idle session interval.</value>
        public int ClearIdleSessionInterval
        {
            get
            {
                if (obj != null && obj["clearIdleSessionInterval"] != null)
                {
                    return int.Parse(obj["clearIdleSessionInterval"].ToString());
                }
                else
                {
                    return ServerConfig.DefaultClearIdleSessionInterval;
                }
            }
        }


        /// <summary>
        /// Gets the idle session timeout time length, in seconds.
        /// </summary>
        /// <value>The idle session time out.</value>
        public int IdleSessionTimeOut
        {
            get
            {
                if (obj != null && obj["idleSessionTimeOut"] != null)
                {
                    return int.Parse(obj["idleSessionTimeOut"].ToString());
                }
                else
                {
                    return ServerConfig.DefaultIdleSessionTimeOut;
                }
            }
        }

        /// <summary>
        /// Gets the certificate config.
        /// </summary>
        /// <value>The certificate config.</value>
        public CertificateConfig CertificateConfig
        {
            get
            {
                if (obj != null && obj["certificate"] != null)
                {
                    return JsonConvert.DeserializeObject<CertificateConfig>(obj["certificate"].ToString());
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Gets X509Certificate configuration.
        /// </summary>
        /// <value>
        /// X509Certificate configuration.
        /// </value>
        public ICertificateConfig Certificate
        {
            get { return CertificateConfig; }
        }

        /// <summary>
        /// Gets the security protocol, X509 certificate.
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
                    return "None";
                }
            }
        }

        /// <summary>
        /// Gets the max allowed length of request.
        /// </summary>
        /// <value>
        /// The max allowed length of request.
        /// </value>
        public int MaxRequestLength
        {
            get
            {
                if (obj != null && obj["maxRequestLength"] != null)
                {
                    return int.Parse(obj["maxRequestLength"].ToString());
                }
                else
                {
                    return ServerConfig.DefaultMaxRequestLength;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether [disable session snapshot]
        /// </summary>
        public bool DisableSessionSnapshot
        {
            get
            {
                if (obj != null && obj["disableSessionSnapshot"] != null)
                {
                    return bool.Parse(obj["disableSessionSnapshot"].ToString());
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Gets the interval to taking snapshot for all live sessions.
        /// </summary>
        public int SessionSnapshotInterval
        {
            get
            {

                if (obj != null && obj["sessionSnapshotInterval"] != null)
                {
                    return int.Parse(obj["sessionSnapshotInterval"].ToString());
                }
                else
                {
                    return ServerConfig.DefaultSessionSnapshotInterval;
                }
            }
        }

        /// <summary>
        /// Gets the connection filters used by obj server instance.
        /// </summary>
        /// <value>
        /// The connection filters's name list, seperated by comma
        /// </value>
        public string ConnectionFilter
        {
            get
            {
                if (obj != null && obj["connectionFilter"] != null)
                {
                    return obj["connectionFilter"].ToString();
                }
                else
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// Gets the command loader, multiple values should be separated by comma.
        /// </summary>
        public string CommandLoader
        {
            get
            {
                if (obj != null && obj["commandLoader"] != null)
                {
                    return obj["commandLoader"].ToString();
                }
                else
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// Gets the start keep alive time, in seconds
        /// </summary>
        public int KeepAliveTime
        {
            get
            {
                if (obj != null && obj["keepAliveTime"] != null)
                {
                    return int.Parse(obj["keepAliveTime"].ToString());
                }
                else
                {
                    return ServerConfig.DefaultKeepAliveTime;
                }
            }
        }

        /// <summary>
        /// Gets the keep alive interval, in seconds.
        /// </summary>
        public int KeepAliveInterval
        {
            get
            {
                if (obj != null && obj["keepAliveInterval"] != null)
                {
                    return int.Parse(obj["keepAliveInterval"].ToString());
                }
                else
                {
                    return ServerConfig.DefaultKeepAliveInterval;
                }
            }
        }

        /// <summary>
        /// Gets the backlog size of socket listening.
        /// </summary>
        public int ListenBacklog
        {
            get
            {
                if (obj != null && obj["listenBacklog"] != null)
                {
                    return int.Parse(obj["listenBacklog"].ToString());
                }
                else
                {
                    return ServerConfig.DefaultListenBacklog;
                }
            }
        }

        /// <summary>
        /// Gets the startup order of the server instance.
        /// </summary>
        public int StartupOrder
        {
            get
            {
                if (obj != null && obj["startupOrder"] != null)
                {
                    return int.Parse(obj["startupOrder"].ToString());
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Gets/sets the size of the sending queue.
        /// </summary>
        /// <value>
        /// The size of the sending queue.
        /// </value>
        public int SendingQueueSize
        {
            get
            {
                if (obj != null && obj["sendingQueueSize"] != null)
                {
                    return int.Parse(obj["sendingQueueSize"].ToString());
                }
                else
                {
                    return ServerConfig.DefaultSendingQueueSize;
                }
            }
        }

        /// <summary>
        /// Gets the logfactory name of the server instance.
        /// </summary>
        public string LogFactory
        {
            get
            {
                if (obj != null && obj["logFactory"] != null)
                {
                    return obj["logFactory"].ToString();
                }
                else
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// Gets the default text encoding.
        /// </summary>
        /// <value>
        /// The text encoding.
        /// </value>
        public string TextEncoding
        {
            get
            {
                if (obj != null && obj["textEncoding"] != null)
                {
                    return obj["textEncoding"].ToString();
                }
                else
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// Gets the listeners' configuration.
        /// </summary>
        public List<Listener> Listeners
        {
            get
            {
                if (obj != null && obj["listeners"] != null)
                {
                    return JsonConvert.DeserializeObject<List<Listener>>(obj["listeners"].ToString());
                }
                else
                {
                    return new List<Listener>();
                }
            }
        }

        /// <summary>
        /// Gets the listeners' configuration.
        /// </summary>
        IEnumerable<IListenerConfig> IServerConfig.Listeners
        {
            get
            {
                return this.Listeners;
            }
        }


        /// <summary>
        /// Gets the child config.
        /// </summary>
        /// <typeparam name="TConfig">The type of the config.</typeparam>
        /// <param name="childConfigName">Name of the child config.</param>
        /// <returns></returns>
        public JToken GetChildConfig(string childConfigName)
        {
            return obj[childConfigName];
        }


    }
}
