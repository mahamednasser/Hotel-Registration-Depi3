
using Estra7a.Services.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace Estra7a.Services.ServiceContracts
{
    public interface IRoomService
    {
        IEnumerable<RoomDto> GetRooms(); 
        

        SelectList GetRoomTypesSelectList(int? selectedId = null);

         Task CreateRoomAsync(AddRoomDto dto , string rootPath);

         void DeleteRoom(int roomId , string rootPath);

        RoomDto GetRoomDetails(int roomId);

        RoomDto GetRoomById(int roomId);
    }
}
