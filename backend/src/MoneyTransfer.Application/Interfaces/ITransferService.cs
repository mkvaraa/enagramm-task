using MoneyTransfer.Application.Common;
using MoneyTransfer.Application.DTOs;

namespace MoneyTransfer.Application.Interfaces;

public interface ITransferService
{
    Task<Result<TransactionDto>> TransferAsync(TransferRequest request, CancellationToken ct = default);
}