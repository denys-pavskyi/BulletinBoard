namespace BulletinBoard.WebClient.Models.Posts;

public class FilterRequestDto
{
    public List<int> SubcategoryIds { get; set; } = new();
    public bool IsActive { get; set; }
}