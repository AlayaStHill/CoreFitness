using CoreFitness.Infrastructure.Persistence.EfCore.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CoreFitness.Infrastructure.Persistence;

// behöver vara async eftersom infrastructureinitializer är async?
// måste hämtas ut från ett scope?
internal static class PersistenceDatabaseInitializer
{
    public static async Task InitializeDatabaseAsync(IServiceProvider serviceProvider, IHostEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(serviceProvider);
        ArgumentNullException.ThrowIfNull(environment);

        
        if (environment.IsDevelopment())
        {
            // För att DbContext är scoped, få en instans av context som är scoped
            using IServiceScope scope = serviceProvider.CreateScope();
            PersistenceContext context = scope.ServiceProvider.GetRequiredService<PersistenceContext>();
            // om databasen inte finns, skapa den
            await context.Database.EnsureCreatedAsync();
        }
        else
        {
            using IServiceScope scope = serviceProvider.CreateScope();
            PersistenceContext context = scope.ServiceProvider.GetRequiredService<PersistenceContext>();
            // Finns det en migrering som inte har körts så kör den, annars gör den inget
            await context.Database.MigrateAsync();
        }
    }


}

