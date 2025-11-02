using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estra7a.Services.DTO
{
    public class CartDto
    {
        public int CartId { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        [NotMapped]
        public decimal TotalPrice => CartItems.Sum(i => i.Price * i.Quantity);


        public ICollection<CartItem> CartItems { get; set; }
    }
}
