namespace MoneyTransfer.Domain.Entities;

public class Transaction
{
    public Guid Id { get; private set; }
    public Guid FromAccountId { get; private set; }
    public Guid ToAccountId { get; private set; }
    public decimal Amount { get; private set; }
    public string Currency { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }

    private Transaction() { }

    public Transaction(Guid fromAccountId, Guid toAccountId, decimal amount, string currency)
    {
        Id = Guid.NewGuid();
        FromAccountId = fromAccountId;
        ToAccountId = toAccountId;
        Amount = amount;
        Currency = currency;
        CreatedAt = DateTime.UtcNow;
    }
}