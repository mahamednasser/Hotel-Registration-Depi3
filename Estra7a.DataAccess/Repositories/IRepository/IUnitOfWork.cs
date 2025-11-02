using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estra7a.DataAccess.Repositories.IRepository
{
    public interface IUnitOfWork
    {
        
        IApplicationUserRepository ApplicationUser { get; }
        IRoomRepository Room { get; }

        IRoomTypeRepository RoomType { get; }
        IBookingRepository Booking { get; }
        ICartRepository cart { get; }
        ICartItemsRepository cartItems { get; }

        IFavoriteRepository Favorite { get; }
        void save();

        Task SaveAsync();
    }
}
