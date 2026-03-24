using CoreFitness.Infrastructure.Persistence.EfCore.Contexts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CoreFitness.Infrastructure.Loggings;

namespace CoreFitness.Infrastructure.Extensions;

public static class InfrastructureServiceCollectionRegistrationExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(environment);

        services.AddLogger();
        services.AddPersistence(configuration, environment);
        services.AddEfCoreContexts(configuration, environment);
        services.AddRepositories();

        return services;
    }
}
