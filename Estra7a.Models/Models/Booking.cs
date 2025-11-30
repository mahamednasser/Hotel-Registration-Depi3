using Estra7a.Models.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public class Booking
{
	[Key]
	public int BookingId { get; set; }

	public int RoomId { get; set; }
    [ForeignKey("RoomId")]
    public virtual Room Room { get; set; }

	public string ApplicationUserId { get; set; }

	[ForeignKey("ApplicationUserId")]
	public virtual ApplicationUser ApplicationUser { get; set; }

	[Required]
    public DateTime CheckInDate { get; set; }

	[Required]
    public DateTime CheckOutDate { get; set; }

	public int NumberOfGuests { get; set; }

    [Column(TypeName = "decimal(18,5)")]
    public decimal TotalPrice { get; set; }

    public DateTime BookingDate { get; set; } = DateTime.Now;
    
    public string phoneNumber { get; set; }

    public int  room_count { get; set; }
    //public string?  bookingStatus { get; set; }= "Pending";
    //public string? sessionId { get; set; }
    //public string? paymentIntentId { get; set; }
    public bool IsActive { get; set; } = true;
}
