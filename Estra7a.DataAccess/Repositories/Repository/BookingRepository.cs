using Estra7a.DataAccess.Data;
using Estra7a.DataAccess.Repositories.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estra7a.DataAccess.Repositories.Repository
{
    public class BookingRepository:Repository<Booking>,IBookingRepository
    {
        private readonly AppDbContext _db;
        public BookingRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Booking obj)
        {
            _db.Bookings.Update(obj);
        }
    }
}
