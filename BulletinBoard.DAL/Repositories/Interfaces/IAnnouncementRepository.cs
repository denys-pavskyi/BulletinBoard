using BulletinBoard.DAL.Entities;

namespace BulletinBoard.DAL.Repositories.Interfaces;

public interface IAnnouncementRepository
{
    Task<List<Announcement>> GetAllAsync();
    Task AddAsync(Announcement announcement);
    Task DeleteByIdAsync(int id);
    Task<Announcement?> GetByIdAsync(Guid id);
    Task UpdateAsync(Announcement announcement);
}