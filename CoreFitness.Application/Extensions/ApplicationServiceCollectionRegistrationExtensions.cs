using CoreFitness.Application.Admin.WorkoutSessions;
using CoreFitness.Application.CustomerService.ContatRequests;
using CoreFitness.Application.MembershipTypes;
using CoreFitness.Application.MyAccount;
using CoreFitness.Domain.Aggregates.WorkoutSessions.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CoreFitness.Application.Extensions;

public static class ApplicationServiceCollectionRegistrationExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddScoped<IContactRequestService, ContactRequestService>();
        services.AddScoped<IMembershipTypeService, MembershipTypeService>();
        services.AddScoped<WorkoutBookingDomainService>();
        services.AddScoped<IMyAccountBookingService, MyAccountBookingService>();
        services.AddScoped<IMyAccountMembershipService, MyAccountMembershipService>();
        services.AddScoped<IWorkoutSessionService, WorkoutSessionService>();

        return services;
    }
}