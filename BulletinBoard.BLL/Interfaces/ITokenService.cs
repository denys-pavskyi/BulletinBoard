using BulletinBoard.DAL.Entities;

namespace BulletinBoard.BLL.Interfaces;

public interface ITokenService
{
    string GenerateToken(User user);
}