using Fundo.Domain.Entities;

namespace Fundo.Domain.Repositories;

public interface ILoanRepository
{
    Task AddAsync(Loan loan, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Loan>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<Loan?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Loan?> GetByIdForUpdateAsync(Guid id, CancellationToken cancellationToken = default);
}