using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estra7a.Services.DTO
{
    public class FavoriteDTO
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public string RoomName { get; set; }
        public string RoomDescription { get; set; }
        public decimal PricePerNight { get; set; }
        public int Capacity { get; set; }
        public string RoomImageUrl { get; set; }
        public string UserId { get; set; }
    }
}
