using CoreFitness.Application.CustomerService.ContatRequests;
using CoreFitness.Application.Members;
using CoreFitness.Application.MembershipTypes;
using CoreFitness.Infrastructure.Persistence.EfCore.Repositories.CustomerService;
using CoreFitness.Infrastructure.Persistence.EfCore.Repositories.Members;
using CoreFitness.Infrastructure.Persistence.EfCore.Repositories.MembershipTypes;
using Microsoft.Extensions.DependencyInjection;

namespace CoreFitness.Infrastructure.Persistence.EfCore.Repositories;

public static class RepositoryRegistrationExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddScoped<IContactRequestRepository, ContactRequestRepository>();
        services.AddScoped<IMembershipTypeRepository, MembershipTypeRepository>();
        services.AddScoped<IMemberRepository, MemberRepository>();

        return services;
    }
}
