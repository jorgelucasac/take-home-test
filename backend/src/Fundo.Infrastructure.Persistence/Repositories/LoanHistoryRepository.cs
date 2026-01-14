using Fundo.Application.Pagination;
using Fundo.Application.Repositories;
using Fundo.Domain.Entities;
using Microsoft.EntityFrameworkCore;

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

    public async Task<PaginatedResponse<LoanHistory>> GetByLoanIdPagedAsync(
       int loanId,
       PaginationQuery pagination,
       CancellationToken cancellationToken = default)
    {
        var baseQuery = _db.LoanHistories
            .AsNoTracking()
            .Where(x => x.LoanId == loanId);

        var total = await baseQuery.CountAsync(cancellationToken);

        var items = await baseQuery
            .OrderByDescending(x => x.Id)
            .Skip(pagination.Skip)
            .Take(pagination.PageSize)
            .ToListAsync(cancellationToken);

        return PaginatedResponse<LoanHistory>.Create(
            items,
            pagination.PageNumber,
            pagination.PageSize,
            total
        );
    }
}