using BulletinBoard.DAL.Configurations;
using BulletinBoard.DAL.Entities;
using BulletinBoard.DAL.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
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

    public async Task AddAsync(Announcement announcement)
    {
        var sql = "EXEC AddAnnouncement @Id, @Title, @Description, @IsActive, @CreatedDate, @SubcategoryId, @UserId";

        await _context.Database.ExecuteSqlRawAsync(sql,
            new SqlParameter("@Id", announcement.Id),
            new SqlParameter("@Title", announcement.Title),
            new SqlParameter("@Description", announcement.Description),
            new SqlParameter("@IsActive", announcement.IsActive),
            new SqlParameter("@CreatedDate", announcement.CreatedDate),
            new SqlParameter("@SubcategoryId", announcement.SubcategoryId),
            new SqlParameter("@UserId", announcement.UserId)
        );
    }


    public async Task DeleteByIdAsync(Guid id)
    {
        var sql = "EXEC DeleteAnnouncementById @Id";

        await _context.Database.ExecuteSqlRawAsync(sql,
            new SqlParameter("@Id", id)
        );
    }

    public async Task<Announcement?> GetByIdAsync(Guid id)
    {
        var announcement = _context.Announcements
            .FromSql($"EXEC GetAnnouncementById @Id={id}")
            .AsEnumerable()
            .FirstOrDefault();

        if (announcement == null)
            return null;

        await _context.Entry(announcement)
            .Reference(a => a.SubCategory)
            .Query()
            .Include(sc => sc.Category)
            .LoadAsync();

        return announcement;
    }

    public async Task UpdateAsync(Announcement announcement)
    {
        await _context.Database.ExecuteSqlInterpolatedAsync($@"
        EXEC UpdateAnnouncement 
            @Id={announcement.Id},
            @Title={announcement.Title},
            @Description={announcement.Description},
            @IsActive={announcement.IsActive},
            @SubcategoryId={announcement.SubcategoryId}");
    }

    public async Task<List<Announcement>> GetAllAnnouncementsByUserIdAsync(Guid userId)
    {
        var userIdParameter = new SqlParameter("@UserId", userId);

        var announcements = await _context.Announcements
            .FromSqlRaw("EXEC GetAllAnnouncementsByUserId @UserId", userIdParameter)
            .ToListAsync();


        return announcements;
    }

    public async Task<List<Announcement>> GetAllAnnouncementsByFilterAsync(string subcategoryIds, bool isActive)
    {
        var subcategoryIdsParameter = new SqlParameter("@SubcategoryIds", subcategoryIds);
        var isActiveParameter = new SqlParameter("@IsActive", isActive);

        var announcements = await _context.Announcements
            .FromSqlRaw("EXEC GetAllAnnouncementsByFilter @SubcategoryIds, @IsActive", subcategoryIdsParameter, isActiveParameter)
            .ToListAsync();

        return announcements;
    }

}