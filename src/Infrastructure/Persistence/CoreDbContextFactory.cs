using Microsoft.EntityFrameworkCore;

namespace CBWC.Infrastructure.Persistence;

public interface ICoreDbContextFactory
{
    Task<ICoreDbContext> CreateAsync();
    Task<ICoreDbContext> CreateWithTrackingAsync();
}

public class CoreDbContextFactory(
    IDbContextFactory<CoreDbContext> contextFactory) : ICoreDbContextFactory
{
    private readonly IDbContextFactory<CoreDbContext> _contextFactory = contextFactory;

    public async Task<ICoreDbContext> CreateAsync()
    {
        var context = await _contextFactory.CreateDbContextAsync();

        context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

        return context;
    }

    public async Task<ICoreDbContext> CreateWithTrackingAsync()
    {
        var context = await _contextFactory.CreateDbContextAsync();

        context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;

        return context;
    }
}
