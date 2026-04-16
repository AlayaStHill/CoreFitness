using CoreFitness.Domain.Aggregates.WorkoutTypes;
using CoreFitness.Infrastructure.Persistence.EfCore.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CoreFitness.Infrastructure.Persistence.EfCore.Seed.WorkoutTypes;

public static class WorkoutTypeInitializer
{
    public static async Task InitializeDefaultAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<PersistenceContext>();

        var groupClassCategory = await dbContext.WorkoutCategories.FirstOrDefaultAsync(x => x.Title == "Group Class");
        var personalTrainingCategory = await dbContext.WorkoutCategories.FirstOrDefaultAsync(x => x.Title == "Personal Training");
        var onlineTrainingCategory = await dbContext.WorkoutCategories.FirstOrDefaultAsync(x => x.Title == "Online Training");

        if (groupClassCategory is null || personalTrainingCategory is null || onlineTrainingCategory is null)
            return;

        if (!await dbContext.WorkoutTypes.AnyAsync(x => x.Title == "HIIT" && x.WorkoutCategoryId == groupClassCategory.Id))
        {
            dbContext.WorkoutTypes.Add(WorkoutType.Create("HIIT", groupClassCategory.Id));
        }
        if (!await dbContext.WorkoutTypes.AnyAsync(x => x.Title == "HIIT" && x.WorkoutCategoryId == onlineTrainingCategory.Id))
        {
            dbContext.WorkoutTypes.Add(WorkoutType.Create("HIIT", onlineTrainingCategory.Id));
        }

        if (!await dbContext.WorkoutTypes.AnyAsync(x => x.Title == "Strength Circuit" && x.WorkoutCategoryId == groupClassCategory.Id))
        {
            dbContext.WorkoutTypes.Add(WorkoutType.Create("Strength Circuit", groupClassCategory.Id));
        }

        if (!await dbContext.WorkoutTypes.AnyAsync(x => x.Title == "Strength Circuit" && x.WorkoutCategoryId == onlineTrainingCategory.Id))
        {
            dbContext.WorkoutTypes.Add(WorkoutType.Create("Strength Circuit", onlineTrainingCategory.Id));
        }

        if (!await dbContext.WorkoutTypes.AnyAsync(x => x.Title == "Yoga Flow" && x.WorkoutCategoryId == onlineTrainingCategory.Id))
        {
            dbContext.WorkoutTypes.Add(WorkoutType.Create("Yoga Flow", onlineTrainingCategory.Id));
        }
        if (!await dbContext.WorkoutTypes.AnyAsync(x => x.Title == "Yoga Flow" && x.WorkoutCategoryId == groupClassCategory.Id))
        {
            dbContext.WorkoutTypes.Add(WorkoutType.Create("Yoga Flow", groupClassCategory.Id));
        }

        await dbContext.SaveChangesAsync(CancellationToken.None);
    }
}
