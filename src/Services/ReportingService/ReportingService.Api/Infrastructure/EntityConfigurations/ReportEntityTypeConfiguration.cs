using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReportingService.Api.Core.Domain.Models;
using ReportingService.Api.Infrastructure.Context;

namespace ReportingService.Api.Infrastructure.EntityConfigurations;

public class ReportEntityTypeConfiguration : IEntityTypeConfiguration<Report>
{
    public void Configure(EntityTypeBuilder<Report> builder)
    {
        builder.ToTable("Report", ReportingContext.DEFAULT_SCHEMA);

        builder.HasKey(pi => pi.Id);
    }
}