using Estra7a.Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estra7a.Services.ServiceContracts
{
    public interface IFavoriteService
    {
        Task<IEnumerable<FavoriteDTO>> GetUserFavoritesAsync(string userId);
        Task<bool> ToggleFavoriteAsync(string userId, int roomId);
        Task<bool> IsFavoriteAsync(string userId, int roomId);
    }
}
