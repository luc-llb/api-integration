using IntegrationApi.Core.Entities;

namespace IntegrationApi.Core.Interfaces
{
    public interface ICharacterRepository
    {
        Task<IEnumerable<Character>> GetAllAsync();
    }
}