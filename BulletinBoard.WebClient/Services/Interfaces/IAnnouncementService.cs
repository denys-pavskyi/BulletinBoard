using BulletinBoard.WebClient.Models.Announcements;

namespace BulletinBoard.WebClient.Services.Interfaces;

public interface IAnnouncementService
{
    Task<List<AnnouncementViewModel>> GetAllAsync();
}