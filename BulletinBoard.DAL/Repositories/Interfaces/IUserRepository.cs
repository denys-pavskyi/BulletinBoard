using BulletinBoard.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace BulletinBoard.DAL.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id);
    Task RegisterUserAsync(User user);
    Task<User?> AuthorizeUserAsync(string username, string password);
}