using BulletinBoard.BLL.Interfaces;
using BulletinBoard.BLL.Models.DtoModels;
using BulletinBoard.BLL.Other;
using BulletinBoard.DAL.Repositories.Interfaces;
using System.Net;

namespace BulletinBoard.BLL.Services;

public class RefreshTokenService: IRefreshTokenService
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly ITokenService _tokenService;
    private readonly IUserRepository _userRepository;

    public RefreshTokenService(IRefreshTokenRepository refreshTokenRepository, ITokenService tokenService, 
        IUserRepository userRepository)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _tokenService = tokenService;
        _userRepository = userRepository;
    }

    public async Task<Result<AuthDto>> RefreshTokenAsync(string oldToken)
    {
        var refreshToken = await _refreshTokenRepository.GetByTokenAsync(oldToken);
        if (refreshToken is null || refreshToken.ExpiresAt < DateTime.UtcNow)
        {
            return Result<AuthDto>.Failure(new ErrorResponse
            {
                Message = "Invalid or expired refresh token.",
                HttpCode = HttpStatusCode.Unauthorized
            });
        }

        var user = await _userRepository.GetByIdAsync(refreshToken.UserId);
        if (user is null)
        {
            return Result<AuthDto>.Failure(new ErrorResponse
            {
                Message = "User not found.",
                HttpCode = HttpStatusCode.NotFound
            });
        }

        var newAccessToken = _tokenService.GenerateAccessToken(user);
        var newRefreshToken = _tokenService.GenerateRefreshToken();
        var newExpiresAt = DateTime.UtcNow.AddDays(7);

        await _refreshTokenRepository.UpdateTokenAsync(oldToken, newRefreshToken, newExpiresAt);

        var dto = new AuthDto
        {
            Id = user.Id,
            Username = user.Username,
            Token = newAccessToken
        };

        return Result<AuthDto>.Success(dto);
    }

}