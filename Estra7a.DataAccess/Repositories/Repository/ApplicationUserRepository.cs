using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Estra7a.DataAccess.Data;
using Estra7a.DataAccess.Repositories.IRepository;
using Estra7a.Models.Models;

namespace Estra7a.DataAccess.Repositories.Repository
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        private AppDbContext _db;

        public ApplicationUserRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(ApplicationUser obj)
        {
           _db.ApplicationUsers.Update(obj);
        }
    }
}
