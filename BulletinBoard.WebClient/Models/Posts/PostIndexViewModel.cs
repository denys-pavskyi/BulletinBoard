namespace BulletinBoard.WebClient.Models.Posts;

public class PostIndexViewModel
{
    public List<CategoryDto> Categories { get; set; } = new();
    public List<PostViewModel> Posts { get; set; } = new();
    public List<int> SelectedSubcategoryIds { get; set; } = new();
    public bool IsActive { get; set; } = true;
}