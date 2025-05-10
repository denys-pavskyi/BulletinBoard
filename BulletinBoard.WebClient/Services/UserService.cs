using BulletinBoard.WebClient.Models.Users;
using BulletinBoard.WebClient.Services.Interfaces;

namespace BulletinBoard.WebClient.Services;

public class UserService: IUserService
{
    private readonly HttpClient _httpClient;

    public UserService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("ApiClient");
    }

    public async Task<UserViewModel?> GetByIdAsync(Guid id)
    {
        return await _httpClient.GetFromJsonAsync<UserViewModel>($"/api/users/{id}");
    }
}