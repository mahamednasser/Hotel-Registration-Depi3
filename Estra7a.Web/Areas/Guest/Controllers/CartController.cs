using Estra7a.DataAccess.Repositories.IRepository;
using Estra7a.Models.Models;
using Estra7a.Services.ServiceContracts;
using Estra7a.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Estra7a.Web.Controllers
{

    [Area("Guest")]
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        public CartController(ICartService cartService, UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork)
        {
            _cartService = cartService;

            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        [Authorize]
        public IActionResult Index()
        {
            var userId = _userManager.GetUserId(User);

            var cart = _unitOfWork.cart
                .GetById(c => c.UserId == userId, "CartItems.Room");

            if (cart == null)
            {
                return View(new Cart { CartItems = new List<CartItem>() });
            }
            if (cart.CartItems == null || !cart.CartItems.Any())
            {
                return View("EmptyCart");
            }

            return View( cart);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateQuantity([FromBody] UpdateCartRequest request)
        {
            var userId = _userManager.GetUserId(User);
            var userCart = _unitOfWork.cart
                .GetById(c => c.UserId == userId, "CartItems");

            if (userCart == null)
                return Json(new { success = false, message = "Cart not found" });

            var cartItem = userCart.CartItems.FirstOrDefault(ci => ci.RoomId == request.RoomId);

            if (cartItem != null)
            {
                cartItem.Quantity = request.Quantity;
                _unitOfWork.cartItems.Update(cartItem);
            }
            else
            {
                var room = _unitOfWork.Room.GetById(r => r.RoomId == request.RoomId);
                if (room != null)
                {
                    _unitOfWork.cartItems.Add(new CartItem
                    {
                        CartId = userCart.CartId,
                        RoomId = room.RoomId,
                        RoomName = room.Name,
                        RoomImage = room.BaseImageUrl,
                        Quantity = request.Quantity,
                        Price = room.PricePerNight,
                        Area=room.Area.ToString(),
                       
                        
                    });
                }
            }

            _unitOfWork.save();
            return Json(new { success = true, totalPrice = userCart.CartItems.Sum(i => i.Price * i.Quantity) });

        }
        [HttpGet]
        [Authorize]
        public IActionResult GetItemQuantity(int roomId)
        {
            var userId = _userManager.GetUserId(User);
            var userCart = _unitOfWork.cart.GetById(c => c.UserId == userId, "CartItems");

            if (userCart == null)
                return Json(new { exists = false });

            var cartItem = userCart.CartItems.FirstOrDefault(ci => ci.RoomId == roomId);

            if (cartItem == null)
                return Json(new { exists = false });

            return Json(new { exists = true, quantity = cartItem.Quantity });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveItem([FromBody] int roomId)
        {
            var userId = _userManager.GetUserId(User);
            var userCart = _unitOfWork.cart.GetById(c => c.UserId == userId, "CartItems");

            if (userCart == null)
                return Json(new { success = false, message = "Cart not found" });

            var cartItem = userCart.CartItems.FirstOrDefault(ci => ci.RoomId == roomId);
            if (cartItem == null)
                return Json(new { success = false, message = "Item not found" });

            _unitOfWork.cartItems.Remove(cartItem);
            _unitOfWork.save();

            var totalPrice = userCart.CartItems
                .Where(ci => ci.RoomId != roomId)
                .Sum(ci => ci.Price * ci.Quantity);

            return Json(new { success = true, totalPrice });
        }
        [HttpGet]
        public IActionResult EmptyCart()
        {
            return View("EmptyCart");
        }



    }
}

