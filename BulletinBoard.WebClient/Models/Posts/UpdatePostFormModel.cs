namespace BulletinBoard.WebClient.Models.Posts;

public class UpdatePostFormModel
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public int SubcategoryId { get; set; }
}