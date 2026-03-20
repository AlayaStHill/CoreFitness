using CoreFitness.Infrastructure.Persistence.EfCore.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CoreFitness.Infrastructure;

public static class PersistenceDatabaseInitializer
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider, IHostEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(serviceProvider);
        ArgumentNullException.ThrowIfNull(environment);

        if (environment.IsDevelopment())
        {
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
