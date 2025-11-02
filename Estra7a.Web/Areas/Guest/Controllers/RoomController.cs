using System.Threading.Tasks;
using Estra7a.DataAccess.Repositories.IRepository;
using Estra7a.DataAccess.Repositories.Repository;
using Estra7a.Models.Models;
using Estra7a.Services.DTO;
using Estra7a.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Estra7a.Web.Areas.Guest.Controllers
{
    [Area("Guest")]
    public class RoomController : Controller
    {
        private readonly IRoomService _roomService;
        private readonly string _rootPath;
        public RoomController(IRoomService roomService, IWebHostEnvironment webHostEnvironment)
        {
            _roomService = roomService;
            _rootPath = webHostEnvironment.WebRootPath;
           
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
                RoomTypes = _roomService.GetRoomTypesSelectList()
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
                NumofAvaliableRooms = viewModel.NumberOfRooms
            };

            await _roomService.CreateRoomAsync(dto, _rootPath);
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        public IActionResult DeleteConfirmed(int roomId)
        {
           // Console.WriteLine("he");
            _roomService.DeleteRoom(roomId , _rootPath);
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
                RoomImages = roomDto.RoomImages,
                NumberOfAvailableRooms=roomDto.NumberOfAvailableRooms
            };
            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var room =  _roomService.GetRoomById(id);
            if (room is null)
                return NotFound();
            EditRoomViewModel viewModel = new()
            {
                Id = room.Id,
                Name = room.Name,
                Description = room.Description,
                NumberOfRooms = room.NumberOfRooms,
                PricePerNight = room.PricePerNight,
                Capacity = room.Capacity,
                Area = room.Area,
                CurrentCover = room.BaseImageUrl,
                CurrentAdditionalImages = room.RoomImages,
                RoomTypes = _roomService.GetRoomTypesSelectList(room.RoomTypeId),
                RoomtypeId = room.RoomTypeId
            };
            return View(viewModel);

        }

    }
}
