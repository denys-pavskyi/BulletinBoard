namespace BulletinBoard.WebClient.Models.Posts;

public class CreatePostFormModel
{
    public string? Title { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public int SubcategoryId { get; set; }
    public Guid UserId { get; set; }
}