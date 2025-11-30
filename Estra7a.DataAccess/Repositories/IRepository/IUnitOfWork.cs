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
        IRoomImagesRepository RoomImages { get; }
        IRoomFeatureRepository RoomFeature { get; }
        IRoomTypeRepository RoomType { get; }
        ICartRepository cart { get; }
        ICartItemsRepository cartItems { get; }
         IBookingRepository Booking { get; }

        IFavoriteRepository Favorite { get; }
        void save();

        Task SaveAsync();
    }
}
