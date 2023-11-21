using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using MovieSeeker.Domain.Entities;

namespace MovieSeeker.Infra.Data.Mappings
{
    public class UserMapping : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(e => e.Id);
            
            builder.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(62);

            builder.Property(e => e.Password)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(e => e.Active)
                .IsRequired()
                .HasDefaultValue(false);
        }
    }
}