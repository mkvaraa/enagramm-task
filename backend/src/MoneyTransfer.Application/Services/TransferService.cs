using Microsoft.EntityFrameworkCore;
using MoneyTransfer.Application.Common;
using MoneyTransfer.Application.DTOs;
using MoneyTransfer.Application.Interfaces;
using MoneyTransfer.Domain.Common;
using MoneyTransfer.Domain.Entities;

namespace MoneyTransfer.Application.Services;

public class TransferService : ITransferService
{
    private readonly IAppDbContext _db;
    private readonly ITransactionManager _tx;

    public TransferService(IAppDbContext db, ITransactionManager tx)
    {
        _db = db;
        _tx = tx;
    }

    public async Task<Result<TransactionDto>> TransferAsync(TransferRequest request, CancellationToken ct = default)
    {
        if (request.FromAccountId == request.ToAccountId)
            return Result<TransactionDto>.Failure("SAME_ACCOUNT", "Cannot transfer to the same account.");

        if (request.Amount <= 0)
            return Result<TransactionDto>.Failure("INVALID_AMOUNT", "Amount must be positive.");

        return await _tx.ExecuteInTransactionAsync(async () =>
        {
            var from = await _db.Accounts.FirstOrDefaultAsync(a => a.Id == request.FromAccountId, ct);
            var to   = await _db.Accounts.FirstOrDefaultAsync(a => a.Id == request.ToAccountId, ct);

            if (from is null)
                return Result<TransactionDto>.Failure("FROM_ACCOUNT_NOT_FOUND", "Sender account not found.");
            if (to is null)
                return Result<TransactionDto>.Failure("TO_ACCOUNT_NOT_FOUND", "Receiver account not found.");

            if (from.Currency != to.Currency)
                return Result<TransactionDto>.Failure("CURRENCY_MISMATCH", "Accounts must have the same currency.");

            try
            {
                from.Debit(request.Amount);
                to.Credit(request.Amount);
            }
            catch (DomainException ex)
            {
                return Result<TransactionDto>.Failure(ex.Code, ex.Message);
            }

            var transaction = new Transaction(from.Id, to.Id, request.Amount, from.Currency);
            _db.Transactions.Add(transaction);

            await _db.SaveChangesAsync(ct);

            var dto = new TransactionDto(
                transaction.Id, transaction.FromAccountId, transaction.ToAccountId,
                transaction.Amount, transaction.Currency, transaction.CreatedAt);

            return Result<TransactionDto>.Success(dto);
        }, ct);
    }
}