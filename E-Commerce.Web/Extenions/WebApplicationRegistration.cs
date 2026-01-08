using E_Commerce.Domain.Contracts;
using E_Commerce.Persistence.Data.DbContexts;
using E_Commerce.Persistence.IdentityData.DbContexts;
using E_Commerce.Services_Abstraction;
using Hangfire;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Web.Extenions;

public static class WebApplicationRegistration
{
    public static async Task<WebApplication> MigrateDatabaseAsync(this WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();
        var dbContextService = scope.ServiceProvider.GetRequiredService<StoreDbContext>();
        var pendingMigration = await dbContextService.Database.GetPendingMigrationsAsync();
        if (pendingMigration.Any())
            await dbContextService.Database.MigrateAsync();

        return app;
    }    public static async Task<WebApplication> MigrateIdentityDatabaseAsync(this WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();
        var dbContextService = scope.ServiceProvider.GetRequiredService<StoreIdentityDbContext>();
        var pendingMigration = await dbContextService.Database.GetPendingMigrationsAsync();
        if (pendingMigration.Any())
            await dbContextService.Database.MigrateAsync();

        return app;
    }

    public static async Task<WebApplication> SeedDatabaseAsync(this WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();
        var dataInitializerService = scope.ServiceProvider.GetRequiredKeyedService<IDataInitializer>("Default");
        await dataInitializerService.InitializeAsync();
        return app;
    }    
    public static async Task<WebApplication> SeedIdentityDatabaseAsync(this WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();
        var dataInitializerService = scope.ServiceProvider.GetRequiredKeyedService<IDataInitializer>("Identity");
        await dataInitializerService.InitializeAsync();
        return app;
    }

    public static async Task<WebApplication> UseRefreshTokenJobsAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var recurringJobManager = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();
        var authService = scope.ServiceProvider.GetRequiredService<IAuthenticationService>();

        recurringJobManager.AddOrUpdate(
            "Clean-Refresh-Tokens",
            () => authService.CleanExpiredRefreshTokensAsync(),
            Cron.Weekly
        );

        return app;
    }

}