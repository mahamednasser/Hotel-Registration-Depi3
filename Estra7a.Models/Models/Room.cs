using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estra7a.Models.Models
{
    public class Room
    {
        public int RoomId {  get; set; }

        public string? Name { get; set; } = string.Empty;

        public int NumberOfRooms{get; set; }

        public int NumberOfAvailableRooms { get; set; }
        public string? Description { get; set; } = string.Empty;

        public string BaseImageUrl { get; set; } = string.Empty;

        public decimal PricePerNight { get; set; }
        public double Area { get; set; }
        public int Capacity { get; set; }

        public double? RoomRate { get; set; } = 4; //  in future will be dynamic

        public int RoomTypeId { get; set; }
        public RoomType RoomType { get; set; } 
        
        public ICollection<RoomImage> RoomImages { get; set; } = new List<RoomImage>(); 
       
    }
}
