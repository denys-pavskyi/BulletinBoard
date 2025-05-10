namespace BulletinBoard.WebClient.Models.Announcements;

public class AnnouncementViewModel
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }

    public string SubcategoryName { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
}