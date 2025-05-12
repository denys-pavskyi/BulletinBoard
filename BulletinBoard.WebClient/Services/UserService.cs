using BulletinBoard.WebClient.Models.Other;
using BulletinBoard.WebClient.Models.Users;
using BulletinBoard.WebClient.Services.Interfaces;
using System.Net.Http.Headers;

namespace BulletinBoard.WebClient.Services;

public class UserService: IUserService
{
    private readonly HttpClient _httpClient;
    private readonly IApiService _apiService;
    private readonly IUserContextService _userContextService;

    public UserService(IHttpClientFactory httpClientFactory, IApiService apiService, IUserContextService userContextService)
    {
        _apiService = apiService;
        _userContextService = userContextService;
        _httpClient = httpClientFactory.CreateClient("ApiClient");
    }

    public async Task<Result<UserViewModel>> GetByIdAsync(Guid userId)
    {
        var token = _userContextService.JwtToken;

        if (string.IsNullOrEmpty(token))
        {
            return Result<UserViewModel>.Failure("User is not authenticated.");
        }
        var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"/api/users/{userId}");
        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.SendAsync(requestMessage);
        return await _apiService.HandleApiResponse<UserViewModel>(response);
    }
}