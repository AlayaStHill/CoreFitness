using Application.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace CoreFitness.Application.Extensions;

public static class ApplicationServiceCollectionRegistrationExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddServices();

        return services;
    }
}