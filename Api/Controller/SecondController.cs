using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using SignalR.MoveShape;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Api.Controller
{
    public class ApiWithHubController<THub> : ApiController
        where THub : IHub
    {
        Lazy<IHubContext> hub = new Lazy<IHubContext>(
            () => GlobalHost.ConnectionManager.GetHubContext<THub>()
        );

        protected IHubContext Hub
        {
            get { return hub.Value; }
        }
    }

    public class MoveController : ApiWithHubController<MoveShapeHub>
    {
        public IEnumerable<string> Get(int x, int y)
        {
            //(Hub as MoveShapeHub).moveShape(x, y);

            Hub.Clients.All.shapeMoved(x, y);

            return new string[] { string.Format("moved to {0}, {0}",x,y)  };
        }

    }
}
