using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace SignalRDemo
{
    [HubName("rateHub")]
    public class RateHub : Hub
    {
        //don't do this in real project
        private static int _rating;

        public void Rate()
        {
            _rating += 1;
          //  Clients.All.rateUpdate(_rating);
            Clients.Client(Context.ConnectionId).rateUpdate(_rating); ;
        }
        public void sever_recice(string rr,int c)
        {

            Clients.Client(Context.ConnectionId).sever_send(rr+c); 
        }
        /// <summary>
        /// The OnConnected event.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public override System.Threading.Tasks.Task OnConnected()
        {
            string clientId = Context.ConnectionId;
            return base.OnConnected();
        }

        public override System.Threading.Tasks.Task OnDisconnected(bool iscall)
        {
            string clientId = Context.ConnectionId;
            return base.OnDisconnected(iscall);
        }
        /// <summary>
        /// The OnReconnected event.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public override System.Threading.Tasks.Task OnReconnected()
        {
            string clientId = Context.ConnectionId;
            return base.OnReconnected();
        }
    }
}