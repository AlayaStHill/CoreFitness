using CoreFitness.Domain.Abstractions.Loggings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CoreFitness.Infrastructure.Persistence.EfCore.Contexts;
// Hanterar kopplingen med databasen och konfigurationen av DbContext beroende på miljö (utveckling eller produktion).
public static class ContextsRegistrationExtension
{
    public static IServiceCollection AddEfCoreContexts(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(environment);

        if (environment.IsDevelopment())
        {
            services.AddSingleton<SqliteConnection>(_ =>
            {
                SqliteConnection connection = new SqliteConnection("Data Source=:memory:;");
                connection.Open();

                return connection;
            });

            services.AddDbContext<PersistenceContext>((serviceProvider, options) =>
            {
                SqliteConnection connection = serviceProvider.GetRequiredService<SqliteConnection>();
                options.UseSqlite(connection);
            });
        }
        else
        {
            services.AddDbContext<PersistenceContext>((serviceProvider, options) =>
            {
                IDomainLogger logger = serviceProvider.GetRequiredService<IDomainLogger>();

                try
                {
                    string connectionString = configuration.GetConnectionString("ProductionDatabase")
                        ?? throw new InvalidOperationException("Production database connection string not found.");

                    options.UseSqlServer(connectionString);
                }
                catch (Exception ex)
                {
                    logger.Log(ex);
                    throw;
                }
            });
        }

        return services;
    }
}
