using Estra7a.DataAccess.Repositories.IRepository;
using Estra7a.DataAccess.Repositories.Repository;
using Estra7a.Models.Models;
using Estra7a.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.Security.Claims;
using Estra7a.Services.ServiceContracts;
using Estra7a.Services.Services;

namespace Estra7a.Web.Areas.Guest.Controllers
{
    [Area("Guest")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly Services.ServiceContracts.IEmailSender _emailSender;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IUnitOfWork unitOfWork,
            Services.ServiceContracts.IEmailSender emailSender)
        {
            this.userManager = userManager;
            SignInManager = signInManager;
            _UnitOfWork = unitOfWork;
            _emailSender = emailSender;
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
            if (!ModelState.IsValid)
            {
                return View(user); 
            }

            ApplicationUser ap = new ApplicationUser()
            {
                UserName = user.Name,
                City = user.Address,
                Email = user.Email,
            };

            IdentityResult idd = await userManager.CreateAsync(ap, user.Password);

            if (idd.Succeeded)
            {
                await SignInManager.SignInAsync(ap, false);

               
                var cart = _UnitOfWork.cart.GetAll().FirstOrDefault(c => c.UserId == ap.Id);
                if (cart == null)
                {
                    cart = new Cart { UserId = ap.Id };
                    _UnitOfWork.cart.Add(cart);
                    _UnitOfWork.save();
                }

                
                return View("Login");
            }

           
            foreach (var item in idd.Errors)
            {
                ModelState.AddModelError("", item.Description);
            }

            return View(user); 
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
                        // check role
                        if (await userManager.IsInRoleAsync(ap, "Admin"))
                        {
                            return RedirectToAction("UserDetails", "AdminAccount", new { area = "Admin" }); 
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

        // Add these methods to your AccountController class

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordVM model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    // Don't reveal that the user doesn't exist
                    return RedirectToAction("ForgotPasswordConfirmation");
                }

                // Generate password reset token
                var token = await userManager.GeneratePasswordResetTokenAsync(user);

                // Create reset link
                var callbackUrl = Url.Action(
                    "ResetPassword",
                    "Account",
                    new { token = token, email = model.Email },
                    protocol: HttpContext.Request.Scheme);

                // Send email
                await _emailSender.SendEmailAsync(
                    model.Email,
                    "Reset Password",
                    $"Please reset your password by clicking here: <a href='{callbackUrl}'>Reset Password</a>");

                return RedirectToAction("ForgotPasswordConfirmation");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            if (token == null || email == null)
            {
                return BadRequest("Invalid password reset token");
            }

            var model = new ResetPasswordVM { Token = token, Email = email };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user doesn't exist
                return RedirectToAction("ResetPasswordConfirmation");
            }

            var result = await userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }
    }


}
