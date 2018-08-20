using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Api
{
        public static class UserHandler //this static class is to store the number of users conected at the same time
        {
            public static HashSet<string> ConnectedIds = new HashSet<string>();
        }

   

        //[Authorize]
        [HubName("messenger")]
        public class MessageHub : Hub
        {
            public void sendMessage(int id, string text) 
            {

                Clients.Others.newMessage(id, text); 

            }

            public override Task OnConnected() 
            {
                UserHandler.ConnectedIds.Add(Context.ConnectionId);
            Clients.Caller(1);
            //    Clients.User.usersConnected(UserHandler.ConnectedIds.Count()); 
                return base.OnConnected();
            }

            public override Task OnReconnected()
            {
                UserHandler.ConnectedIds.Add(Context.ConnectionId);
                Clients.All.usersConnected(UserHandler.ConnectedIds.Count());
                return base.OnConnected();
            }

            //public override Task OnDisconnected()
            //{
            //    UserHandler.ConnectedIds.Remove(Context.ConnectionId);
            //    Clients.All.usersConnected(UserHandler.ConnectedIds.Count());
            //    return base.OnDisconnected();
            //}



        }
    }
//}