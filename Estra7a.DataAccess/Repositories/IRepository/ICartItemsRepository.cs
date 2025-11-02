using Estra7a.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estra7a.DataAccess.Repositories.IRepository
{
   public interface ICartItemsRepository:IRepository<CartItem>
    {
        void Update(CartItem cart);
    }
}
