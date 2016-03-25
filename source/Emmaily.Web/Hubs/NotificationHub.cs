using System;
using System.Threading.Tasks;
using Autofac;
using Emaily.Core.Enumerations;
using Emaily.Services;
using Microsoft.AspNet.SignalR;

namespace Emaily.Web.Hubs
{
    public class SendMessageViewModel<T>
    {
        public T Message { get; set; }
        public string UserId { get; set; }
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

    public class NotificationHubNotifier : INotificationHub
    {
        public void Notify(string userId, NotificationTypeEnum mode, dynamic message)
        {
            GlobalHost.ConnectionManager.GetHubContext<NotificationHub>().Clients.Group(userId).Any(mode.ToString(),message);
        }
    }

    public class NotificationMessage
    {
        public string Sender { get; set; }
        public string Message { get; set; }
        public string Room { get; set; }
        public string Command { get; set; }
        public DateTime At { get; set; }
    }

    public class NotificationHub : Hub
    {
        private readonly string chat = NotificationTypeEnum.Chat.ToString();
        public override Task OnConnected()
        {
            var userId = Context.QueryString["userid"];
            Groups.Add(Context.ConnectionId, "lobby");
            if (!string.IsNullOrWhiteSpace(userId))
            {
                Groups.Add(Context.ConnectionId, userId);
                Say(string.Format("{0} connected", this.Context.User.Identity.Name), userId);
            }
            return base.OnConnected();
        }

        public async Task Connect(string room)
        {
            await this.Groups.Add(this.Context.ConnectionId, room);
        }

        private void Say(string msg, string room = "")
        {
            this.Clients.Group(room).Any(chat, new SendMessageViewModel <string>
            {
                Sender = "System",
                Message = msg,
                UserId = room
            });
        }

        public void Talk(SendMessageViewModel<RoomMessage> msg)
        {
            msg.At = DateTime.UtcNow;
            msg.Sender = this.Context.User.Identity.Name;
            if (string.IsNullOrWhiteSpace(msg.Message.Room))
            {
                this.Clients.AllExcept(this.Context.ConnectionId).Any(chat, msg);
            }
            else {
                this.Clients.Group(msg.Message.Room).Any(chat, msg);
            }
        }
    }
}
