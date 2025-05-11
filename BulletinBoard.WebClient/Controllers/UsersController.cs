using BulletinBoard.WebClient.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BulletinBoard.WebClient.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> Profile()
        {
            var userId = Guid.Parse("D2B23AD3-BD8B-4CA6-AA22-B8E5B3C47CF0");


            var result = await _userService.GetByIdAsync(userId);

            if (!result.IsSuccess)
            {
                TempData["Error"] = result.ErrorMessage ?? "Unable to load user profile.";
                return RedirectToAction("Index", "Posts");
            }

            return View(result.Value);
        }
    }
}
