using BulletinBoard.WebClient.Models.Auth;
using BulletinBoard.WebClient.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BulletinBoard.WebClient.Controllers
{
    public class AuthController : Controller
    {
        private readonly IApiService _apiService;

        public AuthController(IApiService apiService)
        {
            _apiService = apiService;
        }

        public IActionResult Logout()
        {
            return RedirectToAction("Index", "Posts");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var request = new LoginRequest
            {
                Email = model.Email,
                Password = model.Password
            };

            var response = await _apiService.LoginAsync(request);

            if (!response.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, response.ErrorMessage ?? "Login failed");
                return View(model);
            }

            var authData = response.Value!;

            Response.Cookies.Append("jwt", authData.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddHours(1)
            });

            Response.Cookies.Append("uid", authData.Id.ToString(), new CookieOptions
            {
                HttpOnly = false,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddHours(1)
            });

            Response.Cookies.Append("username", authData.Username, new CookieOptions
            {
                HttpOnly = false,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddHours(1)
            });

            return RedirectToAction("Index", "Posts");
        }
    }
}
