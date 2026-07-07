namespace MoneyTransfer.Application.DTOs;

public record TransferRequest(
    Guid FromAccountId,
    Guid ToAccountId,
    decimal Amount
);