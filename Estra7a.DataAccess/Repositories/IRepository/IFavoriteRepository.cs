using Estra7a.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estra7a.DataAccess.Repositories.IRepository
{
    public interface IFavoriteRepository : IRepository<Favorite>
    {
        IEnumerable<Favorite> GetUserFavorites(string userId);
        bool Exists(string userId, int roomId);
    }
}
