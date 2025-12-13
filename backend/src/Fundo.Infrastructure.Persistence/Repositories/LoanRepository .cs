using Fundo.Domain.Entities;
using Fundo.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Fundo.Infrastructure.Persistence.Repositories;

internal class LoanRepository : ILoanRepository

{
    private readonly AppDbContext _db;

    public LoanRepository(AppDbContext db) => _db = db;

    public async Task AddAsync(Loan loan, CancellationToken cancellationToken = default)
    {
        await _db.Loans.AddAsync(loan, cancellationToken);
    }

    public async Task<Loan?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _db.Loans.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<Loan?> GetByIdForUpdateAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _db.Loans.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Loan>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _db.Loans
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}