using BulletinBoard.WebClient.Models.Announcements;

namespace BulletinBoard.WebClient.Services.Interfaces;

public interface IAnnouncementService
{
    Task<List<AnnouncementViewModel>> GetFilteredAsync(List<int> subcategoryIds, bool isActive);
}