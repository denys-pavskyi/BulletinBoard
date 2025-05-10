namespace BulletinBoard.WebClient.Models.Announcements;

public class AnnouncementIndexViewModel
{
    public List<CategoryDto> Categories { get; set; } = new();
    public List<AnnouncementViewModel> Announcements { get; set; } = new();
    public List<int> SelectedSubcategoryIds { get; set; } = new();
    public bool IsActive { get; set; } = true;
}