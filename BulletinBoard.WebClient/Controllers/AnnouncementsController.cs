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


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var allSubcategoryIds = CategoryData.Categories
                .SelectMany(c => c.Subcategories)
                .Select(s => s.Id)
                .ToList();

            var announcements = await _announcementService.GetFilteredAsync(allSubcategoryIds, true);

            var model = new AnnouncementIndexViewModel
            {
                Categories = CategoryData.Categories,
                Announcements = announcements,
                SelectedSubcategoryIds = allSubcategoryIds,
                IsActive = true
            };

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Index(AnnouncementIndexViewModel model)
        {
            var filteredAnnouncements = model.SelectedSubcategoryIds.Any()
                ? await _announcementService.GetFilteredAsync(model.SelectedSubcategoryIds, model.IsActive)
                : new List<AnnouncementViewModel>();


            model.Categories = CategoryData.Categories;
            model.Announcements = filteredAnnouncements;

            return View(model);
        }

        public IActionResult MyAnnouncements()
        {
            // TODO: get user announcements
            return View();
        }

    }
}
