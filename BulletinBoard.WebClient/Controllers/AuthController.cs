using Microsoft.AspNetCore.Mvc;

namespace BulletinBoard.WebClient.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Logout()
        {
            return RedirectToAction("Index", "Announcements");
        }
    }
}
