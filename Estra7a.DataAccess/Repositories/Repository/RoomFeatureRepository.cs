using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Estra7a.DataAccess.Data;
using Estra7a.DataAccess.Repositories.IRepository;
using Estra7a.Models.Models;

namespace Estra7a.DataAccess.Repositories.Repository
{
    public class RoomFeatureRepository : Repository<RoomFeature>, IRoomFeatureRepository
    {
        private readonly AppDbContext _context;
        public RoomFeatureRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
