using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Estra7a.Web.Areas.Guest.Controllers
{
    [Area("Guest")]
    [Authorize]
    public class FavoriteController : Controller
    {
        private readonly IFavoriteService _favoriteService;
        private readonly IRoomService _roomService;

        public FavoriteController(IFavoriteService favoriteService, IRoomService roomService)
        {
            _favoriteService = favoriteService;
            _roomService = roomService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var favorites = await _favoriteService.GetUserFavoritesAsync(userId);

            var rooms = favorites.Select(f => new RoomViewModel
            {
                Id = f.RoomId,
                Name = f.RoomName,
                Description = f.RoomDescription ?? "",
                PricePerNight = f.PricePerNight,
                Capacity = f.Capacity,
                BaseImageUrl = f.RoomImageUrl ?? "/images/default.jpg",
                IsFavorite = true
            }).ToList();

            return View("Index", rooms);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleFavorite(int roomId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isFavorite = await _favoriteService.ToggleFavoriteAsync(userId, roomId);
            return Json(new { success = true, isFavorite });
        }

        [HttpGet]
        public async Task<IActionResult> IsFavorite(int roomId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isFavorite = await _favoriteService.IsFavoriteAsync(userId, roomId);
            return Json(new { isFavorite });
        }
    }
}
