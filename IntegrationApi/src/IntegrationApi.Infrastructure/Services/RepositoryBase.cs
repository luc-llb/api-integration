using IntegrationApi.Core.Interfaces;
using IntegrationApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using IntegrationApi.Core.Exceptions;

namespace IntegrationApi.Infrastructure.Services
{
    /// <summary>
    /// Repository base class for managing entities of type T.
    /// Implement CRUD operations defined in IRepositoryBase.
    /// </summary>
    /// <typeparam name="T">The entity type</typeparam>
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;

        public RepositoryBase(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            if (entity == null)
            {
                throw new ValidationException("entity", "The entity cannot be null.");
            }

            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
            {
                string typeName = typeof(T).Name;
                throw new ResourceNotFoundException($"The resource '{typeName}' with ID {id} was not found.");
            }

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
            {
                string typeName = typeof(T).Name;
                throw new ResourceNotFoundException($"The resource '{typeName}' with ID {id} was not found.");
            }

            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            if (entity == null)
            {
                throw new ValidationException("entity", "The entity cannot be null.");
            }

            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}