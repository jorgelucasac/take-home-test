using Fundo.Domain.Entities;
using Fundo.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Fundo.Infrastructure.Persistence.Seed;

internal static class LoanSeed
{
    public static async Task EnsureSeededAsync(AppDbContext db, CancellationToken cancellationToken = default)
    {
        if (await db.Loans.AnyAsync(cancellationToken)) return;

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