using BulletinBoard.DAL.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace BulletinBoard.DAL.Repositories.Interfaces;

public interface IAnnouncementRepository
{
    Task<List<Announcement>> GetAllAsync();
    Task AddAsync(Announcement announcement);
    Task DeleteByIdAsync(Guid id);
    Task<Announcement?> GetByIdAsync(Guid id);
    Task UpdateAsync(Announcement announcement);
    Task<List<Announcement>> GetAllAnnouncementsByUserIdAsync(Guid userId);
    Task<List<Announcement>> GetAllAnnouncementsByFilterAsync(string subcategoryIds, bool isActive);
}