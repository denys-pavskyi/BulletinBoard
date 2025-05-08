using BulletinBoard.BLL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BulletinBoard.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnnouncementsController : ControllerBase
    {
        private readonly IAnnouncementService _announcementService;
        public AnnouncementsController(IAnnouncementService announcementService)
        {
            _announcementService = announcementService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAnnouncements()
        {
            var result = await _announcementService.GetAllAsync();

            
            return Ok(result);
        }

    }
}
