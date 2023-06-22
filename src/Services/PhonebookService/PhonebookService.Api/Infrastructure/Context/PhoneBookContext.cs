using Microsoft.EntityFrameworkCore;
using PhonebookService.Api.Core.Domain;
using PhonebookService.Api.Infrastructure.EntityConfigurations;

namespace PhonebookService.Api.Infrastructure.Context;

public class PhoneBookContext : DbContext
{
    public const string DEFAULT_SCHEMA = "person";

    public PhoneBookContext(DbContextOptions<PhoneBookContext> options) : base(options)
    {
        
    }

    public DbSet<Person> Persons { get; set; }
    public DbSet<ContactInfo> ContactInfos { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new PersonEntityTypeConfiguration());
        builder.ApplyConfiguration(new ContactInfoEntityTypeConfiguration());
    }
}