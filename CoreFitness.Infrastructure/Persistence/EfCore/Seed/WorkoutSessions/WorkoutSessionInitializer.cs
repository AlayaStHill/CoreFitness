using CoreFitness.Domain.Aggregates.WorkoutSessions;
using CoreFitness.Infrastructure.Persistence.EfCore.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CoreFitness.Infrastructure.Persistence.EfCore.Seed.WorkoutSessions;

public static class WorkoutSessionInitializer
{
    public static async Task InitializeDefaultAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<PersistenceContext>();

        var workoutTypeIds = await dbContext.WorkoutTypes
            .Where(x => x.Title == "Yoga Flow" || x.Title == "HIIT" || x.Title == "Strength Circuit")
            .Select(x => x.Id)
            .ToListAsync();

        if (workoutTypeIds.Count == 0)
            return;

        var sessionSlots = new[]
        {
            new { DayOffset = 1, Hour = 7, Minute = 30 },
            new { DayOffset = 2, Hour = 12, Minute = 0 },
            new { DayOffset = 3, Hour = 18, Minute = 0 },
            new { DayOffset = 4, Hour = 17, Minute = 30 },
            new { DayOffset = 5, Hour = 19, Minute = 0 },
            new { DayOffset = 7, Hour = 8, Minute = 0 },
            new { DayOffset = 8, Hour = 12, Minute = 30 },
            new { DayOffset = 9, Hour = 18, Minute = 30 },
            new { DayOffset = 10, Hour = 17, Minute = 0 },
            new { DayOffset = 11, Hour = 19, Minute = 30 },
            new { DayOffset = 14, Hour = 7, Minute = 0 },
            new { DayOffset = 15, Hour = 12, Minute = 0 },
            new { DayOffset = 16, Hour = 18, Minute = 0 },
            new { DayOffset = 17, Hour = 17, Minute = 30 },
            new { DayOffset = 18, Hour = 19, Minute = 0 },
            new { DayOffset = 21, Hour = 8, Minute = 30 },
            new { DayOffset = 23, Hour = 12, Minute = 30 },
            new { DayOffset = 25, Hour = 18, Minute = 30 },
            new { DayOffset = 27, Hour = 17, Minute = 0 },
            new { DayOffset = 29, Hour = 19, Minute = 30 }
        };

        for (var i = 0; i < sessionSlots.Length; i++)
        {
            var slot = sessionSlots[i];
            var workoutTypeId = workoutTypeIds[i % workoutTypeIds.Count];
            var startsAt = UtcAt(slot.DayOffset, slot.Hour, slot.Minute);
            var duration = i % 3 == 0
                ? TimeSpan.FromMinutes(45)
                : i % 3 == 1
                    ? TimeSpan.FromMinutes(60)
                    : TimeSpan.FromMinutes(75);
            var capacity = i % 4 == 0 ? 16 : 20;

            var exists = await dbContext.WorkoutSessions.AnyAsync(x =>
                x.WorkoutTypeId == workoutTypeId &&
                x.StartsAt == startsAt);

            if (exists)
                continue;

            dbContext.WorkoutSessions.Add(WorkoutSession.Create(
                startsAt,
                workoutTypeId,
                duration,
                capacity));
        }

        await dbContext.SaveChangesAsync(CancellationToken.None);
    }

    private static DateTimeOffset UtcAt(int dayOffset, int hour, int minute)
    {
        var date = DateTimeOffset.UtcNow.Date.AddDays(dayOffset);
        var utcDateTime = new DateTime(date.Year, date.Month, date.Day, hour, minute, 0, DateTimeKind.Utc);
        return new DateTimeOffset(utcDateTime);
    }
}
