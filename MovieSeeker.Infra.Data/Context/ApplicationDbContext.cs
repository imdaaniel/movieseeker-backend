using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace MovieSeeker.Infra.Data.Context
{
    public class ApplicationDbContext : DbContext
    {
        // public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        // { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            Console.WriteLine(Directory.GetCurrentDirectory());
            
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "..", "MovieSeeker.API"))
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            optionsBuilder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
        }

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