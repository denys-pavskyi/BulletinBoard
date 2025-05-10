using AutoMapper;
using Azure.Core;
using BulletinBoard.BLL.Interfaces;
using BulletinBoard.BLL.Models.DtoModels;
using BulletinBoard.BLL.Models.Requests;
using BulletinBoard.BLL.Other;
using BulletinBoard.BLL.Validations.Posts;
using BulletinBoard.DAL.Entities;
using BulletinBoard.DAL.Repositories;
using BulletinBoard.DAL.Repositories.Interfaces;
using FluentValidation;
using System.Net;

namespace BulletinBoard.BLL.Services;

public class PostService: IPostService
{
    private readonly IPostRepository _postRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    private readonly IPostCollectionValidators _postCollectionValidators;

    public PostService(IPostRepository postRepository, IMapper mapper, 
        IPostCollectionValidators postCollectionValidators, IUserRepository userRepository)
    {
        _postRepository = postRepository;
        _mapper = mapper;
        _postCollectionValidators = postCollectionValidators;
        _userRepository = userRepository;
    }

    public async Task<List<PostDto>> GetAllAsync()
    {
        var posts = await _postRepository.GetAllAsync();

        return _mapper.Map<List<PostDto>>(posts);
    }


    public async Task<Result<Guid>> AddAsync(CreateNewPostRequest request)
    {
        var validationResult = await _postCollectionValidators
            .CreateNewPostRequestValidator
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


        var post = _mapper.Map<Post>(request);

        await _postRepository.AddAsync(post);

        return Result<Guid>.Success(post.Id);
    }


    public async Task<Result<PostDto>> GetByIdAsync(Guid id)
    {
        var post = await _postRepository.GetByIdAsync(id);

        var dto = _mapper.Map<PostDto>(post);
        return Result<PostDto>.Success(dto);
    }

    public async Task<Result> UpdateAsync(Guid id, UpdatePostRequest request)
    {
        var existing = await _postRepository.GetByIdAsync(id);
        if (existing is null)
        {
            return Result.Failure(new ErrorResponse
            {
                Message = "Post not found",
                HttpCode = HttpStatusCode.NotFound
            });
        }

        var validationResult = await _postCollectionValidators
            .UpdatePostRequestValidator
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

        await _postRepository.UpdateAsync(existing);

        return Result.Success();
    }

    public async Task<Result> DeleteByIdAsync(Guid id)
    {
        var post = await _postRepository.GetByIdAsync(id);

        if (post is null)
        {
            return Result.Failure(new ErrorResponse
            {
                Message = $"Post with ID = {id} not found",
                HttpCode = HttpStatusCode.NotFound
            });
        }
            

        await _postRepository.DeleteByIdAsync(id);

        return Result.Success();
    }

    public async Task<Result<List<PostDto>>> GetAllByUserIdAsync(Guid userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);

        if (user is null)
        {
            return Result<List<PostDto>>.Failure(new ErrorResponse
            {
                Message = $"User with id {userId} was not found.",
                HttpCode = HttpStatusCode.NotFound
            });
        }

        var posts = await _postRepository.GetAllPostsByUserIdAsync(userId);
        var dto = _mapper.Map<List<PostDto>>(posts);

        return Result<List<PostDto>>.Success(dto);
    }


    public async Task<Result<List<PostDto>>> GetFilteredAsync(List<int> subcategoryIds, bool isActive)
    {
        if (subcategoryIds.Count > 100)
        {
            return Result<List<PostDto>>.Failure(new ErrorResponse
            {
                Message = "Too much categories was specified",
                HttpCode = HttpStatusCode.BadRequest
            });
        }

        var csvSubcategoryIds = string.Join(',', subcategoryIds);

        var posts = await _postRepository
            .GetAllPostsByFilterAsync(csvSubcategoryIds, isActive);

        return Result<List<PostDto>>.Success(_mapper.Map<List<PostDto>>(posts));
    }


}