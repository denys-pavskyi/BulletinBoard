using AutoMapper;
using Azure.Core;
using BulletinBoard.BLL.Interfaces;
using BulletinBoard.BLL.Models.DtoModels;
using BulletinBoard.BLL.Models.Requests;
using BulletinBoard.BLL.Other;
using BulletinBoard.DAL.Entities;
using BulletinBoard.DAL.Repositories.Interfaces;
using System.Net;

namespace BulletinBoard.BLL.Services;

public class AnnouncementService: IAnnouncementService
{
    private readonly IAnnouncementRepository _announcementRepository;
    private readonly IMapper _mapper;

    public AnnouncementService(IAnnouncementRepository announcementRepository, IMapper mapper)
    {
        _announcementRepository = announcementRepository;
        _mapper = mapper;
    }

    public async Task<List<AnnouncementDto>> GetAllAsync()
    {
        var announcements = await _announcementRepository.GetAllAsync();

        return _mapper.Map<List<AnnouncementDto>>(announcements);
    }


    public async Task<Result<Guid>> AddAsync(CreateNewAnnouncementRequest request)
    {
        var announcement = _mapper.Map<Announcement>(request);

        await _announcementRepository.AddAsync(announcement);

        return Result<Guid>.Success(announcement.Id);
    }


    public async Task<Result<AnnouncementDto>> GetByIdAsync(Guid id)
    {
        var announcement = await _announcementRepository.GetByIdAsync(id);

        var dto = _mapper.Map<AnnouncementDto>(announcement);
        return Result<AnnouncementDto>.Success(dto);
    }

    public async Task<Result> UpdateAsync(Guid id, UpdateAnnouncementRequest request)
    {
        var existing = await _announcementRepository.GetByIdAsync(id);
        if (existing is null)
        {
            return Result.Failure(new ErrorResponse
            {
                Message = "Announcement not found",
                HttpCode = HttpStatusCode.NotFound
            });
        }
        
        existing.Title = request.Title;
        existing.Description = request.Description;
        existing.IsActive = request.IsActive;
        existing.SubcategoryId = request.SubcategoryId;

        await _announcementRepository.UpdateAsync(existing);

        return Result.Success();
    }

    public async Task<Result> DeleteByIdAsync(Guid id)
    {
        var announcement = await _announcementRepository.GetByIdAsync(id);

        if (announcement is null)
        {
            return Result.Failure(new ErrorResponse
            {
                Message = $"Announcement with ID = {id} not found",
                HttpCode = HttpStatusCode.NotFound
            });
        }
            

        await _announcementRepository.DeleteByIdAsync(id);

        return Result.Success();
    }
}