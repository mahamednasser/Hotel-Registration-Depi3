using Estra7a.DataAccess.Repositories.IRepository;
using Estra7a.Services.DTO;
using Estra7a.Services.ServiceContracts;
using Estra7a.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Estra7a.Services.Services
{
    public class FavoriteServices : IFavoriteService
    {
        private readonly IUnitOfWork _unitOfWork;

        public FavoriteServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<FavoriteDTO>> GetUserFavoritesAsync(string userId)
        {
            var favorites = _unitOfWork.Favorite
                .GetAll(f => f.UserId == userId, IncludeProp: "Room")
                .Select(f => new FavoriteDTO
                {
                    Id = f.Id,
                    RoomId = f.RoomId,
                    RoomName = f.Room?.Name ?? "No Room",
                    RoomDescription = f.Room?.Description ?? "",
                    PricePerNight = f.Room?.PricePerNight ?? 0,
                    Capacity = f.Room?.Capacity ?? 0,
                    RoomImageUrl = f.Room?.BaseImageUrl ?? "/images/default.jpg",
                    UserId = f.UserId
                })
                .ToList();

            return await Task.FromResult(favorites);
        }




        public async Task<bool> ToggleFavoriteAsync(string userId, int roomId)
        {
            var exists = _unitOfWork.Favorite
                .GetAll()
                .FirstOrDefault(f => f.UserId == userId && f.RoomId == roomId);

            if (exists != null)
            {
                _unitOfWork.Favorite.Remove(exists);
                await _unitOfWork.SaveAsync();
                return false; 
            }
            else
            {
                var fav = new Favorite { UserId = userId, RoomId = roomId };
                _unitOfWork.Favorite.Add(fav);
                await _unitOfWork.SaveAsync();
                return true; 
            }
        }

        
        public async Task<bool> IsFavoriteAsync(string userId, int roomId)
        {
            var exists = _unitOfWork.Favorite
                .GetAll()
                .FirstOrDefault(f => f.UserId == userId && f.RoomId == roomId);

            return await Task.FromResult(exists != null);
        }
    }
}
