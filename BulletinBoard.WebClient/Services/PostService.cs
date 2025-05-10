using BulletinBoard.WebClient.Models.Posts;
using BulletinBoard.WebClient.Services.Interfaces;

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

}