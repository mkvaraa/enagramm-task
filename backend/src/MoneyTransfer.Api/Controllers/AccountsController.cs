using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoneyTransfer.Application.DTOs;
using MoneyTransfer.Application.Interfaces;

namespace MoneyTransfer.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountsController : ControllerBase
{
    private readonly IAppDbContext _db;

    public AccountsController(IAppDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AccountDto>>> GetAll(CancellationToken ct)
    {
        var accounts = await _db.Accounts
            .OrderBy(a => a.Number)
            .Select(a => new AccountDto(a.Id, a.Number, a.OwnerName, a.Balance, a.Currency))
            .ToListAsync(ct);

        return Ok(accounts);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<AccountDto>> GetById(Guid id, CancellationToken ct)
    {
        var account = await _db.Accounts
            .Where(a => a.Id == id)
            .Select(a => new AccountDto(a.Id, a.Number, a.OwnerName, a.Balance, a.Currency))
            .FirstOrDefaultAsync(ct);

        return account is null ? NotFound() : Ok(account);
    }
}