using BulletinBoard.WebClient.Models.Announcements;
using BulletinBoard.WebClient.Services.Interfaces;

namespace BulletinBoard.WebClient.Services;

public class AnnouncementService: IAnnouncementService
{
    private readonly HttpClient _httpClient;

    public AnnouncementService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("ApiClient");
    }

    public async Task<List<AnnouncementViewModel>> GetFilteredAsync(List<int> subcategoryIds, bool isActive)
    {
        var request = new
        {
            SubcategoryIds = subcategoryIds,
            IsActive = isActive
        };

        var response = await _httpClient.PostAsJsonAsync("/api/announcements/filter", request);

        if (!response.IsSuccessStatusCode)
        {
            return new List<AnnouncementViewModel>();
        }

        var result = await response.Content.ReadFromJsonAsync<List<AnnouncementViewModel>>();
        return result ?? new List<AnnouncementViewModel>();
    }
}