namespace MoneyTransfer.Application.DTOs;

public record TransactionDto(
    Guid Id,
    Guid FromAccountId,
    Guid ToAccountId,
    decimal Amount,
    string Currency,
    DateTime CreatedAt
);