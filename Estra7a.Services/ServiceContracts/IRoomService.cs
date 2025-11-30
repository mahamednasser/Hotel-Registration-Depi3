
using Estra7a.Services.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace Estra7a.Services.ServiceContracts
{
    public interface IRoomService
    {
        IEnumerable<RoomDto> GetRooms();
        EditRoomDto? GetRoomForEdit(int roomId);
        Task<string> SaveFileAsync(IFormFile file, string rootPath);
        SelectList GetRoomTypesSelectList(int? selectedId = null);
        public MultiSelectList GetRoomFeaturesSelectList(List<int>? selectedIds = null);
        Task CreateRoomAsync(AddRoomDto dto, string rootPath);

        void DeleteRoom(int roomId, string rootPath);

        RoomDto GetRoomDetails(int roomId);
        bool EditRoom(EditRoomDto dto);
        RoomDto GetRoomById(int roomId);
        public (string, List<ExistingImageDto>) GetRoomImages(int roomId);
        List<RoomDto> GetRelatedRooms(int roomId, int roomTypeId);
         IEnumerable<RoomDto> FilterRooms(string priceRange, string capacityRange, string roomType);
    }
}
