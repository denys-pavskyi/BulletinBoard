namespace BulletinBoard.BLL.Models.DtoModels;

public class PostDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public int SubcategoryId { get; set; }
    public string SubcategoryName { get; set; } = string.Empty;
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = string.Empty;
    public Guid UserId { get; set; }
}