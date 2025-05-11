using BulletinBoard.WebClient.Models.Other;
using BulletinBoard.WebClient.Models.Users;
using BulletinBoard.WebClient.Services.Interfaces;

namespace BulletinBoard.WebClient.Services;

public class UserService: IUserService
{
    private readonly HttpClient _httpClient;
    private readonly IApiService _apiService;

    public UserService(IHttpClientFactory httpClientFactory, IApiService apiService)
    {
        _apiService = apiService;
        _httpClient = httpClientFactory.CreateClient("ApiClient");
    }

    public async Task<Result<UserViewModel>> GetByIdAsync(Guid userId)
    {

        var response = await _httpClient.GetAsync($"/api/users/{userId}");
        return await _apiService.HandleApiResponse<UserViewModel>(response);
    }
}