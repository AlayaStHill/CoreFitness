using CoreFitness.Application.CustomerService.ContatRequests;
using CoreFitness.Infrastructure.Persistence.EfCore.Repositories.CustomerService;
using Microsoft.Extensions.DependencyInjection;

namespace CoreFitness.Infrastructure.Extensions;

public static class RepositoryRegistrationExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddScoped<IContactRequestRepository, ContactRequestRepository>();

        return services;
    }
}
