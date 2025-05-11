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

    public async Task RegisterUserAsync(User user)
    {
        await _context.Database.ExecuteSqlAsync($@"
        EXEC RegisterUser 
            @Id = {user.Id}, 
            @Username = {user.Username}, 
            @Email = {user.Email}, 
            @Password = {user.PasswordHash}");
    }

    public async Task<User?> AuthorizeUserAsync(string username, string password)
    {
        var users = await _context.Users
            .FromSql($@"
            EXEC AuthorizeUser 
                @Username = {username}, 
                @Password = {password}")
            .ToListAsync();

        return users.FirstOrDefault();
    }


}