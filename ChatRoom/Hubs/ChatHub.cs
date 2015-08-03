using Autofac;
using ChatRoom.Data;
using ChatRoom.Entities;
using Microsoft.AspNet.SignalR;
using System;

namespace ChatRoom.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IChatMessagePersister _persister;

        public ChatHub(IChatMessagePersister persister)
        {
            _persister = persister;
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

                _persister.Add(chatMsg);

                //then broadcast
                Clients.All.broadcastMessage(name, message);
            }
            catch(Exception) //All exceptions
            {
                Clients.Caller.showError("Generic error occured");
            }
        }
    }
}