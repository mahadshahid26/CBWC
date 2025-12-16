using CBWC.Application.Interfaces.Repositories;
using CBWC.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CBWC.Infrastructure.Persistence;

public static class Registration
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContextPool<CoreDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("Default"),
                x => x.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(10),
                    errorNumbersToAdd: null)));

        services.AddDbContextFactory<CoreDbContext>(options =>
            options
                .UseSqlServer(configuration.GetConnectionString("Default"))
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

        services.AddSingleton<ICoreDbContextFactory, CoreDbContextFactory>();

        services.AddRepositories();
        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IMemberRepo, MemberRepo>();

        return services;
    }
}
