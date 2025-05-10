using BulletinBoard.DAL.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace BulletinBoard.DAL.Repositories.Interfaces;

public interface IPostRepository
{
    Task<List<Post>> GetAllAsync();
    Task AddAsync(Post post);
    Task DeleteByIdAsync(Guid id);
    Task<Post?> GetByIdAsync(Guid id);
    Task UpdateAsync(Post post);
    Task<List<Post>> GetAllPostsByUserIdAsync(Guid userId);
    Task<List<Post>> GetAllPostsByFilterAsync(string subcategoryIds, bool isActive);
}