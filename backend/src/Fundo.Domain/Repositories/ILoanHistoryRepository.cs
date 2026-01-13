using Fundo.Domain.Entities;

namespace Fundo.Domain.Repositories;

public interface ILoanHistoryRepository
{
    Task AddAsync(LoanHistory loanHistory, CancellationToken cancellationToken = default);
}