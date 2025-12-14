using Fundo.Application.Features.Results;
using Fundo.Application.Features.Shared;
using Fundo.Domain.Repositories;
using MediatR;

namespace Fundo.Application.Features.Queries.GetLoanById;

public class GetLoanByIdHandler : IRequestHandler<GetLoanByIdQuery, Result<LoanResponse>>
{
    private readonly ILoanRepository _loanRepository;

    public GetLoanByIdHandler(ILoanRepository loanRepository)
    {
        _loanRepository = loanRepository;
    }

    public async Task<Result<LoanResponse>> Handle(GetLoanByIdQuery request, CancellationToken cancellationToken)
    {
        var loan = await _loanRepository.GetByIdAsync(request.Id, cancellationToken);
        if (loan == null)
        {
            return Result.Failure<LoanResponse>("Loan not found.", ErrorType.NotFound);
        }
        var response = new LoanResponse(
            loan.Id,
            loan.Amount,
            loan.CurrentBalance,
            loan.ApplicantName,
            loan.Status.ToString()
        );
        return Result.Success(response);
    }
}