using BulletinBoard.BLL.Interfaces;
using BulletinBoard.BLL.Models.DtoModels;
using BulletinBoard.BLL.Models.Requests;
using BulletinBoard.BLL.Other;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BulletinBoard.API.Controllers
{
    [Route("api/posts")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IPostService _postService;
        public PostsController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAllPosts()
        {
            var result = await _postService.GetAllAsync();
            
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddAsync([FromBody] CreateNewPostRequest model)
        {
            var result = await _postService.AddAsync(model);

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
            var result = await _postService.GetByIdAsync(id);
            return Ok(result.Value);
        }


        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] UpdatePostRequest request)
        {
            var result = await _postService.UpdateAsync(id, request);
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
            var result = await _postService.DeleteByIdAsync(id);

            return result.Match(
                NoContent,
                error => error.ToActionResult()
            );
        }

        [Authorize]
        [HttpGet("user/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByUserId(Guid userId)
        {
            //var currentUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));


            var result = await _postService.GetAllByUserIdAsync(userId);

            return result.Match(
                Ok,
                error => error.ToActionResult());
        }

        [HttpPost("filter")]
        public async Task<IActionResult> GetFiltered([FromBody] PostFilterRequest filter)
        {
            var result = await _postService.GetFilteredAsync(filter.SubcategoryIds, filter.IsActive);

            return result.Match<IActionResult>(
                Ok,
                error => error.ToActionResult());
        }



    }
}
