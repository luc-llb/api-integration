using Microsoft.AspNetCore.Mvc;
using IntegrationApi.Core.Entities;
using IntegrationApi.Core.Interfaces;

namespace IntegrationApi.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AnimationsController : ControllerBase
    {
        private readonly IAnimationService _animationService;

        public AnimationsController(IAnimationService animationService)
        {
            _animationService = animationService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Animation>>> GetAnimations()
        {
            var animations = await _animationService.GetAllAsync();
            return Ok(animations);
        }

        /// <summary>
        /// Get animations by search term
        /// </summary>
        /// <param name="searchTerm">String used to search the GraphQL API</param>
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Animation>>> SearchAnimations(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
                return BadRequest("The param searchTerm is required");

            var animations = await _animationService.SearchAnimationsAsync(searchTerm, MediaType.ANIME);

            if (!animations.Any())
                return NotFound($"No animations found for the term '{searchTerm}'");
            
            
            return Ok(animations);
        }

        /// <summary>
        /// Get animation in database by ID
        /// </summary>
        /// <param name="id">Animation's ID in database</param>
        [HttpGet("{id}")]
        public async Task<ActionResult<Animation>> GetAnimation(int id)
        {
            var animation = await _animationService.GetByIdAsync(id);
            return Ok(animation);
        }

        /// <summary>
        /// Search for a animation and save it to the database
        /// </summary>
        /// <param name="searchTerm">String used to search the GraphQL API</param>
        [HttpPost("search-and-save")]
        public async Task<ActionResult<Animation>> SearchAndSaveAnimation(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
                return BadRequest("The param searchTerm is required");

            var animations = await _animationService.SearchAnimationsAsync(searchTerm, MediaType.ANIME);
            var firstAnimation = animations.FirstOrDefault();

            if (firstAnimation == null)
                return NotFound($"No animation found for the term '{searchTerm}'");

            await _animationService.AddAsync(firstAnimation);

            return CreatedAtAction(nameof(GetAnimation), new { id = firstAnimation.Id }, firstAnimation);
        }
    }
}
