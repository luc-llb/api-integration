using Microsoft.EntityFrameworkCore;
using IntegrationApi.Core.Entities;

namespace IntegrationApi.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        
        // Construtor sem parâmetros para o EF Core Design Time
        public AppDbContext() { }
        
        public DbSet<Character> Characters => Set<Character>();
        public DbSet<Animation> Animations => Set<Animation>();
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }
    }
}