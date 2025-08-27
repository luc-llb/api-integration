using System.Collections.Generic;
using System.Threading.Tasks;

namespace IntegrationApi.Core.Interfaces
{
    /// <summary>
    /// Repository interface for managing entities of type T.
    /// Define CRUD operations.
    /// </summary>
    /// <typeparam name="T">The entity type</typeparam>
    public interface IRepositoryBase<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
    }
}