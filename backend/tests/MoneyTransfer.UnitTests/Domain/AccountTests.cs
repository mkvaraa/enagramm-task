using FluentAssertions;
using MoneyTransfer.Domain.Common;
using MoneyTransfer.Domain.Entities;

namespace MoneyTransfer.UnitTests.Domain;

public class AccountTests
{
    [Fact]
    public void Debit_WithSufficientFunds_DecreasesBalance()
    {
        var account = new Account("A001", "Alice", 1000m, "USD");

        account.Debit(300m);

        account.Balance.Should().Be(700m);
    }

    [Fact]
    public void Debit_WithInsufficientFunds_ThrowsDomainException()
    {
        var account = new Account("A001", "Alice", 100m, "USD");

        var act = () => account.Debit(500m);

        act.Should().Throw<DomainException>()
            .Which.Code.Should().Be("INSUFFICIENT_FUNDS");
    }

    [Fact]
    public void Credit_IncreasesBalance()
    {
        var account = new Account("A001", "Alice", 100m, "USD");

        account.Credit(50m);

        account.Balance.Should().Be(150m);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-10)]
    public void Debit_WithInvalidAmount_ThrowsDomainException(decimal amount)
    {
        var account = new Account("A001", "Alice", 1000m, "USD");

        var act = () => account.Debit(amount);

        act.Should().Throw<DomainException>()
            .Which.Code.Should().Be("INVALID_AMOUNT");
    }
}