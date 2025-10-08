namespace BulletinBoard.BLL.Other.Hashers;

public interface IPasswordHasher
{
    string HashPassword(string password);
    bool VerifyPassword(string hashedPassword, string password);
}