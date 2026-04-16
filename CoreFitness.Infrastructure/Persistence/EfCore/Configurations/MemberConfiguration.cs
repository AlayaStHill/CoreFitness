using CoreFitness.Domain.Aggregates.Members;
using CoreFitness.Infrastructure.Identity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreFitness.Infrastructure.Persistence.EfCore.Configurations;

public sealed class MemberConfiguration : IEntityTypeConfiguration<Member>
{
    public void Configure(EntityTypeBuilder<Member> builder)
    {
        builder.ToTable("Members");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(
                id => id.Value,
                value => new MemberId(value))
            .ValueGeneratedNever();

        builder.Property(x => x.UserId)
            .IsRequired()
            .HasMaxLength(450);

        builder.HasIndex(x => x.UserId)
            .IsUnique();

        builder.HasMany(x => x.Memberships)
            .WithOne()
            .HasForeignKey(x => x.MemberId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(x => x.Memberships)
            .HasField("_memberships")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasOne<ApplicationUser>()
            .WithOne()
            .HasForeignKey<Member>(x => x.UserId)
            .HasPrincipalKey<ApplicationUser>(x => x.Id)
            .OnDelete(DeleteBehavior.Cascade);
    }
}