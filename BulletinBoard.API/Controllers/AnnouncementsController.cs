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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAllAnnouncements()
        {
            var result = await _announcementService.GetAllAsync();
            
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddAsync([FromBody] CreateNewAnnouncementRequest model)
        {
            var result = await _announcementService.AddAsync(model);

            return result.Match(
                _ => NoContent(),
                error => error.ToActionResult()
            );


        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var result = await _announcementService.GetByIdAsync(id);
            return Ok(result.Value);
        }


        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] UpdateAnnouncementRequest request)
        {
            var result = await _announcementService.UpdateAsync(id, request);
            return result.Match(
                NoContent,
                error => error.ToActionResult()
            );
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var result = await _announcementService.DeleteByIdAsync(id);

            return result.Match(
                NoContent,
                error => error.ToActionResult()
            );
        }

        [HttpGet("user/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByUserId(Guid userId)
        {
            var result = await _announcementService.GetAllByUserIdAsync(userId);

            return result.Match(
                Ok,
                error => error.ToActionResult());
        }

        [HttpPost("filter")]
        public async Task<IActionResult> GetFiltered([FromBody] AnnouncementFilterRequest filter)
        {
            var result = await _announcementService.GetFilteredAsync(filter.SubcategoryIds, filter.IsActive);

            return result.Match<IActionResult>(
                Ok,
                error => error.ToActionResult());
        }



    }
}
