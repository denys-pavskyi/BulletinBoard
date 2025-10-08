using BulletinBoard.DAL.Entities;

namespace BulletinBoard.BLL.Other.Hashers;

public class PasswordHasher: IPasswordHasher
{
    private readonly Microsoft.AspNetCore.Identity.IPasswordHasher<User> _passwordHasher;

    public PasswordHasher()
    {
        _passwordHasher = new Microsoft.AspNetCore.Identity.PasswordHasher<User>();
    }

    public string HashPassword(string password)
    {
        return _passwordHasher.HashPassword(null, password);
    }

    public bool VerifyPassword(string hashedPassword, string password)
    {
        var result = _passwordHasher.VerifyHashedPassword(null, hashedPassword, password);
        return result == Microsoft.AspNetCore.Identity.PasswordVerificationResult.Success;
    }
}