using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhonebookService.Api.Core.Domain;
using PhonebookService.Api.Infrastructure.Context;

namespace PhonebookService.Api.Infrastructure.EntityConfigurations;

public class PersonEntityTypeConfiguration : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.ToTable("Person", PhoneBookContext.DEFAULT_SCHEMA);

        builder.HasKey(pi => pi.Id);
        
        builder.Property(pa => pa.Ad)
            .IsRequired()
            .HasMaxLength(30);
        
        builder.Property(ps => ps.Soyad)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(pf => pf.Firma)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasMany(pc => pc.ContactInfos)
            .WithOne()
            .HasForeignKey(c => c.PersonId);

        builder.Navigation(n => n.ContactInfos).AutoInclude();

    }
}