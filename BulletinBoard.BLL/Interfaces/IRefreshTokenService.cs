using BulletinBoard.BLL.Models.DtoModels;
using BulletinBoard.BLL.Other;

namespace BulletinBoard.BLL.Interfaces;

public interface IRefreshTokenService
{
    Task<Result<AuthDto>> RefreshTokenAsync(string oldToken);
}