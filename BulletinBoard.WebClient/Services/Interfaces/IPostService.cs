using BulletinBoard.WebClient.Models.Posts;

namespace BulletinBoard.WebClient.Services.Interfaces;

public interface IPostService
{
    Task<List<PostViewModel>> GetFilteredAsync(List<int> subcategoryIds, bool isActive);
    Task<List<PostViewModel>> GetPostsByUserIdAsync(Guid userId);

}