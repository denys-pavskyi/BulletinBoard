using BulletinBoard.BLL.Interfaces;
using BulletinBoard.BLL.Models.Requests;
using BulletinBoard.BLL.Other;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BulletinBoard.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
        {
            var result = await _userService.RegisterUserAsync(request);

            return result.Match(
                NoContent,
                error => error.ToActionResult()
            );
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Login([FromBody] AuthorizeUserRequest request)
        {
            var result = await _userService.AuthorizeUserAsync(request);
            return result.Match(
                Ok,
                error => error.ToActionResult()
            );
        }


    }
}
