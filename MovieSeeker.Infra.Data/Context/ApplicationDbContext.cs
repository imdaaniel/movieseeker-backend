using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using MovieSeeker.Domain.Entities;

namespace MovieSeeker.Infra.Data.Context
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Obtendo todas as classes que implementam IEntityTypeConfiguration<T>
            // Fiz dessa forma pra nao ter que adicionar uma linha para cada classe de mapping
            var mappings = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => typeof(IEntityTypeConfiguration<>).IsAssignableFrom(p) && !p.IsAbstract);

            // Percorrendo cada classe mapping
            foreach (var mappingClass in mappings)
            {
                dynamic mappingInstance = Activator.CreateInstance(mappingClass);
                modelBuilder.ApplyConfiguration(mappingInstance);
            }

            base.OnModelCreating(modelBuilder);
        }
    }
}