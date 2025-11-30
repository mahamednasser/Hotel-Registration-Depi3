using System.Linq.Expressions;
using System.Net;
using Estra7a.DataAccess.Repositories.IRepository;
using Estra7a.Models.Models;
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
            });
        }



        public SelectList GetRoomTypesSelectList(int? selectedId = null)
        {
            var roomTypes = _unitOfWork.RoomType.GetAll();
            return new SelectList(roomTypes, "RoomTypeId", "Name" , selectedId);
            // note : why selectedId becaue in edit view show the selected option
        }

        public MultiSelectList GetRoomFeaturesSelectList(List<int>? selectedIds = null)
        {
            var features = _unitOfWork.RoomFeature.GetAll().ToList();
          
            return new MultiSelectList(features, "Id", "Name", selectedIds);
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

            var selectedFeatures =  _unitOfWork.RoomFeature.GetAll(f => dto.SelectedFeaturesId.Contains(f.Id));
            room.RoomFeatures = selectedFeatures.ToList();

            foreach (var file in dto.RoomImages)
            {
                string fileName = await SaveFileAsync(file, rootPath); 
                room.RoomImages.Add(new RoomImage { ImageUrl = fileName });
            }
            _unitOfWork.Room.Add(room);
            _unitOfWork.save();
        }
     

        public async Task<string> SaveFileAsync(IFormFile file ,string rootPath)
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
        public void DeleteAllRoomImages(Room room, string rootPath)
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
                IncludeProp: "RoomType,RoomImages,RoomFeatures" ,
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
                RoomTypeId = room.RoomType.RoomTypeId,
                BaseImageUrl = room.BaseImageUrl,
                NumberOfRooms = room.NumberOfRooms,
                NumberOfAvailableRooms = room.NumberOfAvailableRooms,
                RoomTypeName = room.RoomType.Name,
                RoomTypeDescription = room.RoomType.Description,
                RoomFeatures = room.RoomFeatures
                                    .Select(feature => new RoomFeatureDto
                                    {
                                        Name = feature.Name,
                                        IconPath = feature.IconPath
                                    }).ToList(),

                RoomImages = room.RoomImages
                                .Select(img => img.ImageUrl)
                                .ToList()
            };

            return roomDto;
        }
        public List<RoomDto> GetRelatedRooms(int roomId , int roomTypeId)
        {
            return _unitOfWork.Room
             .GetAll(
                 room => room.RoomId != roomId && room.RoomTypeId == roomTypeId,
                 IncludeProp: "RoomImages,RoomType,RoomFeatures"
             ).Select(room => new RoomDto
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
                RoomFeatures = room.RoomFeatures.Select(feature => new RoomFeatureDto
                    {
                        Name = feature.Name,
                        IconPath = feature.IconPath
                    }).ToList(),
                RoomImages = room.RoomImages
                    .Select(img => img.ImageUrl)
                    .ToList()
             }).ToList();


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
                                .ToList()
            };

            return roomDto;

        }
        public EditRoomDto? GetRoomForEdit(int roomId)
        {
            var room = _unitOfWork.Room.GetById(
                            r => r.RoomId == roomId,
                            IncludeProp: "RoomType,RoomImages,RoomFeatures",
                            tracked: false
                        );

            if (room == null)
                return null;

            return new EditRoomDto
            {
                Id = room.RoomId,
                Name = room.Name,
                PricePerNight = room.PricePerNight,
                Area = room.Area,
                Capacity = room.Capacity,
                RoomtypeId = room.RoomTypeId,
                NumberOfRooms = room.NumberOfRooms,
                Description = room.Description,
                CurrentCoverImageName = room.BaseImageUrl,
                CurrentFeatures = room.RoomFeatures.Select(feature => feature.Id).ToList(),
                CurrentAdditionalImages = room.RoomImages.Select(img => new ExistingImageDto
                {
                    Id = img.RoomImageId,
                    FileName = img.ImageUrl
                }).ToList()
            };
        }
        public (string ,List<ExistingImageDto>) GetRoomImages(int roomId)
        {
            var roomImages = _unitOfWork.RoomImages.GetAll(roomImage => roomImage.RoomId == roomId)
                 .Select(roomImage => new ExistingImageDto()
                 {
                     Id = roomImage.RoomImageId,
                     FileName = roomImage.ImageUrl
                 }).ToList();
            string currentImage = _unitOfWork.Room.GetById(room => room.RoomId == roomId).BaseImageUrl;
            return (currentImage, roomImages);
        }

        public bool EditRoom(EditRoomDto dto)
        {
            var room = _unitOfWork.Room.GetById(
                r => r.RoomId == dto.Id,
                "RoomImages,RoomFeatures",
                true
            );

            if (room == null)
                throw new Exception("Room not found");

            // =============================
            // Update basic info
            // =============================
            room.Name = dto.Name;
            room.Description = dto.Description;
            room.Area = dto.Area;
            room.NumberOfRooms = dto.NumberOfRooms;
            room.Capacity = dto.Capacity;
            room.PricePerNight = dto.PricePerNight;
            room.RoomTypeId = dto.RoomtypeId;

            // =============================
            // Update Room Features (Many-to-Many)
            // =============================
            if (dto.SelectedFeatureIds != null)
            {
                // load the real feature entities (not fake ones)
                var newFeatures = _unitOfWork.RoomFeature
                    .GetAll()
                    .Where(f => dto.SelectedFeatureIds.Contains(f.Id))
                    .ToList();

                // replace the whole collection
                room.RoomFeatures = newFeatures;
            }

            // =============================
            // Handle Cover Image
            // =============================
            if (!string.IsNullOrEmpty(dto.NewCoverImageName))
            {
                if (!string.IsNullOrEmpty(room.BaseImageUrl))
                {
                    string oldPath = Path.Combine("wwwroot", "RoomImages", room.BaseImageUrl);
                    if (File.Exists(oldPath))
                        File.Delete(oldPath);
                }

                room.BaseImageUrl = dto.NewCoverImageName;
            }

            // =============================
            // Delete Additional Images
            // =============================
            if (dto.AdditionalImagesToDelete != null)
            {
                foreach (var imgId in dto.AdditionalImagesToDelete)
                {
                    var image = room.RoomImages.FirstOrDefault(x => x.RoomImageId == imgId);
                    if (image != null)
                    {
                        string fullPath = Path.Combine("wwwroot", "RoomImages", image.ImageUrl);
                        if (File.Exists(fullPath))
                            File.Delete(fullPath);

                        room.RoomImages.Remove(image);
                    }
                }
            }

            // =============================
            // Add New Additional Images
            // =============================
            if (dto.NewAdditionalImages != null)
            {
                foreach (var fileName in dto.NewAdditionalImages)
                {
                    room.RoomImages.Add(new RoomImage
                    {
                        ImageUrl = fileName
                    });
                }
            }

            _unitOfWork.Room.Update(room);
            _unitOfWork.save();
            return true;
        }


        public IEnumerable<RoomDto> FilterRooms(string priceRange, string capacityRange, string roomType)
        {

            var rooms = _unitOfWork.Room.GetAll(IncludeProp: "RoomType").AsQueryable();

            // ---- Price Filter ----
            if (!string.IsNullOrWhiteSpace(priceRange))
            {
                if (priceRange.Contains("-"))
                {
                    var parts = priceRange.Split('-');
                    if (parts.Length == 2
                        && int.TryParse(parts[0].Trim(), out int min)
                        && int.TryParse(parts[1].Trim(), out int max))
                    {
                        rooms = rooms.Where(r => r.PricePerNight >= min && r.PricePerNight <= max);
                    }
                }
                else if (priceRange.Contains("+"))
                {
                    if (int.TryParse(priceRange.Replace("+", "").Trim(), out int min))
                    {
                        rooms = rooms.Where(r => r.PricePerNight >= min);
                    }
                }
            }

            // ---- Capacity Filter ----
            if (!string.IsNullOrWhiteSpace(capacityRange))
            {
                if (capacityRange.Contains("-"))
                {
                    var parts = capacityRange.Split('-');
                    if (parts.Length == 2
                        && int.TryParse(parts[0].Trim(), out int min)
                        && int.TryParse(parts[1].Trim(), out int max))
                    {
                        rooms = rooms.Where(r => r.Capacity >= min && r.Capacity <= max);
                      
                    }
                }
                else if (capacityRange.Contains("+"))
                {
                    if (int.TryParse(capacityRange.Replace("+", "").Trim(), out int min))
                    {
                        rooms = rooms.Where(r => r.Capacity >= min);
                    }
                }
            }

            // ---- Room Type Filter ----
            if (!string.IsNullOrWhiteSpace(roomType))
            {
                rooms = rooms.Where(r => r.RoomType != null && r.RoomType.Name == roomType);
            }

            // Materialize the query to a list
            var roomList = rooms.Select(r => new RoomDto
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
                RoomTypeName = r.RoomType != null ? r.RoomType.Name : ""
            }).ToList();

            return roomList;

        }

    }
}
