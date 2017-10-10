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
            Clients.All.rateUpdate(_rating);
        }
    }
}