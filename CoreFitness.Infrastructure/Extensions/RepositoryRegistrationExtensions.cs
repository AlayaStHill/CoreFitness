using Microsoft.Extensions.DependencyInjection;

namespace CoreFitness.Infrastructure.Extensions;

public static class RepositoryRegistrationExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        return services;
    }
}
