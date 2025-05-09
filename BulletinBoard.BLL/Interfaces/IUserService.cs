using BulletinBoard.BLL.Models.DtoModels;
using BulletinBoard.BLL.Other;

namespace BulletinBoard.BLL.Interfaces;

public interface IUserService
{
    Task<Result<UserDto>> GetByIdAsync(Guid id);
}