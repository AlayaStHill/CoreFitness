using CoreFitness.Infrastructure.Persistence;
using CoreFitness.Infrastructure.Persistence.EfCore.Seed.MembershipTypes;
using Infrastructure.Identity.Data;
using Microsoft.Extensions.Hosting;

namespace CoreFitness.Infrastructure;

public class InfrastructureInitializer
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider, IHostEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(serviceProvider);
        ArgumentNullException.ThrowIfNull(environment);

        await PersistenceDatabaseInitializer.InitializeDatabaseAsync(serviceProvider, environment);

        await IdentityInitializer.InitializeDefaultRolesAsync(serviceProvider);
        await IdentityInitializer.InitializeDefaultAdminAccountsAsync(serviceProvider);
        await MembershipTypeInitializer.InitializeDefaultAsync(serviceProvider);
    }
}
