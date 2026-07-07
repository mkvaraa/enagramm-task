namespace MoneyTransfer.Application.Interfaces;

public interface ITransactionManager
{
    Task<T> ExecuteInTransactionAsync<T>(Func<Task<T>> action, CancellationToken cancellationToken = default);
}