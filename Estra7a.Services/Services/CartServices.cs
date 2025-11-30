using Estra7a.DataAccess.Repositories.IRepository;
using Estra7a.Models.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Estra7a.Services.Services
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CartService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Cart> GetOrCreateCartAsync(string userId)
        {
            var cart = _unitOfWork.cart.GetAll()
                                        .FirstOrDefault(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                _unitOfWork.cart.Add(cart);
                _unitOfWork.save();
               
            }

            return cart;
        }
    }
}
