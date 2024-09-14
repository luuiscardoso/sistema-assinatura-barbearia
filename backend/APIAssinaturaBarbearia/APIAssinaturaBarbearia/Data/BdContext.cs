using APIAssinaturaBarbearia.Models;
using Microsoft.EntityFrameworkCore;

namespace APIAssinaturaBarbearia.Data
{
    public class BdContext : DbContext 
    {
        public BdContext(DbContextOptions<BdContext> options) : base(options)
        {
        }

        public DbSet<Assinatura> Assinaturas { get; set; }
        public DbSet<Cliente> Clientes { get; set; }

        protected override void OnModelCreating(ModelBuilder md)
        {
            md.Entity<Cliente>()
                .HasOne(c => c.Assinatura)
                .WithOne(a => a.Cliente)
                .HasForeignKey<Cliente>(c => c.AssinaturaId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
