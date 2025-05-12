namespace BulletinBoard.BLL.Models.DtoModels;

public class UserDto
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PasswordHash { get; set; }
    public string Provider { get; set; } = "Local";
}