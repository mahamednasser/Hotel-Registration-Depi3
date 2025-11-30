using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace Estra7a.Web.Controllers
{
    public class CultureController : Controller
    {
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SetLanguage( string culture , string returnUrl )
        {
            if ( !string.IsNullOrEmpty(culture) )
            {
                Response.Cookies.Append(
                    CookieRequestCultureProvider.DefaultCookieName ,
                    CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture , culture)) , // Set both culture and UICulture
                    new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) , IsEssential = true }
                );
            }

            return LocalRedirect(returnUrl ?? "/"); // Fallback to homepage if returnUrl is null
        }
    }
}
