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
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // String de conexão padrão para migrations
                optionsBuilder.UseSqlServer("Server=localhost,1433;Database=animes;User Id=admin;Password=Admin@123456;TrustServerCertificate=True;");
            }
        }
    }
}