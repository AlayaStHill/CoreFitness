using CoreFitness.Domain.Aggregates.WorkoutCategories;
using CoreFitness.Infrastructure.Persistence.EfCore.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CoreFitness.Infrastructure.Persistence.EfCore.Seed.WorkoutCategories;

public static class WorkoutCategoryInitializer
{
    public static async Task InitializeDefaultAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<PersistenceContext>();

        if (!await dbContext.WorkoutCategories.AnyAsync(x => x.Title == "Group Class"))
        {
            dbContext.WorkoutCategories.Add(WorkoutCategory.Create("Group Class"));
        }

        if (!await dbContext.WorkoutCategories.AnyAsync(x => x.Title == "Personal Training"))
        {
            dbContext.WorkoutCategories.Add(WorkoutCategory.Create("Personal Training"));
        }

        if (!await dbContext.WorkoutCategories.AnyAsync(x => x.Title == "Online Training"))
        {
            dbContext.WorkoutCategories.Add(WorkoutCategory.Create("Online Training"));
        }

        await dbContext.SaveChangesAsync(CancellationToken.None);
    }
}
