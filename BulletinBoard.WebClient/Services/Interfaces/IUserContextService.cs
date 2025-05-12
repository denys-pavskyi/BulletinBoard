namespace BulletinBoard.WebClient.Services.Interfaces;

public interface IUserContextService
{
    bool IsAuthenticated { get; }
    Guid? UserId { get; }
    string? Username { get; }

    string? JwtToken { get; }

    void SetJwtToken(string token);

    void RemoveJwtToken();
}