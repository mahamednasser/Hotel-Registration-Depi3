using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estra7a.Models.Models
{
    public class CartItem
    {
        public int CartItemId { get; set; }
        public int CartId { get; set; }
        public int RoomId { get; set; }
        public string RoomName { get; set; } = string.Empty;

        public string RoomImage { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Area { get; set; } = string.Empty;

        public int Quantity { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal Price { get; set; }

       
        public Cart Cart { get; set; }
        public Room Room { get; set; }
    }
}
