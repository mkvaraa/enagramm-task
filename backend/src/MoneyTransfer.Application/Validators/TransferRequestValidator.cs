using FluentValidation;
using MoneyTransfer.Application.DTOs;

namespace MoneyTransfer.Application.Validators;

public class TransferRequestValidator : AbstractValidator<TransferRequest>
{
    public TransferRequestValidator()
    {
        RuleFor(x => x.FromAccountId).NotEmpty();
        RuleFor(x => x.ToAccountId).NotEmpty();
        RuleFor(x => x.Amount).GreaterThan(0);
    }
}