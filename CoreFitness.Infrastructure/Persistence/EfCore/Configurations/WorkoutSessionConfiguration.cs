using CoreFitness.Domain.Aggregates.WorkoutSessions;
using CoreFitness.Domain.Aggregates.WorkoutTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreFitness.Infrastructure.Persistence.Configurations;

public sealed class WorkoutSessionConfiguration : IEntityTypeConfiguration<WorkoutSession>
{
    public void Configure(EntityTypeBuilder<WorkoutSession> builder)
    {
        builder.ToTable("WorkoutSessions", table =>
        {
            table.HasCheckConstraint("CK_WorkoutSessions_Capacity", "[Capacity] > 0");
        });

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(
                id => id.Value,
                value => new WorkoutSessionId(value))
            .ValueGeneratedNever();

        builder.HasOne(x => x.WorkoutType)
           .WithMany()
           .HasForeignKey(x => x.WorkoutTypeId)
           .OnDelete(DeleteBehavior.Restrict)
           .IsRequired();

        //builder.Property(x => x.WorkoutTypeId)
        //    .HasConversion(
        //        id => id.Value,
        //        value => new WorkoutTypeId(value))
        //    .IsRequired();

        builder.Property(x => x.StartsAt)
            .IsRequired();

        builder.Property(x => x.Duration)
            .IsRequired();

        builder.Property(x => x.Capacity)
            .IsRequired();

        builder.HasIndex(x => x.WorkoutTypeId);
        builder.HasIndex(x => x.StartsAt);

        builder.HasOne<WorkoutType>()
            .WithMany()
            .HasForeignKey(x => x.WorkoutTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Bookings)
            .WithOne()
            .HasForeignKey(x => x.WorkoutSessionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(x => x.Bookings)
        .HasField("_bookings")
        .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}