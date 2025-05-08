using BulletinBoard.DAL.Configurations;
using BulletinBoard.DAL.Entities;
using BulletinBoard.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BulletinBoard.DAL.Repositories;

public class AnnouncementRepository: IAnnouncementRepository
{
    private readonly AppDbContext _context;

    public AnnouncementRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Announcement>> GetAllAsync()
    {
        return await _context.Announcements
            .FromSql($"EXEC GetAllAnnouncements")
            .ToListAsync();
    }

}