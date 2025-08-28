using IntegrationApi.Core.Entities;

namespace IntegrationApi.Core.Interfaces
{
    public interface IAnimationService : IRepositoryBase<Animation>
    {
        Task<IEnumerable<Animation>> SearchAnimationsAsync(string searchTerm, MediaType mediaType);
    }
}