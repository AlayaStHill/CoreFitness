using CoreFitness.Domain.Aggregates.CustomerService;
using CoreFitness.Domain.Shared.ValueObjects.EmailAddresses;
using CoreFitness.Domain.Shared.ValueObjects.PhoneNumbers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreFitness.Infrastructure.Persistence.EfCore.Configurations;

public class ContactRequestConfiguration : IEntityTypeConfiguration<ContactRequest>
{
    public void Configure(EntityTypeBuilder<ContactRequest> builder)
    {
        builder.ToTable("ContactRequests");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .ValueGeneratedNever();

        builder.Property(e => e.FirstName)
            .HasMaxLength(ContactRequest.NameMaxLength)
            .IsRequired();

        builder.Property(e => e.LastName)
            .HasMaxLength(ContactRequest.NameMaxLength)
            .IsRequired();

        builder.Property(e => e.Message)
            .HasMaxLength(ContactRequest.MessageMaxLength)
            .IsRequired();

        builder.Property(e => e.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("SYSUTCDATETIME()"); 

        builder.Property(e => e.MarkedAsRead)
            .IsRequired()
        .HasDefaultValue(false);

        builder.OwnsOne(e => e.Email, email =>
        {
            email.Property(e => e.Email)
                .HasColumnName("Email")
                .HasMaxLength(EmailAddress.EmailMaxLength)
                .IsRequired();
        });

        builder.OwnsOne(e => e.PhoneNumber, phone =>
        {
            phone.Property(p => p.Phone)
                .HasColumnName("PhoneNumber")
                .HasMaxLength(PhoneNumber.PhoneNumberMaxLength);
        });

        builder.HasIndex(e => e.CreatedAt);
    }
}