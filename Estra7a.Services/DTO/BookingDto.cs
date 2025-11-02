using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estra7a.Services.DTO
{
   
        public class BookingDto
        {
            public int Id { get; set; }
            public string RoomName { get; set; }
            public DateTime CheckIn { get; set; }
            public DateTime CheckOut { get; set; }
            public decimal TotalPrice { get; set; }
        public DateTime BookingDate { get; set; }
        public string RoomImageUrl { get; set; }

        public int room_count { get; set; }

        public bool IsActive { get; set; } = false;

    }

    
}
