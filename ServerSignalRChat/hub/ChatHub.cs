using Microsoft.AspNetCore.SignalR;

namespace serverSignalRChat.hub
{
    public class ChatHub : Hub
    {
        public async Task Send(int userId, string message)
        {
            await Clients.All.SendAsync("Receive", userId, message);
        }

        public async Task SendToUser(int userId, string receiverConnectionId, string message)
        {
            await Clients.Client(receiverConnectionId).SendAsync("ReceiveToUser", userId, message, receiverConnectionId);
        }

        public string GetConnectionId() => Context.ConnectionId;
    }
}
