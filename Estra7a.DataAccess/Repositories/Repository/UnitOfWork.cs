using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Estra7a.DataAccess.Data;
using Estra7a.DataAccess.Repositories.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Estra7a.DataAccess.Repositories.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private AppDbContext _db;
        
        public IApplicationUserRepository ApplicationUser { get;private set; }

        public IRoomRepository Room { get; private set; }
        public IRoomImagesRepository RoomImages { get; private set; }
        public IRoomTypeRepository RoomType { get; private set; }
        public IRoomFeatureRepository RoomFeature { get; private set; }

        public IBookingRepository Booking { get; private set; }

        public ICartRepository cart { get; private set; }

        public ICartItemsRepository cartItems { get; private set; }

        public IFavoriteRepository Favorite { get; private set; }

        public UnitOfWork( AppDbContext db)
        {
            _db = db;
            ApplicationUser = new ApplicationUserRepository(_db);
            Room = new RoomRepository(_db);
            RoomType = new RoomTypeRepository(_db);
            RoomFeature = new RoomFeatureRepository(_db);
            RoomImages = new RoomImagesRepository(_db);
            Booking = new BookingRepository(_db);
            cart = new CartRepository(_db);
            cartItems = new CartItemsRepository(_db);
            Favorite = new FavoriteRepository(_db);
        }

        public void save()
        {
            _db.SaveChanges();
            
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }


    }
}
