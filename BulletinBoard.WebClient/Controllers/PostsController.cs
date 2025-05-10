using BulletinBoard.WebClient.Data;
using BulletinBoard.WebClient.Models.Posts;
using BulletinBoard.WebClient.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BulletinBoard.WebClient.Controllers
{
    public class PostsController : Controller
    {
        private readonly IPostService _postService;

        public PostsController(IPostService postService)
        {
            _postService = postService;
        }

        public async Task<IActionResult> Index()
        {
            var categories = CategoryData.Categories;
            var allSubcategoryIds = categories.SelectMany(c => c.Subcategories.Select(s => s.Id)).ToList();

            var posts = await _postService.GetFilteredAsync(allSubcategoryIds, true);

            var model = new PostIndexViewModel
            {
                Categories = categories,
                Posts = posts,
                SelectedSubcategoryIds = allSubcategoryIds,
                IsActive = true
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FilterPartial([FromBody] FilterRequestDto request)
        {
            var posts = await _postService.GetFilteredAsync(request.SubcategoryIds, request.IsActive);

            foreach (var item in posts)
            {
                var subcategory = CategoryData.Categories
                    .SelectMany(c => c.Subcategories)
                    .FirstOrDefault(sc => sc.Id == item.SubcategoryId);

                var category = CategoryData.Categories
                    .FirstOrDefault(c => c.Subcategories.Any(sc => sc.Id == item.SubcategoryId));

                if (subcategory != null && category != null)
                {
                    item.SubcategoryName = subcategory.Name;
                    item.CategoryName = category.Name;
                }
            }

            return PartialView("_PostList", posts);
        }


        public async Task<IActionResult> MyPosts()
        {
            var userId = Guid.Parse("D2B23AD3-BD8B-4CA6-AA22-B8E5B3C47CF0");
            var posts = await _postService.GetPostsByUserIdAsync(userId);

            if (!posts.Any())
            {
                return NoContent();
            }

            var viewModel = new MyPostsViewModel
            {
                Posts = posts
            };

            return View(viewModel);
        }

    }
}
