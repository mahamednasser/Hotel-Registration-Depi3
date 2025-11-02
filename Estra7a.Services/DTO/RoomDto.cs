using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estra7a.Services.DTO
{
    public class RoomDto
    {
            public int Id { get; set; }

            public string Name { get; set; } = string.Empty;

            public decimal PricePerNight { get; set; }

            public string? Description { get; set; }

           public string? RoomTypeDescription { get; set; }

            public double Area { get; set; }

            public int Capacity { get; set; }

            public double? Rate {  get; set; }

            public string BaseImageUrl { get; set; } = string.Empty;

            public int NumberOfRooms { get; set; }

            public int NumberOfAvailableRooms { get; set; }

            public int RoomTypeId { get; set; }

            public string RoomTypeName { get; set; } = string.Empty;

            public List<string> RoomImages { get; set; } = new();
        }
    
}
