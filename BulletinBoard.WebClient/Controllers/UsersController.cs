using Microsoft.AspNetCore.Mvc;

namespace BulletinBoard.WebClient.Controllers
{
    public class UsersController : Controller
    {
        public IActionResult Profile()
        {
            // TODO: get data of user from api call
            return View();
        }
    }
}
