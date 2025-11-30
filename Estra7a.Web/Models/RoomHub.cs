using Microsoft.AspNetCore.SignalR;

namespace Estra7a.Web.Hubs
{
    public class RoomHub : Hub
    {
        public async Task RoomCreated(object room)
        {
            await Clients.All.SendAsync("ReceiveRoomCreated", room);
        }

        public async Task RoomDeleted(int roomId)
        {
            await Clients.All.SendAsync("ReceiveRoomDeleted", roomId);
        }

        public async Task RoomUpdated(object room)
        {
            await Clients.All.SendAsync("ReceiveRoomUpdated", room);
        }
    }
}
