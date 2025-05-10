using BulletinBoard.WebClient.Data;
using BulletinBoard.WebClient.Models.Announcements;
using BulletinBoard.WebClient.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BulletinBoard.WebClient.Controllers
{
    public class AnnouncementsController : Controller
    {
        private readonly IAnnouncementService _announcementService;

        public AnnouncementsController(IAnnouncementService announcementService)
        {
            _announcementService = announcementService;
        }

        public async Task<IActionResult> Index()
        {
            var categories = CategoryData.Categories;
            var allSubcategoryIds = categories.SelectMany(c => c.Subcategories.Select(s => s.Id)).ToList();

            var announcements = await _announcementService.GetFilteredAsync(allSubcategoryIds, true);

            var model = new AnnouncementIndexViewModel
            {
                Categories = categories,
                Announcements = announcements,
                SelectedSubcategoryIds = allSubcategoryIds,
                IsActive = true
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FilterPartial([FromBody] FilterRequestDto request)
        {
            var announcements = await _announcementService.GetFilteredAsync(request.SubcategoryIds, request.IsActive);

            foreach (var item in announcements)
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

            return PartialView("_AnnouncementList", announcements);
        }


        //[HttpPost]
        //public async Task<IActionResult> Index(AnnouncementIndexViewModel model)
        //{
        //    var filteredAnnouncements = model.SelectedSubcategoryIds.Any()
        //        ? await _announcementService.GetFilteredAsync(model.SelectedSubcategoryIds, model.IsActive)
        //        : new List<AnnouncementViewModel>();


        //    model.Categories = CategoryData.Categories;
        //    model.Announcements = filteredAnnouncements;

        //    return View(model);
        //}

        public IActionResult MyAnnouncements()
        {
            // TODO: get user announcements
            return View();
        }

    }
}
