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
        var users = await _context.Users
            .FromSqlRaw("EXEC GetUserByEmail @Email", emailParam)
            .ToListAsync();

        return users.FirstOrDefault();
    }

    public async Task AddAsync(User user)
    {
        await _context.Database.ExecuteSqlAsync($@"EXEC CreateUser 
                @Id = {user.Id}, 
                @Username = {user.Username}, 
                @Email = {user.Email}, 
                @PasswordHash = {user.PasswordHash}, 
                @Provider = {user.Provider}, 
                @CreatedAt = {user.CreatedAt}");
    }

}