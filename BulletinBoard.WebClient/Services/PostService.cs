using BulletinBoard.WebClient.Models.Other;
using BulletinBoard.WebClient.Models.Posts;
using BulletinBoard.WebClient.Services.Interfaces;
using System.Net;

namespace BulletinBoard.WebClient.Services;

public class PostService: IPostService
{
    private readonly HttpClient _httpClient;
    private readonly IApiService _apiService;

    public PostService(IHttpClientFactory httpClientFactory, IApiService apiService)
    {
        _apiService = apiService;
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
        var response = await _httpClient.GetAsync($"/api/posts/user/{userId}");
        return await _apiService.HandleApiResponse<List<PostViewModel>>(response);
    }

    public async Task<Result<PostViewModel?>> GetByIdAsync(Guid postId)
    {
        var response = await _httpClient.GetFromJsonAsync<PostViewModel?>($"/api/posts/{postId}");

        if (response == null)
        {
            return Result<PostViewModel?>.Failure("Post not found.");
        }

        return Result<PostViewModel?>.Success(response);
    }

    public async Task<Result> UpdateAsync(Guid id, UpdatePostRequest request)
    {
        var response = await _httpClient.PutAsJsonAsync($"/api/posts/{id}", request);

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
            var response = await _httpClient.DeleteAsync($"/api/posts/{postId}");

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
        var response = await _httpClient.PostAsJsonAsync("/api/posts", request);

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