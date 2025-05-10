namespace BulletinBoard.BLL.Models.Requests;

public class PostFilterRequest
{
    public List<int> SubcategoryIds { get; set; } = new();
    public bool IsActive { get; set; }
}