using BulletinBoard.WebClient.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BulletinBoard.WebClient.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly IUserContextService _userContextService;

        public UsersController(IUserService userService, IUserContextService userContextService)
        {
            _userService = userService;
            _userContextService = userContextService;
        }

        public async Task<IActionResult> Profile()
        {
            if (!_userContextService.IsAuthenticated || _userContextService.UserId is null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var userId = _userContextService.UserId ?? Guid.Empty;


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
