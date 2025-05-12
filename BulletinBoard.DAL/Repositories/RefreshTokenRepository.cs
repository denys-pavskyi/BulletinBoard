using BulletinBoard.DAL.Configurations;
using BulletinBoard.DAL.Entities;
using BulletinBoard.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BulletinBoard.DAL.Repositories;

public class RefreshTokenRepository: IRefreshTokenRepository
{
    private readonly AppDbContext _context;

    public RefreshTokenRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(RefreshToken token)
    {
        await _context.Database.ExecuteSqlAsync($@"
            EXEC CreateRefreshToken 
                @Id = {token.Id}, 
                @UserId = {token.UserId}, 
                @Token = {token.Token}, 
                @ExpiresAt = {token.ExpiresAt}");
    }

    public async Task<RefreshToken?> GetByTokenAsync(string token)
    {
        var refreshTokens = await _context.RefreshTokens
            .FromSql($@"
                EXEC GetRefreshTokenByToken 
                    @Token = {token}")
            .ToListAsync();

        var refreshToken = refreshTokens.FirstOrDefault();

        if (refreshToken is null) return null;

            await _context.Entry(refreshToken)
                .Reference(rt => rt.User)
                .Query()
                .LoadAsync();

        return refreshToken;
    }

    public async Task UpdateTokenAsync(string oldToken, string newToken, DateTime newExpiresAt)
    {
        await _context.Database.ExecuteSqlAsync($@"
            EXEC UpdateRefreshToken 
                @OldToken = {oldToken}, 
                @NewToken = {newToken}, 
                @NewExpiresAt = {newExpiresAt}");
    }

    public async Task DeleteAsync(string token)
    {
        await _context.Database.ExecuteSqlAsync($@"
            EXEC DeleteRefreshToken 
                @Token = {token}");
    }

}