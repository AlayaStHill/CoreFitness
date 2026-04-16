using CoreFitness.Domain.Abstractions.Loggings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CoreFitness.Infrastructure.Loggings;

public static class LoggingRegistrationExtension
{
    public static IServiceCollection AddLogger(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddScoped<IDomainLogger, Logger>();

        return services;
    }
}