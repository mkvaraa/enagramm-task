using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyTransfer.Domain.Entities;

namespace MoneyTransfer.Infrastructure.Persistence.Configurations;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> b)
    {
        b.ToTable("Transactions");
        b.HasKey(t => t.Id);

        b.Property(t => t.Amount).HasPrecision(18, 2);
        b.Property(t => t.Currency).IsRequired().HasMaxLength(3);

        b.HasOne<Account>()
            .WithMany()
            .HasForeignKey(t => t.FromAccountId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne<Account>()
            .WithMany()
            .HasForeignKey(t => t.ToAccountId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasIndex(t => t.CreatedAt);
    }
}