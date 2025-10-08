using BulletinBoard.DAL.Entities;

namespace BulletinBoard.DAL.Repositories.Interfaces;

public interface IRefreshTokenRepository
{
    Task AddAsync(RefreshToken token);
    Task<RefreshToken?> GetByTokenAsync(string token);
    Task UpdateTokenAsync(string oldToken, string newToken, DateTime newExpiresAt);
    Task DeleteAsync(string token);
}