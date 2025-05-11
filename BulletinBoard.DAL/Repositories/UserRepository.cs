using BulletinBoard.DAL.Configurations;
using BulletinBoard.DAL.Entities;
using BulletinBoard.DAL.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace BulletinBoard.DAL.Repositories;

public class UserRepository: IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        var param = new SqlParameter("@Id", id);

        var users = await _context.Users
            .FromSqlRaw("EXEC GetUserById @Id", param)
            .ToListAsync();

        return users.FirstOrDefault();
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        var emailParam = new SqlParameter("@Email", email);
        return await _context.Users
            .FromSqlRaw("EXEC spUser_GetByEmail @Email", emailParam)
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }

    public async Task AddAsync(User user)
    {
        var parameters = new[]
        {
            new SqlParameter("@Id", user.Id),
            new SqlParameter("@Username", user.Username),
            new SqlParameter("@Email", user.Email),
            new SqlParameter("@PasswordHash", user.PasswordHash),
            new SqlParameter("@Provider", user.Provider),
            new SqlParameter("@CreatedAt", user.CreatedAt)
        };

        await _context.Database.ExecuteSqlRawAsync("EXEC spUser_Create @Id, @Username, @Email, @PasswordHash, @Provider, @CreatedAt", parameters);
    }


}