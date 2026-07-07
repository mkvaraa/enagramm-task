using Microsoft.EntityFrameworkCore;
using MoneyTransfer.Domain.Entities;

namespace MoneyTransfer.Infrastructure.Persistence;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        if (await db.Accounts.AnyAsync()) return;

        db.Accounts.AddRange(
            new Account("GE001", "Alice",   1000m, "USD"),
            new Account("GE002", "Bob",      500m, "USD"),
            new Account("GE003", "Charlie",  200m, "USD")
        );

        await db.SaveChangesAsync();
    }
}