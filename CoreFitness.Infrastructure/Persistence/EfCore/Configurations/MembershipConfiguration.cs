using CoreFitness.Domain.Aggregates.Members;
using CoreFitness.Domain.Aggregates.Members.Memberships;
using CoreFitness.Domain.Aggregates.MembershipTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreFitness.Infrastructure.Persistence.EfCore.Configurations;

public sealed class MembershipConfiguration : IEntityTypeConfiguration<Membership>
{
    public void Configure(EntityTypeBuilder<Membership> builder)
    {
        builder.ToTable("Memberships");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(
                id => id.Value,
                value => new MembershipId(value))
            .ValueGeneratedNever();

        builder.Property(x => x.MemberId)
            .HasConversion(
                id => id.Value,
                value => new MemberId(value))
            .IsRequired();

        builder.Property(x => x.MembershipTypeId)
            .HasConversion(
                id => id.Value,
                value => new MembershipTypeId(value))
            .IsRequired();

        builder.Property(x => x.StartDate)
            .IsRequired();

        builder.Property(x => x.Status)
            .HasConversion<string>()
            .HasMaxLength(30)
            .IsRequired();

        builder.HasIndex(x => x.MemberId);

        builder.HasIndex(x => x.MembershipTypeId);

        builder.HasOne<Member>()
            .WithMany(x => x.Memberships)
            .HasForeignKey(x => x.MemberId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<MembershipType>()
            .WithMany()
            .HasForeignKey(x => x.MembershipTypeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}