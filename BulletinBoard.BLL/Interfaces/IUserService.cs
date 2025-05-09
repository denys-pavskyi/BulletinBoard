using BulletinBoard.BLL.Models.DtoModels;
using BulletinBoard.BLL.Models.Requests;
using BulletinBoard.BLL.Other;
using System.Threading.Tasks;

namespace BulletinBoard.BLL.Interfaces;

public interface IUserService
{
    Task<Result<UserDto>> GetByIdAsync(Guid id);
    Task<Result> RegisterUserAsync(RegisterUserRequest request);
    Task<Result<UserDto>> AuthorizeUserAsync(AuthorizeUserRequest request);
}