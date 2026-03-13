using CoreFitness.Domain.Abstractions.Loggings;
using Microsoft.Extensions.DependencyInjection;

namespace CoreFitness.Infrastructure.Loggings;

public static class LoggingRegistrationExtension
{
    public static IServiceCollection AddLogging(this IServiceCollection services)
    {
        services.AddScoped<IDomainLogger, Logger>();

        return services;
    }
}