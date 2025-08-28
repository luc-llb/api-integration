
using IntegrationApi.Core.Entities;
using IntegrationApi.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IntegrationApi.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class CharactersController : ControllerBase
    {
        private readonly ICharacterService _characterService;

        public CharactersController(ICharacterService characterService)
        {
            _characterService = characterService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Character>>> GetCharacters()
        {
            var characters = await _characterService.GetAllAsync();
            return Ok(characters);
        }

        /// <summary>
        /// Get characters by search term
        /// </summary>
        /// <param name="searchTerm">String used to search the GraphQL API</param>
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Character>>> SearchCharacters(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return BadRequest("The param searchTerm is required");
            }

            var characters = await _characterService.SearchCharactersAsync(searchTerm, MediaType.ANIME);
            return Ok(characters);
        }

        /// <summary>
        /// Get character in database by ID
        /// </summary>
        /// <param name="id">Character's ID in database</param>
        [HttpGet("{id}")]
        public async Task<ActionResult<Character>> GetCharacter(int id)
        {
            try
            {
                var character = await _characterService.GetByIdAsync(id);
                return Ok(character);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Character with ID {id} not found");
            }
        }

        /// <summary>
        /// Search for a character and save it to the database
        /// </summary>
        /// <param name="searchTerm">String used to search the GraphQL API</param>
        [HttpPost("search-and-save")]
        public async Task<ActionResult<Character>> SearchAndSaveCharacter(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
                return BadRequest("The param searchTerm is required");
            

            // Busca na API GraphQL
            var characters = await _characterService.SearchCharactersAsync(searchTerm, MediaType.ANIME);
            var firstCharacter = characters.FirstOrDefault();

            if (firstCharacter == null)
                return NotFound($"No character found for the term '{searchTerm}'");

            await _characterService.AddAsync(firstCharacter);

            return CreatedAtAction(nameof(GetCharacter), new { id = firstCharacter.Id }, firstCharacter);
        }
    }
}