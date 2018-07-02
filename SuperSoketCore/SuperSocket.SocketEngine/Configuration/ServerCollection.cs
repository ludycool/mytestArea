using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using Newtonsoft.Json.Linq;
using SuperSocket.Common;
using SuperSocket.SocketBase.Config;

namespace SuperSocket.SocketEngine.Configuration
{
    /// <summary>
    /// Server configuration collection
    /// </summary>
  //  [ConfigurationCollection(typeof(Server), AddItemName = "server")] 
    public class ServerCollection
    {
        public List<Server> list
        {
            set;
            get;
        }


        public ServerCollection(JArray _arry)
        {

            list = new List<Server>();
            foreach (var jt in _arry)  //查找某个字段与值
            {
                Server s = new Server(jt);
                list.Add(s);
            }

        }

        /// <summary>
        /// Adds the new server element.
        /// </summary>
        /// <param name="newServer">The new server.</param>
        public void AddNew(Server newServer)
        {
            list.Add(newServer);
        }

        /// <summary>
        /// Removes the specified server from the configuration.
        /// </summary>
        /// <param name="name">The name.</param>
        public void Remove(string name)
        {

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Name.Equals(name))
                {
                    list.RemoveAt(i);
                    break;
                }
            }
        }
    }
}
