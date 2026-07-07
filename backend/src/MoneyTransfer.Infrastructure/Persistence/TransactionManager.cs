using System.Data;
using Microsoft.EntityFrameworkCore;
using MoneyTransfer.Application.Interfaces;

namespace MoneyTransfer.Infrastructure.Persistence;

public class TransactionManager : ITransactionManager
{
    private readonly AppDbContext _db;

    public TransactionManager(AppDbContext db) => _db = db;

    public async Task<T> ExecuteInTransactionAsync<T>(Func<Task<T>> action, CancellationToken ct = default)
    {
        var strategy = _db.Database.CreateExecutionStrategy();
        return await strategy.ExecuteAsync(async () =>
        {
            await using var tx = await _db.Database.BeginTransactionAsync(IsolationLevel.Serializable, ct);
            var result = await action();
            await tx.CommitAsync(ct);
            return result;
        });
    }
}