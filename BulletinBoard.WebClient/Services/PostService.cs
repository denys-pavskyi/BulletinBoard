using BulletinBoard.WebClient.Models.Other;
using BulletinBoard.WebClient.Models.Posts;
using BulletinBoard.WebClient.Services.Interfaces;
using System.Net;
using System.Net.Http.Headers;

namespace BulletinBoard.WebClient.Services;

public class PostService: IPostService
{
    private readonly HttpClient _httpClient;
    private readonly IApiService _apiService;
    private readonly IUserContextService _userContextService;

    public PostService(IHttpClientFactory httpClientFactory, IApiService apiService, IUserContextService userContextService)
    {
        _apiService = apiService;
        _userContextService = userContextService;
        _httpClient = httpClientFactory.CreateClient("ApiClient");
    }

    public async Task<Result<List<PostViewModel>>> GetFilteredAsync(List<int> subcategoryIds, bool isActive)
    {
        var request = new
        {
            SubcategoryIds = subcategoryIds,
            IsActive = isActive
        };

        var response = await _httpClient.PostAsJsonAsync("/api/posts/filter", request);

        return await _apiService.HandleApiResponse<List<PostViewModel>>(response);
    }

    public async Task<Result<List<PostViewModel>>> GetPostsByUserIdAsync(Guid userId)
    {
        var token = _userContextService.JwtToken;

        if (string.IsNullOrEmpty(token))
        {
            return Result<List<PostViewModel>>.Failure("User is not authenticated.");
        }


        var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"/api/posts/user/{userId}");

        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.SendAsync(requestMessage);

        return await _apiService.HandleApiResponse<List<PostViewModel>>(response);
    }

    public async Task<Result<PostViewModel?>> GetByIdAsync(Guid postId)
    {
        var token = _userContextService.JwtToken;

        if (string.IsNullOrEmpty(token))
        {
            return Result<PostViewModel?>.Failure("User is not authenticated.");
        }

        var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"/api/posts/{postId}");
        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.SendAsync(requestMessage);

        return await _apiService.HandleApiResponse<PostViewModel?>(response);
    }

    public async Task<Result> UpdateAsync(Guid id, UpdatePostRequest request)
    {
        var token = _userContextService.JwtToken;

        if (string.IsNullOrEmpty(token))
        {
            return Result.Failure(new ErrorResponse
            {
                Message = "User is not authenticated."
            });
        }
        var requestMessage = new HttpRequestMessage(HttpMethod.Put, $"/api/posts/{id}")
        {
            Content = JsonContent.Create(request)
        };
        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.SendAsync(requestMessage);


        if (response.IsSuccessStatusCode)
        {
            return Result.Success();
        }

        return Result.Failure(new ErrorResponse
        {
            Message = "Failed to update post",
            HttpCode = HttpStatusCode.BadRequest
        });
    }

    public async Task<Result> DeleteAsync(Guid postId)
    {
        try
        {
            var token = _userContextService.JwtToken;

            if (string.IsNullOrEmpty(token))
            {
                return Result.Failure(new ErrorResponse
                {
                    Message = "User is not authenticated."
                });
            }


            var requestMessage = new HttpRequestMessage(HttpMethod.Delete, $"/api/posts/{postId}");
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(requestMessage); ;

            if (response.IsSuccessStatusCode)
            {
                return Result.Success();
            }
            else
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                return Result.Failure(new ErrorResponse
                {
                    Message = errorResponse,
                    HttpCode = response.StatusCode
                });
            }
        }
        catch (Exception ex)
        {
            return Result.Failure(new ErrorResponse
            {
                Message = $"An error occurred: {ex.Message}",
                HttpCode = HttpStatusCode.InternalServerError
            });
        }
    }

    public async Task<Result> AddAsync(CreatePostFormModel request)
    {
        var token = _userContextService.JwtToken;

        if (string.IsNullOrEmpty(token))
        {
            return Result.Failure(new ErrorResponse
            {
                Message = "User is not authenticated."
            });
        }

        var requestMessage = new HttpRequestMessage(HttpMethod.Post, "/api/posts")
        {
            Content = JsonContent.Create(request)
        };
        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.SendAsync(requestMessage);

        if (response.IsSuccessStatusCode)
        {
            return Result.Success();
        }

        var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        return Result.Failure(errorResponse ?? new ErrorResponse
        {
            Message = "Failed to add post",
            HttpCode = HttpStatusCode.BadRequest
        });
    }

}