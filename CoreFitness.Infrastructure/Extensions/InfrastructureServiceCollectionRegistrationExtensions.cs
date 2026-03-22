using CoreFitness.Infrastructure.Persistence.EfCore.Contexts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CoreFitness.Infrastructure.Extensions;

public static class InfrastructureServiceCollectionRegistrationExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(environment);

        services.AddLogging();
        services.AddPersistence(configuration, environment);
        services.AddEfCoreContexts(configuration, environment);
        services.AddRepositories();

        return services;
    }
}
