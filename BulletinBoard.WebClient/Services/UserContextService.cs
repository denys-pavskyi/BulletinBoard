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
}