using Fundo.Domain.Entities;

namespace Fundo.Application.Repositories;

public interface ILoanRepository
{
    Task AddAsync(Loan loan, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Loan>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<Loan?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<Loan?> GetByIdForUpdateAsync(int id, CancellationToken cancellationToken = default);
}