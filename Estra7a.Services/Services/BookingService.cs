using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Estra7a.Services.DTO;

namespace Estra7a.Services.Services
{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        public BookingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> CreateBookingAsync(AddBookingDto dto)
        {
            var room = _unitOfWork.Room.GetById(r=>r.RoomId==dto.RoomId,tracked:true);
            if(room == null || room.NumberOfAvailableRooms<=0)
            {
                return 0;
            }

            Booking booking = new Booking
            {
                ApplicationUserId = dto.ApplicationUserId,
                RoomId = dto.RoomId,
                CheckInDate = dto.CheckInDate,
                CheckOutDate = dto.CheckOutDate,
                NumberOfGuests = dto.GuestsCount,
                TotalPrice = dto.TotalPrice,
                BookingDate = dto.BookingDate,
                phoneNumber = dto.phoneNumber
               ,room_count=dto.room_count,
                IsActive=dto.IsActive
            };

            room.NumberOfAvailableRooms-=dto.room_count;
            _unitOfWork.Booking.Add(booking);
            _unitOfWork.save();
            return booking.BookingId;


        }

        public BookingDto GetBookingDetails(int id)
        {
            var booking = _unitOfWork.Booking.GetById(b => b.BookingId == id, "Room,ApplicationUser");
            if (booking == null)
                return null;

            return new BookingDto
            {
                Id = booking.BookingId,
                RoomName = booking.Room.Name,
                CheckIn = booking.CheckInDate,
                CheckOut = booking.CheckOutDate,
                TotalPrice = booking.TotalPrice,
               BookingDate = booking.BookingDate,
                RoomImageUrl = booking.Room.BaseImageUrl,
                room_count=booking.room_count,
                IsActive=booking.IsActive
            };
        }

        public IEnumerable<BookingDto> GetBookingsByUser(string userId)
        {
            var bookings = _unitOfWork.Booking.GetAll(b => b.ApplicationUserId == userId, "Room");
            return bookings.Select(b => new BookingDto
            {
                Id = b.BookingId,
                RoomName = b.Room.Name,
                CheckIn = b.CheckInDate,
                CheckOut = b.CheckOutDate,
                TotalPrice = b.TotalPrice,
                BookingDate = b.BookingDate,
                RoomImageUrl=b.Room.BaseImageUrl,
                room_count=b.room_count,
                IsActive=b.IsActive
            });
        }
    }
}
