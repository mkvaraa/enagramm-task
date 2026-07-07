using MoneyTransfer.Application.Interfaces;

namespace MoneyTransfer.UnitTests.Helpers;

public class FakeTransactionManager : ITransactionManager
{
    public Task<T> ExecuteInTransactionAsync<T>(Func<Task<T>> action, CancellationToken ct = default)
        => action();
}