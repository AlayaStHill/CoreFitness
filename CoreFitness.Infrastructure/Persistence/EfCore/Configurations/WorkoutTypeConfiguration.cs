using CoreFitness.Domain.Aggregates.WorkoutCategories;
using CoreFitness.Domain.Aggregates.WorkoutTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreFitness.Infrastructure.Persistence.EfCore.Configurations;

public sealed class WorkoutTypeConfiguration : IEntityTypeConfiguration<WorkoutType>
{
    public void Configure(EntityTypeBuilder<WorkoutType> builder)
    {
        builder.ToTable("WorkoutTypes");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(
                id => id.Value,
                value => new WorkoutTypeId(value))
            .ValueGeneratedNever();

        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.WorkoutCategoryId)
            .HasConversion(
                id => id.Value,
                value => new WorkoutCategoryId(value))
            .IsRequired();

        builder.HasOne(x => x.WorkoutCategory)
            .WithMany()
            .HasForeignKey(x => x.WorkoutCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.Title, x.WorkoutCategoryId })
            .IsUnique();

        builder.HasIndex(x => x.WorkoutCategoryId);

    }
}