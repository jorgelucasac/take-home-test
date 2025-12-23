using Fundo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Fundo.Infrastructure.Persistence.Seed;

public static class LoanSeed
{
    public static async Task EnsureSeededAsync(IServiceProvider provider, CancellationToken cancellationToken = default)
    {
        var scope = provider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        try
        {
            var has = await db.Loans.AnyAsync(cancellationToken);
            if (has) return;
        }
        catch
        {
            // ignored
        }

        db.Database.EnsureCreated();

        var loans = new List<Loan>
        {
            new(25000m, 18750m, "John Doe"),
            new(15000m, 0m, "Jane Smith"),
            new(50000m, 32500m, "Robert Johnson"),
            new(10000m, 0m, "Emily Williams"),
            new(75000m, 72000m, "Michael Brown")
        };

        db.Loans.AddRange(loans);
        await db.SaveChangesAsync(cancellationToken);
    }
}