using BulletinBoard.BLL.Interfaces;
using BulletinBoard.BLL.Models.DtoModels;
using BulletinBoard.BLL.Models.Requests;
using BulletinBoard.BLL.Other;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BulletinBoard.API.Controllers
{
    [Route("api/announcements")]
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

        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] CreateNewAnnouncementRequest model)
        {
            var result = await _announcementService.AddAsync(model);

            if (result.IsSuccess)
            {
                return NoContent();
            }

            return result.Error!.ToActionResult();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var result = await _announcementService.GetByIdAsync(id);
            return Ok(result.Value);
        }

    }
}
