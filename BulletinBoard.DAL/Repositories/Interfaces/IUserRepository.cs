using BulletinBoard.DAL.Entities;

namespace BulletinBoard.DAL.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id);
}