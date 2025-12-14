using Fundo.Domain.Entities;
using Fundo.Domain.Enums;
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
            new(25000m, 18750m, "John Doe", LoanStatus.Active),
            new(15000m, 0m, "Jane Smith", LoanStatus.Paid),
            new(50000m, 32500m, "Robert Johnson", LoanStatus.Active),
            new(10000m, 0m, "Emily Williams", LoanStatus.Paid),
            new(75000m, 72000m, "Michael Brown", LoanStatus.Active)
        };

        db.Loans.AddRange(loans);
        await db.SaveChangesAsync(cancellationToken);
    }
}