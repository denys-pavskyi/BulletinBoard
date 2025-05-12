using BulletinBoard.BLL.Models.DtoModels;
using BulletinBoard.BLL.Models.Requests;
using BulletinBoard.BLL.Other;

namespace BulletinBoard.BLL.Interfaces;

public interface IUserService
{
    Task<Result<UserDto>> GetByIdAsync(Guid id);
    Task<Result<UserDto>> RegisterAsync(RegisterUserRequest request);
    Task<Result<AuthDto>> AuthenticateAsync(LoginRequestDto request);
}