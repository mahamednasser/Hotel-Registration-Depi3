using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Estra7a.Web.Areas.Guest.Controllers
{
    [Area("Guest")]
    [Authorize]
    public class ChatController : Controller
    {
        public IActionResult User()
        {
            return View();
        }

    }
}
