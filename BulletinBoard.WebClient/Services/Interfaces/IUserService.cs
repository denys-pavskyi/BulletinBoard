using BulletinBoard.WebClient.Models.Other;
using BulletinBoard.WebClient.Models.Users;

namespace BulletinBoard.WebClient.Services.Interfaces;

public interface IUserService
{
    Task<Result<UserViewModel>> GetByIdAsync(Guid userId);
}