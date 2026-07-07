using Microsoft.EntityFrameworkCore;
using MoneyTransfer.Domain.Entities;

namespace MoneyTransfer.Application.Interfaces;

public interface IAppDbContext
{
    DbSet<Account> Accounts { get; }
    DbSet<Transaction> Transactions { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}