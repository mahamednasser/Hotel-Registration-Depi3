using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estra7a.Models.Models
{
    public class RoomType
    {
        public int RoomTypeId { get; set; }

        public string Name { get; set; } = string.Empty;         
        public string Description { get; set; } = string.Empty;   
                      
                    

       // public string Amenities { get; set; } = string.Empty; in future

        public ICollection<Room> Rooms { get; set; } = new List<Room>();
    }
}
