using BulletinBoard.BLL.Models.DtoModels;
using BulletinBoard.BLL.Models.Requests;
using BulletinBoard.BLL.Other;
using BulletinBoard.DAL.Entities;

namespace BulletinBoard.BLL.Interfaces;

public interface IAnnouncementService
{
    Task<List<AnnouncementDto>> GetAllAsync();
    Task<Result<Guid>> AddAsync(CreateNewAnnouncementRequest request);
    Task<Result<AnnouncementDto>> GetByIdAsync(Guid id);
    Task<Result> UpdateAsync(Guid id, UpdateAnnouncementRequest request);
    Task<Result> DeleteByIdAsync(Guid id);
}