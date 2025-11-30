using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Estra7a.Web.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize]
    public class ChatController : Controller
    {
        public IActionResult Admin()
        {
            return View();
        }
    }
}
