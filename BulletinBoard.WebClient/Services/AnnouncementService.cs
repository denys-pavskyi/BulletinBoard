using BulletinBoard.WebClient.Models;
using BulletinBoard.WebClient.Services.Interfaces;

namespace BulletinBoard.WebClient.Services;

public class AnnouncementService: IAnnouncementService
{
    private readonly HttpClient _httpClient;

    public AnnouncementService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("ApiClient");
    }

    public async Task<List<AnnouncementViewModel>> GetAllAsync()
    {
        var result = await _httpClient.GetFromJsonAsync<List<AnnouncementViewModel>>("/api/announcements");
        return result ?? new List<AnnouncementViewModel>();
    }
}