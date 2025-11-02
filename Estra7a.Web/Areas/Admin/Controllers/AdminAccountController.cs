using Estra7a.DataAccess.Repositories.IRepository;
using Estra7a.Models.Models;
using Estra7a.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Estra7a.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminAccountController : Controller
    {
       
        public AdminAccountController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _UnitOfWork = unitOfWork;
            _UserManager = userManager;
        }

        public IUnitOfWork _UnitOfWork { get; }
        public UserManager<ApplicationUser> _UserManager { get; }

        [HttpGet]
        public IActionResult UserDetails()
        {
            ApplicationUserVM applicationUser = new ApplicationUserVM()
            {
                UserList = _UnitOfWork.ApplicationUser.GetAll().ToList(),
            };

            return View("UserDetails", applicationUser);
        }

        [HttpPost]
        public async Task<IActionResult> LockUser(string id)
        {
            var user = await _UserManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

           
            user.LockoutEnd = DateTimeOffset.UtcNow.AddYears(1);
            await _UserManager.UpdateAsync(user);

            TempData["Message"] = $"{user.UserName} has been locked.";
            return RedirectToAction("UserDetails");
        }


        [HttpPost]
        public async Task<IActionResult> UnlockUser(string id)
        {
            var user = await _UserManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            user.LockoutEnd = null;
            await _UserManager.UpdateAsync(user);

            TempData["Message"] = $"{user.UserName} has been unlocked.";
            return RedirectToAction("UserDetails");
        }
    }
}
