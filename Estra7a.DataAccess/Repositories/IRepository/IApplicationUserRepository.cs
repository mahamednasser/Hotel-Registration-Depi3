using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Estra7a.Models.Models;
using Estra7a.DataAccess.Data;

namespace Estra7a.DataAccess.Repositories.IRepository
{
    public interface IApplicationUserRepository:IRepository<ApplicationUser>
    {

        void Update(ApplicationUser obj);
    }
}
