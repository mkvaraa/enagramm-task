using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoneyTransfer.Application.DTOs;
using MoneyTransfer.Application.Interfaces;

namespace MoneyTransfer.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionsController : ControllerBase
{
    private readonly IAppDbContext _db;

    public TransactionsController(IAppDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TransactionDto>>> GetAll(CancellationToken ct)
    {
        var list = await _db.Transactions
            .OrderByDescending(t => t.CreatedAt)
            .Select(t => new TransactionDto(t.Id, t.FromAccountId, t.ToAccountId, t.Amount, t.Currency, t.CreatedAt))
            .ToListAsync(ct);

        return Ok(list);
    }
}