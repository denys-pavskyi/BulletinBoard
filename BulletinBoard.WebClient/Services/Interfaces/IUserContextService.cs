namespace BulletinBoard.WebClient.Services.Interfaces;

public interface IUserContextService
{
    bool IsAuthenticated { get; }
    Guid? UserId { get; }
    string? Username { get; }
}