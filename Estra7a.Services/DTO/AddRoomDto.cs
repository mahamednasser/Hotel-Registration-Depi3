using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Estra7a.Services.DTO
{
    public class AddRoomDto
    {
        public string Name { get; set; } = string.Empty;
        public decimal PricePerNight { get; set; }
        public string? Description { get; set; }
        public int RoomtypeId { get; set; }
        public double Area { get; set; }
        public int NumberOfRooms { get; set; }
        public int Capacity { get; set; }

        public List<int> SelectedFeaturesId { get; set; } = new();
        public IFormFile BaseImage { get; set; } = default!;
        public List<IFormFile> RoomImages { get; set; } = new();
    }
}
