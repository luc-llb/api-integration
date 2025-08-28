using IntegrationApi.Core.Entities;

namespace IntegrationApi.Core.Interfaces
{
    public interface ICharacterService : IRepositoryBase<Character>
    {
        Task<IEnumerable<Character>> SearchCharactersAsync(string searchTerm, MediaType mediaType);
    }
}