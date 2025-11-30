using Estra7a.DataAccess.Repositories.IRepository;
using Estra7a.DataAccess.Repositories.Repository;
using Estra7a.Models.Models;
using Estra7a.Services.DTO;
using Estra7a.Web.Hubs;
using Estra7a.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Estra7a.Web.Areas.Guest.Controllers
{
    [Area("Guest")]
    public class RoomController : Controller
    {
        private readonly IRoomService _roomService;
        private readonly string _rootPath;
        private readonly IHubContext<RoomHub> _hubContext;
        private readonly IUnitOfWork _unitofwork;

        public RoomController(IRoomService roomService,
                              IWebHostEnvironment webHostEnvironment,
                              IHubContext<RoomHub> hubContext,
                              IUnitOfWork unitofwork)
        {
            _roomService = roomService;
            _rootPath = webHostEnvironment.WebRootPath;
            _hubContext = hubContext;
            _unitofwork = unitofwork;
        }


        [HttpGet]
        public IActionResult Index()
        {
            var rooms = _roomService.GetRooms();

            var viewModels = rooms.Select(r => new RoomViewModel
            {
                Id = r.Id,
                Name = r.Name,
                PricePerNight = r.PricePerNight,
                Description = r.Description,
                Area = r.Area,
                Capacity = r.Capacity,
                BaseImageUrl = r.BaseImageUrl,
                NumberOfRooms = r.NumberOfRooms,
                NumberOfAvailableRooms = r.NumberOfAvailableRooms,
                RoomTypeName = r.RoomTypeName
            }).ToList();

            return View(viewModels);
        }


        [HttpGet]
        public IActionResult Create()
        {
            AddRoomViewModel viewModel = new()
            {
                RoomTypes = _roomService.GetRoomTypesSelectList(),
                RoomFeatures = _roomService.GetRoomFeaturesSelectList()

            };
            return View(viewModel);
        }






        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddRoomViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.RoomTypes = _roomService.GetRoomTypesSelectList();
                viewModel.RoomFeatures = _roomService.GetRoomFeaturesSelectList();
                return View(viewModel);
            }

            var dto = new AddRoomDto
            {
                Name = viewModel.Name,
                PricePerNight = viewModel.PricePerNight,
                Description = viewModel.Description,
                RoomtypeId = viewModel.RoomtypeId,
                Area = viewModel.Area,
                NumberOfRooms = viewModel.NumberOfRooms,
                Capacity = viewModel.Capacity,
                BaseImage = viewModel.BaseImage,
                RoomImages = viewModel.RoomImages,
                SelectedFeaturesId = viewModel.RoomFeaturesIds,
            };

            await _roomService.CreateRoomAsync(dto, _rootPath);
            var newRoom = _roomService.GetRooms()
            .OrderByDescending(r => r.Id)
            .FirstOrDefault(r => r.Name == dto.Name && r.PricePerNight == dto.PricePerNight);

            // Broadcast via SignalR if room was found
            if (newRoom != null)
            {
                await _hubContext.Clients.All.SendAsync("ReceiveRoomCreated", newRoom);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int roomId)
        {
           // Console.WriteLine("he");
            _roomService.DeleteRoom(roomId , _rootPath);
            // Add SignalR broadcast
            await _hubContext.Clients.All.SendAsync("ReceiveRoomDeleted", roomId);
            return Json(new { success = true });
        }

      

        [HttpGet]
        public IActionResult Details(int id)
        {
            var roomDto = _roomService.GetRoomDetails(id);

            if (roomDto == null)
                return NotFound();

            RoomViewModel viewModel = new()
            {
                Id = roomDto.Id,
                Name = roomDto.Name,
                PricePerNight = roomDto.PricePerNight,
                Description = roomDto.Description,
                Area = roomDto.Area,
                Rate = roomDto.Rate,
                Capacity = roomDto.Capacity,
                BaseImageUrl = roomDto.BaseImageUrl,
                NumberOfRooms = roomDto.NumberOfRooms,
                RoomTypeName = roomDto.RoomTypeName,
                RoomTypeDescription = roomDto.RoomTypeDescription,
                NumberOfAvailableRooms = roomDto.NumberOfAvailableRooms,
                RoomImages = roomDto.RoomImages,
                RoomFeatures = roomDto.RoomFeatures,
            };

            viewModel.RelatedRooms = _roomService.GetRelatedRooms(id, roomDto.RoomTypeId)
            .Select(room => new RoomViewModel
            {
                Id = room.Id,
                Name = room.Name,
                PricePerNight = room.PricePerNight,
                Description = room.Description,
                Area = room.Area,
                Capacity = room.Capacity,
                BaseImageUrl = room.BaseImageUrl,
                NumberOfRooms = room.NumberOfRooms,
                NumberOfAvailableRooms = room.NumberOfAvailableRooms,
                Rate = room.Rate,
                RoomTypeName = room.RoomTypeName,
                RoomTypeDescription = room.RoomTypeDescription,
                RoomImages = room.RoomImages,
                RoomFeatures = room.RoomFeatures
            }).ToList();

            return View(viewModel);
        }






     
      [HttpGet]
      public IActionResult Edit(int id)
      {
                var dto = _roomService.GetRoomForEdit(id);

                if (dto == null)
                    return NotFound();


                var vm = new EditRoomViewModel
                {
                    Id = dto.Id,
                    Name = dto.Name,
                    Description = dto.Description,
                    Area = dto.Area,
                    NumberOfRooms = dto.NumberOfRooms,
                    Capacity = dto.Capacity,
                    PricePerNight = dto.PricePerNight,
                    RoomtypeId = dto.RoomtypeId,
                    RoomFeaturesIds = dto.SelectedFeatureIds,
                    CurrentBaseImageName = dto.CurrentCoverImageName ,
                    CurrentAdditionalImages = dto.CurrentAdditionalImages
                };
                vm.RoomTypes = _roomService.GetRoomTypesSelectList();
                vm.RoomFeatures = _roomService.GetRoomFeaturesSelectList(dto.CurrentFeatures);

                return View(vm);
      }

        


        [HttpPost]
        public async Task<IActionResult> Edit(EditRoomViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.RoomTypes = _roomService.GetRoomTypesSelectList();
                vm.RoomFeatures = _roomService.GetRoomFeaturesSelectList();
                var roomImages = _roomService.GetRoomImages(vm.Id);
                vm.CurrentAdditionalImages = roomImages.Item2;
                vm.CurrentBaseImageName = roomImages.Item1;
                return View(vm);
            }

            string? newCoverName = null;
            if (vm.NewCoverImage != null)
                newCoverName = await _roomService.SaveFileAsync(vm.NewCoverImage, _rootPath);

            List<string> newAdditionalImages = new();
            if (vm.NewAdditionalImages != null)
            {
                foreach (var file in vm.NewAdditionalImages)
                    newAdditionalImages.Add(await _roomService.SaveFileAsync(file, _rootPath));
            }

            var dto = new EditRoomDto
            {
                Id = vm.Id,
                Name = vm.Name,
                Description = vm.Description,
                Area = vm.Area,
                NumberOfRooms = vm.NumberOfRooms,
                Capacity = vm.Capacity,
                PricePerNight = vm.PricePerNight,
                RoomtypeId = vm.RoomtypeId,
                SelectedFeatureIds = vm.RoomFeaturesIds,
                CurrentCoverImageName = vm.CurrentBaseImageName,
                DeleteCover = vm.DeleteCover,
                NewCoverImageName = newCoverName,
                CurrentAdditionalImages = vm.CurrentAdditionalImages
                    .Select(x => new ExistingImageDto { Id = x.Id, FileName = x.FileName })
                    .ToList(),
                AdditionalImagesToDelete = vm.AdditionalImagesToDelete,
                NewAdditionalImages = newAdditionalImages
            };


            _roomService.EditRoom(dto);

            return RedirectToAction(nameof(Index));
        }



        [HttpGet]
        public IActionResult Filter(string priceRange, string capacityRange, string roomType)
        {
            var rooms = _roomService.FilterRooms(priceRange, capacityRange, roomType).ToList();

            var viewModels = rooms.Select(r => new RoomViewModel
            {
                Id = r.Id,
                Name = r.Name,
                PricePerNight = r.PricePerNight,
                Description = r.Description,
                Area = r.Area,
                Capacity = r.Capacity,
                BaseImageUrl = r.BaseImageUrl,
                NumberOfRooms = r.NumberOfRooms,
                NumberOfAvailableRooms = r.NumberOfAvailableRooms,
                RoomTypeName = r.RoomTypeName
            });

            return PartialView("_RoomCardList", viewModels);
        }



        public IActionResult GetAvailable(DateTime checkIn, DateTime checkOut, string Capacity)
        {
            var rooms = _unitofwork.Room.GetAll(IncludeProp: "RoomType").ToList();
            if (rooms.Count > 0)
            {
                foreach (var room in rooms.ToList())
                {
                    var bookings = _unitofwork.Booking.GetAll(b => b.RoomId == room.RoomId).ToList();
                    int reservedCount = bookings.Count(b => checkIn < b.CheckOutDate && checkOut > b.CheckInDate);

                    bool isAvailable = reservedCount < room.NumberOfAvailableRooms;

                    if (!isAvailable || room.Capacity <= int.Parse(Capacity))
                    {
                        rooms.Remove(room);
                    }
                }
                var viewModels = rooms.Select(r => new RoomViewModel
                {
                    Id = r.RoomId,
                    Name = r.Name,
                    PricePerNight = r.PricePerNight,
                    Description = r.Description,
                    Area = r.Area,
                    Capacity = r.Capacity,
                    BaseImageUrl = r.BaseImageUrl,
                    NumberOfRooms = r.NumberOfRooms,
                    NumberOfAvailableRooms = r.NumberOfAvailableRooms,
                    RoomTypeName = r.RoomType.Name
                }).ToList();

                return View("Index", viewModels);

            }


            return RedirectToAction("Index", "Home");

        }


    }
}
