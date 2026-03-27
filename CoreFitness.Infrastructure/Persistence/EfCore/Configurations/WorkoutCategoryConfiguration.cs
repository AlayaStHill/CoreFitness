using CoreFitness.Domain.Aggregates.WorkoutCategories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreFitness.Infrastructure.Persistence.EfCore.Configurations;

public sealed class WorkoutCategoryConfiguration : IEntityTypeConfiguration<WorkoutCategory>
{
    public void Configure(EntityTypeBuilder<WorkoutCategory> builder)
    {
        builder.ToTable("WorkoutCategories");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(
                id => id.Value,
                value => new WorkoutCategoryId(value))
            .ValueGeneratedNever();

        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(x => x.Title)
            .IsUnique();
    }
}