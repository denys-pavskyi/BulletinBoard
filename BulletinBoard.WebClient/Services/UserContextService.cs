using BulletinBoard.WebClient.Services.Interfaces;

namespace BulletinBoard.WebClient.Services;

public class UserContextService: IUserContextService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContextService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public bool IsAuthenticated =>
        _httpContextAccessor.HttpContext?.Request.Cookies.ContainsKey("jwt") == true;

    public Guid? UserId
    {
        get
        {
            var uid = _httpContextAccessor.HttpContext?.Request.Cookies["uid"];
            return Guid.TryParse(uid, out var guid) ? guid : null;
        }
    }

    public string? Username =>
        _httpContextAccessor.HttpContext?.Request.Cookies["username"];

    public string? JwtToken =>
        _httpContextAccessor.HttpContext?.Request.Cookies["jwt"];

    public void SetJwtToken(string token)
    {
        if (_httpContextAccessor.HttpContext?.Response != null)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddHours(1)
            };

            _httpContextAccessor.HttpContext.Response.Cookies.Append("jwt", token, cookieOptions);
        }
    }

    public void RemoveJwtToken()
    {
        if (_httpContextAccessor.HttpContext?.Response != null)
        {
            _httpContextAccessor.HttpContext.Response.Cookies.Delete("jwt");
        }
    }
}