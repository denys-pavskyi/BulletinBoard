namespace BulletinBoard.BLL.Models.Requests;

public class UpdateAnnouncementRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public int SubcategoryId { get; set; }
}