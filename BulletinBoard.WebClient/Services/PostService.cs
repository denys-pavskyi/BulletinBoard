using BulletinBoard.WebClient.Models.Other;
using BulletinBoard.WebClient.Models.Posts;
using BulletinBoard.WebClient.Services.Interfaces;
using System.Net;

namespace BulletinBoard.WebClient.Services;

public class PostService: IPostService
{
    private readonly HttpClient _httpClient;

    public PostService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("ApiClient");
    }

    public async Task<List<PostViewModel>> GetFilteredAsync(List<int> subcategoryIds, bool isActive)
    {
        var request = new
        {
            SubcategoryIds = subcategoryIds,
            IsActive = isActive
        };

        var response = await _httpClient.PostAsJsonAsync("/api/posts/filter", request);

        if (!response.IsSuccessStatusCode)
        {
            return new List<PostViewModel>();
        }

        var result = await response.Content.ReadFromJsonAsync<List<PostViewModel>>();
        return result ?? new List<PostViewModel>();
    }


    public async Task<List<PostViewModel>> GetPostsByUserIdAsync(Guid userId)
    {
        var response = await _httpClient.GetFromJsonAsync<List<PostViewModel>>($"/api/posts/user/{userId}");
        return response ?? new List<PostViewModel>();
    }

    public async Task<PostViewModel?> GetByIdAsync(Guid postId)
    {
        var response = await _httpClient.GetFromJsonAsync<PostViewModel?>($"/api/posts/{postId}");
        return response;

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
                Message = "An error occured",
                HttpCode = HttpStatusCode.BadRequest
            });
        }
    }

}