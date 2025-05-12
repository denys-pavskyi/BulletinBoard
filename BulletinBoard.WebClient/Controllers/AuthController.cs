using BulletinBoard.WebClient.Models.Auth;
using BulletinBoard.WebClient.Services.Interfaces;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using LoginRequest = BulletinBoard.WebClient.Models.Auth.LoginRequest;


namespace BulletinBoard.WebClient.Controllers
{
    public class AuthController : Controller
    {
        private readonly IApiService _apiService;

        public AuthController(IApiService apiService)
        {
            _apiService = apiService;
        }

        [HttpGet]
        public IActionResult Logout()
        {

            Response.Cookies.Delete("jwt");
            Response.Cookies.Delete("uid");
            Response.Cookies.Delete("username");

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

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var registerRequest = new RegisterUserRequest
            {
                Username = model.Username,
                Email = model.Email,
                Password = model.Password
            };

            var response = await _apiService.RegisterAsync(registerRequest);

            if (!response.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, response.ErrorMessage ?? "Registration failed");
                return View(model);
            }

            return RedirectToAction("Login", "Auth");
        }

    }
}
