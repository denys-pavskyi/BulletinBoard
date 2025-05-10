using BulletinBoard.WebClient.Models.Users;

namespace BulletinBoard.WebClient.Services.Interfaces;

public interface IUserService
{
    Task<UserViewModel?> GetByIdAsync(Guid id);
}