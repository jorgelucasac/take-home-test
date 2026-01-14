using Fundo.Application.Pagination;
using Fundo.Domain.Entities;

namespace Fundo.Application.Repositories;

public interface ILoanHistoryRepository
{
    Task AddAsync(LoanHistory loanHistory, CancellationToken cancellationToken = default);

    Task<PaginatedResponse<LoanHistory>> GetByLoanIdPagedAsync(int loanId, PaginationQuery pagination, CancellationToken cancellationToken = default);
}