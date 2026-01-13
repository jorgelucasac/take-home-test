using Fundo.Domain.Entities;
using Fundo.Domain.Repositories;

namespace Fundo.Infrastructure.Persistence.Repositories;

internal class LoanHistoryRepository : ILoanHistoryRepository
{
    private readonly AppDbContext _db;

    public LoanHistoryRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(LoanHistory loanHistory, CancellationToken cancellationToken = default)
    {
        await _db.LoanHistories.AddAsync(loanHistory, cancellationToken);
    }
}