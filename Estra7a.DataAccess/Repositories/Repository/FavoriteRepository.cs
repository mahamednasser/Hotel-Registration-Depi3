using Estra7a.DataAccess.Data;
using Estra7a.DataAccess.Migrations;
using Estra7a.DataAccess.Repositories.IRepository;
using Estra7a.Models.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Estra7a.DataAccess.Repositories.Repository
{
    internal class FavoriteRepository : Repository<Favorite>, IFavoriteRepository
    {
        private readonly AppDbContext _db;
        public FavoriteRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public IEnumerable<Favorite> GetUserFavorites(string userId)
        {
            return _db.Favorites
                .Include(f => f.Room)
                .Where(f => f.UserId == userId)
                .ToList();
        }

        public bool Exists(string userId, int roomId)
        {
            return _db.Favorites.Any(f => f.UserId == userId && f.RoomId == roomId);
        }
    }
}
