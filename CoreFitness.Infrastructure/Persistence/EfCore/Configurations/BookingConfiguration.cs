using CoreFitness.Domain.Aggregates.Members;
using CoreFitness.Domain.Aggregates.WorkoutSessions;
using CoreFitness.Domain.Aggregates.WorkoutSessions.Bookings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreFitness.Infrastructure.Persistence.EfCore.Configurations;

public sealed class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.ToTable("Bookings");

        builder.HasKey(x => new { x.WorkoutSessionId, x.MemberId });

        builder.Property(x => x.WorkoutSessionId)
            .HasConversion(
                id => id.Value,
                value => new WorkoutSessionId(value))
            .IsRequired();

        builder.Property(x => x.MemberId)
            .HasConversion(
                id => id.Value,
                value => new MemberId(value))
            .IsRequired();

        builder.HasIndex(x => x.MemberId);

        builder.HasOne<WorkoutSession>()
            .WithMany(x => x.Bookings)
            .HasForeignKey(x => x.WorkoutSessionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Member>()
            .WithMany()
            .HasForeignKey(x => x.MemberId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}