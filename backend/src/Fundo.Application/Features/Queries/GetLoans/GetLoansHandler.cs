using Fundo.Application.Features.Shared;
using Fundo.Application.Repositories;
using Fundo.Application.Results;
using MediatR;

namespace Fundo.Application.Features.Queries.GetLoans;

public class GetLoansHandler : IRequestHandler<GetLoansQuery, Result<IEnumerable<LoanResponse>>>
{
    private readonly ILoanRepository _loanRepository;

    public GetLoansHandler(ILoanRepository loanReadRepository)
    {
        _loanRepository = loanReadRepository;
    }

    public async Task<Result<IEnumerable<LoanResponse>>> Handle(GetLoansQuery request, CancellationToken cancellationToken)
    {
        var loans = await _loanRepository.GetAllAsync(cancellationToken);
        var loanResponses = loans.Select(loan => new LoanResponse(
            loan.Id,
            loan.Amount,
            loan.CurrentBalance,
            loan.ApplicantName,
            loan.Status.ToString()
        ));

        return Result.Success(loanResponses);
    }
}