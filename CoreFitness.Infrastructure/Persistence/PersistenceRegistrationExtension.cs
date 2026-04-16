using CoreFitness.Application.Shared;
using CoreFitness.Infrastructure.Persistence.EfCore.Contexts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CoreFitness.Infrastructure.Persistence;

public static class PersistenceRegistrationExtension
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(environment);

        services.AddEfCoreContexts(configuration, environment);
        // IUnitOfWork --> DataContext
        services.AddScoped<IUnitOfWork>(serviceProvider => serviceProvider.GetRequiredService<PersistenceContext>());

        return services;
    }
}

