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
    internal class RoomRepository : Repository<Room>, IRoomRepository
    {
        private readonly AppDbContext _context;
        public RoomRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public void Update(Room room)
        {
            _context.Rooms.Update(room);
        }
    }
}
