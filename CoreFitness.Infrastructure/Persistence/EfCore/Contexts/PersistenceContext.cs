using CoreFitness.Application.Shared;
using Microsoft.EntityFrameworkCore;

namespace CoreFitness.Infrastructure.Persistence.EfCore.Contexts;

public class PersistenceContext(DbContextOptions<PersistenceContext> options) : DbContext(options), IUnitOfWork
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PersistenceContext).Assembly);
    }

    /* Add entities below: */
















    //public override async Task<int> SaveChangesAsync(CancellationToken ct)
    //{
    //    DateTime utcNow = DateTime.UtcNow;

    //    foreach (EntityEntry<EntityBase> entry in ChangeTracker.Entries<EntityBase>())
    //    {
    //        if (entry.State == EntityState.Added)
    //        {
    //            entry.Entity.CreatedAt = utcNow;
    //            entry.Entity.ModifiedAt = utcNow;
    //        }
    //        else if (entry.State == EntityState.Modified)
    //        {
    //            entry.Entity.ModifiedAt = utcNow;
    //        }
    //    }

    //    return await base.SaveChangesAsync(ct);
    //}

}

