namespace ReportingService.Api.Infrastructure.Context;

public class ReportingContextSeed
{
    public async Task SeedAsync(ReportingContext context, IWebHostEnvironment env, ILogger<ReportingContextSeed> logger)
    {
        Console.WriteLine("Seeder Method working...");
        Thread.Sleep(4000);
        Console.WriteLine("Seeder Method worked...");
    }
}