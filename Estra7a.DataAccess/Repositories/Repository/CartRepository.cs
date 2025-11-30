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
    class CartRepository : Repository<Cart>, ICartRepository
    {
        private readonly AppDbContext _context;

        public CartRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        
    }
}
