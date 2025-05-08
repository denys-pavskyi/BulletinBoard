using BulletinBoard.DAL.Entities;

namespace BulletinBoard.DAL.Repositories.Interfaces;

public interface IAnnouncementRepository
{
    Task<List<Announcement>> GetAllAsync();
}