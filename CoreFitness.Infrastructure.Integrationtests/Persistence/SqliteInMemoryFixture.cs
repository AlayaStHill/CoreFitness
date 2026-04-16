using CoreFitness.Infrastructure.Persistence.EfCore.Contexts;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace CoreFitness.Infrastructure.IntegrationTests.Persistence;

public sealed class SqliteInMemoryFixture : IAsyncLifetime
{
    private SqliteConnection? _sqliteConnection;
    private CultureInfo? _originalDefaultThreadCurrentCulture;
    private CultureInfo? _originalDefaultThreadCurrentUICulture;

    public DbContextOptions<PersistenceContext> Options { get; private set; } = default!;

    /*
    - Skapar SQLite in-memory
    - Öppnar anslutningen
    - Skapar tabeller
    - Sparar DbContextOptions
    */
    public async Task InitializeAsync()
    {
        _originalDefaultThreadCurrentCulture = CultureInfo.DefaultThreadCurrentCulture;
        _originalDefaultThreadCurrentUICulture = CultureInfo.DefaultThreadCurrentUICulture;
        CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
        CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

        _sqliteConnection =
            new SqliteConnection("Data Source=:memory:;Cache=Shared");

        await _sqliteConnection.OpenAsync();

        Options =
            new DbContextOptionsBuilder<PersistenceContext>()
                .UseSqlite(_sqliteConnection)
                .EnableSensitiveDataLogging()
                .Options;

        await using PersistenceContext context =
            new PersistenceContext(Options);

        await context.Database.EnsureCreatedAsync();
    }

    public async Task DisposeAsync()
    {
        if (_sqliteConnection is not null)
            await _sqliteConnection.DisposeAsync();

        CultureInfo.DefaultThreadCurrentCulture = _originalDefaultThreadCurrentCulture;
        CultureInfo.DefaultThreadCurrentUICulture = _originalDefaultThreadCurrentUICulture;
    }


    // Ger varje test en ny DbContext
    public PersistenceContext CreateContext()
    {
        return new PersistenceContext(Options);
    }

    public async Task ClearDatabaseAsync()
    {
        await using PersistenceContext context = CreateContext();

        await context.Database.ExecuteSqlRawAsync("PRAGMA foreign_keys = OFF;");

        try
        {
            var tableNames = context.Model
                .GetEntityTypes()
                .Select(entityType => entityType.GetTableName())
                .OfType<string>()
                .Distinct();

            foreach (var tableName in tableNames)
            {
                await context.Database.ExecuteSqlRawAsync($"DELETE FROM \"{tableName}\";");
            }
        }
        finally
        {
            await context.Database.ExecuteSqlRawAsync("PRAGMA foreign_keys = ON;");
        }
    }
}

// Stänger databasen efter alla tester
[CollectionDefinition(Name)]
public sealed class SqliteInMemoryCollection : ICollectionFixture<SqliteInMemoryFixture>
{
    public const string Name = "SqliteInMemory";
}

