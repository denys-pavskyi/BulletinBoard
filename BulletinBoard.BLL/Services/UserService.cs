using AutoMapper;
using BulletinBoard.BLL.Interfaces;
using BulletinBoard.BLL.Models.DtoModels;
using BulletinBoard.BLL.Other;
using BulletinBoard.DAL.Entities;
using BulletinBoard.DAL.Repositories.Interfaces;
using System.Net;
using BulletinBoard.BLL.Models.Requests;
using BulletinBoard.BLL.Other.Hashers;
using BulletinBoard.DAL.Repositories;

namespace BulletinBoard.BLL.Services;

public class UserService: IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public UserService(IUserRepository userRepository, IMapper mapper, 
        IPasswordHasher passwordHasher, ITokenService tokenService, 
        IRefreshTokenRepository refreshTokenRepository)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
        _refreshTokenRepository = refreshTokenRepository;
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

    public async Task<Result<UserDto>> RegisterAsync(RegisterUserRequest request)
    {

        var existingUser = await _userRepository.GetByEmailAsync(request.Email);
        if (existingUser != null)
        {
            return Result<UserDto>.Failure(new ErrorResponse
            {
                Message = "User with this email already exists.",
                HttpCode = HttpStatusCode.BadRequest
            });
        }


        var hashedPassword = _passwordHasher.HashPassword(request.Password);

        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = request.Username,
            Email = request.Email,
            PasswordHash = hashedPassword,
            Provider = "Local",
            CreatedAt = DateTime.UtcNow
        };

        await _userRepository.AddAsync(user);

        var userDto = _mapper.Map<UserDto>(user);

        return Result<UserDto>.Success(userDto);
    }


    public async Task<Result<AuthDto>> AuthenticateAsync(LoginRequestDto request)
    {

        var user = await _userRepository.GetByEmailAsync(request.Email);
        if (user == null)
        {
            return Result<AuthDto>.Failure(new ErrorResponse
            {
                Message = "Invalid credentials.",
                HttpCode = HttpStatusCode.Unauthorized
            });
        }

        var isPasswordValid = _passwordHasher.VerifyPassword(user.PasswordHash, request.Password);
        if (!isPasswordValid)
        {
            return Result<AuthDto>.Failure(new ErrorResponse
            {
                Message = "Invalid credentials.",
                HttpCode = HttpStatusCode.Unauthorized
            });
        }


        var accessToken = _tokenService.GenerateAccessToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken();
        var expiresAt = DateTime.UtcNow.AddDays(7);


        await _refreshTokenRepository.AddAsync(new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Token = refreshToken,
            ExpiresAt = expiresAt
        });

        return Result<AuthDto>.Success(new AuthDto
        {
            Id = user.Id,
            Username = user.Username,
            Token = accessToken
        });
    }

}