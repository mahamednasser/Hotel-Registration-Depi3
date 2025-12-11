using Estra7a.DataAccess.Repositories.IRepository;
using Estra7a.DataAccess.Repositories.Repository;
using Estra7a.Models.Models;
using Estra7a.Services.DTO;
using Estra7a.Services.Services;
using Estra7a.Web.ViewModels;
using Estra7a_Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Stripe.Checkout;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Estra7a.Web.Areas.Guest.Controllers
{
    [Area("Guest")]
    public class BookingController : Controller
    {
        private readonly IBookingService _bookingService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailSender _emailSender;

        public BookingController(IBookingService bookingService, UserManager<ApplicationUser> userManager,IUnitOfWork unitOfWork, IEmailSender emailSender)
        {
            _emailSender = emailSender;
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
        public async Task<IActionResult> Create(int roomId, DateTime checkIn, DateTime checkOut, int guests, string PhoneNumber,int room_count, IFormFile NationalIdImage)
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
            var phoneRegex = new Regex(@"^(010|011|012|015)[0-9]{8}$");

            if (string.IsNullOrEmpty(PhoneNumber) || !phoneRegex.IsMatch(PhoneNumber))
            {
                ModelState.AddModelError("PhoneNumber", "Phone Number Must Start With 010/011/012/015.");
                return View("Book", roomViewModel);
            }
            if (guests <=0 || guests > room.Capacity)
            {
                ModelState.AddModelError("", $"Invalid number of guests.{room.Capacity} Guests at most.");
                return View("Book", roomViewModel);
            }
            if(room_count>room.NumberOfAvailableRooms)
            {
                ModelState.AddModelError("", $"Available rooms:{room.NumberOfAvailableRooms}");
                return View("Book", roomViewModel);
            }

            ////Ai validator
            //if (NationalIdImage == null || NationalIdImage.Length == 0)
            //{
            //    ModelState.AddModelError("", "Please upload your National ID image.");
            //    return View("Book", roomViewModel);
            //}

            //var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "IDUploads");
            //Directory.CreateDirectory(folder);

            //var filePath = Path.Combine(folder, NationalIdImage.FileName);

            //using (var stream = new FileStream(filePath, FileMode.Create))
            //    await NationalIdImage.CopyToAsync(stream);

            //var input = new IDValidator.ModelInput
            //{
            //    ImageSource = System.IO.File.ReadAllBytes(filePath)
            //};

            //var prediction = IDValidator.Predict(input);

            //if (prediction.PredictedLabel != "valid")
            //{
            //    ModelState.AddModelError("", "Your National ID is invalid \n Please upload a clear image.");
            //    return View("Book", roomViewModel);
            //}




            var diff =checkOut - checkIn;

            var dto = new AddBookingDto
            {
                ApplicationUserId = userId,
                RoomId = room.RoomId,
                CheckInDate = checkIn,
                CheckOutDate = checkOut,
                GuestsCount = guests,
                TotalPrice = room.PricePerNight * (decimal)(((checkOut - checkIn).TotalDays) + 1) *room_count,
                BookingDate = DateTime.Now,
                phoneNumber = PhoneNumber,
                RoomImageUrl=room.BaseImageUrl,
                room_count=room_count
            };










           
            HttpContext.Session.SetString("PendingBooking" , JsonConvert.SerializeObject(dto));


            ViewBag.room= roomViewModel;
            
           


            var domain = "https://localhost:7285/";
            var options = new Stripe.Checkout.SessionCreateOptions
            {

                SuccessUrl = domain + $"Guest/Booking/confirm" ,
                CancelUrl = domain + $"Guest/Booking/MyBookings" ,
                LineItems = new List<Stripe.Checkout.SessionLineItemOptions>() ,

                Mode = "payment" ,
            };

            var sessionLineItem = new SessionLineItemOptions()
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmount = (long)( dto.TotalPrice * 100 ) ,
                    Currency = "usd" ,
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = room.Name ,
                    }

                } ,
                Quantity = room_count


            };
            options.LineItems.Add(sessionLineItem);


            var service = new SessionService();
            Session session = service.Create(options);
            //var booking = _unitOfWork.Booking.GetById(x => x.BookingId == Bookingid);
            //booking.paymentIntentId = session.PaymentIntentId;
            //booking.sessionId = session.Id;
            //_unitOfWork.Booking.Update(booking);
            //_unitOfWork.save();
            Response.Headers.Add("Location" , session.Url);
            return new StatusCodeResult(303);








           
        }
        //Ai validator
        [HttpPost]
        public async Task<JsonResult> ValidateNationalId(IFormFile NationalIdImage)
        {
            if (NationalIdImage == null || NationalIdImage.Length == 0)
            {
                return Json(new { isValid = false, message = "Please upload your National ID image." });
            }

            var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "IDUploads");
            Directory.CreateDirectory(folder);
            var filePath = Path.Combine(folder, NationalIdImage.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
                await NationalIdImage.CopyToAsync(stream);

            var input = new IDValidator.ModelInput
            {
                ImageSource = System.IO.File.ReadAllBytes(filePath)
            };

            var prediction = IDValidator.Predict(input);

            if (prediction.PredictedLabel != "valid")
            {
                return Json(new { isValid = false, message = "Your National ID is invalid. Please upload a clear image." });
            }

            return Json(new { isValid = true, message = "Valid ID" });
        }

        public async Task<IActionResult> confirm()
        {

            var dtoJson = HttpContext.Session.GetString("PendingBooking");
            if ( dtoJson == null )
                return BadRequest("Booking info missing");

            var dtoo = JsonConvert.DeserializeObject<AddBookingDto>(dtoJson);

            // بعد إنشاء الحجز:
            HttpContext.Session.Remove("PendingBooking");


            var Bookingid = await _bookingService.CreateBookingAsync(dtoo);
            if ( Bookingid <= 0 )
                return BadRequest("Room not available"); 
            var id = Bookingid;
            var bookingDetails = _unitOfWork.Booking.GetById(x => x.BookingId == id);
            if (bookingDetails == null)
            {
                return NotFound();
            }
            var room = _unitOfWork.Room.GetById(r => r.RoomId == bookingDetails.RoomId, tracked: false);
            var dto = new AddBookingDto
            {
                ApplicationUserId = bookingDetails.ApplicationUserId,
                RoomId = bookingDetails.RoomId,
                CheckInDate = bookingDetails.CheckInDate,
                CheckOutDate = bookingDetails.CheckOutDate,
                GuestsCount = bookingDetails.NumberOfGuests,
                TotalPrice = bookingDetails.TotalPrice,
                BookingDate = bookingDetails.BookingDate,
                phoneNumber = bookingDetails.phoneNumber,
                RoomImageUrl = room.BaseImageUrl,
                room_count = bookingDetails.room_count,


            };

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

            var user = (await _userManager.GetUsersInRoleAsync("Admin")).FirstOrDefault();
            if (user != null && user.Email != null)
            {

                await _emailSender.SendEmailAsync(user.Email, "Booking Confirmed", $"New booking for room {room.Name} from {bookingDetails.CheckInDate.ToShortDateString()} to {bookingDetails.CheckOutDate.ToShortDateString()} has been confirmed.");
            }

            ViewBag.room = roomViewModel;
            //bookingDetails.bookingStatus = "Confirmed";
            //_unitOfWork.Booking.Update(bookingDetails);
            //_unitOfWork.save();
            return View("View", dto);

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

