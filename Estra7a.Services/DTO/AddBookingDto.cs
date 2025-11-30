using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estra7a.Services.DTO
{
    public class AddBookingDto
    {
        public string ApplicationUserId { get; set; }
        public int RoomId { get; set; }
        public int BookingId { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int GuestsCount { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime BookingDate { get; set; } = DateTime.Now;

        [RegularExpression(@"^(\+201|01)[0-2,5]{1}[0-9]{8}$", ErrorMessage = "Invalid Egyptian phone number .")]
        public string phoneNumber { get; set; }
        public string RoomImageUrl { get; set; }
        public int room_count { get; set; }

        public string? bookingStatus { get; set; }
        public bool IsActive { get; set; } = true;
    }

}
