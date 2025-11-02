using System.Net;
using Estra7a.Services.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Estra7a.Services.Services
{
    public class RoomService : IRoomService
    {
        private readonly IUnitOfWork _unitOfWork;
        public RoomService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public IEnumerable<RoomDto> GetRooms()
        {
            var rooms = _unitOfWork.Room.GetAll(
                Filter: null,
                IncludeProp: "RoomType"
            ).ToList();

            return rooms.Select(r => new RoomDto
            {
                Id = r.RoomId,
                Name = r.Name,
                PricePerNight = r.PricePerNight,
                Description = r.Description,
                Area = r.Area,
                Rate = r.RoomRate,
                Capacity = r.Capacity,
                BaseImageUrl = r.BaseImageUrl,
                NumberOfRooms = r.NumberOfRooms,
                RoomTypeName = r.RoomType.Name
                ,NumberOfAvailableRooms=r.NumberOfAvailableRooms
            });
        }



        public SelectList GetRoomTypesSelectList(int? selectedId = null)
        {
            var roomTypes = _unitOfWork.RoomType.GetAll();
            return new SelectList(roomTypes, "RoomTypeId", "Name" , selectedId);
            // note : why selectedId becaue in edit view show the selected option
        }
        public async Task CreateRoomAsync(AddRoomDto dto , string rootPath)
        {
           
            string imageName = await SaveFileAsync(dto.BaseImage, rootPath);

            Room room = new()
            {
                Name = dto.Name,
                PricePerNight = dto.PricePerNight,
                Description = dto.Description,
                RoomTypeId = dto.RoomtypeId,
                BaseImageUrl = imageName,
                Area = dto.Area,
                NumberOfRooms = dto.NumberOfRooms,
                NumberOfAvailableRooms = dto.NumberOfRooms,
                Capacity = dto.Capacity,
            };

            foreach (var file in dto.RoomImages)
            {
                string fileName = await SaveFileAsync(file, rootPath); 
                room.RoomImages.Add(new RoomImage { ImageUrl = fileName });
            }
            _unitOfWork.Room.Add(room);
            _unitOfWork.save();
        }
     

        private async Task<string> SaveFileAsync(IFormFile file ,string rootPath)
        {
            string fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            string fullPath = Path.Combine(rootPath, "RoomImages", fileName);
            // wwwroot/RoomImages/fileName

            using var stream = File.Create(fullPath);
            await file.CopyToAsync(stream);
            return fileName;
        }

      
        public void DeleteRoom(int roomId , string rootPath)
        {
                Room room = _unitOfWork.Room.GetById(
                    r => r.RoomId == roomId,
                    IncludeProp: "RoomImages",
                    tracked: true) ;
            DeleteAllRoomImages(room, rootPath);
            _unitOfWork.Room.Remove(room);
            _unitOfWork.save();

            // in future will also delete this room iamges (Done)
        }
        private void DeleteAllRoomImages(Room room, string rootPath)
        {
            if (!string.IsNullOrEmpty(room.BaseImageUrl))
            {
                string coverPath = Path.Combine(rootPath, "RoomImages", room.BaseImageUrl);
                if (File.Exists(coverPath))
                {
                    File.Delete(coverPath);
                }
            }
            if (room.RoomImages.Any())
            {
                foreach (var image in room.RoomImages)
                {
                    string imagePath = Path.Combine(rootPath, "RoomImages", image.ImageUrl);

                    if (File.Exists(imagePath))
                    {
                        File.Delete(imagePath);
                    }
                }
            }

        }
        public RoomDto? GetRoomDetails(int roomId)
        {
            var room = _unitOfWork.Room.GetById(
                r => r.RoomId == roomId,
                IncludeProp: "RoomType,RoomImages",
                tracked: false
            );

            if (room == null)
                return null;

            RoomDto roomDto = new()
            {
                Id = room.RoomId,
                Name = room.Name,
                PricePerNight = room.PricePerNight,
                Description = room.Description,
                Area = room.Area,
                Rate = room.RoomRate,
                Capacity = room.Capacity,
                BaseImageUrl = room.BaseImageUrl,
                NumberOfRooms = room.NumberOfRooms,
                RoomTypeName = room.RoomType.Name,
                RoomTypeDescription = room.RoomType.Description,
                RoomImages = room.RoomImages
                                .Select(img => img.ImageUrl)
                                .ToList(),
                NumberOfAvailableRooms= room.NumberOfAvailableRooms
            };

            return roomDto;
        }


        public RoomDto GetRoomById(int roomId)
        {
            var room =  _unitOfWork.Room.GetById(
                r => r.RoomId == roomId,
                IncludeProp: "RoomType,RoomImages",
                tracked: true
            );
            if (room == null)
                return null;

            RoomDto roomDto = new()
            {
                Id = room.RoomId,
                Name = room.Name,
                PricePerNight = room.PricePerNight,
                Description = room.Description,
                Area = room.Area,
                Rate = room.RoomRate,
                Capacity = room.Capacity,
                BaseImageUrl = room.BaseImageUrl,
                NumberOfRooms = room.NumberOfRooms,
                RoomTypeName = room.RoomType.Name,
                RoomTypeId = room.RoomTypeId,
                RoomTypeDescription = room.RoomType.Description,
                RoomImages = room.RoomImages
                                .Select(img => img.ImageUrl)
                                .ToList(),
                NumberOfAvailableRooms=room.NumberOfAvailableRooms
            };

            return roomDto;

        }

     
    }
}
