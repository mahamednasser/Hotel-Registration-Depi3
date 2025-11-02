using Estra7a.DataAccess.Repositories.IRepository;
using Estra7a.DataAccess.Repositories.Repository;
using Estra7a.Models.Models;
using Estra7a.Services.DTO;
using Estra7a.Services.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Estra7a.Web.Areas.Guest.Controllers
{
    [Area("Guest")]
    public class BookingController : Controller
    {
        private readonly IBookingService _bookingService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        public BookingController(IBookingService bookingService, UserManager<ApplicationUser> userManager,IUnitOfWork unitOfWork)
        {
            _bookingService = bookingService;
            _userManager = userManager;
            _unitOfWork= unitOfWork;
        }
        [HttpGet]
        public async Task<IActionResult> CreateBooking(int roomid)
        {
            var room = _unitOfWork.Room.GetById(r => r.RoomId == roomid,tracked:true);

            if (room == null)
            {
                return BadRequest("Room not available");

            }
            var user = await _userManager.GetUserAsync(User);
            var roomViewModel = new RoomViewModel
            {
                Id = room.RoomId,
                Name = room.Name,
                Description = room.Description,
                PricePerNight = room.PricePerNight,
                Capacity = room.Capacity,
                Area = room.Area,
                BaseImageUrl = room.BaseImageUrl,
                NumberOfAvailableRooms = room.NumberOfAvailableRooms,
                RoomTypeName = room.RoomType?.Name ?? string.Empty,
                RoomImages = room.RoomImages.Select(ri => ri.ImageUrl).ToList(),
                Rate = room.RoomRate,
                RoomTypeDescription = room.RoomType?.Description ?? string.Empty              
            };
            
            return View("Book", roomViewModel);
        }


        [HttpPost]
        public async Task<IActionResult> Create(int roomId, DateTime checkIn, DateTime checkOut, int guests, string PhoneNumber,int room_count)
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["ErrorMessage"] = "You must be logged in to make a booking.";
                return RedirectToAction("Login", "Account", new { area = "Guest" });
            }

            var userId = _userManager.GetUserId(User);
            var room = _unitOfWork.Room.GetById(r=>r.RoomId == roomId, tracked: true);
            if ( room==null)
            {
                return BadRequest("Room not available");
                           
            }
            var roomViewModel = new RoomViewModel
            {
                Id = room.RoomId,
                Name = room.Name,
                Description = room.Description,
                PricePerNight = room.PricePerNight,
                Capacity = room.Capacity,
                Area = room.Area,
                BaseImageUrl = room.BaseImageUrl,
                NumberOfAvailableRooms = room.NumberOfAvailableRooms,
                RoomTypeName = room.RoomType?.Name ?? string.Empty,
                RoomImages = room.RoomImages.Select(ri => ri.ImageUrl).ToList(),
                Rate = room.RoomRate,
                RoomTypeDescription = room.RoomType?.Description ?? string.Empty
            };
            if(ModelState.IsValid==false)
            {
                return View("Book", roomViewModel);
            }
            if (checkIn >= checkOut || checkIn < DateTime.Today)
            {
                ModelState.AddModelError("", "Invalid check-in or check-out dates.");
                return View("Book", roomViewModel);

            }
            if(guests <=0 || guests > room.Capacity)
            {
                ModelState.AddModelError("", $"Invalid number of guests.{room.Capacity} Guests at most.");
                return View("Book", roomViewModel);
            }
            if(room_count>room.NumberOfAvailableRooms)
            {
                ModelState.AddModelError("", $"Available rooms:{room.NumberOfAvailableRooms}");
                return View("Book", roomViewModel);
            }
            var dto = new AddBookingDto
            {
                ApplicationUserId = userId,
                RoomId = room.RoomId,
                CheckInDate = checkIn,
                CheckOutDate = checkOut,
                GuestsCount = guests,
                TotalPrice = room.PricePerNight * (decimal)(checkOut - checkIn).TotalDays *room_count,
                BookingDate = DateTime.Now,
                phoneNumber = PhoneNumber,
                RoomImageUrl=room.BaseImageUrl,
                room_count=room_count
            };
            var Bookingid = await _bookingService.CreateBookingAsync(dto);
            if (Bookingid<=0)
                return BadRequest("Room not available");
            ViewBag.room= roomViewModel;
            dto.BookingId= Bookingid;
            TempData["Success"] = "Booking successfully.";
            return View("View",dto);
        }

        public IActionResult MyBookings()
        {
            var userId = _userManager.GetUserId(User);
            var bookings = _bookingService.GetBookingsByUser(userId);
            return View(bookings);
        }

        [HttpPost]
        public IActionResult CancelBooking(int bookingId)
        {
          
            var booking = _unitOfWork.Booking.GetById(b => b.BookingId == bookingId,tracked: true);

            if (booking == null)
            {
                return NotFound();  
            }

    
            var room = _unitOfWork.Room.GetById(r => r.RoomId == booking.RoomId, tracked: true);
            if (room != null)
            {
                room.NumberOfAvailableRooms += booking.room_count;
            }

            booking.IsActive = false;

            _unitOfWork.save();

            TempData["Success"] = "Booking has been cancelled successfully.";
            return RedirectToAction("MyBookings");
        }


    }
}

