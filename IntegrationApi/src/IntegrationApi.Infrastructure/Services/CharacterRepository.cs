using IntegrationApi.Core.Entities;
using IntegrationApi.Core.Interfaces;
using IntegrationApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace IntegrationApi.Infrastructure.Services
{
    public class CharacterRepository : ICharacterRepository
    {
        private readonly AppDbContext _context;

        public CharacterRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Character>> GetAllAsync()
        {
            return await _context.Characters.ToListAsync();
        }
    }
}