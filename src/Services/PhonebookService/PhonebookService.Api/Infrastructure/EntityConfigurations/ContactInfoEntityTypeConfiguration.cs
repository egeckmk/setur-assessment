using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhonebookService.Api.Core.Domain;
using PhonebookService.Api.Infrastructure.Context;

namespace PhonebookService.Api.Infrastructure.EntityConfigurations;

public class ContactInfoEntityTypeConfiguration : IEntityTypeConfiguration<ContactInfo>
{
    public void Configure(EntityTypeBuilder<ContactInfo> builder)
    {
        builder.ToTable("ContactInfo", PhoneBookContext.DEFAULT_SCHEMA);

        builder.HasKey(pi => pi.Id);
    }
}