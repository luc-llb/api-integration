using Microsoft.EntityFrameworkCore;
using IntegrationApi.Core.Entities;
using System.Reflection;

namespace IntegrationApi.Infrastructure.Data
{
    /// <summary>
    /// Application database context.
    /// </summary>
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public AppDbContext() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var entityTypes = Assembly.GetAssembly(typeof(BaseEntity))?
                .GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && typeof(BaseEntity).IsAssignableFrom(t));

            if (entityTypes != null)
            {
                foreach (var type in entityTypes)
                {
                    modelBuilder.Entity(type);
                }
            }
            else
            {
                throw new InvalidOperationException("No entity types found in the assembly.");
            }
        }

    }
}