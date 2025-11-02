using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estra7a.Models.Models
{
    public class RoomImage
    {
        public int RoomImageId { get; set; }

        public string ImageUrl { get; set; } = string.Empty;

        public int RoomId { get; set; }

        public Room? Room { get; set; }

    }
}
