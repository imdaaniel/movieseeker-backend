using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using MovieSeeker.Domain.Entities;

namespace MovieSeeker.Infra.Data.Mappings
{
    public class UsuarioMapping : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.HasKey(e => e.Id);
            
            builder.Property(e => e.Nome)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(e => e.Sobrenome)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(62);

            builder.Property(e => e.Senha)
                .IsRequired()
                .HasMaxLength(255);
        }
    }
}