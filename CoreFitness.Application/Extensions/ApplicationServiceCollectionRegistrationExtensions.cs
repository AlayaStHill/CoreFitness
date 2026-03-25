using CoreFitness.Application.CustomerService.ContatRequests;
using Microsoft.Extensions.DependencyInjection;

namespace CoreFitness.Application.Extensions;

public static class ApplicationServiceCollectionRegistrationExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddScoped<IContactRequestService, ContactRequestService>();

        return services;
    }
}