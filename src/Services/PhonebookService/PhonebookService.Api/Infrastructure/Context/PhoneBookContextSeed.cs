using Microsoft.Data.SqlClient;
using Polly;

namespace PhonebookService.Api.Infrastructure.Context;

public class PhoneBookContextSeed
{
    public async Task SeedAsync(PhoneBookContext context, IWebHostEnvironment env, ILogger<PhoneBookContextSeed> logger)
    {
        Console.WriteLine("Seeder Method working...");
        System.Threading.Thread.Sleep(4000);
        Console.WriteLine("Seeder Method worked...");
    }
}