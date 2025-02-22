using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace APIAssinaturaBarbearia.Infrastructure.Data
{
    public class BdContextFactory : IDesignTimeDbContextFactory<BdContext>
    {
        public BdContext CreateDbContext(string[] args)
        {
            var basePath = Path.Combine(Directory.GetCurrentDirectory(), "../APIAssinaturaBarbearia");

            if (!Directory.Exists(basePath))
            {
                throw new DirectoryNotFoundException($"O diretório base '{basePath}' não foi encontrado!");
            }

            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<BdContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new BdContext(optionsBuilder.Options);
        }
    }
}
