using Microsoft.AspNetCore.SignalR;
using Tensorflow.Contexts;

namespace Estra7a.Web.Models
{
    public class ChatHub : Hub
    {
        private static readonly Dictionary<string, string> userConnections = new();

        public override async Task OnConnectedAsync()
        {
            var userName = Context.User?.Identity?.Name ?? Context.ConnectionId;


            Console.WriteLine($"[Connected] {userName}");

            userConnections[userName] = Context.ConnectionId;

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var user = userConnections.FirstOrDefault(x => x.Value == Context.ConnectionId);
            if (!string.IsNullOrEmpty(user.Key))
            {
                userConnections.Remove(user.Key);
                Console.WriteLine($"[Disconnected] {user.Key}");
            }

            await base.OnDisconnectedAsync(exception);
        }


        public async Task SendMessageToAdmin(string user, string message)
        {

            var adminName = userConnections.Keys
                                .FirstOrDefault(k => k.ToLower().Contains("admin"));

            Console.WriteLine($"[SendMessageToAdmin] From {user} To {adminName}");

            if (adminName != null && userConnections.TryGetValue(adminName, out var adminConnectionId))
            {
                await Clients.Client(adminConnectionId).SendAsync("ReceiveMessage", user, message);
            }
            else
            {
                Console.WriteLine("[SendMessageToAdmin] Admin not connected!");
            }


            await Clients.Caller.SendAsync("ReceiveMessage", user, message);
        }


        public async Task SendMessageToUser(string targetUser, string message)
        {
            Console.WriteLine($"[SendMessageToUser] To {targetUser}");

            if (userConnections.TryGetValue(targetUser, out var userConnectionId))
            {
                await Clients.Client(userConnectionId).SendAsync("ReceiveMessage", "Admin", message);
            }
            else
            {
                await Clients.Caller.SendAsync("ReceiveMessage", "System", $"User {targetUser} is not connected.");
            }
        }
    }
}
