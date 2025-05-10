using BulletinBoard.WebClient.Models;

namespace BulletinBoard.WebClient.Services.Interfaces;

public interface IAnnouncementService
{
    Task<List<AnnouncementViewModel>> GetAllAsync();
}