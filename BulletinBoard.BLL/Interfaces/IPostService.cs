using BulletinBoard.BLL.Models.DtoModels;
using BulletinBoard.BLL.Models.Requests;
using BulletinBoard.BLL.Other;

namespace BulletinBoard.BLL.Interfaces;

public interface IPostService
{
    Task<List<PostDto>> GetAllAsync();
    Task<Result<Guid>> AddAsync(CreateNewPostRequest request);
    Task<Result<PostDto>> GetByIdAsync(Guid id);
    Task<Result> UpdateAsync(Guid id, UpdatePostRequest request);
    Task<Result> DeleteByIdAsync(Guid id);
    Task<Result<List<PostDto>>> GetAllByUserIdAsync(Guid userId);
    Task<Result<List<PostDto>>> GetFilteredAsync(List<int> subcategoryIds, bool isActive);
}