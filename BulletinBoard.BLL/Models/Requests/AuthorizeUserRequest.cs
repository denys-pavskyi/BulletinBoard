namespace BulletinBoard.BLL.Models.Requests;

public class AuthorizeUserRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}