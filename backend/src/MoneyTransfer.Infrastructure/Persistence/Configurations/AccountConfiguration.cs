using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyTransfer.Domain.Entities;

namespace MoneyTransfer.Infrastructure.Persistence.Configurations;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> b)
    {
        b.ToTable("Accounts");
        b.HasKey(a => a.Id);

        b.Property(a => a.Number).IsRequired().HasMaxLength(50);
        b.HasIndex(a => a.Number).IsUnique();

        b.Property(a => a.OwnerName).IsRequired().HasMaxLength(200);
        b.Property(a => a.Currency).IsRequired().HasMaxLength(3);
        b.Property(a => a.Balance).HasPrecision(18, 2);

        b.Property(a => a.RowVersion).IsRowVersion();
    }
}