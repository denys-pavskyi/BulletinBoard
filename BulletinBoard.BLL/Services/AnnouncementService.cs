using AutoMapper;
using Azure.Core;
using BulletinBoard.BLL.Interfaces;
using BulletinBoard.BLL.Models.DtoModels;
using BulletinBoard.BLL.Models.Requests;
using BulletinBoard.BLL.Other;
using BulletinBoard.BLL.Validations.Announcements;
using BulletinBoard.DAL.Entities;
using BulletinBoard.DAL.Repositories;
using BulletinBoard.DAL.Repositories.Interfaces;
using FluentValidation;
using System.Net;

namespace BulletinBoard.BLL.Services;

public class AnnouncementService: IAnnouncementService
{
    private readonly IAnnouncementRepository _announcementRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    private readonly IAnnouncementCollectionValidators _announcementCollectionValidators;

    public AnnouncementService(IAnnouncementRepository announcementRepository, IMapper mapper, 
        IAnnouncementCollectionValidators announcementCollectionValidators, IUserRepository userRepository)
    {
        _announcementRepository = announcementRepository;
        _mapper = mapper;
        _announcementCollectionValidators = announcementCollectionValidators;
        _userRepository = userRepository;
    }

    public async Task<List<AnnouncementDto>> GetAllAsync()
    {
        var announcements = await _announcementRepository.GetAllAsync();

        return _mapper.Map<List<AnnouncementDto>>(announcements);
    }


    public async Task<Result<Guid>> AddAsync(CreateNewAnnouncementRequest request)
    {
        var validationResult = await _announcementCollectionValidators
            .CreateNewAnnouncementRequestValidator
            .ValidateAsync(request);

        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user is null)
        {
            return Result<Guid>.Failure(new ErrorResponse
            {
                Message = $"User with id {request.UserId.ToString()} was not found.",
                HttpCode = HttpStatusCode.NotFound
            });
        }

        if (!validationResult.IsValid)
        {
            var errorMessages = validationResult.Errors
                .Select(e => e.ErrorMessage)
                .ToList();

            return Result<Guid>.Failure(new ErrorResponse
            {
                Message = $"Validation failed",
                HttpCode = HttpStatusCode.BadRequest,
                Errors = errorMessages
            });
        }


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

        var validationResult = await _announcementCollectionValidators
            .UpdateAnnouncementRequestValidator
            .ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            var errorMessages = validationResult.Errors
                .Select(e => e.ErrorMessage)
                .ToList();

            return Result.Failure(new ErrorResponse
            {
                Message = $"Validation failed",
                HttpCode = HttpStatusCode.BadRequest,
                Errors = errorMessages
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

    public async Task<Result<List<AnnouncementDto>>> GetAllByUserIdAsync(Guid userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);

        if (user is null)
        {
            return Result<List<AnnouncementDto>>.Failure(new ErrorResponse
            {
                Message = $"User with id {userId} was not found.",
                HttpCode = HttpStatusCode.NotFound
            });
        }

        var announcements = await _announcementRepository.GetAllAnnouncementsByUserIdAsync(userId);
        var dto = _mapper.Map<List<AnnouncementDto>>(announcements);

        return Result<List<AnnouncementDto>>.Success(dto);
    }


    public async Task<Result<List<AnnouncementDto>>> GetFilteredAsync(List<int> subcategoryIds, bool isActive)
    {
        if (subcategoryIds.Count > 100)
        {
            return Result<List<AnnouncementDto>>.Failure(new ErrorResponse
            {
                Message = "Too much categories was specified",
                HttpCode = HttpStatusCode.BadRequest
            });
        }

        var csvSubcategoryIds = string.Join(',', subcategoryIds);

        var announcements = await _announcementRepository
            .GetAllAnnouncementsByFilterAsync(csvSubcategoryIds, isActive);

        return Result<List<AnnouncementDto>>.Success(_mapper.Map<List<AnnouncementDto>>(announcements));
    }


}