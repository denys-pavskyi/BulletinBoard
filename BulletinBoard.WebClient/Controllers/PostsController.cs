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


        //[HttpPost]
        //public async Task<IActionResult> Index(PostIndexViewModel model)
        //{
        //    var filteredPosts = model.SelectedSubcategoryIds.Any()
        //        ? await _postService.GetFilteredAsync(model.SelectedSubcategoryIds, model.IsActive)
        //        : new List<PostViewModel>();


        //    model.Categories = CategoryData.Categories;
        //    model.Posts = filteredPosts;

        //    return View(model);
        //}

        public IActionResult MyPosts()
        {
            // TODO: get user posts
            return View();
        }

    }
}
