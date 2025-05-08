using BulletinBoard.BLL.Models.DtoModels;

namespace BulletinBoard.BLL.Interfaces;

public interface IAnnouncementService
{
    Task<List<AnnouncementDto>> GetAllAsync();
}