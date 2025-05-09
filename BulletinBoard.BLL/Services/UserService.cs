using AutoMapper;
using BulletinBoard.BLL.Interfaces;
using BulletinBoard.BLL.Models.DtoModels;
using BulletinBoard.BLL.Other;
using BulletinBoard.DAL.Entities;
using BulletinBoard.DAL.Repositories.Interfaces;
using System.Net;
using BulletinBoard.BLL.Models.Requests;

namespace BulletinBoard.BLL.Services;

public class UserService: IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<Result<UserDto>> GetByIdAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user is null)
        {
            return Result<UserDto>.Failure(new ErrorResponse
            {
                Message = "User not found.",
                HttpCode = HttpStatusCode.NotFound
            });
        }

        var userDto = _mapper.Map<UserDto>(user);
        return Result<UserDto>.Success(userDto);
    }

    public async Task<Result> RegisterUserAsync(RegisterUserRequest request)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = request.Username,
            Email = request.Email,
            Password = request.Password
        };

        await _userRepository.RegisterUserAsync(user);

        return Result.Success();
    }

    public async Task<Result<UserDto>> AuthorizeUserAsync(AuthorizeUserRequest request)
    {
        var user = await _userRepository.AuthorizeUserAsync(request.Username, request.Password);

        if (user is null)
        {
            return Result<UserDto>.Failure(new ErrorResponse
            {
                Message = "Invalid username or password.",
                HttpCode = HttpStatusCode.Forbidden
            });
        }

        var userDto = _mapper.Map<UserDto>(user);

        return Result<UserDto>.Success(userDto);
    }

}