using CBWC.Infrastructure.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CBWC.Infrastructure.Persistence;

public interface ICoreDbContext : IAsyncDisposable
{
    public DbSet<Member> Members { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

public class CoreDbContext(
    DbContextOptions<CoreDbContext> options) : DbContext(options), ICoreDbContext
{
    public DbSet<Member> Members { get; set; } = default!;

    public override Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
        => base.SaveChangesAsync(cancellationToken);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
    }

}
