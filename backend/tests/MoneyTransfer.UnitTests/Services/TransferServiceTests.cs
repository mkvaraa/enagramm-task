using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MoneyTransfer.Application.DTOs;
using MoneyTransfer.Application.Services;
using MoneyTransfer.Domain.Entities;
using MoneyTransfer.Infrastructure.Persistence;
using MoneyTransfer.UnitTests.Helpers;

namespace MoneyTransfer.UnitTests.Services;

public class TransferServiceTests
{
    private readonly AppDbContext _db;
    private readonly TransferService _service;
    private readonly Account _alice;
    private readonly Account _bob;

    public TransferServiceTests()
    {
        _db = TestDbContextFactory.Create();

        _alice = new Account("A001", "Alice", 1000m, "USD");
        _bob   = new Account("A002", "Bob",    500m, "USD");

        _db.Accounts.AddRange(_alice, _bob);
        _db.SaveChanges();

        _service = new TransferService(_db, new FakeTransactionManager());
    }

    [Fact]
    public async Task Transfer_ValidRequest_DebitsSenderAndCreditsReceiver()
    {
        var request = new TransferRequest(_alice.Id, _bob.Id, 300m);

        var result = await _service.TransferAsync(request);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Amount.Should().Be(300m);

        (await _db.Accounts.FindAsync(_alice.Id))!.Balance.Should().Be(700m);
        (await _db.Accounts.FindAsync(_bob.Id))!.Balance.Should().Be(800m);

        (await _db.Transactions.CountAsync()).Should().Be(1);
    }

    [Fact]
    public async Task Transfer_InsufficientFunds_ReturnsFailureAndDoesNotChangeBalances()
    {
        var request = new TransferRequest(_alice.Id, _bob.Id, 5000m);

        var result = await _service.TransferAsync(request);

        result.IsSuccess.Should().BeFalse();
        result.ErrorCode.Should().Be("INSUFFICIENT_FUNDS");

        (await _db.Accounts.FindAsync(_alice.Id))!.Balance.Should().Be(1000m);
        (await _db.Accounts.FindAsync(_bob.Id))!.Balance.Should().Be(500m);

        (await _db.Transactions.CountAsync()).Should().Be(0);
    }

    [Fact]
    public async Task Transfer_SameAccount_ReturnsFailure()
    {
        var request = new TransferRequest(_alice.Id, _alice.Id, 100m);

        var result = await _service.TransferAsync(request);

        result.IsSuccess.Should().BeFalse();
        result.ErrorCode.Should().Be("SAME_ACCOUNT");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-50)]
    public async Task Transfer_InvalidAmount_ReturnsFailure(decimal amount)
    {
        var request = new TransferRequest(_alice.Id, _bob.Id, amount);

        var result = await _service.TransferAsync(request);

        result.IsSuccess.Should().BeFalse();
        result.ErrorCode.Should().Be("INVALID_AMOUNT");
    }

    [Fact]
    public async Task Transfer_UnknownSenderAccount_ReturnsFailure()
    {
        var request = new TransferRequest(Guid.NewGuid(), _bob.Id, 100m);

        var result = await _service.TransferAsync(request);

        result.IsSuccess.Should().BeFalse();
        result.ErrorCode.Should().Be("FROM_ACCOUNT_NOT_FOUND");
    }

    [Fact]
    public async Task Transfer_DifferentCurrencies_ReturnsFailure()
    {
        var eur = new Account("A003", "Eve", 100m, "EUR");
        _db.Accounts.Add(eur);
        await _db.SaveChangesAsync();

        var request = new TransferRequest(_alice.Id, eur.Id, 50m);

        var result = await _service.TransferAsync(request);

        result.IsSuccess.Should().BeFalse();
        result.ErrorCode.Should().Be("CURRENCY_MISMATCH");
    }
}