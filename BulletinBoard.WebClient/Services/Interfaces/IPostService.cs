using BulletinBoard.WebClient.Models.Other;
using BulletinBoard.WebClient.Models.Posts;

namespace BulletinBoard.WebClient.Services.Interfaces;

public interface IPostService
{
    Task<Result<List<PostViewModel>>> GetFilteredAsync(List<int> subcategoryIds, bool isActive);
    Task<Result<List<PostViewModel>>> GetPostsByUserIdAsync(Guid userId);
    Task<Result<PostViewModel?>> GetByIdAsync(Guid postId);
    Task<Result> UpdateAsync(Guid id, UpdatePostRequest request);
    Task<Result> DeleteAsync(Guid postId);
    Task<Result> AddAsync(CreatePostFormModel request);
}