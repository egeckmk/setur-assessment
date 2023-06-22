
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using PhonebookService.Api.Infrastructure.Context;

namespace PhonebookService.Api.Extensions;

public static class DbContextRegistration
{
    public static IServiceCollection ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<PhoneBookContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), sqlServerOptionsAction: sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly(typeof(Program).GetTypeInfo().Assembly.GetName().Name);
                    sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                });
            });
        return services;
    }
}