using Estra7a.DataAccess.Data;
using Estra7a.DataAccess.Repositories.IRepository;
using Estra7a.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estra7a.DataAccess.Repositories.Repository
{
    class CartItemsRepository: Repository<CartItem>, ICartItemsRepository
    {
        private readonly AppDbContext _context;

        public CartItemsRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public void Update(CartItem cart)
        {
            _context.CartItems.Update(cart);
        }
    }
}
