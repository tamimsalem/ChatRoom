using Autofac;
using ChatRoom.Data;
using ChatRoom.Entities;
using Microsoft.AspNet.SignalR;
using System;
using System.Threading.Tasks;

namespace ChatRoom.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IChatMessagePersister _messagePersister;
        private readonly IUserPersister _userPersister;

        public ChatHub(IChatMessagePersister messagePersister, IUserPersister userPersister)
        {
            _messagePersister = messagePersister;
            _userPersister = userPersister;
        }

        public void Send(string name, string message)
        {
            try
            {
                //Save first
                var chatMsg = new ChatMessage()
                {
                    Name = name,
                    Message = message
                };

                _messagePersister.Add(chatMsg);

                //then broadcast
                Clients.All.broadcastMessage(name, message);
            }
            catch(Exception) //All exceptions
            {
                Clients.Caller.showError("Generic error occured");
            }
        }

        public override Task OnConnected()
        {
            var id = Context.QueryString["id"];
            var name = Context.QueryString["name"];

            var allUsers = _userPersister.GetAll();

            var user = new User()
            {
                Id = id,
                Name = name
            };

            _userPersister.Add(user);

            Clients.Caller.load(allUsers);

            Clients.Others.joined(user);
            
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var id = Context.QueryString["id"];

            var user = new User()
            {
                Id = id
            };

            _userPersister.Delete(user);

            Clients.All.left(user);

            return base.OnDisconnected(stopCalled);
        }
    }
}