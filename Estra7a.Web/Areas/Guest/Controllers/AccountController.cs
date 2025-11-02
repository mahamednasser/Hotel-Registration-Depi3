using Estra7a.DataAccess.Repositories.IRepository;
using Estra7a.DataAccess.Repositories.Repository;
using Estra7a.Models.Models;
using Estra7a.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Estra7a.Web.Areas.Guest.Controllers
{
    [Area("Guest")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,IUnitOfWork unitOfWork)
        {
            this.userManager = userManager;
            SignInManager = signInManager;
            _UnitOfWork = unitOfWork;
        }

        public SignInManager<ApplicationUser> SignInManager { get; }
        public IUnitOfWork _UnitOfWork { get; }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserVM user)
        {
            ApplicationUser ap = new ApplicationUser()
            {
                UserName = user.Name,
                PasswordHash = user.Password,
                City = user.Address,
                Email = user.Email, 

            };
            if (ModelState.IsValid)
            {
                IdentityResult idd = await userManager.CreateAsync(ap, user.Password);

                if (idd.Succeeded)
                {
                    await SignInManager.SignInAsync(ap, false);

                    //create cart if it not found
                    var cart = _UnitOfWork.cart.GetAll().FirstOrDefault(c => c.UserId == ap.Id);
                    if (cart == null)
                    {
                        cart = new Cart { UserId = ap.Id };
                        _UnitOfWork.cart.Add(cart);
                        _UnitOfWork.save();
                    }


                }
                else
                {
                    foreach (var item in idd.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }

            return View("Login");

        }

        public async Task<IActionResult> Signout()
        {
            await SignInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }


       
        [HttpGet]

        public IActionResult Login()
        {
            return View("Login");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginUserVM logininfo)
        {
            if (ModelState.IsValid)
            {
                var ap = await userManager.FindByNameAsync(logininfo.Username);

                if (ap != null)
                {
                   
                    if (ap.LockoutEnd != null && ap.LockoutEnd > DateTimeOffset.UtcNow)
                    {
                        ModelState.AddModelError("", "Your account is locked. Please contact the administrator.");
                        return View(logininfo);
                    }

                  
                    bool validPassword = await userManager.CheckPasswordAsync(ap, logininfo.Password);
                    if (validPassword)
                    {
                        await SignInManager.SignInAsync(ap, logininfo.RememberMe);
                        //create cart if not found
                        var cart = _UnitOfWork.cart.GetAll().FirstOrDefault(c => c.UserId == ap.Id);
                        if (cart == null)
                        {
                            cart = new Cart { UserId = ap.Id };
                            _UnitOfWork.cart.Add(cart);
                            _UnitOfWork.save();
                        }
                        return Redirect("~/Home/Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid password.");
                        return View(logininfo);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "User not found.");
                    return View(logininfo);
                }
            }

            return View(logininfo);
        }


        [HttpGet]
        public IActionResult Update()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); 
            var user = _UnitOfWork.ApplicationUser.GetById(c => c.Id == userId);

            if (user == null)
            {
                return NotFound(); 
            }

            return View(user); 
        }

        [HttpPost]
        public IActionResult Update(ApplicationUser userFromRequest)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userFromDB = _UnitOfWork.ApplicationUser.GetById(c => c.Id == userId);

            if (userFromDB == null)
            {
                return NotFound();
            }

            userFromDB.PhoneNumber = userFromRequest.PhoneNumber;
            userFromDB.name = userFromRequest.name;
            userFromDB.City = userFromRequest.City;
            userFromDB.StreetAddress = userFromRequest.StreetAddress;

            _UnitOfWork.ApplicationUser.Update(userFromDB);
            _UnitOfWork.save();

            return View(userFromDB);
        }
    }


}
