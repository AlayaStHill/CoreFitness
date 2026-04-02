using CoreFitness.Domain.Aggregates.MembershipTypes;
using CoreFitness.Domain.Aggregates.MembershipTypes.MembershipBenefits;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreFitness.Infrastructure.Persistence.EfCore.Configurations;

public sealed class MembershipBenefitConfiguration : IEntityTypeConfiguration<MembershipBenefit>
{
    public void Configure(EntityTypeBuilder<MembershipBenefit> builder)
    {
        builder.ToTable("MembershipBenefits");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(
                id => id.Value,
                value => new MembershipBenefitId(value))
            .ValueGeneratedNever();

        builder.Property(x => x.MembershipTypeId)
            .HasConversion(
                id => id.Value,
                value => new MembershipTypeId(value))
            .IsRequired();

        builder.Property(x => x.Text)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasOne<MembershipType>()
          .WithMany(x => x.Benefits)
          .HasForeignKey(x => x.MembershipTypeId)
          .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => new { x.MembershipTypeId, x.Text })
            .IsUnique();
    }
}