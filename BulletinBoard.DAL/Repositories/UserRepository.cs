using BulletinBoard.DAL.Configurations;
using BulletinBoard.DAL.Repositories.Interfaces;

namespace BulletinBoard.DAL.Repositories;

public class UserRepository: IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }
}