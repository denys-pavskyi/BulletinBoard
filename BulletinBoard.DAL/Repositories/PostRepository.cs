using BulletinBoard.DAL.Configurations;
using BulletinBoard.DAL.Entities;
using BulletinBoard.DAL.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace BulletinBoard.DAL.Repositories;

public class PostRepository: IPostRepository
{
    private readonly AppDbContext _context;

    public PostRepository(AppDbContext context)
    {
        _context = context;
    }


    public async Task<List<Post>> GetAllAsync()
    {
        return await _context.Posts
            .FromSql($"EXEC GetAllPosts")
            .ToListAsync();
    }

    public async Task AddAsync(Post post)
    {
        var sql = "EXEC AddPost @Id, @Title, @Description, @IsActive, @CreatedDate, @SubcategoryId, @UserId";

        await _context.Database.ExecuteSqlRawAsync(sql,
            new SqlParameter("@Id", post.Id),
            new SqlParameter("@Title", post.Title),
            new SqlParameter("@Description", post.Description),
            new SqlParameter("@IsActive", post.IsActive),
            new SqlParameter("@CreatedDate", post.CreatedDate),
            new SqlParameter("@SubcategoryId", post.SubcategoryId),
            new SqlParameter("@UserId", post.UserId)
        );
    }


    public async Task DeleteByIdAsync(Guid id)
    {
        var sql = "EXEC DeletePostById @Id";

        await _context.Database.ExecuteSqlRawAsync(sql,
            new SqlParameter("@Id", id)
        );
    }

    public async Task<Post?> GetByIdAsync(Guid id)
    {
        var posts = await _context.Posts
            .FromSql($"EXEC GetPostById @Id={id}")
            .ToListAsync();
            
        var post = posts.FirstOrDefault();

        if (post == null)
            return null;

        await _context.Entry(post)
            .Reference(a => a.SubCategory)
            .Query()
            .Include(sc => sc.Category)
            .LoadAsync();

        return post;
    }

    public async Task UpdateAsync(Post post)
    {
        await _context.Database.ExecuteSqlInterpolatedAsync($@"
        EXEC UpdatePost 
            @Id={post.Id},
            @Title={post.Title},
            @Description={post.Description},
            @IsActive={post.IsActive},
            @SubcategoryId={post.SubcategoryId}");
    }

    public async Task<List<Post>> GetAllPostsByUserIdAsync(Guid userId)
    {
        var userIdParameter = new SqlParameter("@UserId", userId);

        var posts = await _context.Posts
            .FromSqlRaw("EXEC GetAllPostsByUserId @UserId", userIdParameter)
            .ToListAsync();


        return posts;
    }

    public async Task<List<Post>> GetAllPostsByFilterAsync(string subcategoryIds, bool isActive)
    {
        var subcategoryIdsParameter = new SqlParameter("@SubcategoryIds", subcategoryIds);
        var isActiveParameter = new SqlParameter("@IsActive", isActive);

        var posts = await _context.Posts
            .FromSqlRaw("EXEC GetAllPostsByFilter @SubcategoryIds, @IsActive", subcategoryIdsParameter, isActiveParameter)
            .ToListAsync();

        return posts;
    }

}