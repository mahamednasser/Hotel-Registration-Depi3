using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Estra7a.DataAccess.Data;
using Estra7a.DataAccess.Repositories.IRepository;
using Estra7a.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace Estra7a.DataAccess.Repositories.Repository
{
    public class RoomTypeRepository : Repository<RoomType>, IRoomTypeRepository
    {
        private AppDbContext _db;

        public RoomTypeRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
