namespace MoneyTransfer.Application.DTOs;

public record AccountDto(
    Guid Id,
    string Number,
    string OwnerName,
    decimal Balance,
    string Currency
);