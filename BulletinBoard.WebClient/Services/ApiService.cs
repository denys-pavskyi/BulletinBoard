using BulletinBoard.WebClient.Services.Interfaces;
using System.Net;
using BulletinBoard.WebClient.Models.Other;
using System.Text;
using BulletinBoard.WebClient.Models.Auth;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace BulletinBoard.WebClient.Services;

public class ApiService: IApiService
{
    private readonly HttpClient _httpClient;
    private readonly IUserContextService _userContextService;

    public ApiService(IHttpClientFactory httpClientFactory, IUserContextService userContextService)
    {
        _userContextService = userContextService;
        _httpClient = httpClientFactory.CreateClient("ApiClient");
    }


    public async Task<Result<T>> HandleApiResponse<T>(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadFromJsonAsync<T>();
            return Result<T>.Success(data);
        }

        switch (response.StatusCode)
        {
            case HttpStatusCode.Unauthorized:
                return Result<T>.Failure("Unauthorized access - please login.");
            case HttpStatusCode.Forbidden:
                return Result<T>.Failure("Access forbidden - you do not have the necessary permissions.");
            case HttpStatusCode.NotFound:
                return Result<T>.Failure("Resource not found.");
            case HttpStatusCode.InternalServerError:
                return Result<T>.Failure("Server error occurred. Please try again later.");
            default:
                var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                return Result<T>.Failure(error?.Message ?? "Unknown error occurred.");
        }
    }

    public async Task<Result<AuthDto>> LoginAsync(LoginRequest request)
    {
        var json = JsonConvert.SerializeObject(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");


        var response = await _httpClient.PostAsync("/api/auth/login", content);

        return await HandleApiResponse<AuthDto>(response);
    }


    public async Task<Result<AuthDto>> RegisterAsync(RegisterUserRequest request)
    {
        var json = JsonConvert.SerializeObject(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("/api/auth/register", content);

        return await HandleApiResponse<AuthDto>(response);
    }

}