using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using MovieSeeker.Domain.Entities;

namespace MovieSeeker.Infra.Data.Mappings
{
    public class UserActivationMapping : IEntityTypeConfiguration<UserActivation>
    {
        public void Configure(EntityTypeBuilder<UserActivation> builder)
        {
            builder.HasKey(e => e.Id);
            
            builder.HasOne<User>(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(e => e.SendingMoment)
                .IsRequired();

            builder.Property(e => e.ActivationMoment)
                .IsRequired(false);

            builder.Property(e => e.Expired)
                .IsRequired()
                .HasDefaultValue(false);
        }
    }
}