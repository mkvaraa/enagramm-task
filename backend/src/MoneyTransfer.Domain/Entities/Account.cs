using MoneyTransfer.Domain.Common;

namespace MoneyTransfer.Domain.Entities;

public class Account
{
    public Guid Id { get; private set; }
    public string Number { get; private set; } = null!;
    public string OwnerName { get; private set; } = null!;
    public decimal Balance { get; private set; }
    public string Currency { get; private set; } = null!;
    public byte[] RowVersion { get; private set; } = Array.Empty<byte>();

    private Account() { }

    public Account(string number, string ownerName, decimal initialBalance, string currency)
    {
        if (string.IsNullOrWhiteSpace(number))
            throw new DomainException("INVALID_ACCOUNT_NUMBER", "Account number is required.");
        if (initialBalance < 0)
            throw new DomainException("INVALID_INITIAL_BALANCE", "Initial balance cannot be negative.");

        Id = Guid.NewGuid();
        Number = number;
        OwnerName = ownerName;
        Balance = initialBalance;
        Currency = currency;
    }

    public void Debit(decimal amount)
    {
        if (amount <= 0)
            throw new DomainException("INVALID_AMOUNT", "Amount must be positive.");
        if (Balance < amount)
            throw new DomainException("INSUFFICIENT_FUNDS", $"Account {Number} has insufficient funds.");

        Balance -= amount;
    }

    public void Credit(decimal amount)
    {
        if (amount <= 0)
            throw new DomainException("INVALID_AMOUNT", "Amount must be positive.");

        Balance += amount;
    }
}