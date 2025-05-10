using BulletinBoard.WebClient.Models;

namespace BulletinBoard.WebClient.Services.Interfaces;

public interface IUserService
{
    Task<UserViewModel?> GetByIdAsync(Guid id);
}