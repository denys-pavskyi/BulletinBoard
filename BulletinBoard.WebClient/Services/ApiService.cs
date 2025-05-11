using BulletinBoard.WebClient.Services.Interfaces;
using System.Net;
using BulletinBoard.WebClient.Models.Other;

namespace BulletinBoard.WebClient.Services;

public class ApiService: IApiService
{
    private readonly HttpClient _httpClient;

    public ApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
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
}