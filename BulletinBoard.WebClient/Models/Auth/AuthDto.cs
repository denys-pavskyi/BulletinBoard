namespace BulletinBoard.WebClient.Models.Auth;

public class AuthDto
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
}