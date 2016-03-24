using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Emaily.Web.Hubs
{
    public class SendMessageViewModel<T>
    {
        public T Message { get; set; }
        public long User { get; set; }
        public string Sender { get; set; }
        public DateTime At { get; set; }

        public SendMessageViewModel()
        {
            this.At = DateTime.UtcNow;
        } 
    }

    public class RoomMessage
    {
        public string Room { get; set; }
        public string Message { get; set; }
    }

    public class NotificationHub : Hub
    {
        readonly ILifetimeScope _hubLifetimeScope;

        public NotificationHub(ILifetimeScope lifetimeScope)
        {
            _hubLifetimeScope = lifetimeScope.BeginLifetimeScope();
        }
             
        public async Task Connect(string room)
        {
            await this.Groups.Add(this.Context.ConnectionId, room);
            Talk(new SendMessageViewModel<RoomMessage> { Sender="System", Message = new RoomMessage() { Room = room, Message = string.Format("{0} joined.", this.Context.User.Identity.Name) } });
        }

        internal void Talk<T>(SendMessageViewModel<T> msg)
        {
            msg.At = DateTime.UtcNow;
            msg.Sender = this.Context.User.Identity.Name;
            this.Clients.AllExcept(this.Context.ConnectionId).Any(msg);
        }

        public void Talk(SendMessageViewModel<RoomMessage> msg)
        {
            msg.At = DateTime.UtcNow;
            msg.Sender = this.Context.User.Identity.Name;
            if (string.IsNullOrWhiteSpace(msg.Message.Room))
            {
                this.Clients.AllExcept(this.Context.ConnectionId).Broadcast(msg);
            }
            else {
                this.Clients.Group(msg.Message.Room).Any(msg);
            }
        }

        protected override void Dispose(bool disposing)
        {
            // Dipose the hub lifetime scope when the hub is disposed.
            if (disposing && _hubLifetimeScope != null)
                _hubLifetimeScope.Dispose();

            base.Dispose(disposing);
        }
    }
}
