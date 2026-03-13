using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions;

public static class ServiceRegistrationExtension
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
       /* Defensiv kod, guard clause, tidigt stoppa felaktig input - förhindrar ett mer otydligt NullReferenceException. Regel: publika metoder ska validera sina argument.
          När aspnet skapar builder skapas en instans av IServiceCollection (builder.Services), som denna metod anropas på (services är aldrig null) */
        ArgumentNullException.ThrowIfNull(services);


        return services;
    }
}