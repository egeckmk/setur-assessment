using Microsoft.EntityFrameworkCore;
using ReportingService.Api.Core.Domain.Models;
using ReportingService.Api.Infrastructure.EntityConfigurations;

namespace ReportingService.Api.Infrastructure.Context;

public class ReportingContext : DbContext
{
    public const string DEFAULT_SCHEMA = "reporting";

    public ReportingContext(DbContextOptions<ReportingContext> options) : base(options)
    {
    }

    public DbSet<Report> Reports { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new ReportEntityTypeConfiguration());
    }
}