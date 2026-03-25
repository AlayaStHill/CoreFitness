using CoreFitness.Infrastructure.Persistence;
using Microsoft.Extensions.Hosting;

namespace CoreFitness.Infrastructure;

// registrera databaseinitializer
public class InfrastructureInitializer
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider, IHostEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(serviceProvider);
        ArgumentNullException.ThrowIfNull(environment);

        await PersistenceDatabaseInitializer.InitializeDatabaseAsync(serviceProvider, environment); 


    }
}
