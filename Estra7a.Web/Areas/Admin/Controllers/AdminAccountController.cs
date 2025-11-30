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
        private readonly IUnitOfWork _UnitOfWork;
        private readonly UserManager<ApplicationUser> _UserManager;
        private readonly RoleManager<IdentityRole> _RoleManager;

        public AdminAccountController(
            IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _UnitOfWork = unitOfWork;
            _UserManager = userManager;
            _RoleManager = roleManager;
        }

        // ================= User Management =================
        [HttpGet]
        public IActionResult UserDetails()
        {
            var users = _UnitOfWork.ApplicationUser.GetAll().ToList();
            var roles = _RoleManager.Roles.ToList();

            var userRolesDict = new Dictionary<string, List<string>>();
            foreach (var user in users)
            {
                var rolesForUser = _UserManager.GetRolesAsync(user).Result.ToList();
                userRolesDict.Add(user.Id, rolesForUser);
            }

            var vm = new ApplicationUserVM
            {
                UserList = users,
                RolesList = roles,
                UserRoles = userRolesDict
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> LockUser(string id)
        {
            var user = await _UserManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            user.LockoutEnd = DateTimeOffset.UtcNow.AddYears(1);
            await _UserManager.UpdateAsync(user);

            TempData["Message"] = $"{user.UserName} has been locked.";
            return RedirectToAction("UserDetails");
        }

        [HttpPost]
        public async Task<IActionResult> UnlockUser(string id)
        {
            var user = await _UserManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            user.LockoutEnd = null;
            await _UserManager.UpdateAsync(user);

            TempData["Message"] = $"{user.UserName} has been unlocked.";
            return RedirectToAction("UserDetails");
        }

        // ================= Role Management =================

        // List all roles
        [HttpGet]
        public IActionResult Roles()
        {
            var roles = _RoleManager.Roles.ToList();
            return View(roles);
        }

        // Create new role - GET
        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        // Create new role - POST
        [HttpPost]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            if (!string.IsNullOrEmpty(roleName))
            {
                if (!await _RoleManager.RoleExistsAsync(roleName))
                {
                    await _RoleManager.CreateAsync(new IdentityRole(roleName));
                    TempData["Message"] = $"Role {roleName} created successfully.";
                    return RedirectToAction("Roles");
                }
                ModelState.AddModelError("", "Role already exists.");
            }
            return View();
        }

        // Assign role to user - GET
        [HttpGet]
        public async Task<IActionResult> AssignRole(string userId)
        {
            var user = await _UserManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            var roles = _RoleManager.Roles.ToList();
            var userRoles = await _UserManager.GetRolesAsync(user);

            var model = new AssignRoleVM
            {
                UserId = user.Id,
                UserName = user.UserName,
                Roles = roles.Select(r => new RoleCheckVM
                {
                    RoleName = r.Name,
                    IsSelected = userRoles.Contains(r.Name)
                }).ToList()
            };

            return View(model);
        }

        // Assign role to user - POST
        [HttpPost]
        public async Task<IActionResult> AssignRole(string UserId, Dictionary<string, bool> Roles)
        {
            var user = await _UserManager.FindByIdAsync(UserId);
            if (user == null) return NotFound();

            var userRoles = await _UserManager.GetRolesAsync(user);
            await _UserManager.RemoveFromRolesAsync(user, userRoles); // إزالة الرولات القديمة

            var selectedRoles = Roles.Where(r => r.Value).Select(r => r.Key);
            await _UserManager.AddToRolesAsync(user, selectedRoles);

            TempData["Message"] = $"Roles updated for {user.UserName}";
            return RedirectToAction("UserDetails");
        }

        // Edit Role - GET
        [HttpGet]
        public async Task<IActionResult> EditRole(string roleId)
        {
            var role = await _RoleManager.FindByIdAsync(roleId);
            if (role == null) return NotFound();

            var model = new EditRoleVM
            {
                Id = role.Id,
                RoleName = role.Name
            };

            return View(model);
        }

        // Edit Role - POST
        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleVM model)
        {
            if (!ModelState.IsValid) return View(model);

            var role = await _RoleManager.FindByIdAsync(model.Id);
            if (role == null) return NotFound();

            role.Name = model.RoleName;
            var result = await _RoleManager.UpdateAsync(role);

            if (result.Succeeded)
            {
                TempData["Message"] = "Role updated successfully.";
                return RedirectToAction("Roles");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(model);
        }

        // Delete Role - POST
        [HttpPost]
        public async Task<IActionResult> DeleteRole(string roleId)
        {
            var role = await _RoleManager.FindByIdAsync(roleId);
            if (role == null) return NotFound();

            var result = await _RoleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                TempData["Message"] = "Role deleted successfully.";
            }
            else
            {
                TempData["Error"] = "Error deleting role.";
            }

            return RedirectToAction("Roles");
        }

    }
}
