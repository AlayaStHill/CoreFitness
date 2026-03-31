using CoreFitness.Application.CustomerService.ContatRequests;
using CoreFitness.Application.MembershipTypes;
using Microsoft.Extensions.DependencyInjection;

namespace CoreFitness.Application.Extensions;

public static class ApplicationServiceCollectionRegistrationExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddScoped<IContactRequestService, ContactRequestService>();
        services.AddScoped<IMembershipTypeService, MembershipTypeService>();


        return services;
    }
}