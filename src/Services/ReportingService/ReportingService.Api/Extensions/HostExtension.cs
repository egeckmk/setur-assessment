using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Polly;
using ReportingService.Api.Infrastructure.Context;

namespace ReportingService.Api.Extensions;

public static class HostExtension
{
    public static IApplicationBuilder MigrateDbContext(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var services = scope.ServiceProvider;

        var logger = services.GetRequiredService<ILogger<ReportingContext>>();

        var context = services.GetService<ReportingContext>();

        try
        {
            logger.LogInformation("Migrating database associated with context {DbContextName}", nameof(ReportingContext));

            var retry = Policy.Handle<SqlException>()
                .WaitAndRetry(new TimeSpan[]
                {
                    TimeSpan.FromSeconds(3),
                    TimeSpan.FromSeconds(5),
                    TimeSpan.FromSeconds(8),
                });
            
            context.Database.EnsureCreated();
            context.Database.Migrate();
                
            logger.LogInformation("Migrated database associated with context {DbContextName}", nameof(ReportingContext));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while migrating the database used on context {DbContextName}", nameof(ReportingContext));
        }

        return app;
    }
}