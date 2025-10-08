using BulletinBoard.DAL.Entities;

namespace BulletinBoard.BLL.Interfaces;

public interface ITokenService
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken();
}