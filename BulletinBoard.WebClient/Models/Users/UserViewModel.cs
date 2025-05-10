namespace BulletinBoard.WebClient.Models.Users;

public class UserViewModel
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}