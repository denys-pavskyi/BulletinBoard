using BulletinBoard.WebClient.Data;
using BulletinBoard.WebClient.Models.Posts;
using BulletinBoard.WebClient.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace BulletinBoard.WebClient.Controllers
{
    public class PostsController : Controller
    {
        private readonly IPostService _postService;
        private readonly IUserContextService _userContextService;

        public PostsController(IPostService postService, IUserContextService userContextService)
        {
            _postService = postService;
            _userContextService = userContextService;
        }

        public async Task<IActionResult> Index()
        {
            var categories = CategoryData.Categories;
            var allSubcategoryIds = categories.SelectMany(c => c.Subcategories.Select(s => s.Id)).ToList();

            var postsResult = await _postService.GetFilteredAsync(allSubcategoryIds, true);

            var posts = postsResult.Value;

            if (posts != null)
            {
                foreach (var post in posts)
                {
                    if (post.CreatedDate.Kind == DateTimeKind.Utc)
                    {
                        post.CreatedDate = TimeZoneInfo.ConvertTimeFromUtc(post.CreatedDate, TimeZoneInfo.Local);
                    }
                    else
                    {
                        post.CreatedDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(post.CreatedDate, DateTimeKind.Utc), TimeZoneInfo.Local);
                    }
                }
            }

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
            var result = await _postService.GetFilteredAsync(request.SubcategoryIds, request.IsActive);

            if (!result.IsSuccess)
            {
                TempData["Error"] = result.ErrorMessage ?? "Unable to filter posts";
                return RedirectToAction("Index", "Posts");
            }

            var posts = result.Value;

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

            if (posts != null)
            {
                foreach (var post in posts)
                {
                    if (post.CreatedDate.Kind == DateTimeKind.Utc)
                    {
                        post.CreatedDate = TimeZoneInfo.ConvertTimeFromUtc(post.CreatedDate, TimeZoneInfo.Local);
                    }
                    else
                    {
                        post.CreatedDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(post.CreatedDate, DateTimeKind.Utc), TimeZoneInfo.Local);
                    }
                }
            }

            return PartialView("_PostList", posts);
        }


        public async Task<IActionResult> MyPosts()
        {
            if (!_userContextService.IsAuthenticated || _userContextService.UserId is null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var userId = _userContextService.UserId ?? Guid.Empty;
            var result = await _postService.GetPostsByUserIdAsync(userId);

            if (!result.IsSuccess)
            {

                TempData["Error"] = result.ErrorMessage ?? "Unable to load posts";
                return RedirectToAction("Index", "Posts");
            }

            var posts = result.Value;

            if (posts != null)
            {
                foreach (var post in posts)
                {
                    if (post.CreatedDate.Kind == DateTimeKind.Utc)
                    {
                        post.CreatedDate = TimeZoneInfo.ConvertTimeFromUtc(post.CreatedDate, TimeZoneInfo.Local);
                    }
                    else
                    {
                        post.CreatedDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(post.CreatedDate, DateTimeKind.Utc), TimeZoneInfo.Local);
                    }
                }
            }

            var viewModel = new MyPostsViewModel
            {
                Posts = posts
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> RenderSinglePostPartial(Guid postId)
        {
            var result = await _postService.GetByIdAsync(postId);
            if (!result.IsSuccess) return NotFound();

            var post = result.Value;
            var categories = CategoryData.Categories;
            var subcategory = categories.SelectMany(c => c.Subcategories).FirstOrDefault(sc => sc.Id == post.SubcategoryId);
            var category = categories.FirstOrDefault(c => c.Subcategories.Any(sc => sc.Id == post.SubcategoryId));

            if (subcategory != null && category != null)
            {
                post.SubcategoryName = subcategory.Name;
                post.CategoryName = category.Name;
            }

            return PartialView("_MyPostsList", new List<PostViewModel> { post });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UpdatePostFormModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Invalid form data";
                return RedirectToAction("MyPosts");
            }

            var request = new UpdatePostRequest
            {
                Title = model.Title,
                Description = model.Description,
                SubcategoryId = model.SubcategoryId,
                IsActive = model.IsActive
            };

            var result = await _postService.UpdateAsync(model.Id, request);
            if (result.IsSuccess)
            {
                TempData["Success"] = "Post updated successfully";
                return RedirectToAction("MyPosts");
            }

            TempData["Error"] = result.Error.Message ?? "Failed to update post";
            return RedirectToAction("MyPosts");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid postId)
        {
            var result = await _postService.DeleteAsync(postId);
            if (result.IsSuccess)
            {
                TempData["Success"] = "Post deleted successfully";
                return RedirectToAction("MyPosts");
            }

            TempData["Error"] = result.Error.Message ?? "Failed to delete the post.";
            return RedirectToAction("MyPosts");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePostFormModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Invalid form data";
                return RedirectToAction("MyPosts");
            }

            var result = await _postService.AddAsync(model);
            if (result.IsSuccess)
            {
                TempData["Success"] = "Post created successfully";
                return RedirectToAction("MyPosts");
            }

            TempData["Error"] = result.Error.Message ?? "Failed to create post";
            return RedirectToAction("MyPosts");
        }

    }
}
